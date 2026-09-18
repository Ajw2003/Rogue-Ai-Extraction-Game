using System.Collections.Generic;
using System.IO;
using RogueAi.Castle;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace RogueAi.EditorTools
{
    /// <summary>
    /// Renders a generated castle to PNG so a layout change can be judged by eye instead of by
    /// reading coordinates out of a log. Produces an overhead orthographic floor plan and a
    /// standing eye-level view from the player's spawn, for each of several seeds.
    ///
    /// Run headlessly with -executeMethod RogueAi.EditorTools.CastleScreenshotForge.CaptureAll.
    /// A real graphics device is required, so the batchmode invocation must NOT pass -nographics.
    /// </summary>
    public static class CastleScreenshotForge
    {
        private const string k_ScenePath = "Assets/_Project/Scenes/RaidScene.unity";
        private const string k_OutputFolder = "docs/generated/castle-screenshots";
        private const string k_GroundObjectName = "Ground";

        private const int k_CaptureWidth = 1600;
        private const int k_CaptureHeight = 900;

        // Wide enough to frame the whole CurtainWall ring (radius 5 cells at 12m) with margin.
        private const float k_PlanOrthographicSize = 78f;
        private const float k_PlanAltitude = 200f;

        private const float k_EyeHeight = 1.65f;

        private static readonly int[] k_Seeds = { 12345, 777, 20260917 };

        [MenuItem("Tools/Plunderspell/Capture Castle Screenshots")]
        public static void CaptureAll()
        {
            string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), k_OutputFolder);
            Directory.CreateDirectory(outputDirectory);

            if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null)
            {
                Debug.LogError("[CastleShots] No graphics device. Re-run without -nographics.");
                EditorApplication.Exit(1);
                return;
            }

            EditorSceneManager.OpenScene(k_ScenePath, OpenSceneMode.Single);

            var generator = Object.FindFirstObjectByType<ProceduralCastleGenerator>();
            if (generator == null)
            {
                Debug.LogError($"[CastleShots] {k_ScenePath} has no ProceduralCastleGenerator.");
                EditorApplication.Exit(1);
                return;
            }

            EnsureSunlight();

            GameObject ground = GameObject.Find(k_GroundObjectName);

            foreach (int seed in k_Seeds)
            {
                ProceduralCastleData data = generator.Generate(seed);

                // The ground plane fills the frame with flat colour from directly overhead, hiding
                // the wall seams and archways the plan exists to show.
                SetGroundVisible(ground, false);
                CapturePlan(data, seed, outputDirectory);
                SetGroundVisible(ground, true);

                CaptureEyeLevel(data, seed, outputDirectory);
                Debug.Log($"[CastleShots] seed {seed}: {data.PlacedModules.Count} modules captured.");
            }

            generator.ClearGenerated();
            Debug.Log($"[CastleShots] Wrote {k_Seeds.Length * 2} images to {k_OutputFolder}.");
        }

        /// <summary>Top-down orthographic floor plan: shows gaps, overlaps and overall shape.</summary>
        private static void CapturePlan(ProceduralCastleData data, int seed, string outputDirectory)
        {
            Vector3 center = LayoutCenter(data);
            var position = new Vector3(center.x, k_PlanAltitude, center.z);

            RenderFrom(position, Quaternion.Euler(90f, 0f, 0f), isOrthographic: true,
                Path.Combine(outputDirectory, $"seed-{seed}-plan.png"));
        }

        /// <summary>
        /// Standing eye-level view from the crypt centre looking outward — the check that a player
        /// can actually see through aligned archways rather than into a sealed wall.
        /// </summary>
        private static void CaptureEyeLevel(ProceduralCastleData data, int seed, string outputDirectory)
        {
            Vector3 origin = data.CryptStartIndex >= 0
                ? data.PlacedModules[data.CryptStartIndex].Position
                : LayoutCenter(data);

            var position = new Vector3(origin.x, k_EyeHeight, origin.z);

            RenderFrom(position, Quaternion.Euler(0f, 0f, 0f), isOrthographic: false,
                Path.Combine(outputDirectory, $"seed-{seed}-eye.png"));
        }

        private static void RenderFrom(Vector3 position, Quaternion rotation, bool isOrthographic,
            string filePath)
        {
            var cameraGo = new GameObject("CastleScreenshotCamera");
            cameraGo.transform.SetPositionAndRotation(position, rotation);

            var camera = cameraGo.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.06f, 0.06f, 0.08f);
            camera.orthographic = isOrthographic;
            camera.orthographicSize = k_PlanOrthographicSize;
            camera.fieldOfView = 70f;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 1000f;

            var renderTexture = new RenderTexture(k_CaptureWidth, k_CaptureHeight, 24);
            camera.targetTexture = renderTexture;
            camera.Render();

            RenderTexture previousActive = RenderTexture.active;
            RenderTexture.active = renderTexture;

            var texture = new Texture2D(k_CaptureWidth, k_CaptureHeight, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0f, 0f, k_CaptureWidth, k_CaptureHeight), 0, 0);
            texture.Apply();

            RenderTexture.active = previousActive;
            File.WriteAllBytes(filePath, texture.EncodeToPNG());

            Object.DestroyImmediate(texture);
            camera.targetTexture = null;
            renderTexture.Release();
            Object.DestroyImmediate(renderTexture);
            Object.DestroyImmediate(cameraGo);
        }

        private static void SetGroundVisible(GameObject ground, bool isVisible)
        {
            if (ground == null)
            {
                return;
            }

            var renderer = ground.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = isVisible;
            }
        }

        /// <summary>Mean of every placed module position, so the plan frames the castle it built.</summary>
        private static Vector3 LayoutCenter(ProceduralCastleData data)
        {
            List<ProceduralCastleData.PlacedModule> modules = data.PlacedModules;
            if (modules.Count == 0)
            {
                return Vector3.zero;
            }

            var sum = Vector3.zero;
            for (int i = 0; i < modules.Count; i++)
            {
                sum += modules[i].Position;
            }

            return sum / modules.Count;
        }

        /// <summary>
        /// Without a light the plan renders as flat silhouettes, which hides exactly the seams and
        /// wall thicknesses these captures exist to show.
        /// </summary>
        private static void EnsureSunlight()
        {
            foreach (Light existing in Object.FindObjectsByType<Light>(FindObjectsSortMode.None))
            {
                if (existing.type == LightType.Directional)
                {
                    return;
                }
            }

            var lightGo = new GameObject("CastleScreenshotSun");
            var light = lightGo.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.1f;
            lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }
    }
}
