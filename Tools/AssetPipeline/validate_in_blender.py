"""
In-Blender validation gate. Runs with full bmesh/mesh access right after a
prop is built (and again, re-imported, right after export) so every check
from the directive's "AUTOMATED 3D VALIDATION CHECKS" list has real geometry
to inspect rather than guessing from a re-parsed file.

Returns a list of human-readable issue strings; empty list == pass.
"""
import math

import bmesh
import palette as pal

QUAD_DOMINANT_MIN = 0.75
UV_EPS = 1e-4
BASE_PIVOT_TOLERANCE = 0.03  # metres of slack for "pivot sits at the base"


def validate_object(obj, tri_budget: int) -> list[str]:
    issues = []

    # ── transforms ──────────────────────────────────────────────
    loc, rot, scale = obj.location, obj.rotation_euler, obj.scale
    if any(abs(c) > 1e-6 for c in loc):
        issues.append(f"pivot not at origin: location={tuple(loc)}")
    if any(abs(c) > 1e-6 for c in rot):
        issues.append(f"rotation not reset: euler={tuple(rot)}")
    if any(abs(c - 1.0) > 1e-6 for c in scale):
        issues.append(f"scale not applied: scale={tuple(scale)}")

    bm = bmesh.new()
    bm.from_mesh(obj.data)
    bm.faces.ensure_lookup_table()
    bm.edges.ensure_lookup_table()
    bm.verts.ensure_lookup_table()

    # ── transforms: pivot sits at the prop's base (every builder is
    #    authored bottom-flush at local Z=0, so this is a real invariant,
    #    not just "object.location == origin") ─────────────────────
    min_z = min((v.co.z for v in bm.verts), default=0.0)
    if abs(min_z) > BASE_PIVOT_TOLERANCE:
        issues.append(f"pivot not at base: lowest vertex Z={min_z:.4f} (want ~0)")

    # ── geometry: manifold ──────────────────────────────────────
    non_manifold = [e for e in bm.edges if not e.is_manifold]
    if non_manifold:
        issues.append(f"{len(non_manifold)} non-manifold edges")

    # ── geometry: isolated / duplicate verts ───────────────────
    isolated = [v for v in bm.verts if not v.link_faces]
    if isolated:
        issues.append(f"{len(isolated)} isolated vertices")
    dupes = _duplicate_vert_count(bm)
    if dupes:
        issues.append(f"{dupes} duplicate-position vertices")

    # ── geometry: quad-dominant ─────────────────────────────────
    n_faces = len(bm.faces)
    n_quads = sum(1 for f in bm.faces if len(f.verts) == 4)
    quad_ratio = (n_quads / n_faces) if n_faces else 0.0
    if quad_ratio < QUAD_DOMINANT_MIN:
        issues.append(f"not quad-dominant: {quad_ratio:.0%} quads (need >={QUAD_DOMINANT_MIN:.0%})")

    # ── budget: triangle count ──────────────────────────────────
    tri_count = sum(len(f.verts) - 2 for f in bm.faces)
    if tri_count > tri_budget:
        issues.append(f"over budget: {tri_count} tris > {tri_budget}")

    # ── UV: unwrapped, in-bounds, one-pigment-cell-per-face ─────
    uv_layer = bm.loops.layers.uv.active
    if uv_layer is None:
        issues.append("no active UV layer")
    else:
        grid = pal.PALETTE_GRID
        for f in bm.faces:
            us = [loop[uv_layer].uv.x for loop in f.loops]
            vs = [loop[uv_layer].uv.y for loop in f.loops]
            lo_u, hi_u, lo_v, hi_v = min(us), max(us), min(vs), max(vs)
            if lo_u < -UV_EPS or hi_u > 1 + UV_EPS or lo_v < -UV_EPS or hi_v > 1 + UV_EPS:
                issues.append(f"face {f.index} UV outside 0-1: u[{lo_u:.3f},{hi_u:.3f}] v[{lo_v:.3f},{hi_v:.3f}]")
                continue
            cell_u0, cell_v0 = math.floor(lo_u * grid), math.floor(lo_v * grid)
            cell_u1, cell_v1 = math.floor(min(hi_u * grid, grid - UV_EPS)), math.floor(min(hi_v * grid, grid - UV_EPS))
            if (cell_u0, cell_v0) != (cell_u1, cell_v1):
                issues.append(f"face {f.index} UV straddles two palette cells (bleed)")

    bm.free()
    return issues


def _duplicate_vert_count(bm, dist=1e-5) -> int:
    seen = {}
    dupes = 0
    for v in bm.verts:
        key = (round(v.co.x / dist), round(v.co.y / dist), round(v.co.z / dist))
        # only counts as a true duplicate if verts are coincident AND
        # unconnected (a shared-position vert with an edge between them is
        # legitimate topology, not a duplicate)
        bucket = seen.setdefault(key, [])
        for other in bucket:
            if other not in {e.other_vert(v) for e in v.link_edges}:
                dupes += 1
                break
        bucket.append(v)
    return dupes
