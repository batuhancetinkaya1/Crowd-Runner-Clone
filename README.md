# Crowd-Runner-Clone

Welcome to the **Crowd-Runner-Clone** project! This repository contains a Unity-based crowd runner game with dynamic mechanics involving doors, tiles, chunks, enemies, and fights. Though the game is unfinished, it demonstrates advanced mechanics and is designed for further development.

## Table of Contents
- [Features](#features)
- [Installation](#installation)
- [Gameplay](#gameplay)
- [Tile and Chunk System](#tile-and-chunk-system)
- [Enemy, Bonus Type, and Amount System](#enemy-bonus-type-and-amount-system)
- [Fight Mechanics](#fight-mechanics)
- [Project Structure](#project-structure)
- [Future Improvements](#future-improvements)
- [License](#license)

---

## Features
- Dynamic level generation with a mix of normal tiles and fight tiles.
- Advanced mechanics for doors, bonuses, and enemy spawning.
- Fight mechanics with strategic decision-making.
- Modular structure to enable further enhancements.

## Installation
1. **Clone the repository**:
   ```bash
   git clone https://github.com/batuhancetinkaya1/Crowd-Runner-Clone.git
   ```
2. **Open the project** in Unity (tested with Unity version [Add Unity version here]).
3. **Play the game** by opening the `Main Scene` and pressing the Play button in the Unity Editor.

## Gameplay
The objective is to guide your crowd through dynamically generated levels, overcoming obstacles and enemies to reach the goal. The levels consist of five normal tiles and one fight tile, ensuring a mix of challenges and action.

### Controls
- **Mobile**: Slide your finger on the screen to move.
- **Desktop**: Press and hold the left mouse button, then slide to move.

## Tile and Chunk System
The game uses a dynamic tile and chunk system to generate levels:
- **Normal Tiles**: Standard paths with doors and bonuses.
- **Fight Tiles**: Special tiles containing enemies.
- **Chunks**: Combinations of tiles generated as the player progresses.

## Enemy, Bonus Type, and Amount System
The type and amount of enemies and bonuses are determined by a weighted random system. The mathematics behind the system is as follows:

1. **Enemy Spawning**:
   - Let `E` represent the total number of enemies to spawn on a fight tile.
   - `E = BaseEnemies + (Level * EnemyMultiplier)`
     - `BaseEnemies` is a fixed minimum number of enemies.
     - `Level` represents the current level or progress.
     - `EnemyMultiplier` adjusts difficulty scaling with player progress.
   - Each enemy type is assigned a probability `P(Type)` such that:
     - `Sum(P(Type)) = 1`
     - A random value `R` between 0 and 1 determines the type of enemy spawned based on cumulative probabilities.

2. **Bonus Distribution**:
   - Bonuses are randomly placed on normal tiles.
   - Let `B` represent the total number of bonuses.
   - `B = BaseBonuses + Random(0, MaxAdditionalBonuses)`
     - `BaseBonuses` is the guaranteed number of bonuses.
     - `MaxAdditionalBonuses` introduces variability.
   - Each bonus type has a probability `P(BonusType)` similar to enemies.

3. **Dynamic Adjustments**:
   - Both enemy and bonus spawn counts are adjusted dynamically based on player performance (e.g., score or remaining crowd size).

## Fight Mechanics
When the player enters a fight tile:
- **Enemy Waves**: Enemies spawn in waves, with each wave consisting of `W` enemies calculated as:
  - `W = WaveBase + (Level * WaveMultiplier)`
    - `WaveBase` is the minimum number of enemies per wave.
    - `WaveMultiplier` scales the wave size with the level.
- **Combat**:
  - The player's crowd engages enemies using a proportional damage system:
    - Damage dealt by the player = `PlayerCount * AttackPower`
    - Damage taken = `EnemyCount * EnemyAttackPower`
  - Combat continues until all enemies are defeated or the player's crowd size reaches zero.
- **Victory Conditions**: All enemies must be defeated to proceed.
- **Reset System**: If the player loses, the level restarts with reduced difficulty to encourage progress.

## Project Structure
### [Assets](https://github.com/batuhancetinkaya1/Crowd-Runner-Clone/tree/main/Assets)
Contains all resources used in the game.

### [Animations](https://github.com/batuhancetinkaya1/Crowd-Runner-Clone/tree/main/Assets/Animations)
Includes animation clips and controllers for smooth gameplay visuals.

### [Scripts](https://github.com/batuhancetinkaya1/Crowd-Runner-Clone/tree/main/Assets/Scripts)
Organized into subfolders for different game elements:
- **[Camera](https://github.com/batuhancetinkaya1/Crowd-Runner-Clone/tree/main/Assets/Scripts/Camera)**: Handles camera movement and transitions.
- **[Door & Bonus](https://github.com/batuhancetinkaya1/Crowd-Runner-Clone/tree/main/Assets/Scripts/Door%26Bonus)**: Manages door mechanics and bonus collection.
- **[Enemy](https://github.com/batuhancetinkaya1/Crowd-Runner-Clone/tree/main/Assets/Scripts/Enemy)**: Defines enemy behavior and interactions.
- **[Game](https://github.com/batuhancetinkaya1/Crowd-Runner-Clone/tree/main/Assets/Scripts/Game)**: Core game logic and state management.
- **[Player](https://github.com/batuhancetinkaya1/Crowd-Runner-Clone/tree/main/Assets/Scripts/Player)**: Player controls and crowd management.
- **[Tile](https://github.com/batuhancetinkaya1/Crowd-Runner-Clone/tree/main/Assets/Scripts/Tile)**: Handles tile generation and properties.

### [Prefabs](https://github.com/batuhancetinkaya1/Crowd-Runner-Clone/tree/main/Assets/Prefabs)
Reusable prefabs for tiles, enemies, and bonuses.

### [Sprites](https://github.com/batuhancetinkaya1/Crowd-Runner-Clone/tree/main/Assets/Sprites)
Visual assets for the game.

## Future Improvements
- Add credits and controls panel in the main menu.
- Enhance fight mechanics with more animations and effects.
- Implement an online leaderboard for competitive gameplay.
- Polish the user interface for better player experience.

## License
This project is open source and available under the [MIT License](LICENSE). Feel free to use, modify, and distribute the code as per the license terms.

---

This game is a work in progress and will soon be available on [itch.io](https://itch.io). Stay tuned for updates!

