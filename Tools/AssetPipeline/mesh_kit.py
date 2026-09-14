"""
Small bmesh helper kit shared by every Plunderspell prop builder.

Runs only inside Blender's Python (imports bpy/bmesh/mathutils). Keeps the
per-asset builder functions in builders.py short: build a part with a
primitive helper, immediately tag it with a pigment, move on.
"""
import bmesh
import bpy
from mathutils import Matrix, Vector

import palette as pal


def new_bmesh():
    return bmesh.new()


def _mat(loc=(0, 0, 0), rot=None, scale=(1, 1, 1)):
    t = Matrix.Translation(Vector(loc))
    r = rot.to_matrix().to_4x4() if rot is not None else Matrix.Identity(4)
    s = Matrix.Diagonal(Vector((*scale, 1)))
    return t @ r @ s


def add_box(bm, size, loc=(0, 0, 0), rot=None):
    """Axis-aligned box. size = (x, y, z) full extents."""
    ret = bmesh.ops.create_cube(bm, size=1.0)
    verts = ret["verts"]
    sx, sy, sz = size
    for v in verts:
        v.co.x *= sx
        v.co.y *= sy
        v.co.z *= sz
    m = _mat(loc, rot, (1, 1, 1))
    bmesh.ops.transform(bm, matrix=m, verts=verts)
    faces = list({f for v in verts for f in v.link_faces})
    return faces


def add_cylinder(bm, radius, depth, loc=(0, 0, 0), rot=None, segments=10,
                  radius2=None, cap_ends=True, scale=(1, 1, 1)):
    """Cylinder/cone along local Z, then placed by loc/rot. `scale` squashes
    the cross-section (e.g. thinning a blade) before rot/loc are applied."""
    ret = bmesh.ops.create_cone(
        bm, cap_ends=cap_ends, cap_tris=False, segments=segments,
        radius1=radius, radius2=radius2 if radius2 is not None else radius,
        depth=depth,
    )
    verts = ret["verts"]
    m = _mat(loc, rot, scale)
    bmesh.ops.transform(bm, matrix=m, verts=verts)
    faces = list({f for v in verts for f in v.link_faces})
    return faces


def add_sphere(bm, radius, loc=(0, 0, 0), segments=12, rings=8, scale=(1, 1, 1)):
    ret = bmesh.ops.create_uvsphere(
        bm, u_segments=segments, v_segments=rings, radius=radius,
    )
    verts = ret["verts"]
    m = _mat(loc, None, scale)
    bmesh.ops.transform(bm, matrix=m, verts=verts)
    faces = list({f for v in verts for f in v.link_faces})
    return faces


def paint(bm, faces, pigment_name, uv_layer, inset=0.12):
    """Tag faces with a flat pigment: solid material colour + a small UV
    footprint inset inside that pigment's palette-atlas cell.

    Multiple parts intentionally sample the same cell (a flat colour has no
    detail to tile, so this is the palette-swatch equivalent of mirroring —
    harmless reuse, not an accidental overlap). What must never happen is a
    face's UV bleeding into a *different* pigment's cell; validate_in_blender
    checks exactly that invariant.
    """
    col, row = pal.swatch_cell(pigment_name)
    grid = pal.PALETTE_GRID
    u0 = (col + inset) / grid
    u1 = (col + 1 - inset) / grid
    v0 = (row + inset) / grid
    v1 = (row + 1 - inset) / grid
    corners = [(u0, v0), (u1, v0), (u1, v1), (u0, v1)]

    mat_index = _material_slot_for(pigment_name)
    for f in faces:
        f.material_index = mat_index
        n = len(f.loops)
        for i, loop in enumerate(f.loops):
            loop[uv_layer].uv = corners[i % 4] if n <= 4 else _cycle(corners, i, n)


def _cycle(corners, i, n):
    # fan out a polygon with >4 verts across the same inset square
    t = (i / max(n - 1, 1))
    lo, hi = corners[0], corners[2]
    return (lo[0] + (hi[0] - lo[0]) * t, lo[1] + (hi[1] - lo[1]) * t)


_MATERIAL_ORDER: list[str] = []


def _material_slot_for(pigment_name: str) -> int:
    if pigment_name not in _MATERIAL_ORDER:
        _MATERIAL_ORDER.append(pigment_name)
    return _MATERIAL_ORDER.index(pigment_name)


def used_pigments() -> list[str]:
    return list(_MATERIAL_ORDER)


def reset_material_order():
    _MATERIAL_ORDER.clear()


def finalize_to_object(bm, name: str, pigments_used: list[str], palette_image_path: str):
    """bmesh -> a real Object with one material per used pigment, all sharing
    the single palette texture (that's what keeps material count to 1 image
    reference no matter how many pigments a prop uses)."""
    bmesh.ops.recalc_face_normals(bm, faces=bm.faces)
    mesh = bpy.data.meshes.new(name + "Mesh")
    bm.to_mesh(mesh)
    bm.free()

    obj = bpy.data.objects.new(name, mesh)
    bpy.context.collection.objects.link(obj)

    img = _get_or_load_palette_image(palette_image_path)
    for pigment in pigments_used:
        mat = bpy.data.materials.new(f"{name}_{pigment}")
        mat.use_nodes = True
        bsdf = mat.node_tree.nodes.get("Principled BSDF")
        tex_node = mat.node_tree.nodes.new("ShaderNodeTexImage")
        tex_node.image = img
        tex_node.interpolation = "Closest"
        mat.node_tree.links.new(bsdf.inputs["Base Color"], tex_node.outputs["Color"])
        mat.node_tree.nodes.active = tex_node
        mesh.materials.append(mat)

    return obj


_PALETTE_IMAGE_CACHE: dict[str, "bpy.types.Image"] = {}


def _get_or_load_palette_image(path: str):
    if path not in _PALETTE_IMAGE_CACHE:
        img = bpy.data.images.load(path, check_existing=True)
        _PALETTE_IMAGE_CACHE[path] = img
    return _PALETTE_IMAGE_CACHE[path]


def apply_all_transforms(obj):
    bpy.context.view_layer.objects.active = obj
    for o in bpy.context.selected_objects:
        o.select_set(False)
    obj.select_set(True)
    bpy.ops.object.transform_apply(location=False, rotation=True, scale=True)


def set_origin_to_base(obj):
    """Pivot at the object's local (0,0,0), which every builder already
    treats as the prop's handhold/base — so this just re-zeroes the mesh
    if construction drifted, then re-applies location."""
    bpy.context.view_layer.objects.active = obj
    for o in bpy.context.selected_objects:
        o.select_set(False)
    obj.select_set(True)
    obj.location = (0, 0, 0)
    bpy.ops.object.transform_apply(location=True, rotation=False, scale=False)
