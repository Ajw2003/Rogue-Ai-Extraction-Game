# Plunderspell Design System

Plunderspell is a physics-driven, voice-cast heist game: one wizard, or as many as
eight, robbing candlelit castles across four historical Ages. This system is drawn
from the project's pitch bible (`docs/generated/plunderspell-moodboard.html`) — the
single committed visual world for the game: **a candlelit lair at 2am**, painted in
pigments a real medieval illuminator would have ground by hand.

It documents the identity system as written — colour, type, iconography, and the
editorial "hairline grid" layout language the pitch bible uses throughout. It does
**not** describe the game's current in-engine HUD, which is a separate, much simpler
placeholder (`Assets/_Project/Scripts/Runtime/UI/UITheme.cs`) built with Unity's
built-in font and a handful of flat colors — that code has not yet been brought in
line with this identity.

## Content fundamentals

Every colour is named for the pigment a real illuminator would have ground to make
it — never chosen off a colour wheel. That constraint is the whole discipline:
**verdigris carries every interactive and arcane thing** (buttons, glow, links,
anything the player can touch or that magic touches), **orpiment is spent on nothing
but gold and value** (so a flash of that yellow in a dark room always means loot,
without a word of explanation), and **madder marks alarm** — fire, blood, the moment
a household wakes. Lapis is reserved for the voice: incantations, portals, and
whatever answers them. No other colour is allowed to borrow these meanings.

Three further pigments — flint, malachite, umber — carry no symbolism; they're for
the parts of the world that are simply the world (stone, foliage, leather).

## Visual foundations

**Colour.** One dark theme. `bone-black` is the ground everything sits on; `ash` and
`ash-hi` are raised surface and its hover/inset state. `vellum` (never pure white —
"nothing in this century is white"), `vellum-dim` and `vellum-faint` step text down
from primary to secondary to tertiary/rule colour. See `tokens.json` for the full
pigment list with exact hex values and usage notes.

**Type.** Three families, each doing one job and never swapping: **Eczar** (display —
headings, the wordmark, lexicon terms, pull-quotes), **Spectral** (body copy, at a
deliberately generous 300-weight lede), and **Overpass Mono** (labels — eyebrows,
kickers, tags, tabular stats and timings). All three are Google Fonts; no font files
ship with this system.

**Spacing & shape.** The layout is built from a 1px hairline grid: cards
(`.grid > div`, `.plate`, `.pig`, `.mood`, `.beat`, `.layer`) sit in a CSS grid whose
1px gaps are filled with `line-soft`, so every card reads as separated by a single
hairline rather than a gap or a shadow. **No border-radius is used anywhere in the
identity** — every corner is square, deliberately, in place of the rounded-corner /
soft-shadow convention. Section rhythm runs on an 84px vertical block padding; card
interiors run tighter, 16–26px.

## Iconography

Icons in the identity are hand-drawn linework **sigils** — circular seals built from
a thin outer ring (`line`, 1.5px), a dashed inner ring in the section's accent colour,
and a small glyph (a key-and-wand, a radiant eye, a swinging weight) drawn in 2–3px
strokes in `orpiment` or the local accent, never filled. There is no icon *set* in the
conventional sense — each sigil is bespoke to the section it marks — but they share
that one construction consistently: ring, dashed ring, 2–3px glyph, no fill.

## Usage rules

- `verdigris` only for interactive/arcane elements — never as a general accent or
  decorative fill.
- `orpiment` only for gold, treasure, and value — the instant a player should read
  "this is loot," it's this colour and nothing else.
- `madder` only for alarm/danger states (a hunt, a fire, a critical timer) — not for
  general warm accents.
- `lapis` only for the voice/portal/arcane-response register — distinct from
  `verdigris`'s general arcane/interactive use.
- Never round a corner. Use a 1px hairline in `line` or `line-soft` to separate
  content instead of a shadow or a gap.
- Labels (eyebrows, kickers, tags) are always mono, uppercase, letter-spaced, and
  small (10–11px) — body and display type never take on a label's job.
