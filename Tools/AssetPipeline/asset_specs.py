"""
The 13 vertical-slice props this pipeline is responsible for — one entry per
ScriptableObject already sitting in Assets/_Project/Data/{Inventory,Loot}
with no mesh behind it yet. `builder` names a function in builders.py.
"""

WEAPON_SPECS = [
    # key,              builder,               tri_budget, out subdir
    dict(key="BronzeSword",    builder="build_bronze_sword",    tri_budget=400, subdir="Weapons"),
    dict(key="Longsword",      builder="build_longsword",       tri_budget=450, subdir="Weapons"),
    dict(key="FlintlockPistol",builder="build_flintlock_pistol",tri_budget=600, subdir="Weapons"),
    dict(key="Matchlock",      builder="build_matchlock",       tri_budget=650, subdir="Weapons"),
    dict(key="PaviseShield",   builder="build_pavise_shield",   tri_budget=500, subdir="Weapons"),
    dict(key="PlateHelm",      builder="build_plate_helm",      tri_budget=550, subdir="Weapons"),
    dict(key="PowderGrenade",  builder="build_powder_grenade",  tri_budget=250, subdir="Weapons"),
    dict(key="RoundShield",    builder="build_round_shield",    tri_budget=450, subdir="Weapons"),
]

LOOT_SPECS = [
    dict(key="AncientRelic", builder="build_ancient_relic", tri_budget=450, subdir="Loot"),
    dict(key="CopperPot",    builder="build_copper_pot",    tri_budget=400, subdir="Loot"),
    dict(key="GoldenGoblet", builder="build_golden_goblet", tri_budget=350, subdir="Loot"),
    dict(key="HeavyChest",   builder="build_heavy_chest",   tri_budget=700, subdir="Loot"),
    dict(key="SilverPlate",  builder="build_silver_plate",  tri_budget=300, subdir="Loot"),
]

ALL_SPECS = WEAPON_SPECS + LOOT_SPECS
