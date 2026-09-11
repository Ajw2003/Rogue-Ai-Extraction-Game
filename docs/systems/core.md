# Core

Shared infrastructure every other module sits on: the singleton base, the event bus, and the state
machine contract. Ported from `RogueLikeSlop@ThirdPerson`.

`RogueAi.Core` references nothing. Every other assembly references it, so anything added here is
paid for everywhere — keep it small.

## How it works

- **`SingletonBase<T>`** — one scene-owned instance per type, claimed in `Awake`. A second instance
  destroys itself. `PersistBetweenScenes` (default true) controls `DontDestroyOnLoad`.
- **`EventManager`** — a typed publish/subscribe bus. Subscribers are held by `WeakReference`, so a
  destroyed listener does not keep its subscriber alive or throw on publish. Publishing is
  reentrancy-safe: unsubscribes that happen *during* a publish are queued and applied afterwards
  rather than mutating the list mid-iteration.
- **`BaseStateMachine` / `IState` / `PlayerState`** — `Enter`/`Update`/`Exit`/`FixedUpdate`. States
  are plain C# objects constructed once in `Awake`, not MonoBehaviours, which keeps their logic
  testable without a scene.

## Invariants

- **`SingletonBase.Instance` never creates anything.** An absent instance means no scene owner has
  been set up yet; that is information the caller needs, not a problem to paper over by spawning a
  GameObject.
- **A subclass overriding `OnDestroy` must call `base.OnDestroy()`.** See Traps.

## Traps

- **Unity's fake null and `?.`** — a destroyed `UnityEngine.Object` is not a real C# null. It
  compares equal to null through Unity's overloaded `==`, but `?.` bypasses that overload entirely
  and happily dereferences it. Since call sites are written `EventManager.Instance?.Publish(...)`,
  the getter collapses a destroyed instance to a genuine null (`_instance != null ? _instance : null`)
  so `?.` behaves. Removing that ternary makes every `?.` call site a latent null-ref after teardown.

- **`OnDestroy` is a hiding trap, not an overriding one.** `SingletonBase.OnDestroy` is `protected
  virtual` and clears `_instance`. A subclass that declares `private void OnDestroy()` *hides* it
  rather than overriding it — Unity calls only the most-derived one, the base never runs, and
  `_instance` is left pointing at a destroyed object. The compiler warns (CS0114) but does not error.
  `EventManager` gets this right with `protected override void OnDestroy()` + `base.OnDestroy()`;
  any future singleton must do the same.
