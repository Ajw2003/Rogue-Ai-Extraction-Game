# Plunderspell — Handbook

**The complete explanation of the game, for someone who has never heard of it.**

A four-player co-operative heist game set in the past. You play a wizard who is bad at wizardry and
worse with money. You break into castles, take everything you can physically carry, and try to get
back out. You cast your spells by saying the words aloud into your microphone.

> *Four friends, whisper-shouting a dead language at a locked door, carrying a stolen altarpiece down
> a staircase, badly.* — the shortest honest description of the game

**Illustrated version:** `docs/plunderspell-handbook.html` (open in a browser). It carries three
diagrams this file describes in words — the raid timeline, the voice-casting pipeline, and the castle
ward plan. This markdown file is the text of record; the HTML is the presentation of it.

**Related:** [`plunderspell.md`](plunderspell.md) is the pitch bible, written in character.
[`plans/plunderspell.md`](plans/plunderspell.md) is the engineering plan.

---

## Contents

1. [The premise](#1-the-premise)
2. [One session, start to finish](#2-one-session-start-to-finish)
3. [Casting with your voice](#3-casting-with-your-voice)
4. [Objects, weight and violence](#4-objects-weight-and-violence)
5. [Loot and what it is worth](#5-loot-and-what-it-is-worth)
6. [Noise and the household](#6-noise-and-the-household)
7. [Dying, and being carried](#7-dying-and-being-carried)
8. [How castles are built](#8-how-castles-are-built)
9. [The four Ages](#9-the-four-ages)
10. [The lair and progression](#10-the-lair-and-progression)
11. [What makes it different](#11-what-makes-it-different)
12. [Quality of life](#12-quality-of-life)
13. [What it is not](#13-what-it-is-not)
14. [Common questions](#14-common-questions)
15. [Glossary](#15-glossary)

---

## 1. The premise

You and up to three friends are wizards. Not good ones. You share a damp lair, a great deal of debt,
and one genuinely remarkable possession: a portal that opens onto the past.

You are not heroes and there is no quest. You are in the business of theft. Every night you open the
portal onto some century that is richer than yours, break into a castle, take as much as your arms
can hold, and run back through before anyone catches you. Then you sell it, pay down what you owe,
and do it again.

The wizardry is real but unreliable. You know perhaps forty words of power, and to use one you must
**say it out loud** — which works beautifully right up until you are frightened, out of breath, or
being chased down a spiral staircase by a man with a poleaxe. Then you fumble the syllables, and the
spell does something else entirely.

That gap — between the spell you meant and the spell you got — is where the game lives.

> **The short version.** A co-operative first-person heist game for one to four players. Around 25 to
> 40 minutes per run. You steal physical objects, carry them out by hand, and cast spells by
> speaking. PC, with everything running on your own machine and a friend's — there are no servers.

---

## 2. One session, start to finish

The clearest way to understand Plunderspell is to walk through a single run. Everything below is one
continuous session, and the whole game is made of repetitions of this shape.

**Before you go — the lair.** You start in the lair. It is the only place that persists between runs,
and it is where everything you own lives. Here you choose which of your forty-odd incantations you
are taking (you cannot carry all of them), pick one physical weapon, and choose which century to open
the portal onto. You also see what you owe, which is always more than you have. There is no time
pressure in the lair.

**Going through.** Everyone steps through together. You arrive outside a castle that was assembled by
the game moments ago and that nobody, including the host, has seen before. It is night. You have no
map. From this point a timer runs — generous, visible, and when it expires the portal home closes.

**Getting in and going deeper.** Castles are built as a series of rings, each harder to enter than
the last, with the treasure concentrated at the centre. You break in at the outside and work inward:
over the curtain wall, across the bailey, into the keep, and if you are brave, down into the crypt.
The deeper you go the more everything is worth — and the further you are from the way out, which is
entirely the point.

**Taking things.** When you find something valuable you pick it up, physically. A candlestick goes
under one arm. A chest takes both hands and slows you down. A full altarpiece cannot be lifted by one
person at all; someone has to take the other end, and the two of you have to get it down the stairs
together without letting go. There is no inventory screen, at any point, ever.

**Being noticed.** The castle has people in it, and they react to sound — not to a scripted trigger,
but to actual noise travelling outward from where it was made. Footsteps carry a little. A shouted
incantation carries a lot. A window breaking carries further. A hand cannon carries to the whole
building. The household wakes in stages, each louder and harder to undo than the last.

**Getting out.** Now you carry all of it back through everything you woke on the way in. This is the
part people remember. Somebody drops something irreplaceable. Somebody says the wrong word. Somebody
dies and has to be carried, which means somebody else puts the gold down. **Only what physically
crosses back through the portal comes home** — not what you had, not what you saw, what you were
actually holding when you stepped through.

**Afterwards.** Back at the lair everything is valued and added to the pile. You pay what is due, and
spend what is left on new words, better weapons, or improvements to the lair. Then you go again.

> **Diagram (HTML):** *the shape of a run.* The lair is untimed; the clock starts at the portal.
> Tension climbs as you move inward because value and distance-from-the-exit climb together — so the
> hardest part of the game is always the journey back out, carrying everything.

---

## 3. Casting with your voice

This is the feature the game is built around, so it is worth being precise about how it works.

### The mechanic

You hold down a key — the *cast key* — and speak an incantation. The game listens while the key is
held, matches what you said against its list of known words, and resolves the result the moment you
finish. It takes roughly a fifth of a second.

It is **push-to-cast**, never always-listening. The microphone is only read while you hold the key,
so the game cannot hear your room and you cannot cast by accident during a conversation.

Recognition happens **entirely on your own computer**. A small speech model ships inside the game. No
audio is uploaded anywhere, there is no account or subscription involved, and it works with your
internet connection off.

### Three possible outcomes

| Outcome | What happens |
|---|---|
| **Confident match** | The spell you meant. |
| **Near match** | The misfire twin — a real, hand-authored wrong spell. Never random. |
| **No match** | Nothing happens, and nothing is spent. |

Unrecognised speech costing nothing is the load-bearing detail: the game never punishes an accent or
a cough. A *near* match — a real word said badly — is the only branch that surprises you.

> **Diagram (HTML):** *the voice casting pipeline*, showing that three-way branch.

### The words

Around forty incantations, deliberately chosen to sound unlike one another so the game rarely
confuses two — and deliberately easy to slur when panicking, which is where misfires come from.

| Say this | What it does | Cast | Fumble it, and |
|---|---|---|---|
| `IGNIS` | A dart of fire. Cheap and fast, and it sets tapestries and thatch alight, which is usually not what you wanted. | 180 ms | your own beard catches |
| `FRANGO` | Shatters things — masonry, locks, bars. Also every fragile thing you are carrying. | 210 ms | the floor beneath you |
| `LEVO` | Lifts an object at range. The workhorse: grab, swing, stack, or throw anything with weight. | 190 ms | lifts you instead, badly |
| `AURUM VOCO` | For four seconds everything valuable glows through walls. You glow too, and everyone can see it. | 340 ms | the gold screams |
| `TONITRUS` | A thunderclap that shoves. Clears a doorway, drops a portcullis, launches a friend. | 260 ms | every light goes out |
| `SOMNUS` | Sleep — and it must be **whispered**. The game measures volume, so shouting it wakes the room. | 220 ms | a loud noise, right where you are |
| `CADAVER SURGE` | Raises a corpse as a porter. Slow, obedient, carries loot, follows you home. | 480 ms | it rises disobedient |
| `PORTA` | Tears open an emergency way home, here and now. Ruinously expensive. | 200 ms | opens on the wrong century |

### Volume is part of the input

The game hears *how loudly* you speak as well as *what* you say. Some spells demand a whisper and
refuse to work if shouted; others want a real shout and fizzle if mumbled.

> **If you can't or won't speak.** Every incantation can also be bound to a key and cast silently.
> Voice is the intended way to play; it is never the required way. Nobody is locked out for being
> mute, for sharing walls with a sleeping household, or for simply not wanting to shout Latin at a
> monitor.

---

## 4. Objects, weight and violence

Every object in the world is a real physical body with mass. That single decision produces most of
the game's behaviour.

### There is no separate combat system

Damage is calculated from how fast something was moving when it hit. That is the whole rule. A sword
is a heavy object with a sharp end that you swung quickly; a dropped chest is a heavy object that was
moving downward. The game does not distinguish between them.

The consequence is that the castle itself is an armoury: a portcullis dropped on someone, a wardrobe
shoved down a stairwell, a chandelier cut loose over a dining hall, a friend launched across a room
by a mispronounced `TONITRUS` who lands on a guard.

### Carrying things

- **Small items** — under a stone — are pocketed and do not slow you down.
- **One-handed items** occupy an arm. You can still fight, but not with both hands.
- **Two-handed items** occupy you completely. You can walk, and you can drop them. That is all.
- **Two-person items** cannot be lifted alone. Someone takes each end and you move in agreement —
  through doorways, down stairs, around corners not designed with you in mind.

You can grab, rotate, push out, pull in, and throw anything you hold. Creatures included: a small
enough enemy can be picked up, carried, throttled and thrown at a larger one.

### Fragility

Stained glass, reliquaries, painted manuscripts and alchemical glassware all break on hard impact —
computed the same way damage is. The richest things in any castle are the least able to survive you
panicking, which is a design decision rather than an accident.

---

## 5. Loot and what it is worth

Every valuable object carries three numbers, all readable by looking at it. No menus, no rarity
colours.

| | |
|---|---|
| **Worth** | What it sells for back at the lair, in coin. Shown on inspection, so you are never guessing whether something is worth the trouble. |
| **Bulk** | How much of you it occupies, in *stone*. The real currency of a run — you are limited by arms and stairs, not by a bag size. |
| **Fragility** | Whether it survives being dropped, thrown, or caught in a blast. The most valuable items are usually the most delicate. |

A silver ewer is worth a little, weighs 2 stone, and tucks under one arm. A reliquary is worth a great
deal, weighs 3, and shatters if you fall over. The altarpiece in the chapel is worth more than
everything else in the building put together, weighs 14, needs two of you, and will not fit through
the window you came in by.

This is the entire inventory system. Greed is legible without a single number on the interface,
because greed looks like a person struggling down a staircase with both arms full.

---

## 6. Noise and the household

The castle is not a set of enemies waiting in rooms. It is a household that is currently asleep, and
your entire relationship with it is governed by sound.

Noise is a real event with a location and a loudness. It spreads outward, gets quieter with distance,
and is muffled by walls and doors. Anyone it reaches responds according to how loud it was on arrival.
Nothing is a scripted trigger — the same castle plays differently depending purely on how carefully
you move.

### The four states

| State | Behaviour | Escalates when | Calms down? |
|---|---|---|---|
| **Calm** | Patrol routes | a sound carries | — |
| **Stirred** | One guard investigates | a body or breakage is found | **Yes** — stay quiet ~45s |
| **Roused** | They sweep in pairs | you are seen carrying | **No** |
| **Hue & Cry** | Bells, gates, all of them | — | **No** |

**The one-way door is between Stirred and Roused.** Below it, patience fixes your mistakes — freeze,
wait, and the castle forgets. Above it, nothing you do calms the building down, and the run becomes a
question of how much you can carry out before it closes around you.

> **Diagram (HTML):** *the four alarm states*, with the escalation triggers and the decay path.

### Roughly how loud things are

| Action | Carries | Note |
|---|---|---|
| Walking, crouched | almost nothing | The default way to move if you have any sense. |
| Running | a room or two | Fine in open ground, expensive indoors. |
| A spoken incantation | several rooms | Casting is never free — the word itself is a noise. |
| Breaking a window or lock | a wing | Fast entry, and everyone nearby knows where you came in. |
| Armour or a chest hitting stone | a wing | The most common way a careful run ends. |
| A hand cannon or grenado | the whole castle | There is no quiet way to use gunpowder. |

Note what this does to the voice system: **casting a spell is itself a noise**, and whispering
`SOMNUS` is quieter than shouting it. The two core systems are wired directly into each other rather
than sitting side by side.

---

## 7. Dying, and being carried

Death is not the end of your run, and it is not free either. It converts you from a person into a
problem.

When you go down you drop everything where you fall. Your body becomes an object weighing 12 stone —
a two-handed item somebody now has to choose to pick up.

If a teammate carries you through the portal you are revived at the lair for a fee. If nobody does,
you lose the weapon and incantations you brought on that run, but **nothing you had already banked**.
Your lair, your money and your unlocks are never at risk.

The cost is not really the fee. It is that a pair of hands carrying you is a pair of hands not
carrying gold — so every death is a live, arguable decision about what your friend is worth in coin,
made out loud, under time pressure, while something is chasing you.

> **Nobody sits and watches.** A downed player stays in the world as a spectator who can still speak,
> still ping locations, and still see what their friends are doing — which in practice means they
> spend the time giving directions and being ignored. The wait is never longer than the run.

---

## 8. How castles are built

Every castle is generated fresh and no two runs use the same one. But they are not random tangles of
corridors — they are built the way real castles were built, and that structure is what makes them
readable.

A castle is a set of nested rings — **curtain wall, outer bailey, inner ward, keep, and the crypt at
the centre**. You enter at the outside, and each ring inward is better defended and richer than the
last.

Because value increases toward the centre, the richest room is always furthest from the exit. A
successful raid therefore guarantees a long retreat back through every part of the castle you have
already disturbed. No separate escape sequence has to be designed; the layout produces one.

> **Diagram (HTML):** *the concentric castle plan*, showing the entry point, the value gradient, and
> the retreat path.

### How the pieces fit

Rooms are hand-built by artists as modules, each with labelled connection points — a door here, an
arrow-loop there, a staircase going up, a murder-hole in the ceiling. The generator fits them together
by matching those points, which is why results look like architecture rather than a maze.

Every generated castle is then checked by the game before you see it, to prove there is a route from
the crypt back to a way out. If there is not, it is thrown away and a new one is built.

> **Why everyone sees the same castle.** The host picks a single random number and sends it to
> everyone. Each player's machine builds the identical castle from that number alone. Nothing about
> the building is sent over the network, which is why a four-hundred-room fortress costs no more to
> play than an empty field.

---

## 9. The four Ages

Your lair sits outside of time, so a century is a destination rather than a chapter. Each has its own
architecture, riches, defenders and weapons, and you choose which to open the portal onto before each
run.

| Age | What it looks like | What you steal | What you can carry |
|---|---|---|---|
| **The Bronze Age**<br>c. 1200 BC | Mud-brick palace complexes, painted plaster, low ceilings and firelight. Many defenders, barely armoured. | Ingots, faience, ceremonial bronze, sealed amphorae | Khopesh, sling, oxhide shield, fire-pot |
| **The High Medieval**<br>c. 1250 | Stone keeps, curtain walls, spiral stairs. Small garrison in good steel. The Age you learn the game on. | Reliquaries, altar plate, illuminated psalters, coin | Arming sword, mace, crossbow, boiling oil |
| **The Late Medieval**<br>c. 1450 | Fortresses within fortresses, murder-holes and crooked gates, built to stop people like you. Guards hunt in pairs. | Plate armour, tapestry, banking ledgers, jewels | Poleaxe, hand cannon, pavise, caltrops |
| **The Age of Powder**<br>c. 1620 | Wide halls, enormous windows, glass everywhere — and magazines of black powder that turn any fight into demolition. | Cabinets of curiosity, mirrors, astrolabes, silver | Wheellock pistol, rapier, grenado, petard |

### The rule that matters

**Anything you carry out of one Age, you can carry into another.** Nothing stops you taking a
wheellock pistol from 1620 back to 1200 BC, where gunpowder does not exist and nobody has any idea
what to do about it.

This is deliberate and it will not be balanced away. It is the game's progression: every Age you
unlock makes every earlier Age easier and stranger, and the fun of a late run is watching a century
meet a weapon it has no answer for.

---

## 10. The lair and progression

The lair is your hub, your home, and the only thing that survives a run. It is also the reason you go
back out.

**Debt.** You owe money, due on a schedule regardless of whether you raid — so standing still is not a
neutral choice, it is a losing one. This is the game's pressure valve: it never forces you out on any
particular night, but it makes the decision to go a real one.

**What you spend on:**

- **Words** — new incantations, and more slots to carry them in. You will always know more words than
  you can take on a run, so a loadout is a genuine choice.
- **Iron** — weapons, bought, repaired, or dragged home from a century that had not invented them yet.
  Each has real weight, so arming yourself costs carrying capacity.
- **Stone** — lair upgrades. A better portal reaches further back; a strongroom protects more; a
  workshop repairs what broke. These persist permanently.
- **Company** — porters raised from the dead, and other things you should probably not have brought
  home, which live in the lair and make themselves useful.

Progression is horizontal in difficulty terms: you do not become tougher, you become better equipped
and better spoken. A late-game wizard still dies to a single well-swung poleaxe.

---

## 11. What makes it different

Plenty of games share one of these. The combination is what makes Plunderspell a different thing
rather than a variation.

1. **Your voice is the controller.** Not voice chat, and not a novelty command layer bolted onto a
   normal game. Speaking is the primary way you use your main ability, and the game reads both your
   words and your volume.
2. **Failure is authored, not punished.** Mishearing produces a specific, hand-written wrong spell
   rather than an error message. The system's weakest point is deliberately converted into its best
   source of comedy.
3. **No inventory, anywhere.** Everything you steal is a physical object occupying real space in your
   hands. Greed is a posture and a walking speed, not a number in the corner of the screen.
4. **Two-person carries.** The best loot cannot be taken by one player. Co-operation is enforced by
   physics rather than by a lock that needs two keys.
5. **Casting is loud.** Your magic and the stealth system are the same system. Using your abilities is
   what gets you caught, so every spell is a cost-benefit decision rather than a cooldown.
6. **Anachronism as progression.** Instead of bigger numbers, you get to break earlier centuries with
   later technology. Power fantasy through historical unfairness.
7. **Architecture that generates its own tension.** Because castles are rings with the treasure at the
   centre, the escape sequence comes from the level's shape rather than being designed per map.
8. **Your friend's body has a price.** Reviving is not a button. It is twelve stone of cargo competing
   directly with the gold, and the argument about it happens out loud, while running.

---

## 12. Quality of life

A game about shouting into a microphone with friends has an unusually long list of ways to make
someone uncomfortable. Most of these exist because of that.

### Voice, comfort and access

- **Push-to-cast, never open mic.** The microphone is only read while you hold the cast key. The game
  cannot hear your room, and you can never cast by accident mid-conversation.
- **Every word has a key.** All forty incantations can be bound and cast silently, and the two input
  methods are equally strong — nobody chooses between comfort and winning.
- **Separate from voice chat.** Casting does not transmit to your friends' headsets as chat audio.
  They hear your incantation in the game world, positioned where your character is.
- **First-run calibration.** A short setup that checks your microphone level, has you say three words,
  and tells you honestly whether it is hearing you well before you load into a raid.
- **"Heard as" readout.** A small line of text shows what the game understood, every time. When
  something unexpected happens you always know whether it was you, the game, or the design.
- **Loudness meter.** Because some spells need a whisper and others a shout, an on-screen meter shows
  how loud the game is hearing you.
- **Incantation subtitles.** You see what teammates cast as readable text above them. Useful in chaos,
  essential for deaf and hard-of-hearing players, and funnier than hearing it.

### Playing with other people

- **Drop in, drop out.** Players can join a lair between runs and leave at any time. Leaving mid-raid
  does not end anyone else's run.
- **The host can leave.** If the person hosting disconnects the session moves to someone else instead
  of collapsing. Nobody loses a raid because a friend's router died.
- **Rejoin in progress.** Drop out and come back into the same raid at the portal, with whatever you
  were carrying still lying where you fell.
- **Markers without talking.** A ping system for loot, exits, danger and "come here" — so a silent
  player, or one who has just been told to shut up, is still a full participant.
- **Regroup beacon.** One player can raise a marker every party member sees through walls. Getting
  separated in a dark castle is a good story once and a frustration twice.
- **Solo is a real mode.** One player gets adjusted loot values and no two-person carries in the
  critical path, rather than a four-player raid you are expected to survive alone.

### Respecting your time and your eyes

- **The clock is always visible.** You always know how long is left, and the game never quietly
  extends itself.
- **Nothing expires.** No battle pass, no season, no daily login, nothing lost by not playing for a
  month. Going out is driven by debt inside the fiction, not anxiety outside it.
- **Save the last 45 seconds.** One keypress writes the previous three-quarters of a minute to a video
  file. In a game whose whole appeal is the thing that just happened, requiring people to have had
  recording software running is absurd.
- **Honest run summary.** Afterwards: who carried what, what broke and what it had been worth, and the
  run's worst misfire, credited by name.
- **Flash and shake controls.** Explosions, fire and impacts can be reduced independently. Camera
  shake, head-bob and field of view are all adjustable, and none are tied to difficulty.
- **Readable in the dark.** Valuables are marked by shape as well as by gold, so the game's one
  load-bearing colour never carries information alone. Subtitle size and background are adjustable,
  and every control remaps.

---

## 13. What it is not

Extraction games carry a lot of baggage. Several of these are the first thing people assume.

- **Not player-versus-player.** Nobody is hunting you but the castle. No other squads, no betrayal
  mechanics, no way to steal from a stranger.
- **You do not lose your progress.** A failed run costs the loadout you took and the loot you were
  carrying. Your lair, money, words and upgrades are never at risk.
- **Not a live service.** No seasons, no battle pass, no rotating storefront, no reason to log in on a
  Tuesday.
- **No microtransactions.** Everything in the game is bought with money you stole inside the game.
- **Not always-online.** Sessions are hosted by one of the players. No matchmaking service to go down,
  nothing to queue for.
- **Not a horror game.** Dark, tense, and things chase you — but the register is comedy. Nothing in it
  is trying to frighten you for its own sake.

---

## 14. Common questions

**Do I need a microphone?**
No. Every incantation can be bound to a key and cast silently, and a key-bound player is exactly as
capable as a speaking one. You will have a different kind of fun, but not less of it.

**Is my voice being sent anywhere?**
No. Recognition runs on your own machine using a model that ships inside the game. No audio leaves
your computer, no account is attached to it, and it works with your internet disconnected.

**What if the game can't understand my accent?**
Unrecognised speech does nothing and costs nothing — there is no penalty for being misheard. The
vocabulary is deliberately small and the words chosen to sound unlike one another, and accent coverage
is tested as a core requirement rather than a late polish task. And the key bindings are always there.

**Can I play on my own?**
Yes. Solo is a supported mode with its own tuning — adjusted values and no two-person carries standing
between you and finishing a run. It is a quieter, more careful game alone, and a genuinely different
one.

**How long is a session?**
A single raid runs 25 to 40 minutes, with the clock visible throughout. Time in the lair between raids
is untimed. You can comfortably play exactly one.

**What happens if I die?**
You drop what you were carrying and become a 12-stone body. A teammate can carry you through the
portal to revive you for a fee. If nobody does you lose the weapon and words you brought on that run —
but nothing you had already banked, and nothing permanent.

**Is it scary?**
Tense rather than frightening. It is dark and things chase you, but the tone is comic — much closer to
a heist going wrong than to horror. Flash, shake and gore settings are all adjustable.

**Can my friend join halfway through?**
Between raids, always. Mid-raid, they can join at the portal. And if someone's connection drops the
session moves to another player rather than ending.

---

## 15. Glossary

Most of these are real historical terms, used the way the period used them.

| Term | Meaning |
|---|---|
| **Bailey** | The open courtyard inside a castle's outer wall. Usually the first place you can be seen from a height. |
| **Bulk** | How much of you an object occupies, measured in stone. The real constraint on a run. |
| **Extraction** | Getting back through the portal. Only what you are physically holding at that moment comes home. |
| **Hue and cry** | The historical obligation for a whole community to pursue a fleeing criminal. Here, the top alarm state. |
| **Incantation** | One of the roughly forty words of power. Spoken aloud, or bound to a key. |
| **Keep** | The fortified tower at a castle's centre. The rich part, and the hard part. |
| **The lair** | Your persistent hub, outside of time. The only thing a bad run cannot take from you. |
| **Misfire** | What you get when an incantation is recognised as its near neighbour. Written by hand, never random. |
| **Porter** | A raised corpse that carries loot for you. Slow, obedient, and it follows you home. |
| **Stone** | The unit of bulk, after the historical measure of weight. A silver ewer is 2; a body is 12. |
| **Stratum** | One of the four Ages, named as archaeological layers because you dig down through them. |
| **Ward** | One ring of a castle's defences. Castles here are built as nested wards, poorest outside, richest within. |

---

*Handbook v1.0 — a co-operative heist game for 1–4 players · PC / Steam*
