"""
Bake the pigment palette into one square PNG texture: a 4x4 grid of flat
colour swatches. Props sample this instead of using photographic materials,
matching the moodboard's illuminated-manuscript, named-pigment look.

Run with plain python3 (no bpy needed):
    python3 Tools/AssetPipeline/make_palette_texture.py
"""
import os
import sys

from PIL import Image

sys.path.insert(0, os.path.dirname(__file__))
from palette import PALETTE_GRID, PIGMENTS, hex_to_rgb01, swatch_cell  # noqa: E402

CELL_PX = 64
SIZE_PX = CELL_PX * PALETTE_GRID
OUT_PATH = os.path.join(
    os.path.dirname(__file__), "..", "..",
    "Assets", "_Project", "Art", "Textures", "PlunderspellPalette.png",
)


def build() -> str:
    img = Image.new("RGB", (SIZE_PX, SIZE_PX), (0, 0, 0))
    for name in PIGMENTS:
        col, row = swatch_cell(name)
        r, g, b = hex_to_rgb01(PIGMENTS[name])
        rgb255 = (round(r * 255), round(g * 255), round(b * 255))
        # swatch_cell()'s row is a UV-space row (v=0 at the bottom, per
        # Blender/OpenGL convention). PIL's pixel rows run the other way
        # (y=0 at the top), so flip here to keep the two in agreement —
        # otherwise every mesh samples the pigment from the wrong row.
        flipped_row = PALETTE_GRID - 1 - row
        x0, y0 = col * CELL_PX, flipped_row * CELL_PX
        for x in range(x0, x0 + CELL_PX):
            for y in range(y0, y0 + CELL_PX):
                img.putpixel((x, y), rgb255)

    out_path = os.path.abspath(OUT_PATH)
    os.makedirs(os.path.dirname(out_path), exist_ok=True)
    img.save(out_path)
    return out_path


if __name__ == "__main__":
    path = build()
    print(f"wrote palette texture: {path}")
