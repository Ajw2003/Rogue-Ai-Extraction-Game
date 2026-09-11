# PLUNDERSPELL

**A four-player co-op extraction game. You cast spells by shouting them out loud, and everything
you steal you have to physically carry home.**

Status: pitch / design bible v0.1 — no implementation yet.
Interactive mood board: see `docs/plunderspell-moodboard.html` (open in a browser).

---

## 1. The pitch

You and three friends are failing wizards. You share a damp lair, a stack of unpaid debts, and a
portal that opens onto the past. Every night you step through it, break into somewhere you
absolutely should not be, and try to get back out with something worth more than your own life.

Extraction games run on one feeling: *I have too much to lose and not enough time.* Plunderspell
keeps that and adds two things that have not been properly combined before.

**Voice casting.** Not a hotkey. You hold a key, say `FRANGO`, and the wall breaks. Your friends
hear you say it half a second before it happens. Six people whisper-shouting Latin at a locked door
while something enormous walks toward them is a clip. Every session produces a clip.

**Physical loot.** There is no inventory grid. A golden altarpiece is an object with mass that two
people carry between them, and it will not fit through the window you came in by. You will drop
things. You will throw a reliquary at a knight, watch it shatter, and know exactly how much money
just left the room.

The comedy is not written. It is what happens when physics and a microphone are both allowed to go
wrong at once.

### Why this, why now

| | |
|---|---|
| **Proven appetite** | Four-player co-op horror-comedy is the most reliably viral shape in PC gaming. It sells on friends recording friends. It needs a mechanic that generates footage, not a marketing budget. |
| **Unserved fantasy** | That entire genre is set in space stations, sewers and abandoned facilities. Nobody has taken the formula somewhere with gold in it. Castles, crypts and cathedrals are the most loot-dense fiction that exists. |
| **Unclaimed input** | Everyone already has a mic open, because these games run on voice chat. Using it as a *control input* costs the player nothing and has never been the core verb of a co-op game. |

**The one-line version:** four friends whisper-shouting dead languages at a locked door while
carrying a stolen altarpiece down a staircase, badly.

---

## 2. The loop — 25 to 40 minutes

1. **The Lair** — persistent hub. Bind spells, pick a weapon, choose an Age, decide how greedy you
   feel. Debts are due whether you go out or not.
2. **The Portal** — step through into a procedurally built castle in a chosen century. Once all
   four are in, the portal narrows.
3. **The Plunder** — break in. Find the treasury, the crypt, the reliquary. Everything has weight,
   value and fragility. Noise attracts the household. So does shouting.
4. **The Hue & Cry** — something notices. The castle wakes. Now you are carrying a chandelier down
   a spiral stair with a war-hound behind you and a spell you keep mispronouncing.
5. **The Extraction** — only what physically crosses the portal comes home. Drop a friend's body or
   drop the gold: a real choice, and it will be remembered.

The lair is the only thing that persists. That is what makes losing a run hurt.

---

## 3. Pillar one — the tongue

Hold the cast key. Speak the word. Recognition happens **on the player's own machine** in under a
tenth of a second. No servers, no subscription, no audio ever leaves the PC.

The lexicon is deliberately small and deliberately hard: about forty words chosen to sound nothing
like each other when whispered, but tempting to slur when panicking. **Mishearing is not a bug we
hide — it is content we write.** Every incantation has a documented failure twin, and the failure is
always funnier than the success.

| Incantation | Cadence | Effect | Cast | Near-miss |
|---|---|---|---|---|
| `IGNIS` (2 syl) | short, hard | Ember dart. Cheap, fast, sets tapestries and thatch alight — rarely what you wanted. | 180 ms | *INGUIS* — you set your own beard alight |
| `FRANGO` (2 syl) | plosive | Shatter. Breaks masonry, locks, bars — and every fragile thing you are holding. | 210 ms | *FRAGO* — you shatter the floor you're on |
| `LEVO` (2 syl) | open, sustained | Telekinetic lift. The heavy-loot verb: grab, swing, stack, hurl anything with mass. | 190 ms | *LEVE* — you lift yourself, badly |
| `AURUM VOCO` (4 syl) | two-beat | Gold-sense. Everything valuable glows through stone for four seconds. Also makes you glow. | 340 ms | *AURUM LOCO* — the gold screams instead |
| `TONITRUS` (3 syl) | rolling | Thunderclap. A physics shove: clears a doorway, drops a portcullis, launches a friend. | 260 ms | *TENEBRIS* — every light in the room dies |
| `SOMNUS` (2 syl) | soft, whispered | Sleep. Must be spoken *quietly* — the mic measures volume, and shouting it wakes the room. | 220 ms | *SONUS* — a loud noise, exactly where you are |
| `CADAVER SURGE` (5 syl) | long, deliberate | Raise a corpse as a porter. It carries loot. It follows you home. | 480 ms | it raises, and it is not obedient |
| `PORTA` (2 syl) | final | Emergency extraction. Ruinously expensive. Opens a hole home, here, now. | 200 ms | *PORTO* — it opens onto the wrong Age |

**Runs offline.** A small keyword-spotting model ships inside the build. No API key, no per-player
cost, no outage can take the game down.

**Accessible by default.** Every incantation is also bindable to a key. Voice is the intended way to
play, never the required one — nobody is locked out for being mute, shy, or in a shared house.

**Volume is a mechanic.** Loudness is read as well as words. Some spells demand a whisper, some
demand a real shout. The game asks you to be brave in your own living room.

---

## 4. Pillar two — the hands

Every item is a rigid body with mass, and every rigid body can hurt someone. There is no separate
combat system: hitting a man with a candlestick and looting a candlestick are the same code path.

Damage comes from impact velocity, so a sword is simply a heavy thing with a sharp end that you
swung fast. That one rule turns the whole castle into a weapon — a dropped portcullis, a shoved
wardrobe, a chandelier cut loose above a dining hall, a friend flung by a mispronounced `TONITRUS`.

It also makes greed legible without a single UI element. Bulk is measured in **stone**. A silver
ewer is 2 st and fits under one arm. The altarpiece is 14 st, needs two people, and the moment
either of you lets go it falls down the stairs and stops being worth anything.

- **Carry & hurl** — grab anything, rotate it, push it deeper or pull it closer, throw it. Creatures
  included: they can be picked up, carried, throttled and thrown.
- **Fragility** — stained glass, reliquaries, illuminated manuscripts and alchemical glass break on
  impact. The most valuable loot is the least able to survive you panicking.
- **Corpses are objects** — a dead friend is a 12 st item. Carry them out and revive them at the
  lair, at the cost of the gold that pair of hands would otherwise have carried.
- **The castle reacts** — noise is a physical event, not a scripted trigger. A shattered window, a
  shouted incantation, a dropped suit of armour: each propagates as sound and wakes what it reaches.

---

## 5. Pillar three — the Ages

Your lair sits outside time, so every Age is a destination rather than a chapter. Each has its own
architecture, loot, defenders and weapon tier.

**The rule that makes this sing:** whatever you carry out of one Age, you can carry into another. A
wheellock pistol is a curiosity in 1620 and an act of god in 1200 BC. We do not balance that away —
that *is* the progression.

### Stratum I — The Bronze Age (c. 1200 BC)
Palace complexes of mud-brick and painted plaster, grain stores and god-kings. Low ceilings, narrow
doors, torchlight. Defenders are many, poorly armoured, and utterly unprepared for anything you
bring back from later.
- **Loot:** ingots, faience, ceremonial bronze, sealed amphorae
- **Kit:** khopesh · sling · oxhide shield · fire-pot
- **Hazard:** fire spreads faster here than anywhere else

### Stratum II — The High Medieval (c. 1250)
The default Age and the vertical slice. Curtain walls, spiral stairs, a chapel worth more than the
rest of the castle combined. Garrisons are small but armoured, and the household wakes in stages.
- **Loot:** reliquaries, altar plate, illuminated psalters, coin
- **Kit:** arming sword · mace · crossbow · boiling oil
- **Hazard:** spiral stairs turn clockwise — against you

### Stratum III — The Late Medieval (c. 1450)
Concentric fortresses built specifically to stop people like you. Murder-holes, dog-legged gates,
guard rotations. The first Age where defenders have gunpowder and the architecture assumes a siege.
- **Loot:** Burgundian plate, tapestry, banking ledgers, jewels
- **Kit:** poleaxe · hand cannon · pavise · caltrops
- **Hazard:** guards hunt in pairs

### Stratum IV — The Age of Powder (c. 1620)
Wide halls, huge windows, immense glass, and magazines of black powder that turn any fight into
demolition. The richest, most fragile, most flammable loot in the game.
- **Loot:** cabinets of curiosity, mirrors, astrolabes, silver services
- **Kit:** wheellock pistol · rapier · grenado · petard
- **Hazard:** one stray `IGNIS` near the magazine ends the raid

---

## 6. Level generation — castles are not dungeons

Every procedural co-op game builds a web of rooms and corridors. A real castle is not that. It is a
set of nested rings, each harder to get into than the last, and that structure does the game design
for us.

Generate outward-in: **curtain wall → outer bailey → inner ward → keep → undercroft and crypt.**
Value rises with every ring you cross, so the deepest, richest room is also the furthest from the
way out — and **extraction is always a fighting retreat back through everything you already woke
up.** You never walk out of a good run calmly.

- **Socket-matched modules.** Rooms are prefabs with tagged joins: `door`, `window`, `arrow-loop`,
  `stair-up`, `stair-down`, `murder-hole`. Assembly matches sockets, keeping results
  architecturally plausible and letting artists add rooms without a programmer.
- **One integer on the wire.** The host rolls a seed and replicates it before load; every client
  builds the identical castle locally. No geometry is replicated, so a 400-room fortress costs the
  same bandwidth as an empty one.
- **Determinism is a hard requirement.** Use an explicitly-passed `System.Random` instance — never
  `UnityEngine.Random`, whose global static state is shared with VFX and audio, making generation
  order-dependent and clients divergent.
- **Validated, not hoped-for.** After generation, flood-fill to prove the crypt can reach a portal.
  If it cannot, throw the castle away and reroll. A layout that soft-locks a run is worse than a
  slow loading screen.

> **Clean-room constraint.** The `feature/Owen/PCG` branch is off-limits. Its author has left the
> project, so it is not a reference, not a prototype scaffold and not a shortcut. Do not open those
> files, do not diff against them, and never merge, cherry-pick or rebase that branch into this
> lineage. The generator is written from this specification and from castle architecture, nothing
> else. See `docs/plans/plunderspell.md` for the full provenance note.

---

## 7. The arsenal

No damage numbers on screen. Weapons are described by what they weigh and what they do to a room,
because that is what the physics actually simulates. Heft is in stone.

| Weapon | Age | Reach | Heft | Noise | Notes |
|---|---|---:|---:|---|---|
| Khopesh | Bronze | 40 | 3 st | low | Sickle of cast bronze. Hooks a shield away, same swing opens the body. Slow, and it bends. |
| Sling | Bronze | 85 | 0 st | none | Cord and a stone. Nearly silent, absurdly cheap, kills a man in a helmet. Needs space to swing. |
| Oxhide shield | Bronze | 15 | 4 st | mid | A wall you carry. Stops arrows dead. Also the fastest way down a flight of stairs. |
| Arming sword | High Med. | 52 | 2 st | low | The honest default. Fast in a corridor, light enough to keep a hand free for loot. |
| Flanged mace | High Med. | 35 | 5 st | high | Doesn't care about armour or doors. Loudest thing in the game that isn't on fire. |
| Crossbow | High Med. | 95 | 3 st | none | One bolt, then a long terrible reload during which you are a man holding a plank. Drops a guard before he shouts. |
| Poleaxe | Late Med. | 78 | 6 st | mid | Axe, hammer and spike on six feet of oak. Wins every open hall, useless in a stairwell — where you'll be. |
| Hand cannon | Late Med. | 58 | 4 st | max | A tube of iron you point and hope about. In the Bronze Age it is a religious experience. |
| Caltrops | Late Med. | 20 | 1 st | low | Thrown by the handful behind you. Almost no damage; wins more chases than anything else. |
| Wheellock pistol | Powder | 46 | 1 st | high | Concealable, one shot, no fuse to give you away. The only ranged weapon you can draw while carrying a chandelier. |
| Rapier | Powder | 68 | 1 st | none | Weightless and lethally fast, and it breaks if you hit anything solid. A duelist's toy in a building full of armour. |
| Grenado | Powder | 44 | 1 st | max | A powder sphere with a lit fuse that you physically throw. It rolls. It rolls back. It has no opinion about whose side you're on. |

---

## 8. Art direction

### Palette

Named for the pigments a real illuminator would have ground in the period. That is not decoration —
it constrains us honestly, because these are the only colours that could exist in this world.

| Pigment | Hex | Role |
|---|---|---|
| Bone Black | `#14120E` | Charred bone. The ground of everything: unlit corners, interface backdrops. |
| Vellum | `#DCD2BA` | Scraped calfskin. All primary text and parchment UI. Never pure white — nothing here is bleached. |
| Verdigris | `#5FA288` | Oxidised copper. The accent, the arcane glow, every interactive affordance. |
| Orpiment | `#C9A227` | Arsenic yellow, beautiful and toxic. Loot, value, candleflame. **Nothing else.** |
| Madder Lake | `#C4542E` | Boiled madder root. Fire, alarm, wounds, the moment the household wakes. |
| Ground Lapis | `#7A6AA0` | More costly than gold. Reserved for the voice: incantation text, portals, the necromantic. |

The discipline: **verdigris carries the interface, orpiment is reserved exclusively for things worth
money, and nothing else in the game is allowed to be gold.** When a player sees that yellow, it
always means the same thing.

### Mood

Stylised, not photoreal. Chunky readable silhouettes, hand-painted texture, and lighting that is
almost entirely diegetic. If you can see it, something in the room is burning.

Six frames the game should be able to produce: **The Lair** (one candle, deep warm falloff into
black) · **The Treasury** (darkness with gold bleeding in from one corner) · **The Portal** (cold
lapis thrown upward onto faces — the only unnatural colour in the world) · **The Hue & Cry** (madder
and flame from below, everything readable as silhouette) · **The Approach** (pre-dawn verdigris
mist, wet stone, low contrast) · **Bronze Age Interior** (ochre plaster and smoke, warmer and
dustier than every later Age).

- **Silhouette first** — robes, hoods and bulky loot read at 10 m in the dark. Every character reads
  as a shape before it reads as a person.
- **Diegetic light only** — torches, candles, hearths, spell effects. No ambient fill. Darkness is a
  real obstacle and light is a resource you carry and can drop.
- **Texture over polygons** — hand-painted albedo, soot in the crevices, gilt worn off the high
  points. Cheap to author, ages well, doesn't chase fidelity we can't win.

---

## 9. Production reality

This repository is not a blank Unity project. The `claude/steam-multiplayer-framework-xia7ch`
lineage already contains a working first-person physics extraction game with Steam multiplayer — and
it already has a `SpellBook`.

| Status | System |
|---|---|
| **Built** | First-person Rigidbody player, 10-state FSM, health, dodge, ground-snap, air control — `PlayerStateMachine.cs` |
| **Built** | Physics grab / carry / rotate / throw with velocity-scaled impact damage — `Item.cs`, `ItemManager.cs` |
| **Built** | ScriptableObject-driven spell casting: fire rate, spread, projectile count, homing, reload — `SpellBook.cs`, `SpellStats.cs` |
| **Built** | Enemy FSM, 7 states including a picked-up state. Monsters are already carryable, throwable and throttleable. |
| **Built** | Steam multiplayer: PurrNet with Steam transport, PurrLobby, invites, rich presence, cold-launch joins — `SteamInviteGateway.cs` |
| **Built** | Core: singletons, event bus, state machine base, `IHealth`, seven separate assembly definitions |
| **Delete** | Planetary gravity — isolated in `Runtime/Gravity/`, 2 files, 3 consumers |
| **New** | Castle generator: concentric wards, socket-matched modules, replicated seed, layout validation |
| **New** | Voice: mic capture, on-device keyword spotting, spell lexicon, misfire table |
| **New** | Loot: value, bulk in stone, fragility hooked into the existing impact path |
| **New** | Ages as ScriptableObjects; extraction portal; raid timer |
| **New** | The lair: persistent hub, debts, progression |

The single largest piece of work in the pivot is **deletion**. Planetary gravity lives behind its own
assembly definition and is two files with three consumers. That is an afternoon, not a rewrite.

---

## 10. Milestones

**M0 — Fork clean, cut gravity (~1 week).** Branch from the trunk, delete the Gravity assembly,
restore world gravity across the player states and items, confirm nothing regressed. Crates and
players fall along −Y and land flat; thrown items still deal velocity-scaled damage; all assemblies
compile with zero gravity references.

**M1 — Prove the voice (~2 weeks).** A bare grey room and four incantations, nothing else. Target
>90% top-1 across four accents on the 40-word lexicon, under 150 ms from word-end to effect, and
misfires that land as jokes rather than frustration. This is the riskiest assumption in the project;
everything is sequenced to answer it early.

**M2 — The vertical slice (~6 weeks).** One castle, one Age (High Medieval), four spells, four
players, the full loop end to end. Castle generator built fresh; loot with value, bulk and
fragility; two-person carries; an extraction portal that only counts what physically crosses it; a
lair that remembers what came back. This is the thing you show people.

**M3 — Open the other Ages (ongoing).** Ages are content, not engineering, once the ScriptableObject
exists. Each adds a room set, a loot table, an enemy roster and a weapon tier — and immediately
multiplies every Age before it, because the weapons travel. The anachronism rule stays unbalanced on
purpose.

---

*Pitch bible v0.1 — fork of Rogue-Ai-Extraction-Game · Unity 6000.3.15f1 · PurrNet + Steam*
