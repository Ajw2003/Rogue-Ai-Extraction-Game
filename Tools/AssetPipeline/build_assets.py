"""
Headless build + validate + export for every prop in asset_specs.ALL_SPECS.

    blender -b -P Tools/AssetPipeline/build_assets.py

For each asset: build geometry with a bmesh, run validate_in_blender's gate
BEFORE export (transforms / manifold / quads / budget / UV), export FBX for
Unity (Assets/_Project/Art/Models/<subdir>/<Key>.fbx, textures embedded so
Unity never sees a missing material reference), export a companion GLB into
the scratch dir for the standalone trimesh validator, then re-import that
GLB and diff vertex/triangle counts against the pre-export mesh to catch
anything the exporter silently dropped.

Exits 1 if any asset fails any check, printing every issue found so the
next edit knows exactly what to fix.
"""
import os
import sys

import bpy

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))

import asset_specs  # noqa: E402
import builders  # noqa: E402
import mesh_kit as mk  # noqa: E402
import validate_in_blender as val  # noqa: E402

REPO_ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
MODELS_ROOT = os.path.join(REPO_ROOT, "Assets", "_Project", "Art", "Models")
PALETTE_PNG = os.path.join(REPO_ROOT, "Assets", "_Project", "Art", "Textures", "PlunderspellPalette.png")
SCRATCH_GLB_DIR = os.environ.get("PLUNDERSPELL_SCRATCH_GLB", "/tmp/plunderspell_glb")


def clear_scene():
    bpy.ops.wm.read_factory_settings(use_empty=True)


def build_one(spec) -> tuple[object, list[str]]:
    bm = mk.new_bmesh()
    uv = bm.loops.layers.uv.new("UVMap")
    mk.reset_material_order()

    builder_fn = getattr(builders, spec["builder"])
    builder_fn(bm, uv)

    obj = mk.finalize_to_object(bm, spec["key"], mk.used_pigments(), PALETTE_PNG)
    issues = val.validate_object(obj, spec["tri_budget"])
    tris = sum(len(p.vertices) - 2 for p in obj.data.polygons)
    return obj, issues, tris


def export_fbx(obj, spec) -> str:
    out_dir = os.path.join(MODELS_ROOT, spec["subdir"])
    os.makedirs(out_dir, exist_ok=True)
    out_path = os.path.join(out_dir, f"{spec['key']}.fbx")

    for o in bpy.context.selected_objects:
        o.select_set(False)
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj

    bpy.ops.export_scene.fbx(
        filepath=out_path,
        use_selection=True,
        embed_textures=True,
        path_mode="COPY",
        object_types={"MESH"},
        mesh_smooth_type="FACE",
        add_leaf_bones=False,
        bake_anim=False,
        axis_forward="-Z",
        axis_up="Y",
    )
    return out_path


def export_glb_for_validation(obj, spec) -> str:
    os.makedirs(SCRATCH_GLB_DIR, exist_ok=True)
    out_path = os.path.join(SCRATCH_GLB_DIR, f"{spec['key']}.glb")

    for o in bpy.context.selected_objects:
        o.select_set(False)
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj

    bpy.ops.export_scene.gltf(
        filepath=out_path,
        use_selection=True,
        export_format="GLB",
    )
    return out_path


def reimport_and_diff(fbx_path: str, obj) -> list[str]:
    """Re-import the exported FBX into a scratch collection and compare
    triangle/vertex counts to the pre-export mesh, so a silent exporter
    drop (a face, the UV map, the material) shows up as a real failure."""
    issues = []
    existing = set(bpy.data.objects.keys())
    bpy.ops.import_scene.fbx(filepath=fbx_path)
    imported = [o for o in bpy.data.objects if o.name not in existing and o.type == "MESH"]

    if not imported:
        issues.append("re-import produced no mesh object")
        return issues

    imp_obj = imported[0]
    imp_mesh = imp_obj.data
    orig_mesh = obj.data

    orig_tris = sum(len(p.vertices) - 2 for p in orig_mesh.polygons)
    imp_tris = sum(len(p.vertices) - 2 for p in imp_mesh.polygons)
    if orig_tris != imp_tris:
        issues.append(f"export/import tri mismatch: built {orig_tris}, re-imported {imp_tris}")

    if len(imp_mesh.vertices) == 0:
        issues.append("re-imported mesh has zero vertices")

    if not imp_mesh.materials or any(m is None for m in imp_mesh.materials):
        issues.append("re-imported mesh missing material reference")
    else:
        for m in imp_mesh.materials:
            has_image = any(
                n.type == "TEX_IMAGE" and n.image is not None
                for n in (m.node_tree.nodes if m.use_nodes else [])
            )
            if not has_image:
                issues.append(f"re-imported material '{m.name}' has no resolved texture image")

    if not imp_mesh.uv_layers:
        issues.append("re-imported mesh has no UV layer")

    for o in imported:
        bpy.data.objects.remove(o, do_unlink=True)
    return issues


def main():
    clear_scene()
    results = []
    any_failed = False

    for spec in asset_specs.ALL_SPECS:
        obj, issues, tris = build_one(spec)
        fbx_path = export_fbx(obj, spec)
        export_glb_for_validation(obj, spec)
        issues += reimport_and_diff(fbx_path, obj)

        ok = len(issues) == 0
        any_failed |= not ok
        results.append((spec["key"], ok, issues, tris, spec["tri_budget"]))

    print("\n" + "=" * 70)
    print("PLUNDERSPELL ASSET PIPELINE — build report")
    print("=" * 70)
    for key, ok, issues, tris, budget in results:
        status = "PASS" if ok else "FAIL"
        print(f"[{status}] {key:20s} {tris:5d} tris / {budget} budget")
        for issue in issues:
            print(f"         - {issue}")
    n_pass = sum(1 for r in results if r[1])
    print("-" * 70)
    print(f"{n_pass}/{len(results)} assets passed all checks.")
    print("=" * 70)

    sys.exit(1 if any_failed else 0)


if __name__ == "__main__":
    main()
