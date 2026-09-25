# Sky: Dec: Beyond The Szpont

Hybrid roguelike game developed in Unity as a university team project.

The project combines three gameplay layers:

- procedurally generated **3D world map**,
- procedurally generated **2D dungeon crawler**,
- turn-based **3D card battles**.

The game uses additive scene loading to connect these systems while preserving player progression and game state. The card-game opponent is controlled using Unity ML-Agents.

## Gameplay

<p align="center">
  <a href="docs/videos/gameplay.mp4">▶ Watch gameplay</a>
</p>

<p align="center">
  <em>Short gameplay demonstration showing the 3D world map, 2D dungeon and card battle.</em>
</p>

## Key Features

- Procedural 3D world map
- Procedural 2D dungeons
- A* pathfinding
- Random Walk and Corridor First generation
- Turn-based card combat
- Unity ML-Agents opponent
- Additive scene loading
- Persistent game state between scenes
- Roguelike progression

## Technical Stack

- Unity 6 (6000.2.10f1)
- C#
- Unity ML-Agents
- Blender
- Git / GitHub
- ScriptableObjects

## Project Architecture

The project is divided into several interconnected gameplay systems:

- **3D World Map** – procedurally generated map with multiple paths and interactive locations.
- **2D Dungeon Module** – procedurally generated levels using Random Walk and Corridor First algorithms.
- **Card Battle System** – turn-based combat with decks, lanes, resources and boss encounters.
- **Scene Management** – additive scene loading used to switch between gameplay layers while preserving game state.
- **AI System** – Unity ML-Agents-based opponent used during card battles.
- **Progression System** – dungeon rewards affect the player's deck and influence upcoming boss encounters.

## How to Run

1. Clone the repository.
2. Open the project in Unity 6 (6000.2.10f1) or a compatible newer version.
3. Open:

   `Assets/Scenes/MenuScene`

4. Press **Play**.

## Team

- Paweł Ledwoń
- Alan Pawleta
- Bartosz Szwej
- Jakub Zając

Developed for the **Computer Game Programming** course at the Silesian University of Technology.
