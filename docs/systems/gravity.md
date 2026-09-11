# Gravity

Adjustable, source-driven gravity. Supports classic directional gravity and Mario-Galaxy-style
spherical planets, and lets a body move between them without any special-casing at the boundary.

Ported from `RogueLikeSlop@ThirdPerson` (`Managers/GravitySource.cs`, `Player/PlayerController.cs`).

## How it works

Two components:

- **`GravitySource`** — a field in the world. `Spherical` pulls toward its own origin (a planet);
  `Directional` pulls along a transform-local axis (a room with a conventional floor). Each has an
  `influenceRadius`. Sources self-register into a static list in `OnEnable` and remove themselves in
  `OnDisable`, so nothing searches the scene at runtime.
- **`GravityReceiver`** — sits on any Rigidbody that should obey the field. Each `FixedUpdate` it
  resolves the dominant source, applies the pull as a force, and slerps the body so its local up
  opposes the pull.

Resolution is nearest-source-wins among sources whose `influenceRadius` contains the body. When no
source is in range the previous one is retained rather than falling back to world up, so a body
coasting through empty space keeps its orientation instead of snapping mid-flight.

`GravityReceiver.Up` is the contract the rest of the game moves against. Gameplay code never
references `Vector3.up`: the player controller walks on the plane perpendicular to `Up`, jumps along
it, and decomposes velocity with `Vector3.Dot(velocity, Up)` where it would otherwise read `.y`.

## Invariants

- **`useGravity` is always false on a body with a `GravityReceiver`.** Unity's built-in gravity pulls
  along world `-Y` and would fight the field. `Item.cs` in particular must never set it back to true
  — doing so is what made thrown items fall world-down instead of toward the planet.
- **`freezeRotation` is always true on a body with a `GravityReceiver`.** Rotation is owned by this
  component (alignment) and by look input (yaw); the physics solver must never contribute. Without
  it, any collision applies angular velocity and the body tumbles — a character capsule falls over,
  and a carried item fights its carry rotation. It is also what makes the direct `transform.rotation`
  write in `AlignToGravityUp` safe: there is no angular velocity left for the write to discard.
- **Gravity is never replicated.** `GetGravityDirection` is a pure function of position over static
  level data, so every client computes an identical result independently. Adding network sync here
  would cost bandwidth to reproduce something already deterministic.

## Traps

- Sources are found through the static registry, not `FindObjectsByType`. A `GravitySource` that is
  disabled at the moment a body needs it simply does not exist as far as resolution is concerned.
- Nearest-wins compares distance to the source origin, not to its surface. Two overlapping fields
  with very different radii can hand over at a point that looks wrong; tune `influenceRadius` rather
  than adding special cases.
