A labeled progress bar used on weapon and loot "plate" cards (Reach / Heft /
Noise) to communicate a relative quantity without a raw number as the primary
read.

## Guidelines

- Label and value are mono (`mono.stat-label` / `mono.value`), in
  `vellum-faint` and `vellum-dim`.
- Track is `line`; fill colour carries meaning, not decoration --
  `verdigris` (default/neutral stat), `orpiment` (a "heft"/weight-class
  stat), `madder` (a "risk"/noise/danger stat).
- Never mix fill colours within a single bar, and never use `lapis` here --
  it's reserved for the voice/arcane register.
