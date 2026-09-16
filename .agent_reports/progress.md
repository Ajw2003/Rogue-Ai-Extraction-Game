## 1. claude/plunderspell-asset-pipeline-2cx4w4 (merged)
- Clean merge, no conflicts (2 commits: fbx meta files, art import validator, CI workflows, headless test harness).
- Compile: 0 `error CS` in compile_asset-pipeline.log. Batchmode exited cleanly.
- EditMode tests: 1/1 passed (results_asset-pipeline.xml) — GeneratedPropsImportCleanly.

## 2. origin/claude/plunderspell-enemy-assets-nsfuqn (merged)
- Clean merge, no conflicts (3 commits: 5 rigged enemy models, EnemyForge Blender pipeline, render review contact sheets).
- Compile: 0 `error CS` in compile_enemy-assets.log. Clean batchmode exit.
- EditMode tests: 1/1 passed (results_enemy-assets.xml).

## 3. origin/claude/plunderspell-ui-system-ur2o22 (merged)
- Merge itself was clean (no git conflicts), but surfaced a real structural problem: two
  .asmdef files in Assets/_Project/Scripts/Runtime/Core/ (pre-existing Core.asmdef vs new
  Plunderspell.Core.asmdef). Fixed by relocating the new branch's 8 script files + its asmdef
  into Core/GameFlow/, preserving .meta files so GUIDs/references stayed intact.
- Compile: 0 error CS after the fix.
- EditMode: 12/12 passed.
- PlayMode: found and fixed a real bug in UIScreenshotPlayModeTests (WaitForEndOfFrame is not
  supported under -batchmode at all — see KNOWN_ISSUES.md for full diagnosis). Rewrote the
  capture to use a dedicated camera + RenderTexture instead. Verified 34/34 PlayMode tests pass,
  and visually inspected all 7 produced screenshots as real rendered UI.
- Also committed ~100 Unity-generated .meta files this and the prior enemy-assets branch never
  committed themselves (needed for stable GUIDs).
