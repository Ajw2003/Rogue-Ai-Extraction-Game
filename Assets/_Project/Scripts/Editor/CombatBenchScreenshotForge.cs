using System.IO;
using RogueAi.Guards;
using RogueAi.Playtest;
using RogueAi.Raid;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RogueAi.EditorTools
{
    /// <summary>
    /// Photographs the combat bench with a spawn on the floor. See docs/systems/combat-bench.md,
    /// "Verification", for the headless invocation and why the panel can never appear in these.
    /// </summary>
    public static class CombatBenchScreenshotForge
    {
        private const string k_ScenePath = "Assets/_Project/Scenes/CombatBench.unity";
        private const string k_OutputFolder = "docs/generated/combat-bench-screenshots";

        private const int k_SpawnCount = 5;
        private const float k_EyeHeight = 1.65f;

        private static readonly (string Name, Vector3 Offset, bool LooksAtCentre)[] k_Views =
        {
            ("plan", new Vector3(0f, 46f, 0f), true),
            ("aerial-ne", new Vector3(26f, 18f, 26f), true),
            ("aerial-sw", new Vector3(-26f, 18f, -26f), true),
            ("eye", new Vector3(0f, k_EyeHeight, -9f), true),
        };

        [MenuItem("Tools/Plunderspell/Capture Combat Bench Screenshots")]
        public static void CaptureAll()
        {
            if (!SceneScreenshot.HasGraphicsDevice)
            {
                Debug.LogError("[BenchShots] No graphics device. Re-run without -nographics.");
                EditorApplication.Exit(1);
                return;
            }

            string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), k_OutputFolder);
            Directory.CreateDirectory(outputDirectory);

            EditorSceneManager.OpenScene(k_ScenePath, OpenSceneMode.Single);

            var bench = Object.FindFirstObjectByType<CombatBench>();
            if (bench == null)
            {
                Debug.LogError($"[BenchShots] {k_ScenePath} has no CombatBench.");
                EditorApplication.Exit(1);
                return;
            }

            GameObject prefab = FirstSpawnablePrefab(bench.Roster);
            if (prefab != null)
            {
                bench.Spawn(prefab, k_SpawnCount, GuardAlertState.Chasing);
            }
            else
            {
                Debug.LogWarning("[BenchShots] Roster has no prefab; capturing an empty arena.");
            }

            int written = 0;
            foreach ((string name, Vector3 offset, bool looksAtCentre) in k_Views)
            {
                bool isPlan = name == "plan";
                Quaternion rotation = isPlan
                    ? Quaternion.Euler(90f, 0f, 0f)
                    : Quaternion.LookRotation(new Vector3(0f, k_EyeHeight, 0f) - offset, Vector3.up);

                SceneScreenshot.Capture(offset, rotation, isPlan, 24f,
                    Path.Combine(outputDirectory, $"bench-{name}.png"));
                written++;
            }

            bench.ClearSpawned();
            Debug.Log($"[BenchShots] Wrote {written} images to {k_OutputFolder}, " +
                      $"with {k_SpawnCount} spawned enemies in frame.");
        }

        private static GameObject FirstSpawnablePrefab(EnemyRoster roster)
        {
            if (roster == null)
            {
                return null;
            }

            for (int i = 0; i < roster.Entries.Count; i++)
            {
                EnemyRoster.Entry entry = roster.Entries[i];
                if (entry != null && entry.Prefab != null)
                {
                    return entry.Prefab;
                }
            }

            return null;
        }
    }
}
