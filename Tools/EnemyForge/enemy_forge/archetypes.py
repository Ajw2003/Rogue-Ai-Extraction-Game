"""The Plunderspell enemy roster — the High Medieval household, c. 1250.

Plunderspell is a co-operative heist: broke wizards open a portal into the past and
rob castles. The enemies are not monsters, they are a sleeping household that wakes
in stages when you make noise. This roster is built on the alarm ladder the game
already implements in `AlarmState` — Calm, Stirred, Roused, Hue & Cry — so each
archetype is the answer to "who turns up at this stage".

Colour follows docs/plunderspell-moodboard.html, which spends orpiment on money and
nothing else. No member of the household wears gold. The only saturated colour on a
guard is his lantern flame, and the only lapis in the roster is in the crypt, because
a word raised it.

All measurements are metres in Unity scale. Models stand on z = 0, centred on
x = y = 0, and face -Y (which Blender's FBX exporter maps to Unity's +Z forward).
`CastleGuard.cs` reads from an eye height of 1.6 m, which is what sets the scale.
"""

from __future__ import annotations

import math
from dataclasses import dataclass, field

from .parts import Part


@dataclass
class Archetype:
    name: str
    role: str
    height: float
    parts: list[Part]
    bones: list[dict]
    tri_budget: int
    bevel: float = 0.008
    wear: float = 1.0
    grounded: bool = True
    # Only an arcane archetype may carry the voice's colour; materials.py enforces it.
    arcane: bool = False

    @property
    def families(self) -> set[str]:
        return {part.mat for part in self.parts}


def ring(count: int, radius: float, z: float, start_deg: float = 0.0, **part_kwargs):
    """Repeat a part evenly around the Z axis, yawed to face outward."""
    base_loc = part_kwargs.pop("loc", (0.0, 0.0, 0.0))
    base_rot = part_kwargs.pop("rot", (0.0, 0.0, 0.0))
    out = []
    for i in range(count):
        yaw = start_deg + 360.0 * i / count
        theta = math.radians(yaw)
        out.append(Part(
            loc=(base_loc[0] + math.cos(theta) * radius,
                 base_loc[1] + math.sin(theta) * radius,
                 base_loc[2] + z),
            rot=(base_rot[0], base_rot[1], base_rot[2] + yaw),
            **part_kwargs,
        ))
    return out


# --------------------------------------------------------------------------------
# Shared human proportions
#
# Fractions of total height, so every figure in the roster is the same man at a
# different size and build. Keeping one source of proportions is what makes the
# household read as one family rather than five unrelated models.
# --------------------------------------------------------------------------------

ANKLE, KNEE, HIP = 0.050, 0.278, 0.528
WAIST, CHEST, SHOULDER, NECK = 0.590, 0.700, 0.812, 0.826
SHOULDER_X, HIP_X = 0.106, 0.056


def humanoid_bones(h: float) -> list[dict]:
    """The standard biped rig, scaled to a figure `h` metres tall."""
    return [
        dict(name="Root", head=(0, 0, 0), tail=(0, 0, 0.20 * h)),
        dict(name="Hips", head=(0, 0, HIP * h), tail=(0, 0, WAIST * h), parent="Root"),
        dict(name="Spine", head=(0, 0, WAIST * h), tail=(0, 0, CHEST * h), parent="Hips"),
        dict(name="Chest", head=(0, 0, CHEST * h), tail=(0, 0, NECK * h), parent="Spine"),
        dict(name="Head", head=(0, 0, NECK * h), tail=(0, 0, 0.98 * h), parent="Chest"),
        dict(name="Shoulder.L", head=(0.035 * h, 0, SHOULDER * h),
             tail=(SHOULDER_X * h, 0, SHOULDER * h), parent="Chest", mirror=True),
        dict(name="UpperArm.L", head=(SHOULDER_X * h, 0, SHOULDER * h),
             tail=(SHOULDER_X * h, 0, 0.640 * h), parent="Shoulder.L", mirror=True),
        dict(name="Forearm.L", head=(SHOULDER_X * h, 0, 0.640 * h),
             tail=(SHOULDER_X * h, 0, 0.500 * h), parent="UpperArm.L", mirror=True),
        dict(name="Hand.L", head=(SHOULDER_X * h, 0, 0.500 * h),
             tail=(SHOULDER_X * h, 0, 0.430 * h), parent="Forearm.L", mirror=True),
        dict(name="Thigh.L", head=(HIP_X * h, 0, HIP * h),
             tail=(HIP_X * h, 0, KNEE * h), parent="Hips", mirror=True),
        dict(name="Shin.L", head=(HIP_X * h, 0, KNEE * h),
             tail=(HIP_X * h, 0, ANKLE * h), parent="Thigh.L", mirror=True),
        dict(name="Foot.L", head=(HIP_X * h, 0, ANKLE * h),
             tail=(HIP_X * h, -0.09 * h, 0.02 * h), parent="Shin.L", mirror=True),
    ]


def humanoid_body(h: float, torso: str = "wool", legs: str = "wool",
                  arms: str = "linen", bulk: float = 1.0) -> list[Part]:
    """The bare figure every household member is built on.

    `bulk` widens the torso and limbs for heavier men without changing height, so a
    sergeant in plate reads as the same species as the watchman in a tunic.
    """
    b = bulk
    return [
        # Feet and legs
        Part("box", (HIP_X * h, -0.035 * h, 0.025 * h),
             (0.058 * h * b, 0.145 * h, 0.05 * h), "leather", "Foot.L", mirror=True),
        Part("cyl", (HIP_X * h, 0, 0.165 * h),
             (0.085 * h * b, 0.085 * h * b, 0.228 * h), legs, "Shin.L",
             mirror=True, segments=10, taper=1.25),
        Part("sphere", (HIP_X * h, 0, KNEE * h),
             (0.095 * h * b, 0.095 * h * b, 0.085 * h), legs, "Shin.L",
             mirror=True, segments=10, rings=7),
        Part("cyl", (HIP_X * h, 0, 0.403 * h),
             (0.115 * h * b, 0.115 * h * b, 0.250 * h), legs, "Thigh.L",
             mirror=True, segments=10, taper=0.82),

        # Hips and torso
        Part("box", (0, 0, 0.556 * h), (0.200 * h * b, 0.115 * h * b, 0.075 * h),
             torso, "Hips"),
        Part("cyl", (0, 0, 0.640 * h), (0.190 * h * b, 0.120 * h * b, 0.110 * h),
             torso, "Spine", segments=12, taper=1.12),
        Part("cyl", (0, 0, 0.757 * h), (0.225 * h * b, 0.135 * h * b, 0.130 * h),
             torso, "Chest", segments=12, taper=0.92),

        # Neck and head
        Part("cyl", (0, 0, 0.845 * h), (0.060 * h, 0.060 * h, 0.048 * h),
             "flesh", "Head", segments=10),
        Part("sphere", (0, -0.004 * h, 0.905 * h), (0.098 * h, 0.112 * h, 0.125 * h),
             "flesh", "Head", segments=14, rings=10),

        # Arms
        Part("sphere", (SHOULDER_X * h, 0, SHOULDER * h),
             (0.082 * h * b, 0.088 * h * b, 0.076 * h), torso, "Shoulder.L",
             mirror=True, segments=10, rings=7),
        Part("cyl", (SHOULDER_X * h, 0, 0.726 * h),
             (0.068 * h * b, 0.068 * h * b, 0.165 * h), arms, "UpperArm.L",
             mirror=True, segments=10, taper=0.88),
        Part("cyl", (SHOULDER_X * h, 0, 0.572 * h),
             (0.058 * h * b, 0.058 * h * b, 0.145 * h), arms, "Forearm.L",
             mirror=True, segments=10, taper=0.86),
        Part("box", (SHOULDER_X * h, -0.008 * h, 0.463 * h),
             (0.042 * h, 0.055 * h, 0.075 * h), "flesh", "Hand.L", mirror=True),
    ]


# --------------------------------------------------------------------------------
# Watchman — Calm. Walks the patrol routes. Unarmoured, and carrying the only
# light in the room, which is what makes him avoidable rather than dangerous.
# --------------------------------------------------------------------------------

def _watchman() -> Archetype:
    h = 1.75
    parts = humanoid_body(h, torso="wool", legs="wool", arms="wool", bulk=0.96)
    parts += [
        # Hood and shoulder cape, the cheapest way to say "not a soldier".
        Part("cone", (0, -0.004 * h, 0.918 * h), (0.135 * h, 0.150 * h, 0.115 * h),
             "wool", "Head", segments=10, taper=0.30),
        Part("cone", (0, 0, 0.800 * h), (0.290 * h, 0.235 * h, 0.085 * h),
             "wool", "Chest", segments=12, taper=0.62),
        # Belt and pouch
        Part("cyl", (0, 0, 0.585 * h), (0.200 * h, 0.128 * h, 0.022 * h),
             "leather", "Hips", segments=12),
        Part("box", (-0.085 * h, -0.058 * h, 0.560 * h),
             (0.060 * h, 0.038 * h, 0.065 * h), "leather", "Hips"),

        # The lantern, and the one thing on this model that has to be visible: the
        # flame is the only warm note on a calm household. It is built as a frame of
        # four corner posts between two plates, NOT a solid box — an enclosed
        # emissive is invisible, which is the trap this roster already hit once.
        Part("box", (-SHOULDER_X * h, -0.030 * h, 0.344 * h),
             (0.094 * h, 0.094 * h, 0.012 * h), "oak", "Hand.R"),
        Part("box", (-SHOULDER_X * h, -0.030 * h, 0.446 * h),
             (0.094 * h, 0.094 * h, 0.012 * h), "oak", "Hand.R"),
        Part("cyl", (-SHOULDER_X * h, -0.030 * h, 0.470 * h),
             (0.011 * h, 0.011 * h, 0.040 * h), "mail", "Hand.R", segments=6),
    ]
    for dx, dy in ((-1, -1), (-1, 1), (1, -1), (1, 1)):
        parts.append(Part("box",
                          (-SHOULDER_X * h + dx * 0.039 * h, -0.030 * h + dy * 0.039 * h,
                           0.395 * h),
                          (0.013 * h, 0.013 * h, 0.104 * h), "oak", "Hand.R"))
    parts += [
        Part("cyl", (-SHOULDER_X * h, -0.030 * h, 0.386 * h),
             (0.050 * h, 0.050 * h, 0.058 * h), "tallow", "Hand.R", segments=10),
        Part("cone", (-SHOULDER_X * h, -0.030 * h, 0.428 * h),
             (0.030 * h, 0.030 * h, 0.040 * h), "tallow", "Hand.R",
             segments=8, taper=0.12),

        # A staff, not a weapon — he is the one who fetches someone else.
        Part("cyl", (SHOULDER_X * h + 0.018 * h, -0.030 * h, 0.470 * h),
             (0.020 * h, 0.020 * h, 0.940 * h), "oak", "Hand.L", segments=8),
    ]
    return Archetype("Watchman", "Calm — walks the patrol routes, carries the lantern",
                     h, parts, humanoid_bones(h), tri_budget=6000, wear=0.9)


# --------------------------------------------------------------------------------
# Man-at-Arms — Stirred. The one guard who comes to check on a noise. Mail and a
# kettle helm: the garrison is "small and well-dressed in steel".
# --------------------------------------------------------------------------------

def _man_at_arms() -> Archetype:
    h = 1.80
    parts = humanoid_body(h, torso="mail", legs="wool", arms="mail", bulk=1.04)
    parts += [
        # Mail skirt over the hips, split for walking.
        Part("cone", (0, 0, 0.502 * h), (0.262 * h, 0.190 * h, 0.108 * h),
             "mail", "Hips", segments=12, taper=0.86),
        # Surcoat over the mail, undyed wool.
        Part("cone", (0, 0, 0.660 * h), (0.278 * h, 0.182 * h, 0.235 * h),
             "linen", "Spine", segments=12, taper=0.88),
        Part("cyl", (0, 0, 0.560 * h), (0.245 * h, 0.155 * h, 0.026 * h),
             "leather", "Hips", segments=12),

        # Kettle helm. The brim is the silhouette of the century, so it is wide and
        # sits at the bowl's base; a domed cone reads as beaten steel, a sphere reads
        # as a ball balanced on a plate.
        Part("cone", (0, -0.004 * h, 0.934 * h), (0.172 * h, 0.180 * h, 0.115 * h),
             "steel", "Head", segments=14, taper=0.34),
        Part("cyl", (0, -0.004 * h, 0.884 * h), (0.176 * h, 0.184 * h, 0.028 * h),
             "steel", "Head", segments=14),
        Part("cone", (0, -0.004 * h, 0.868 * h), (0.208 * h, 0.216 * h, 0.030 * h),
             "steel", "Head", segments=18, taper=0.76),
        # Mail coif framing the face.
        Part("cone", (0, 0.008 * h, 0.856 * h), (0.176 * h, 0.180 * h, 0.082 * h),
             "mail", "Head", segments=12, taper=1.12),

        # Arming sword, sheathed at the left hip. A sheathed sword keeps the
        # silhouette clean; a drawn one crossed the body and read as a bar.
        Part("cyl", (0.150 * h, 0.055 * h, 0.395 * h),
             (0.040 * h, 0.040 * h, 0.360 * h), "leather", "Hips",
             rot=(16, 0, -7), segments=8, taper=0.72),
        Part("cyl", (0.171 * h, 0.006 * h, 0.578 * h),
             (0.026 * h, 0.026 * h, 0.072 * h), "leather", "Hips",
             rot=(16, 0, -7), segments=8),
        Part("box", (0.167 * h, 0.017 * h, 0.542 * h),
             (0.088 * h, 0.020 * h, 0.016 * h), "steel", "Hips", rot=(16, 0, -7)),
        Part("sphere", (0.175 * h, -0.004 * h, 0.612 * h),
             (0.034 * h, 0.034 * h, 0.030 * h), "steel", "Hips", segments=10, rings=7),
    ]
    return Archetype("ManAtArms", "Stirred — the one guard who comes to check",
                     h, parts, humanoid_bones(h), tri_budget=7500, wear=1.0)


# --------------------------------------------------------------------------------
# Sergeant — Roused. Leads the sweep, and carries the horn that turns a sweep into
# the Hue and Cry. He wears madder because he *is* the moment the household wakes.
# --------------------------------------------------------------------------------

def _sergeant() -> Archetype:
    h = 1.85
    parts = humanoid_body(h, torso="mail", legs="mail", arms="mail", bulk=1.12)
    parts += [
        # Coat of plates over the mail: steel bands across the chest.
        Part("cyl", (0, 0, 0.757 * h), (0.268 * h, 0.168 * h, 0.150 * h),
             "steel", "Chest", segments=12, taper=0.94),
        Part("cyl", (0, 0, 0.660 * h), (0.252 * h, 0.162 * h, 0.075 * h),
             "steel", "Spine", segments=12, taper=0.97),
        # Madder tabard — the alarm's colour on the man who raises it.
        Part("box", (0, -0.094 * h, 0.648 * h), (0.132 * h, 0.016 * h, 0.300 * h),
             "madder", "Spine"),
        Part("box", (0, 0.094 * h, 0.648 * h), (0.132 * h, 0.016 * h, 0.300 * h),
             "madder", "Spine"),
        Part("cyl", (0, 0, 0.556 * h), (0.265 * h, 0.170 * h, 0.028 * h),
             "leather", "Hips", segments=12),

        # Great helm: a flat-topped steel cylinder with a vision slit.
        Part("cyl", (0, -0.004 * h, 0.902 * h), (0.178 * h, 0.186 * h, 0.118 * h),
             "steel", "Head", segments=14, taper=0.92),
        Part("cone", (0, -0.004 * h, 0.962 * h), (0.172 * h, 0.180 * h, 0.036 * h),
             "steel", "Head", segments=14, taper=0.55),
        Part("box", (0, -0.090 * h, 0.926 * h), (0.126 * h, 0.020 * h, 0.017 * h),
             "mail", "Head"),
        Part("box", (0, -0.090 * h, 0.884 * h), (0.024 * h, 0.018 * h, 0.034 * h),
             "mail", "Head"),

        # Pauldrons, so the sweep reads as wider than a lone guard.
        Part("sphere", (SHOULDER_X * h * 1.10, 0, SHOULDER * h),
             (0.125 * h, 0.128 * h, 0.098 * h), "steel", "Shoulder.L",
             mirror=True, segments=12, rings=8),

        # Flanged mace — "no opinion about armour, none whatsoever about doors".
        Part("cyl", (SHOULDER_X * h + 0.020 * h, -0.014 * h, 0.352 * h),
             (0.034 * h, 0.034 * h, 0.250 * h), "oak", "Hand.L", segments=8),
        Part("cyl", (SHOULDER_X * h + 0.020 * h, -0.014 * h, 0.212 * h),
             (0.070 * h, 0.070 * h, 0.086 * h), "steel", "Hand.L", segments=8),
    ]
    parts += ring(4, 0.038 * h, 0.212 * h, kind="box", size=(0.050 * h, 0.015 * h, 0.066 * h),
                  mat="steel", bone="Hand.L", loc=(SHOULDER_X * h + 0.020 * h, -0.014 * h, 0))
    parts += [
        # The horn, slung at the right hip. This is the object that ends your raid.
        Part("cone", (-0.148 * h, 0.052 * h, 0.512 * h),
             (0.046 * h, 0.046 * h, 0.132 * h), "bone", "Hips",
             rot=(70, 24, 0), segments=10, taper=0.42),
    ]
    return Archetype("Sergeant", "Roused — leads the sweep, carries the horn",
                     h, parts, humanoid_bones(h), tri_budget=9000, wear=1.0)


# --------------------------------------------------------------------------------
# War-Hound — the chase. Named in the moodboard's Hue and Cry beat: "a war-hound at
# your heels". It is the reason standing still stops working.
# --------------------------------------------------------------------------------

def _war_hound() -> Archetype:
    s = 0.86  # shoulder height; the body is built in multiples of it
    parts = [
        # Barrel and haunches
        Part("cyl", (0, 0.050, 0.62 * s), (0.40 * s, 0.44 * s, 0.86 * s),
             "wool", "Spine", rot=(90, 0, 0), segments=12, taper=0.92),
        Part("sphere", (0, 0.330, 0.64 * s), (0.44 * s, 0.46 * s, 0.46 * s),
             "wool", "Hips", segments=12, rings=9),
        Part("sphere", (0, -0.230, 0.66 * s), (0.42 * s, 0.40 * s, 0.44 * s),
             "wool", "Chest", segments=12, rings=9),

        # Neck and head, carried low the way a running dog does.
        Part("cyl", (0, -0.360, 0.72 * s), (0.26 * s, 0.26 * s, 0.34 * s),
             "wool", "Neck", rot=(64, 0, 0), segments=10, taper=0.88),
        Part("box", (0, -0.500, 0.76 * s), (0.22 * s, 0.26 * s, 0.22 * s),
             "wool", "Head"),
        Part("box", (0, -0.598, 0.723 * s), (0.155 * s, 0.165 * s, 0.150 * s),
             "wool", "Head"),
        Part("sphere", (0, -0.672, 0.735 * s), (0.075 * s, 0.060 * s, 0.060 * s),
             "leather", "Head", segments=8, rings=6),
        # Ears laid back
        Part("cone", (0.085 * s * 1.0, -0.455, 0.855 * s), (0.10 * s, 0.13 * s, 0.14 * s),
             "wool", "Head", mirror=True, rot=(-28, 0, 0), segments=6, taper=0.25),
        # Bared teeth, the only bright thing on it
        Part("box", (0, -0.648, 0.678 * s), (0.105 * s, 0.075 * s, 0.024 * s),
             "bone", "Head"),

        # Fore legs
        Part("cyl", (0.135 * s, -0.240, 0.44 * s), (0.115 * s, 0.115 * s, 0.40 * s),
             "wool", "ForeLeg.L", mirror=True, segments=8, taper=0.80),
        Part("cyl", (0.135 * s, -0.235, 0.14 * s), (0.085 * s, 0.085 * s, 0.24 * s),
             "wool", "ForePaw.L", mirror=True, segments=8, taper=0.92),
        Part("box", (0.135 * s, -0.275, 0.030 * s), (0.115 * s, 0.185 * s, 0.060 * s),
             "wool", "ForePaw.L", mirror=True),

        # Hind legs
        Part("sphere", (0.150 * s, 0.330, 0.50 * s), (0.26 * s, 0.34 * s, 0.34 * s),
             "wool", "HindLeg.L", mirror=True, segments=10, rings=7),
        Part("cyl", (0.145 * s, 0.290, 0.24 * s), (0.105 * s, 0.105 * s, 0.34 * s),
             "wool", "HindPaw.L", mirror=True, rot=(-16, 0, 0), segments=8, taper=0.82),
        Part("box", (0.145 * s, 0.235, 0.030 * s), (0.110 * s, 0.180 * s, 0.060 * s),
             "wool", "HindPaw.L", mirror=True),

        # Tail and a studded collar
        Part("cyl", (0, 0.480, 0.72 * s), (0.085 * s, 0.085 * s, 0.42 * s),
             "wool", "Tail", rot=(-58, 0, 0), segments=8, taper=0.45),
        Part("cyl", (0, -0.375, 0.735 * s), (0.30 * s, 0.30 * s, 0.10 * s),
             "leather", "Neck", rot=(64, 0, 0), segments=12),
    ]
    parts += ring(5, 0.135 * s, 0.735 * s, kind="box", size=(0.045 * s, 0.045 * s, 0.045 * s),
                  mat="steel", bone="Neck", loc=(0, -0.375, 0))

    bones = [
        dict(name="Root", head=(0, 0, 0), tail=(0, 0, 0.22)),
        dict(name="Hips", head=(0, 0.330, 0.64 * s), tail=(0, 0.100, 0.63 * s), parent="Root"),
        dict(name="Spine", head=(0, 0.100, 0.63 * s), tail=(0, -0.180, 0.65 * s), parent="Hips"),
        dict(name="Chest", head=(0, -0.180, 0.65 * s), tail=(0, -0.330, 0.68 * s), parent="Spine"),
        dict(name="Neck", head=(0, -0.330, 0.68 * s), tail=(0, -0.470, 0.75 * s), parent="Chest"),
        dict(name="Head", head=(0, -0.470, 0.75 * s), tail=(0, -0.720, 0.71 * s), parent="Neck"),
        dict(name="Tail", head=(0, 0.430, 0.68 * s), tail=(0, 0.660, 0.92 * s), parent="Hips"),
        dict(name="ForeLeg.L", head=(0.135 * s, -0.240, 0.62 * s),
             tail=(0.135 * s, -0.240, 0.28 * s), parent="Chest", mirror=True),
        dict(name="ForePaw.L", head=(0.135 * s, -0.240, 0.28 * s),
             tail=(0.135 * s, -0.290, 0.02 * s), parent="ForeLeg.L", mirror=True),
        dict(name="HindLeg.L", head=(0.150 * s, 0.330, 0.62 * s),
             tail=(0.145 * s, 0.310, 0.36 * s), parent="Hips", mirror=True),
        dict(name="HindPaw.L", head=(0.145 * s, 0.310, 0.36 * s),
             tail=(0.145 * s, 0.245, 0.02 * s), parent="HindLeg.L", mirror=True),
    ]
    return Archetype("WarHound", "The chase — reacts to noise, runs you down",
                     0.90, parts, bones, tri_budget=6500, bevel=0.005, wear=0.8)


# --------------------------------------------------------------------------------
# Crypt-Risen — what sleeps under the keep, and what a fumbled CADAVER SURGE gets
# you. The only model carrying lapis: a word raised it, so it wears the voice.
# --------------------------------------------------------------------------------

def _crypt_risen() -> Archetype:
    h = 1.70
    parts = humanoid_body(h, torso="leather", legs="leather", arms="leather", bulk=0.86)
    parts += [
        # Grave wrappings, hanging loose and torn.
        Part("cone", (0, 0, 0.560 * h), (0.245 * h, 0.180 * h, 0.180 * h),
             "linen", "Hips", segments=8, taper=0.86),
        Part("cone", (0, 0, 0.745 * h), (0.245 * h, 0.170 * h, 0.155 * h),
             "linen", "Chest", segments=8, taper=0.80),
    ]
    # Torn hem, leaning outward so the silhouette is ragged rather than a clean cone.
    for frac, z, length, lean in ((0.00, 0.455, 0.20, 14.0), (0.20, 0.430, 0.16, 10.0),
                                  (0.40, 0.470, 0.23, 18.0), (0.62, 0.425, 0.15, 9.0),
                                  (0.82, 0.462, 0.21, 16.0)):
        yaw = 360.0 * frac
        theta = math.radians(yaw)
        parts.append(Part("shard",
                          (math.cos(theta) * 0.125 * h, math.sin(theta) * 0.105 * h, z * h),
                          (0.075 * h, 0.075 * h, length * h), "linen", "Hips",
                          rot=(0.0, lean, yaw), segments=4))
    parts += [
        # The skull: bone where the face should be, and the word still burning in it.
        Part("sphere", (0, -0.004 * h, 0.905 * h), (0.100 * h, 0.115 * h, 0.128 * h),
             "bone", "Head", segments=14, rings=10),
        Part("box", (0, -0.062 * h, 0.885 * h), (0.072 * h, 0.030 * h, 0.028 * h),
             "lapis", "Head"),
        Part("box", (0.030 * h, -0.070 * h, 0.918 * h), (0.028 * h, 0.022 * h, 0.024 * h),
             "lapis", "Head", mirror=True),
        # The glow leaks out of the ribs too.
        Part("box", (0, -0.070 * h, 0.735 * h), (0.105 * h, 0.030 * h, 0.075 * h),
             "lapis", "Chest"),
        # Bone hands, fingers splayed.
        Part("box", (SHOULDER_X * h, -0.008 * h, 0.463 * h),
             (0.046 * h, 0.058 * h, 0.080 * h), "bone", "Hand.L", mirror=True),
    ]
    for dx, yaw in ((-0.026, -16.0), (0.0, 0.0), (0.026, 16.0)):
        parts.append(Part("shard", (SHOULDER_X * h + dx * h, -0.030 * h, 0.405 * h),
                          (0.016 * h, 0.016 * h, 0.085 * h), "bone", "Hand.L",
                          mirror=True, rot=(16.0, 0.0, yaw), segments=4))
    return Archetype("CryptRisen", "The crypt — raised by a word, obedient to nobody",
                     h, parts, humanoid_bones(h), tri_budget=8000, bevel=0.006,
                     wear=1.2, arcane=True)


ROSTER = [
    _watchman(),
    _man_at_arms(),
    _sergeant(),
    _war_hound(),
    _crypt_risen(),
]

BY_NAME = {a.name: a for a in ROSTER}
