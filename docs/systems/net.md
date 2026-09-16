# Net

Steam invite plumbing that sits alongside PurrLobby rather than inside it. `RogueAi.Net`
references `RogueAi.Core`, the vendored PurrLobby runtime assembly, and Steamworks.NET.

## How it works

- **`SteamInviteGateway`** — closes the two gaps in PurrLobby's Steam invite flow (plan Phase 5:
  cold launch via `+connect_lobby <id>` on the command line, and the friends-list "Join Game"
  button via rich presence) without editing the vendored `SteamLobbyProvider`. It drives the
  provider's existing public API (`JoinLobbyAsync`, `OnLobbyUpdated`/`OnLobbyLeft`) instead of
  adding a second join path — the provider already handles the in-overlay invite path itself via
  `OnGameLobbyJoinRequested`.
- Cold-launch join polls `SteamLobbyProvider.IsSteamClientAvailable` for up to
  `MaxSteamReadyPollFrames` frames before attempting to join. A friend accepting an invite while
  the game is closed launches it with the command-line argument before Steam is guaranteed to have
  finished initializing (`handleSteamInit`, or an external Steam bootstrapper, runs independently
  of this component) — there is no "Steam is ready" event to await instead, so polling is the
  available option.
- Rich presence is set on `OnLobbyUpdated` (covers both creating and joining a lobby) and cleared
  on `OnLobbyLeft` by writing a null value to the `connect` key, which is Steamworks.NET's
  documented way to unset a rich presence key.

## Traps

- `OnLobbyLeft` only fires from `SteamLobbyProvider.LeaveLobbyAsync()`, the parameterless overload
  that operates on the local player's current lobby. It does not fire for the `LeaveLobbyAsync(string
  lobbyId)` overload, nor for an implicit disconnect (network drop, being kicked). Rich presence
  can go stale in those paths; fixing it needs a change inside the vendored provider, which Task B
  deliberately avoided.
