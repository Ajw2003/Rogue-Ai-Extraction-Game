using System.Collections.Generic;
using System.IO;
using RogueAi.Spells;
using RogueAi.Spells.Vfx;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RogueAi.EditorTools
{
    /// <summary>
    /// Photographs every spell's visual, so "the spells are visible" is something you can look at
    /// rather than something a test asserts about a GameObject's existence.
    ///
    /// See docs/systems/spells.md, "Seeing a cast".
    /// </summary>
    public static class SpellVfxScreenshotForge
    {
        private const string k_ScenePath = "Assets/_Project/Scenes/CombatBench.unity";
        private const string k_OutputFolder = "docs/generated/spell-vfx-screenshots";
        private const string k_BoltPath = "Assets/_Project/Prefabs/Projectiles/Bolt.prefab";

        private const float k_Spacing = 4.5f;

        /// <summary>Mid-bloom: bright, well grown, not yet faded out.</summary>
        private const float k_CaptureProgress = 0.4f;

        private static readonly SpellId[] k_Showcase =
        {
            SpellId.Ignis, SpellId.Frango, SpellId.Levo, SpellId.AurumVoco,
            SpellId.Tonitrus, SpellId.Somnus, SpellId.CadaverSurge, SpellId.Porta,
            SpellId.MisfireIgnis,
        };

        [MenuItem("Tools/Plunderspell/Capture Spell VFX Screenshots")]
        public static void CaptureAll()
        {
            if (!SceneScreenshot.HasGraphicsDevice)
            {
                Debug.LogError("[SpellShots] No graphics device. Re-run without -nographics.");
                EditorApplication.Exit(1);
                return;
            }

            string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), k_OutputFolder);
            Directory.CreateDirectory(outputDirectory);

            EditorSceneManager.OpenScene(k_ScenePath, OpenSceneMode.Single);

            var spawned = new List<GameObject>();
            float x = 0f;

            foreach (SpellId spell in k_Showcase)
            {
                SpellLook look = SpellLookbook.For(spell);
                var at = new Vector3(x, 1.6f, 0f);

                SpellBurst burst = SpellBurst.Spawn(at, look.Colour, look.Radius, 0.45f);
                burst.SetProgress(k_CaptureProgress);
                burst.name = $"Burst_{spell}";
                spawned.Add(burst.gameObject);

                if (look.Style == SpellVisualStyle.Bolt)
                {
                    GameObject bolt = SpawnTintedBolt(at + Vector3.forward * 2.2f, look.Colour);
                    if (bolt != null)
                    {
                        bolt.name = $"Bolt_{spell}";
                        spawned.Add(bolt);
                    }
                }

                x += k_Spacing;
            }

            float centreX = (x - k_Spacing) * 0.5f;
            var focus = new Vector3(centreX, 1.6f, 0f);
            float aspect = SceneScreenshot.DefaultWidth / (float)SceneScreenshot.DefaultHeight;
            float orthographicSize = (centreX + k_Spacing) / aspect;

            // Camera stays inside the arena: the bench wall is opaque, and a camera behind it
            // photographs the wall.
            var front = new Vector3(centreX, focus.y, -16f);
            SceneScreenshot.Capture(front, Quaternion.identity, isOrthographic: true,
                orthographicSize, Path.Combine(outputDirectory, "spells-lineup.png"));

            var angled = new Vector3(centreX - 14f, 7f, -14f);
            SceneScreenshot.Capture(angled, Quaternion.LookRotation(focus - angled, Vector3.up),
                isOrthographic: false, orthographicSize,
                Path.Combine(outputDirectory, "spells-lineup-angled.png"));

            var close = new Vector3(0f, 1.6f, -5f);
            SceneScreenshot.Capture(close,
                Quaternion.LookRotation(new Vector3(0f, 1.6f, 0f) - close, Vector3.up),
                isOrthographic: false, orthographicSize,
                Path.Combine(outputDirectory, "spell-ignis-close.png"));

            foreach (GameObject go in spawned)
            {
                Object.DestroyImmediate(go);
            }

            Debug.Log($"[SpellShots] Wrote 3 images to {k_OutputFolder} for " +
                      $"{k_Showcase.Length} spell looks.");
        }

        private static GameObject SpawnTintedBolt(Vector3 position, Color colour)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(k_BoltPath);
            if (prefab == null)
            {
                Debug.LogWarning($"[SpellShots] No projectile at {k_BoltPath}.");
                return null;
            }

            GameObject bolt = Object.Instantiate(prefab, position, Quaternion.identity);
            if (bolt.TryGetComponent(out ProjectileTint tint))
            {
                tint.Apply(colour);
            }

            return bolt;
        }
    }
}
