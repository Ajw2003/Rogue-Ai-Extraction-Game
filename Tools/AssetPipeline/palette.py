"""
Shared pigment palette for Plunderspell props.

Colours are lifted directly from docs/plunderspell-moodboard.html (the pitch-bible
CSS custom properties). Kept as one source of truth so the palette texture
generator, the Blender builder, and the standalone validator never drift apart.

Grid layout: PALETTE_GRID x PALETTE_GRID swatches baked into one square texture.
Every mesh samples flat colour by UV-mapping a face into a swatch's cell — no
photographic texturing, matching the moodboard's "named pigment" flat-colour
illuminated-manuscript aesthetic.
"""

PALETTE_GRID = 4  # 4x4 = 16 cells, one per named pigment below

PIGMENTS = {
    "bone_black":   "#14120E",  # charred bone, the ground
    "ash":          "#1E1A14",  # raised surface
    "ash_hi":       "#282318",  # hover / inset
    "vellum":       "#DCD2BA",  # calfskin, primary text
    "vellum_dim":   "#9A9078",
    "vellum_faint": "#635C4C",
    "verdigris":    "#5FA288",  # oxidised copper accent
    "verdigris_lo": "#2E4C41",
    "orpiment":     "#C9A227",  # arsenic yellow - value, loot, gold only
    "madder":       "#C4542E",  # madder lake - heat, danger, blood
    "lapis":        "#7A6AA0",  # ground lapis - the arcane, the voice
    "line":         "#332D22",
    # extra material pigments used by prop builders, named after the era kit lists
    "bronze":       "#B98A34",  # ceremonial bronze / khopesh / ingots
    "iron":         "#6E7278",  # arming sword / mace / poleaxe steel
    "oak":          "#4A3A28",  # haft / shield board timber
    "leather":      "#5A4630",  # straps, oxhide
}

# Deterministic swatch order -> grid cell index, so every run of the builder
# lays the texture out identically.
_ORDER = list(PIGMENTS.keys())


def swatch_cell(name: str) -> tuple[int, int]:
    """Return the (col, row) cell a pigment occupies in the PALETTE_GRID atlas."""
    idx = _ORDER.index(name)
    return idx % PALETTE_GRID, idx // PALETTE_GRID


def hex_to_rgb01(hex_code: str) -> tuple[float, float, float]:
    hex_code = hex_code.lstrip("#")
    r, g, b = (int(hex_code[i:i + 2], 16) / 255.0 for i in (0, 2, 4))
    return r, g, b


# Era -> accent pigment, per the moodboard's stratigraphy section, used to
# lightly re-tint metal parts (e.g. hilt wrappings) per acquisition era.
ERA_ACCENT = {
    "BronzeAge":    "bronze",
    "HighMedieval": "verdigris",
    "LateMedieval": "madder",
    "AgeOfPowder":  "lapis",
}

if len(PIGMENTS) > PALETTE_GRID * PALETTE_GRID:
    raise ValueError("PALETTE_GRID too small for pigment count")
