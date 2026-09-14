"""
Render a turntable-style preview screenshot of every exported prop.

    blender -b -P Tools/AssetPipeline/render_previews.py

Imports each FBX exactly as it was exported (so what you see is what
Unity will actually import — embedded palette texture included), frames it
with a camera sized to its own bounding box, lights it with a simple
three-point rig, and renders one PNG per asset into
Tools/AssetPipeline/previews/. Also stitches all 13 into one contact sheet
(previews/_contact_sheet.png) for a quick side-by-side look.
"""
import math
import os
import sys

import bpy
from mathutils import Vector

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import asset_specs  # noqa: E402
import palette as pal  # noqa: E402

REPO_ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
MODELS_ROOT = os.path.join(REPO_ROOT, "Assets", "_Project", "Art", "Models")
OUT_DIR = os.path.join(os.path.dirname(__file__), "previews")
RES = 640
LENS_MM = 50.0
SENSOR_MM = 36.0

# Lights sit at multiples of the object's radius, so their power scales with
# radius² — otherwise a goblet renders blown out and a pavise renders murky.
KEY_WATTS = 300.0
FILL_WATTS = 80.0
RIM_WATTS = 130.0


def clear_scene():
    bpy.ops.wm.read_factory_settings(use_empty=True)


def setup_world_background():
    world = bpy.data.worlds.new("PreviewWorld")
    world.use_nodes = True
    bpy.context.scene.world = world
    r, g, b = pal.hex_to_rgb01(pal.PIGMENTS["bone_black"])
    world.node_tree.nodes["Background"].inputs[0].default_value = (r, g, b, 1.0)
    world.node_tree.nodes["Background"].inputs[1].default_value = 1.0


def import_asset(spec) -> "bpy.types.Object":
    path = os.path.join(MODELS_ROOT, spec["subdir"], f"{spec['key']}.fbx")
    existing = set(bpy.data.objects.keys())
    bpy.ops.import_scene.fbx(filepath=path)
    imported = [o for o in bpy.data.objects if o.name not in existing and o.type == "MESH"]
    return imported[0]


def frame_and_light(obj):
    for o in list(bpy.data.objects):
        if o.type in {"CAMERA", "LIGHT"}:
            bpy.data.objects.remove(o, do_unlink=True)

    bbox = [obj.matrix_world @ v.co for v in obj.data.vertices]
    xs, ys, zs = [p.x for p in bbox], [p.y for p in bbox], [p.z for p in bbox]
    center = ((min(xs) + max(xs)) / 2, (min(ys) + max(ys)) / 2, (min(zs) + max(zs)) / 2)

    # Frame off the bounding *sphere*, not the widest axis: seen from a
    # three-quarter angle an object presents its diagonal, so half the
    # longest axis under-estimates the distance needed and crops the shot.
    center_v = Vector(center)
    radius = max(max((p - center_v).length for p in bbox), 0.03)

    half_fov = math.atan((SENSOR_MM / 2) / LENS_MM)
    cam_dist = radius / math.tan(half_fov) * 1.12
    cam_dir = (1.0, -1.3, 0.85)
    cam_len = math.sqrt(sum(c * c for c in cam_dir))
    cam_pos = tuple(center[i] + cam_dir[i] / cam_len * cam_dist for i in range(3))

    cam_data = bpy.data.cameras.new("PreviewCam")
    cam_data.lens = LENS_MM
    cam_obj = bpy.data.objects.new("PreviewCam", cam_data)
    bpy.context.collection.objects.link(cam_obj)
    cam_obj.location = cam_pos

    direction = (center[0] - cam_pos[0], center[1] - cam_pos[1], center[2] - cam_pos[2])
    cam_obj.rotation_euler = _look_at(direction)
    bpy.context.scene.camera = cam_obj

    key = bpy.data.lights.new("Key", type="AREA")
    key.energy = KEY_WATTS * radius ** 2
    key.size = radius * 3
    key_obj = bpy.data.objects.new("Key", key)
    bpy.context.collection.objects.link(key_obj)
    key_obj.location = (center[0] + radius * 2, center[1] - radius * 2.5, center[2] + radius * 3)
    key_obj.rotation_euler = _look_at((center[0] - key_obj.location[0],
                                        center[1] - key_obj.location[1],
                                        center[2] - key_obj.location[2]))

    fill = bpy.data.lights.new("Fill", type="AREA")
    fill.energy = FILL_WATTS * radius ** 2
    fill.size = radius * 3
    fill.color = pal.hex_to_rgb01(pal.PIGMENTS["verdigris"])
    fill_obj = bpy.data.objects.new("Fill", fill)
    bpy.context.collection.objects.link(fill_obj)
    fill_obj.location = (center[0] - radius * 2.5, center[1] - radius * 1.5, center[2] + radius)
    fill_obj.rotation_euler = _look_at((center[0] - fill_obj.location[0],
                                         center[1] - fill_obj.location[1],
                                         center[2] - fill_obj.location[2]))

    rim = bpy.data.lights.new("Rim", type="AREA")
    rim.energy = RIM_WATTS * radius ** 2
    rim.size = radius * 2
    rim.color = pal.hex_to_rgb01(pal.PIGMENTS["orpiment"])
    rim_obj = bpy.data.objects.new("Rim", rim)
    bpy.context.collection.objects.link(rim_obj)
    rim_obj.location = (center[0] - radius * 1.5, center[1] + radius * 3, center[2] + radius * 1.5)
    rim_obj.rotation_euler = _look_at((center[0] - rim_obj.location[0],
                                        center[1] - rim_obj.location[1],
                                        center[2] - rim_obj.location[2]))


def _look_at(direction) -> tuple:
    import mathutils
    d = mathutils.Vector(direction).normalized()
    rot_quat = d.to_track_quat("-Z", "Y")
    return rot_quat.to_euler()


def render(spec):
    os.makedirs(OUT_DIR, exist_ok=True)
    out_path = os.path.join(OUT_DIR, f"{spec['key']}.png")
    scene = bpy.context.scene
    scene.render.engine = "CYCLES"
    scene.cycles.device = "CPU"
    scene.cycles.samples = 32
    scene.cycles.use_denoising = False
    scene.view_layers[0].cycles.use_denoising = False
    scene.render.resolution_x = RES
    scene.render.resolution_y = RES
    scene.render.image_settings.file_format = "PNG"
    scene.render.filepath = out_path
    bpy.ops.render.render(write_still=True)
    return out_path


def main():
    clear_scene()
    setup_world_background()
    paths = []
    for spec in asset_specs.ALL_SPECS:
        clear_scene()
        setup_world_background()
        obj = import_asset(spec)
        frame_and_light(obj)
        path = render(spec)
        paths.append(path)
        print(f"rendered {spec['key']} -> {path}")

    print("renders complete; run make_contact_sheet.py (plain python3) to build the sheet")


if __name__ == "__main__":
    main()
