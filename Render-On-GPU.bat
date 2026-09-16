@echo off
rem Render the Plunderspell enemy review sheets on the GPU.
rem
rem Double-click this file, or drag it into a cmd window and press Enter.
rem It locates the repo relative to itself and finds Blender automatically,
rem so it does not matter what directory you run it from.
rem
rem Any extra arguments are passed through to render_enemies.py, e.g.
rem     Render-On-GPU.bat --resolution 1024 --samples 64 --only VaultWarden

setlocal
title Plunderspell - GPU render

rem %~dp0 is this file's own folder, which is the repo root.
cd /d "%~dp0"

rem Prefer Blender on PATH; otherwise try the default install locations,
rem newest first. Blackwell cards (RTX 50xx) need 4.5 or newer for OptiX.
set "BLENDER="
for %%B in (blender.exe) do if exist "%%~$PATH:B" set "BLENDER=%%~$PATH:B"

if not defined BLENDER for %%V in (5.0 4.5 4.4 4.3 4.2) do (
  if not defined BLENDER if exist "C:\Program Files\Blender Foundation\Blender %%V\blender.exe" (
    set "BLENDER=C:\Program Files\Blender Foundation\Blender %%V\blender.exe"
  )
)

if not defined BLENDER (
  echo.
  echo Could not find blender.exe on PATH or in C:\Program Files\Blender Foundation.
  echo Install Blender 5.0, or edit this file and set BLENDER to its full path:
  echo     set "BLENDER=D:\wherever\blender.exe"
  echo.
  pause
  exit /b 1
)

echo Blender: %BLENDER%
echo Repo:    %CD%
echo.

"%BLENDER%" --background --python "Tools\EnemyForge\render_enemies.py" -- --device OPTIX %*
set "STATUS=%ERRORLEVEL%"

echo.
if not "%STATUS%"=="0" (
  echo Render FAILED with exit code %STATUS%. The error is above.
) else (
  echo Done. Renders are in: %CD%\Enemy_Renders_Review
)
echo.
pause
exit /b %STATUS%
