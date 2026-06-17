# Adaptive AI vs Static Difficulty (Unity Game Project)

**Adaptive AI vs Static Difficulty** is a 2D top-down combat prototype developed in Unity as part of a dissertation investigating the impact of performance-based adaptive enemy AI on player experience.

> **Dissertation Title:** Adaptive Difficulty in Action: Investigating How Performance-Based Enemy AI Affects Player Experience
> **Module:** COM6016M, York St John University
> **Author:** Ty Bingham (230254233)

The game features two distinct AI modes - **Static** and **Adaptive** - selectable from the main menu. In the Static condition, enemy parameters remain fixed throughout the session. In the Adaptive condition, a Dynamic Difficulty Adjustment (DDA) system evaluates player performance every ten seconds and dynamically scales enemy damage output, movement speed and health in response. Each session lasts **150 seconds**, after which gameplay data is automatically logged to a CSV file.

## How to Run

1. Download the latest release from the GitHub repository (link provided in the accompanying Word document)
2. Unzip the downloaded file
3. Run **Adaptive AI vs Static Difficulties.exe** to launch the game
4. Select **Static** or **Adaptive** from the main menu to begin a session

No Unity installation is required to run the executable.

## Game Modes

- **Static Mode** - Enemy stats remain fixed at default values for the entire 150-second session
- **Adaptive Mode** - Enemy damage, speed and health scale dynamically based on player deaths and damage taken, evaluated at ten-second intervals using a priority-ordered rule set

## Project Details

- **Engine:** Unity
- **Language:** C#
- **Platform Target:** Windows (PC)
- **Control Scheme:** Keyboard and mouse
- **Project Type:** Dissertation Artefact - COM6016M, York St John University

## Folder Structure

Inside `/Assets/`:

- `/Art/` - Background artwork
- `/Enemy/` - Enemy animations, spritesheets and audio
- `/Environment/` - Materials, environment prefabs, shaders, textures and tile palette
- `/Music/` - Background music tracks
- `/Player/` - Player animations, spritesheets and audio
- `/Prefabs/` - Enemy and player prefabs
- `/Scripts/` - All C# scripts
  - `DifficultyManager.cs` - Core DDA logic and multiplier evaluation
  - `PlayerPerformanceTracker.cs` - Tracks deaths and damage taken
  - `DataLogger.cs` - Logs session data to CSV
  - `EnemyController.cs` - Enemy health, damage and collision
  - `EnemyAI.cs` - Enemy pathfinding and movement
  - `Pathfinding.cs` - A* pathfinding implementation
  - `PathfindingGrid.cs` - Grid generation from wall tilemap
  - `EnemySpawner.cs` - Fixed-interval enemy spawning
  - `SessionTimer.cs` - 150-second session timer
  - `PlayerMovement.cs` - Player movement and animation
  - `PlayerAttack.cs` - Arrow projectile spawning
  - `Arrow.cs` - Projectile behaviour and collision
  - `EnemyHealthBar.cs` - Scaled health bar display
  - `UIManager.cs` - Live UI display and session controls
  - `MusicManager.cs` - Background music with fade in and fade out
  - `MainMenu.cs` - Mode selection and scene loading
- `/TextMeshPro/` - Text rendering assets

## Data Logging

At the end of each session, the following data is written to a CSV file stored at Unity's persistent data path:

| Field           | Description                                 |
|-----------------|---------------------------------------------|
| Session         | Session number                              |
| Mode            | Static or Adaptive                          |
| Deaths          | Total deaths across the session             |
| DamageTaken     | Total damage taken across the session       |
| TimeSeconds     | Session duration in seconds                 |
| FinalMultiplier | Final difficulty multiplier (Adaptive only) |

## Known Issues

- Enemies may occasionally become stuck or behave erratically near wall tiles due to pathfinding edge cases
- Players standing still before the first enemy spawns may prevent enemy movement from initialising
- Player attack is limited to horizontal directions without moving in vertical directions

## Third-Party Assets and Licensing Summary

The following assets were used under appropriate royalty-free or Unity Asset Store licenses.

| Asset Type                       | Source / License                                           | Usage                              | Modifications                  |
|----------------------------------|------------------------------------------------------------|------------------------------------|--------------------------------|
| Music                            | DARK FANTASY STUDIO - NOISE ALCHEMY, YouTube (Royalty Free)| Background music                   | Volume adjusted                |
| Character sprites and animations | 2D Character - Cute & Chibi: Free Pack — Unity Asset Store | Enemy and player sprites and audio | Integrated into custom scripts |
| Environment tiles                | Pixel Art Top Down - Basic — Unity Asset Store             | Background tile palette            | Arranged by author             |

## Attributions

- **Relaxing RPG Exploration Music 30 Min**
  DARK FANTASY STUDIO - NOISE ALCHEMY
  YouTube — Royalty Free, No Copyright
  https://www.youtube.com/watch?v=h4pTjcUI0r4
  Used as background music throughout gameplay. Volume adjusted in Unity's AudioSource component.

- **2D Character - Cute & Chibi: Free Pack (Unique Skill Animated Prefab with SFX)**
  Unity Asset Store
  Used for enemy and player character sprites, animations and sound effects.
  Animations and audio integrated into custom Unity scripts; original sprite assets used as-is.

- **Pixel Art Top Down - Basic**
  Unity Asset Store
  Used for the background tile palette.
  Tiles selected and arranged by the author to create the game environment; no modifications made to the original tile assets.

---

*Developed as part of COM6016M Dissertation, York St John University, 2025–2026*