# Known issues found during the 2026-09-15 branch integration

## PlayMode screenshot tests hang forever under `-nographics` batchmode

**Symptom:** `Unity.exe -batchmode -nographics -projectPath . -runTests -testPlatform PlayMode ...`
never returns and produces no further log output after Unity finishes loading the test scene.
From the outside this looks identical to a stuck/frozen process — no error, no timeout, no exit.

**Root cause:** [`UIScreenshotPlayModeTests.CapturesAllUIScreens`](../Assets/_Project/Scripts/Tests/PlayMode/UIScreenshotPlayModeTests.cs)
(added by the `plunderspell-ui-system` branch) does, per captured UI state:

```csharp
yield return new WaitForEndOfFrame();
ScreenCapture.CaptureScreenshot(...);
```

`WaitForEndOfFrame` waits for a real rendered frame to finish. `-nographics` disables the
rendering surface entirely, so that frame never completes and the coroutine — and the whole
test run — hangs indefinitely. There is no timeout on the Unity side; it will wait forever.

**Evidence:** `test_ui-system-playmode.log` shows normal forward progress (import, backup-scene
load, `[VoiceServiceLocator] Auto-registered MockVoiceInputService (editor/headless/no-mic)`,
mode service init, licensing resolved) for about a minute, then stops dead at
`TrimDiskCacheJob: Current cache size 0mb` — exactly the point where the first `CaptureState`
call would hit `WaitForEndOfFrame`. The process kept running with zero further log output until
it was killed ~8 minutes later.

**How this surfaced as a process problem, not just a test problem:** the agent running the
integration ran this Unity invocation as a *synchronous, foreground* command with a 10-minute
timeout and no streamed output. A blocking foreground call gives no visibility while it runs and
cannot be interrupted or checked on mid-flight — the user had to kill the task manually to regain
the agent's attention. Structural fix: any Unity batchmode invocation that can run long (PlayMode
tests especially) should run in the background with periodic status checks, never as a blind
synchronous wait.

**Fix for the test itself (not yet applied — needs a decision):**
- Simplest: run PlayMode screenshot tests without `-nographics` (drop the flag; `-batchmode`
  alone still produces no visible window on this machine, but keeps a real rendering surface
  so `WaitForEndOfFrame` actually completes).
- Alternative: guard the screenshot capture so it degrades gracefully headless (e.g. skip the
  `WaitForEndOfFrame`/`CaptureScreenshot` calls under `Application.isBatchMode && SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null`),
  so the test still exercises the state-cycling logic in fully headless CI without hanging.

**Process fix already applied going forward:** all further Unity batchmode calls in this
integration run in the background with polling instead of blocking synchronously.
