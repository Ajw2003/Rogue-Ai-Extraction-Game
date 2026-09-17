using System.Collections.Generic;
using System.IO;
using RogueAi.Castle;
using RogueAi.Guards;
using RogueAi.Raid;
using RogueAi.Status;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RogueAi.EditorTools
{
    /// <summary>
    /// Authors one prefab per enemy model, plus the <see cref="EnemyRoster"/> that posts them to
    /// zones. Run once; the prefabs and the roster are committed assets from then on, editable by
    /// hand like any other.
    ///
    /// See docs/systems/raid-scene-assembly.md ("Enemy prefabs") for why the prefabs are variants of
    /// the model rather than copies of it, and for the per-enemy tuning table.
    /// </summary>
    public static class EnemyPrefabForge
    {
        private const string ModelDirectory = "Assets/Models/Enemies";
        private const string PrefabDirectory = "Assets/_Project/Prefabs/Enemies";
        private const string RosterPath = "Assets/_Project/Data/Enemies/EnemyRoster.asset";

        /// <summary>One enemy's tuning and the zones it garrisons.</summary>
        private readonly struct EnemySpec
        {
            public readonly string Name;
            public readonly float PatrolSpeed;
            public readonly float ChaseSpeed;
            public readonly float SightRange;
            public readonly float MaxHealth;
            public readonly (CastleZone Zone, int Weight)[] Posts;

            public EnemySpec(string name, float patrolSpeed, float chaseSpeed, float sightRange,
                float maxHealth, params (CastleZone, int)[] posts)
            {
                Name = name;
                PatrolSpeed = patrolSpeed;
                ChaseSpeed = chaseSpeed;
                SightRange = sightRange;
                MaxHealth = maxHealth;
                Posts = posts;
            }
        }

        // Tuned from the roles recorded in Assets/Models/Enemies/enemy_manifest.json. Weights put the
        // common soldiery on the outside and the rare, dangerous things in the Keep and the Crypt.
        private static readonly EnemySpec[] Specs =
        {
            new EnemySpec("Watchman",       2.0f, 4.0f, 14f,  70f,
                (CastleZone.CurtainWall, 12), (CastleZone.OuterBailey, 10)),
            new EnemySpec("ManAtArms",      2.0f, 4.5f, 15f, 100f,
                (CastleZone.OuterBailey, 8), (CastleZone.InnerWard, 8)),
            new EnemySpec("Sergeant",       2.2f, 5.0f, 17f, 130f,
                (CastleZone.InnerWard, 5), (CastleZone.Keep, 5)),
            new EnemySpec("WarHound",       2.8f, 6.5f, 12f,  55f,
                (CastleZone.OuterBailey, 6), (CastleZone.InnerWard, 6)),
            new EnemySpec("SigilWisp",      3.0f, 5.5f, 16f,  35f,
                (CastleZone.InnerWard, 5), (CastleZone.Keep, 4)),
            new EnemySpec("VaultWarden",    1.4f, 3.2f, 13f, 200f,
                (CastleZone.Keep, 6), (CastleZone.Crypt, 5)),
            // A sentry that never leaves its post: speed 0 means it tracks and fires without walking.
            new EnemySpec("HexTurret",      0f,   0f,   20f,  90f,
                (CastleZone.CurtainWall, 4), (CastleZone.Keep, 3)),
            new EnemySpec("ArcRevenant",    1.8f, 3.8f, 18f, 150f,
                (CastleZone.Keep, 3), (CastleZone.Crypt, 5)),
            new EnemySpec("CryptRisen",     1.6f, 4.2f, 12f,  80f,
                (CastleZone.Crypt, 12)),
            new EnemySpec("GildedColossus", 1.2f, 2.8f, 15f, 400f,
                (CastleZone.Crypt, 2)),
        };

        [MenuItem("Tools/Plunderspell/Forge Enemy Prefabs + Roster")]
        public static void Forge()
        {
            EnsureFolder(PrefabDirectory);
            EnsureFolder(Path.GetDirectoryName(RosterPath).Replace('\\', '/'));

            var roster = LoadOrCreate<EnemyRoster>(RosterPath);
            roster.Entries.Clear();

            var built = new List<string>();
            var missing = new List<string>();

            foreach (EnemySpec spec in Specs)
            {
                string modelPath = $"{ModelDirectory}/{spec.Name}/{spec.Name}.fbx";
                var model = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
                if (model == null)
                {
                    missing.Add(modelPath);
                    continue;
                }

                GameObject prefab = BuildPrefab(model, spec);
                built.Add(prefab.name);

                foreach ((CastleZone zone, int weight) in spec.Posts)
                {
                    roster.Entries.Add(new EnemyRoster.Entry
                    {
                        EnemyId = spec.Name,
                        Zone = zone,
                        Weight = weight,
                        Prefab = prefab
                    });
                }
            }

            EditorUtility.SetDirty(roster);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (missing.Count > 0)
                Debug.LogError($"Plunderspell: {missing.Count} enemy model(s) missing: {string.Join(", ", missing)}");

            Debug.Log($"Plunderspell: forged {built.Count} enemy prefab(s) into {PrefabDirectory} " +
                      $"and {roster.Entries.Count} roster posting(s) into {RosterPath}.");
        }

        /// <summary>
        /// Saves a prefab variant of the model with the components a guard needs. A variant, not a
        /// copy, so re-exporting the .blend flows straight through to every prefab.
        /// </summary>
        private static GameObject BuildPrefab(GameObject model, EnemySpec spec)
        {
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(model);
            instance.name = spec.Name;

            Bounds bounds = MeasureBounds(instance);
            float radius = Mathf.Max(0.3f, Mathf.Max(bounds.size.x, bounds.size.z) * 0.5f);
            float height = Mathf.Max(0.5f, bounds.size.y);

            // The models are exported feet-on-origin, so the collider is centred on half its height.
            var capsule = instance.AddComponent<CapsuleCollider>();
            capsule.radius = radius;
            capsule.height = height;
            capsule.center = new Vector3(0f, height * 0.5f, 0f);

            var agent = instance.AddComponent<NavMeshAgent>();
            agent.radius = radius;
            agent.height = height;
            agent.speed = Mathf.Max(spec.PatrolSpeed, 0.01f);
            agent.angularSpeed = 240f;
            agent.acceleration = 12f;
            agent.stoppingDistance = 0.8f;
            // A turret is placed, not steered: letting it avoid others would drift it off its post.
            agent.obstacleAvoidanceType = spec.PatrolSpeed <= 0f
                ? ObstacleAvoidanceType.NoObstacleAvoidance
                : ObstacleAvoidanceType.LowQualityObstacleAvoidance;

            instance.AddComponent<StatusEffectReceiver>();
            CastleGuard guard = instance.AddComponent<CastleGuard>();
            ApplyGuardTuning(guard, spec, height);

            string path = $"{PrefabDirectory}/{spec.Name}.prefab";
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(instance, path);
            Object.DestroyImmediate(instance);
            return prefab;
        }

        /// <summary>
        /// Writes the guard's inspector fields. They are private and serialized, so this goes through
        /// SerializedObject rather than widening the runtime API just for an authoring tool.
        /// </summary>
        private static void ApplyGuardTuning(CastleGuard guard, EnemySpec spec, float height)
        {
            var so = new SerializedObject(guard);
            so.FindProperty("_sightRange").floatValue = spec.SightRange;
            so.FindProperty("_patrolSpeed").floatValue = spec.PatrolSpeed;
            so.FindProperty("_chaseSpeed").floatValue = spec.ChaseSpeed;
            so.FindProperty("_maxHealth").floatValue = spec.MaxHealth;
            // Eyes sit near the top of the model, so the line-of-sight ray clears its own collider.
            so.FindProperty("_eyeHeight").floatValue = height * 0.9f;
            // Castle geometry is on Default; without this the sight check never hits a wall and
            // guards see through the castle.
            so.FindProperty("_geometryLayers").intValue = 1;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private static Bounds MeasureBounds(GameObject instance)
        {
            var renderers = instance.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length == 0)
                return new Bounds(Vector3.zero, Vector3.one);

            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
                bounds.Encapsulate(renderers[i].bounds);
            return bounds;
        }

        private static T LoadOrCreate<T>(string path) where T : ScriptableObject
        {
            var existing = AssetDatabase.LoadAssetAtPath<T>(path);
            if (existing != null)
                return existing;

            var created = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(created, path);
            return created;
        }

        private static void EnsureFolder(string path)
        {
            if (Directory.Exists(path))
                return;
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
        }
    }
}
