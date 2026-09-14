"""
One function per prop. Every builder is authored bottom-flush: the lowest
vertex of the finished mesh sits at local Z=0 (the "base" pivot the
validator checks for), and the shape rises along +Z from there. Blender's
default FBX export axis settings (-Z forward, Y up) then land this the
right way up for Unity without any extra axis fiddling.

Each function just stacks mesh_kit primitives and paints them straight from
the pigment palette (mesh_kit.paint) — no manual UV work needed here.
"""
import math

from mathutils import Euler

import mesh_kit as mk


def build_bronze_sword(bm, uv):
    mk.paint(bm, mk.add_sphere(bm, 0.022, loc=(0, 0, 0.022)), "bronze", uv)
    mk.paint(bm, mk.add_cylinder(bm, 0.017, 0.13, loc=(0, 0, 0.022 + 0.065), segments=8), "leather", uv)
    top_grip = 0.022 + 0.13
    mk.paint(bm, mk.add_box(bm, (0.10, 0.022, 0.02), loc=(0, 0, top_grip + 0.01)), "bronze", uv)
    blade_base = top_grip + 0.02
    blade_len = 0.46
    mk.paint(bm, mk.add_cylinder(
        bm, 0.028, blade_len, loc=(0, 0, blade_base + blade_len / 2),
        segments=4, radius2=0.002, scale=(1, 0.32, 1),
    ), "bronze", uv)


def build_longsword(bm, uv):
    mk.paint(bm, mk.add_sphere(bm, 0.026, loc=(0, 0, 0.026)), "orpiment", uv)
    mk.paint(bm, mk.add_cylinder(bm, 0.019, 0.22, loc=(0, 0, 0.026 + 0.11), segments=8), "leather", uv)
    top_grip = 0.026 + 0.22
    mk.paint(bm, mk.add_box(bm, (0.16, 0.024, 0.022), loc=(0, 0, top_grip + 0.011)), "iron", uv)
    blade_base = top_grip + 0.022
    blade_len = 0.68
    mk.paint(bm, mk.add_cylinder(
        bm, 0.03, blade_len, loc=(0, 0, blade_base + blade_len / 2),
        segments=4, radius2=0.002, scale=(1, 0.28, 1),
    ), "iron", uv)


def build_flintlock_pistol(bm, uv):
    # Stands on its butt. The read comes from the raked grip, a barrel
    # that is clearly a long round tube (not a box), and wood visibly
    # running under it — the old version was one pale slab.
    along_y = Euler((math.radians(90), 0, 0))
    rake = Euler((math.radians(-16), 0, 0))

    mk.paint(bm, mk.add_box(bm, (0.038, 0.062, 0.016), loc=(0, -0.026, 0.008)), "bronze", uv)
    mk.paint(bm, mk.add_box(bm, (0.031, 0.052, 0.115), loc=(0, -0.010, 0.072), rot=rake), "oak", uv)
    mk.paint(bm, mk.add_box(bm, (0.033, 0.024, 0.016), loc=(0, -0.004, 0.100)), "lapis", uv)

    breech_z = 0.140
    mk.paint(bm, mk.add_box(bm, (0.030, 0.080, 0.046), loc=(0, 0.030, breech_z)), "iron", uv)

    barrel_z = breech_z + 0.012
    barrel_len = 0.215
    barrel_start = 0.045
    mk.paint(bm, mk.add_cylinder(
        bm, 0.0105, barrel_len, loc=(0, barrel_start + barrel_len / 2, barrel_z),
        rot=along_y, segments=10,
    ), "iron", uv)
    mk.paint(bm, mk.add_box(
        bm, (0.026, 0.150, 0.024), loc=(0, 0.110, barrel_z - 0.026),
    ), "oak", uv)
    mk.paint(bm, mk.add_cylinder(
        bm, 0.0135, 0.014, loc=(0, barrel_start + barrel_len - 0.012, barrel_z),
        rot=along_y, segments=10,
    ), "bronze", uv)

    mk.paint(bm, mk.add_box(bm, (0.013, 0.020, 0.040), loc=(0, 0.000, barrel_z + 0.030),
                            rot=Euler((math.radians(22), 0, 0))), "iron", uv)
    mk.paint(bm, mk.add_box(bm, (0.019, 0.016, 0.014), loc=(0, 0.026, barrel_z + 0.020)), "bronze", uv)
    mk.paint(bm, mk.add_box(bm, (0.011, 0.050, 0.010), loc=(0, 0.044, breech_z - 0.036)), "bronze", uv)
    mk.paint(bm, mk.add_box(bm, (0.008, 0.010, 0.020), loc=(0, 0.036, breech_z - 0.028)), "iron", uv)


def build_matchlock(bm, uv):
    # A shoulder arm laid along +Y, butt at the origin end. Previously a
    # bare stick; the gun read needs a butt that drops below the barrel
    # line, a fore-stock carrying a visibly round barrel, and the
    # serpentine with its lit match standing up off the lock.
    along_y = Euler((math.radians(90), 0, 0))
    barrel_z = 0.132

    mk.paint(bm, mk.add_box(bm, (0.060, 0.020, 0.120), loc=(0, 0.010, 0.060)), "iron", uv)
    mk.paint(bm, mk.add_box(bm, (0.056, 0.210, 0.115), loc=(0, 0.112, 0.0575)), "oak", uv)
    mk.paint(bm, mk.add_box(bm, (0.040, 0.150, 0.062), loc=(0, 0.275, 0.082)), "oak", uv)
    mk.paint(bm, mk.add_box(bm, (0.046, 0.440, 0.055), loc=(0, 0.565, 0.088)), "oak", uv)

    barrel_len = 0.700
    mk.paint(bm, mk.add_cylinder(
        bm, 0.020, barrel_len, loc=(0, 0.270 + barrel_len / 2, barrel_z),
        rot=along_y, segments=10,
    ), "iron", uv)
    mk.paint(bm, mk.add_cylinder(
        bm, 0.025, 0.016, loc=(0, 0.955, barrel_z), rot=along_y, segments=10,
    ), "bronze", uv)

    mk.paint(bm, mk.add_box(bm, (0.020, 0.028, 0.014), loc=(0, 0.318, barrel_z + 0.020)), "bronze", uv)
    mk.paint(bm, mk.add_box(bm, (0.012, 0.016, 0.056), loc=(0, 0.300, barrel_z + 0.050)), "iron", uv)
    mk.paint(bm, mk.add_cylinder(
        bm, 0.007, 0.022, loc=(0, 0.300, barrel_z + 0.088), segments=6,
    ), "madder", uv)
    mk.paint(bm, mk.add_box(bm, (0.012, 0.075, 0.016), loc=(0, 0.345, 0.055)), "iron", uv)
    mk.paint(bm, mk.add_box(bm, (0.009, 0.011, 0.022), loc=(0, 0.338, 0.068)), "iron", uv)


def build_pavise_shield(bm, uv):
    w, d, h = 0.5, 0.04, 0.9
    mk.paint(bm, mk.add_box(bm, (w, d, h), loc=(0, 0, h / 2)), "oak", uv)
    mk.paint(bm, mk.add_box(bm, (0.05, 0.02, h * 0.94), loc=(0, -d / 2 - 0.01, h / 2)), "iron", uv)
    mk.paint(bm, mk.add_box(bm, (w * 0.9, 0.006, 0.1), loc=(0, -d / 2 - 0.014, h * 0.62)), "madder", uv)


def build_plate_helm(bm, uv):
    # A great helm is a flat-topped drum, not a skull cap — the silhouette
    # is the whole read, so it's a straight-sided cylinder with the face
    # furniture (cross reinforce, ocularium, breaths) applied proud of the
    # front facets at a common depth so they sit flush with each other.
    drum_h = 0.235                # taller than it is wide, or it reads as a pail
    r_low, r_high = 0.104, 0.113
    # Eight sides, turned half a segment, so one flat facet squarely faces
    # -Y. The face furniture is applied to that panel; on a rounder drum
    # the bars ran off the curve and floated outside the silhouette.
    facet_front = Euler((0, 0, math.radians(22.5)))
    mk.paint(bm, mk.add_cylinder(
        bm, r_low, drum_h, loc=(0, 0, drum_h / 2), rot=facet_front, segments=8, radius2=r_high,
    ), "iron", uv)
    # skull plate: tapers in, so the top reads as part of the helm rather
    # than a lid sitting on a bucket. Overlaps down into the drum to keep
    # its lower ring off the drum's upper one (coincident rings = dupes).
    cap_h = 0.036
    mk.paint(bm, mk.add_cylinder(
        bm, r_high * 0.98, cap_h, loc=(0, 0, drum_h + cap_h / 2 - 0.008),
        rot=facet_front, segments=8, radius2=r_high * 0.66,
    ), "iron", uv)

    panel_y = -r_high * math.cos(math.radians(22.5))
    applied_d = 0.024                       # sits ~4mm proud of the panel
    applied_y = panel_y + applied_d / 2 - 0.004
    mk.paint(bm, mk.add_box(
        bm, (0.024, applied_d, drum_h * 0.99), loc=(0, applied_y, drum_h / 2),
    ), "verdigris", uv)
    for side in (-1, 1):
        # kept inside the front facet's half-width (r*sin 22.5°) so the
        # bars don't wrap the corner and float off the silhouette
        mk.paint(bm, mk.add_box(
            bm, (0.030, applied_d, 0.019), loc=(side * 0.027, applied_y, drum_h * 0.74),
        ), "bone_black", uv)
        mk.paint(bm, mk.add_box(
            bm, (0.022, applied_d, 0.010), loc=(side * 0.025, applied_y, drum_h * 0.48),
        ), "bone_black", uv)


def build_powder_grenade(bm, uv):
    body_r = 0.06
    mk.paint(bm, mk.add_sphere(bm, body_r, loc=(0, 0, body_r)), "bone_black", uv)
    collar_h = 0.02
    mk.paint(bm, mk.add_cylinder(bm, 0.015, collar_h, loc=(0, 0, body_r * 2 + collar_h / 2), segments=8), "bronze", uv)
    fuse_h = 0.05
    mk.paint(bm, mk.add_cylinder(
        bm, 0.006, fuse_h, loc=(0, 0, body_r * 2 + collar_h + fuse_h / 2), segments=6,
    ), "madder", uv)


def build_round_shield(bm, uv):
    radius = 0.35
    thickness = 0.03
    mk.paint(bm, mk.add_cylinder(
        bm, radius, thickness, loc=(0, 0, thickness / 2), segments=16,
    ), "oak", uv)
    boss_r = 0.07
    mk.paint(bm, mk.add_sphere(bm, boss_r, loc=(0, 0, thickness + boss_r * 0.5)), "bronze", uv)


def build_ancient_relic(bm, uv):
    base_h = 0.04
    mk.paint(bm, mk.add_box(bm, (0.22, 0.22, base_h), loc=(0, 0, base_h / 2)), "bone_black", uv)
    body_h = 0.34
    mk.paint(bm, mk.add_cylinder(
        bm, 0.09, body_h, loc=(0, 0, base_h + body_h / 2), segments=6, radius2=0.06,
    ), "bronze", uv)
    head_r = 0.06
    mk.paint(bm, mk.add_sphere(bm, head_r, loc=(0, 0, base_h + body_h + head_r)), "bronze", uv)
    mk.paint(bm, mk.add_box(bm, (0.03, 0.02, 0.03), loc=(0, -0.06, base_h + body_h * 0.7)), "orpiment", uv)


def build_copper_pot(bm, uv):
    body_h = 0.16
    mk.paint(bm, mk.add_cylinder(
        bm, 0.13, body_h, loc=(0, 0, body_h / 2), segments=12, radius2=0.11,
    ), "bronze", uv)
    rim_h = 0.015
    mk.paint(bm, mk.add_cylinder(bm, 0.135, rim_h, loc=(0, 0, body_h + rim_h / 2), segments=12), "bronze", uv)
    handle_z = body_h * 0.6
    mk.paint(bm, mk.add_box(bm, (0.04, 0.02, 0.015), loc=(0.15, 0, handle_z)), "iron", uv)
    mk.paint(bm, mk.add_box(bm, (0.04, 0.02, 0.015), loc=(-0.15, 0, handle_z)), "iron", uv)


def build_golden_goblet(bm, uv):
    base_r = 0.05
    base_h = 0.015
    mk.paint(bm, mk.add_cylinder(bm, base_r, base_h, loc=(0, 0, base_h / 2), segments=10), "orpiment", uv)
    stem_h = 0.08
    mk.paint(bm, mk.add_cylinder(bm, 0.012, stem_h, loc=(0, 0, base_h + stem_h / 2), segments=8), "orpiment", uv)
    bowl_h = 0.09
    bowl_z = base_h + stem_h
    mk.paint(bm, mk.add_cylinder(
        bm, 0.02, bowl_h, loc=(0, 0, bowl_z + bowl_h / 2), segments=10, radius2=0.055,
    ), "orpiment", uv)


def build_heavy_chest(bm, uv):
    w, d, h = 0.5, 0.32, 0.28
    mk.paint(bm, mk.add_box(bm, (w, d, h), loc=(0, 0, h / 2)), "oak", uv)
    lid_h = 0.10
    mk.paint(bm, mk.add_box(bm, (w * 1.02, d * 1.05, lid_h), loc=(0, 0, h + lid_h / 2)), "oak", uv)
    mk.paint(bm, mk.add_box(bm, (w * 1.02, 0.03, h + lid_h), loc=(0, 0, (h + lid_h) / 2)), "iron", uv)
    mk.paint(bm, mk.add_box(bm, (0.05, 0.02, 0.05), loc=(0, -d / 2 - 0.01, h - 0.02)), "orpiment", uv)


def build_silver_plate(bm, uv):
    # Traced from the underside centre, out along the bottom, over the
    # raised lip, then back down into the well — so the plate actually
    # dishes instead of reading as two stacked discs.
    profile = [
        (0.006, 0.000),
        (0.120, 0.012),
        (0.152, 0.030),
        (0.156, 0.036),
        (0.148, 0.038),
        (0.115, 0.030),
        (0.050, 0.010),
        (0.006, 0.007),
    ]
    mk.paint(bm, mk.add_lathe(bm, profile, segments=16), "iron", uv)
