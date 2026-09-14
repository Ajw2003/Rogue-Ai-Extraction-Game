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
    tilt = Euler((math.radians(-12), 0, 0))
    mk.paint(bm, mk.add_box(bm, (0.03, 0.05, 0.11), loc=(0, -0.015, 0.055), rot=tilt), "oak", uv)
    mk.paint(bm, mk.add_box(bm, (0.03, 0.09, 0.035), loc=(0, 0.02, 0.11 + 0.0175)), "iron", uv)
    frame_top = 0.11 + 0.035
    barrel_len = 0.16
    mk.paint(bm, mk.add_cylinder(
        bm, 0.012, barrel_len, loc=(0, 0.02 + barrel_len / 2, frame_top - 0.01),
        rot=Euler((math.radians(90), 0, 0)), segments=8,
    ), "iron", uv)
    mk.paint(bm, mk.add_box(bm, (0.014, 0.018, 0.03), loc=(0, 0.0, frame_top + 0.005)), "lapis", uv)


def build_matchlock(bm, uv):
    stock_len = 0.5
    mk.paint(bm, mk.add_box(bm, (0.04, stock_len, 0.05), loc=(0, stock_len / 2, 0.025)), "oak", uv)
    barrel_len = 0.5
    barrel_start = stock_len
    mk.paint(bm, mk.add_cylinder(
        bm, 0.016, barrel_len, loc=(0, barrel_start + barrel_len / 2, 0.04),
        rot=Euler((math.radians(90), 0, 0)), segments=8,
    ), "iron", uv)
    mk.paint(bm, mk.add_box(bm, (0.02, 0.03, 0.045), loc=(0, stock_len * 0.75, 0.06)), "madder", uv)


def build_pavise_shield(bm, uv):
    w, d, h = 0.5, 0.04, 0.9
    mk.paint(bm, mk.add_box(bm, (w, d, h), loc=(0, 0, h / 2)), "oak", uv)
    mk.paint(bm, mk.add_box(bm, (0.05, 0.02, h * 0.94), loc=(0, -d / 2 - 0.01, h / 2)), "iron", uv)
    mk.paint(bm, mk.add_box(bm, (w * 0.9, 0.006, 0.1), loc=(0, -d / 2 - 0.014, h * 0.62)), "madder", uv)


def build_plate_helm(bm, uv):
    skull_r = 0.13
    mk.paint(bm, mk.add_sphere(bm, skull_r, loc=(0, 0, skull_r), scale=(1, 1, 1.04), segments=12, rings=8), "iron", uv)
    mk.paint(bm, mk.add_box(bm, (0.14, 0.05, 0.16), loc=(0, -0.10, skull_r * 0.8)), "iron", uv)
    mk.paint(bm, mk.add_box(bm, (0.02, 0.20, 0.03), loc=(0, 0, skull_r * 2 + 0.005)), "verdigris", uv)


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
    radius = 0.14
    thickness = 0.015
    mk.paint(bm, mk.add_cylinder(
        bm, radius, thickness, loc=(0, 0, thickness / 2), segments=16,
    ), "iron", uv)
    medallion_r = radius * 0.5
    medallion_t = thickness * 0.6
    mk.paint(bm, mk.add_cylinder(
        bm, medallion_r, medallion_t, loc=(0, 0, thickness + medallion_t / 2), segments=16,
    ), "vellum", uv)
