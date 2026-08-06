# GemRush

A minimal arcade game built in **Unity 6000.0.62f1** as a technical test for RuneHeads.

## How to play

Move with **WASD** (or left stick). Collect the gems that spawn around the arena before the timer runs out. Beat your best score — it's saved between sessions.

**Flow:** Menu (shows best score) → Game (60s match with a 3-2-1 countdown) → Results (final score + new record banner) → back to Menu.

## Running the build

Download the zip from the latest release, extract, and run `GemRush.exe` (Windows x64).

## Architecture choices

- **`GameConfig` (ScriptableObject)** — all tuning parameters left open by the brief (match duration, arena size, spawn interval, max concurrent gems, move speed) live in a single designer-editable asset. Rebalancing requires no code changes.
- **Event-driven gameplay** — `MatchTimer`, `GemSpawner`, `ScoreCounter`, `MatchCountdown` and the UI views communicate through C# events and never hold direct references to each other. Each component can be understood, tested and replaced in isolation.
- **Persistence behind an interface** — `IHighScoreRepository` with a `PlayerPrefs` implementation. Swappable for JSON files or cloud saves without touching gameplay code.
- **`MatchFlowController`** — the single writer of persisted state and the only component that triggers the Game → Results transition. The new-record check happens *before* saving, and the persisted best is compared against the freshly finished match.
- **Cross-scene data** — a minimal static `GameSession` carries the last score and the new-record flag to the Results scene. Deliberately simple: for two values, a `DontDestroyOnLoad` service or a locator pattern would be overkill.
- **Gem pooling** — gems are recycled through `UnityEngine.Pool.ObjectPool`. At this scale (~40 spawns per match) it is not strictly necessary, but it keeps allocations flat with mobile targets in mind, and gem lifecycle is made explicit via an `Initialize` method rather than relying on Unity callback order.
- **No magic strings** — scene names are centralized in a `SceneNames` static class.

## Multi-device considerations

The brief targets "potentially every device". This delivery includes a Windows build, but the project is structured so that no platform is precluded:

- **Input** goes through the Input System's action maps — keyboard and gamepad work out of the box, and touch (on-screen stick) is an additional binding, not a refactor.
- **UI** uses a Canvas Scaler (scale with screen size), so layouts hold across resolutions and aspect ratios.
- **Simulation is frame-rate independent** — movement and timers use `Time.deltaTime`, physics runs in `FixedUpdate`.
- **URP** as render pipeline, scaling from low-end mobile to desktop.
- **Flat allocations** during gameplay thanks to gem pooling, with mobile GC pressure in mind.
- **Persistence** via `PlayerPrefs` works on every Unity platform with no conditional code (on WebGL it maps to IndexedDB).

No platform-specific code (`#if UNITY_*`) was needed anywhere.

## Small touches

- 3-2-1-GO countdown with ease-out scaling before input and timer start (`Time.timeScale` freeze, animated in unscaled time).
- Score punch-scale on pickup, timer turning red in the last 10 seconds.
- Gems rotate and bob with a randomized phase so they never move in lockstep.
- Short pause on match end before transitioning to Results, to let the final moment breathe.

## What I'd add or improve with more time

- **Audio** — pickup SFX with slight pitch randomization, countdown beeps, looping background music persisting across scenes. I had this prototyped but cut it to respect the indicative time budget and keep the delivered scope clean.
- **Pickup particle burst** — pooled one-shot particles on gem collection, sharing a generic `PrefabPool<T>` wrapper with the gems.
- **Touch controls** — on-screen joystick for mobile, since the Input System setup already abstracts the input source.
- **Gem variants** — different values, rare gems, time-bonus pickups.
- **Automated PlayMode test** — covering the score → results → menu flow and the high-score persistence contract.
- **Scene transitions** — a simple fade via an additive loading scene or Addressables.

## Project structure

```
Assets/
  Config/        GameConfig asset
  Prefabs/       Gem prefab
  Scenes/        Menu, Game, Results
  Scripts/
    Core/        GameConfig, GameSession, SceneNames, IHighScoreRepository (+ PlayerPrefs impl)
    Gameplay/    PlayerController, Gem, GemSpawner, MatchTimer, ScoreCounter,
                 MatchCountdown, MatchFlowController
    UI/          HudView, MenuView, ResultsView
```
