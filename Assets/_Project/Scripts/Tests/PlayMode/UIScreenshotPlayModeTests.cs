using System.Collections;
using System.IO;
using NUnit.Framework;
using Plunderspell.Core;
using Plunderspell.UI;
using UnityEngine;
using UnityEngine.TestTools;

namespace Plunderspell.Tests.PlayMode
{
    /// <summary>
    /// Automated visual proof: cycles through every GameState and screenshots the result to
    /// ./UI_Verification_Screenshots/. Run via Window > General > Test Runner (Play Mode tab) in the
    /// Editor, or headless with:
    /// Unity -batchmode -projectPath &lt;path&gt; -runTests -testPlatform PlayMode
    ///   -testResults UI_Verification_Screenshots/playmode-results.xml -logFile - -quit
    /// </summary>
    public class UIScreenshotPlayModeTests
    {
        private const string OutputFolder = "UI_Verification_Screenshots";

        [UnityTest]
        public IEnumerator CapturesAllUIScreens()
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), OutputFolder);
            Directory.CreateDirectory(directory);

            yield return null;

            var uiRoot = Object.FindFirstObjectByType<UIRoot>();
            Assert.IsNotNull(uiRoot, "UIRoot was not bootstrapped into the play mode scene.");

            yield return CaptureState(GameState.MainMenu, "01_MainMenu", directory);
            yield return CaptureState(GameState.Playing, "02_HUD", directory);
            yield return CaptureState(GameState.Paused, "03_PauseMenu", directory);

            var demoItem = ScriptableObject.CreateInstance<ItemDefinition>();
            demoItem.ItemName = "Cursed Doubloon";
            demoItem.MaxStack = 8;
            GameServices.Inventory.AddItem(demoItem, 5);
            yield return CaptureState(GameState.Inventory, "04_Inventory", directory);

            yield return CaptureState(GameState.Settings, "05_Settings", directory);

            GameServices.PlayerStats.AddGold(250);
            yield return CaptureState(GameState.Victory, "06_Victory", directory);
            yield return CaptureState(GameState.GameOver, "07_GameOver", directory);

            Assert.IsTrue(File.Exists(Path.Combine(directory, "01_MainMenu.png")), "Expected a screenshot file to have been written.");
        }

        private static IEnumerator CaptureState(GameState state, string fileName, string directory)
        {
            GameServices.GameState.ChangeState(state);
            yield return null;
            yield return new WaitForEndOfFrame();
            ScreenCapture.CaptureScreenshot(Path.Combine(directory, fileName + ".png"));
            yield return null;
        }
    }
}
