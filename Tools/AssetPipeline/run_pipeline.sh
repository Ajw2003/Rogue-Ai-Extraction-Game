#!/usr/bin/env bash
# Full asset pipeline: palette -> build+validate -> validate -> render -> sheet.
#
# PYTHONHASHSEED matters. Blender's FBX exporter derives object UIDs from
# Python string hashes, which are salted per process, so without it the same
# mesh exports different bytes every run (measured: 127 differing bytes per
# file; with it, only the 3 bytes of the export timestamp).
#
# Usage:  Tools/AssetPipeline/run_pipeline.sh [--force]
#         --force re-exports and re-renders everything, ignoring the manifest.
set -euo pipefail

cd "$(dirname "$0")/../.."
export PYTHONHASHSEED=0

FORCE="${1:-}"

echo "== palette texture =="
python3 Tools/AssetPipeline/make_palette_texture.py

echo "== build + validate (gate 1) =="
blender -b -P Tools/AssetPipeline/build_assets.py -- ${FORCE} \
  | grep -E "^\[|re-exported|WARNING"

echo "== standalone validator (gate 2) =="
python3 Tools/AssetPipeline/validate_asset.py --all

echo "== previews =="
blender -b -P Tools/AssetPipeline/render_previews.py -- ${FORCE} \
  | grep -E "^rendered|^unchanged|re-rendered"

echo "== contact sheet =="
python3 Tools/AssetPipeline/make_contact_sheet.py

echo
echo "Done. 'git status' should be clean unless something actually changed."
