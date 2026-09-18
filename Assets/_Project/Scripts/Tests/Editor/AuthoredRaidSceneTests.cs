using NUnit.Framework;
using Player;
using RogueAi.Guards;
using RogueAi.Loot;
using RogueAi.Raid;
using RogueAi.UI;
using StateMachine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueAi.Tests.Editor
{
    /// <summary>
    /// Guards the wiring of the hand-authored raid scene. See
    /// docs/systems/raid-scene-assembly.md, "Authored, not generated", for why this exists.
    /// </summary>
    public class AuthoredRaidSceneTests
    {
        private const string k_ScenePath = "Assets/_Project/Scenes/RaidScene.unity";
        private const string k_PlayerPrefabPath = "Assets/_Project/Prefabs/RaidPlayer.prefab";

        private Scene m_scene;

        [SetUp]
        public void SetUp()
        {
            m_scene = EditorSceneManager.OpenScene(k_ScenePath, OpenSceneMode.Additive);
        }

        [TearDown]
        public void TearDown()
        {
            if (m_scene.IsValid())
            {
                EditorSceneManager.CloseScene(m_scene, true);
            }
        }

        private T Find<T>() where T : Object
        {
            foreach (GameObject root in m_scene.GetRootGameObjects())
            {
                var found = root.GetComponentInChildren<T>(true);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        [Test]
        public void Test_ThePlayerIsAnInstanceOfTheAuthoredPrefab()
        {
            var stateMachine = Find<PlayerStateMachine>();
            Assert.IsNotNull(stateMachine, $"{k_ScenePath} has no PlayerStateMachine.");

            Assert.IsTrue(PrefabUtility.IsPartOfPrefabInstance(stateMachine.gameObject),
                "The player must be a prefab instance, or editing RaidPlayer.prefab does not " +
                "reach the raid.");

            string source = AssetDatabase.GetAssetPath(
                PrefabUtility.GetCorrespondingObjectFromSource(stateMachine.gameObject));
            Assert.AreEqual(k_PlayerPrefabPath, source);
        }

        /// <summary>
        /// The regression that started all this: the scene carries the shipping controller, and the
        /// playtest harness is not in the raid.
        /// </summary>
        [Test]
        public void Test_ThePlayerUsesTheShippingControllerNotThePlaytestHarness()
        {
            Assert.IsNotNull(Find<PlayerInputController>(),
                "The raid player needs PlayerInputController; without it nothing drives the " +
                "state machine and the player reads as unresponsive.");
            Assert.IsNull(Find<RogueAi.Playtest.FreeLookPlaytestController>(),
                "FreeLookPlaytestController is the ItemGym harness. In the raid it silently " +
                "replaces the real player and undoes the issue 9 input gate.");
        }

        [Test]
        public void Test_ThePlayerCarriesWhatTheRaidExpectsOfIt()
        {
            Assert.IsNotNull(Find<IntruderTag>(), "Guards find intruders through IntruderTag.");
            Assert.IsNotNull(Find<LootInteractor>(), "Nothing can be picked up without a LootInteractor.");
            Assert.IsNotNull(Find<Camera>(), "The raid scene has no camera to see out of.");
        }

        /// <summary>
        /// A generated scene had its references wired by the builder every run. An authored one keeps
        /// whatever was last saved, so a reference dropped by hand stays dropped until someone plays
        /// it — this is what replaces the builder as the check.
        /// </summary>
        [Test]
        public void Test_TheDirectorAndHudAreStillWiredToTheScene()
        {
            var director = Find<RaidDirector>();
            Assert.IsNotNull(director, $"{k_ScenePath} has no RaidDirector.");

            var serialized = new SerializedObject(director);
            foreach (string field in new[]
                     {
                         "_generator", "_lootSpawner", "_guardSpawner", "_extractionZone",
                         "_lair", "_alarm", "_navigation", "_playerRoot",
                     })
            {
                SerializedProperty property = serialized.FindProperty(field);
                Assert.IsNotNull(property, $"RaidDirector has no field {field}.");
                Assert.IsNotNull(property.objectReferenceValue,
                    $"RaidDirector.{field} is unassigned in the authored scene.");
            }

            var presenter = Find<RaidHudPresenter>();
            Assert.IsNotNull(presenter, $"{k_ScenePath} has no RaidHudPresenter.");

            var hud = new SerializedObject(presenter);
            foreach (string field in new[] { "_director", "_extractionZone", "_alarm", "_lair", "_interactor" })
            {
                SerializedProperty property = hud.FindProperty(field);
                Assert.IsNotNull(property, $"RaidHudPresenter has no field {field}.");
                Assert.IsNotNull(property.objectReferenceValue,
                    $"RaidHudPresenter.{field} is unassigned in the authored scene.");
            }
        }
    }
}
