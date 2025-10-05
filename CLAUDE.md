# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository Overview

THREEMATCHlua is a Unity-based match-3 puzzle game project built with Unity 2022.3.13f1c1. The project integrates XLua for Lua scripting capabilities and uses the Universal Render Pipeline (URP). The codebase consists of several key components:

1. **Three Match Core** - The core match-3 game mechanics, organized in MVC architecture:
   - Model: Grid system, tile states, and matching logic
   - View: UI and visual representation of the tiles
   - Controller: Game logic and player input handling

2. **Octree and Pathfinding** - A spatial partitioning system and A* pathfinding implementation for navigation

3. **UI Framework** - A custom UI management system with panel stacking

4. **XLua Integration** - Lua scripting capabilities through XLua framework

## Development Environment

- Unity Editor: 2022.3.13f1c1
- Platform: Windows (win32)
- Render Pipeline: Universal Render Pipeline (URP)
- External Plugins: XLua, DOTween

## Building and Running the Project

### Opening the Project
1. Open Unity Hub and add the project by selecting the repository root directory
2. Open the project in Unity 2022.3.13f1c1 (or compatible version)
3. Allow Unity to import all assets and compile scripts

### Running the Game
1. Open the main scene located in Assets/Scenes
2. Click the Play button in the Unity Editor to run the game in the editor

### Building the Game
1. From Unity menu, select File > Build Settings
2. Configure platform settings as needed
3. Click "Build" or "Build and Run" to create an executable

## Project Architecture

### Game Structure
- `GameRoot.cs`: The main entry point and singleton for game management
- UI system uses a panel stack with BasePanel class derivatives
- Scene management through SceneControl
- Match-3 gameplay is managed by Match3GamePlay.cs

### Key Components

#### Three Match Core
- `Match3GamePlay.cs`: Controls game logic, player input, and state management
- `Grid2D.cs`: 2D grid system for tile placement
- `Tile.cs`: Visual representation of grid tiles
- `Move.cs`: Handles player moves and validates them
- `MatchPosition.cs`: Tracks matched tile positions

#### Octree and Pathfinding
- `Octree.cs`: Spatial partitioning system for efficient collision detection
- `Graph.cs`: Node-based graph for A* pathfinding
- `MoveMent.cs`: Handles entity movement along paths

#### UI System
- `UIManager.cs`: Manages UI panels with push/pop operations
- `BasePanel.cs`: Base class for all UI panels
- `UIMethods.cs`: Utility functions for UI operations

#### XLua Integration
- XLua framework is set up but appears to be in early integration stages
- No custom Lua scripts beyond examples are currently implemented

## Development Guidelines

### Code Organization
- Follow existing MVC pattern for new features
- Place new scripts in appropriate folders under Assets/Scripts
- Maintain separation between Model, View, and Controller components

### UI Development
- Create new panels by extending BasePanel class
- Register UI panels with UIManager
- Use the push/pop pattern for panel navigation

### Match-3 Extensions
- Extend TileState enum for new tile types
- Modify Match3GamePlay for new game mechanics
- Update Match3Skin for visual changes

### Lua Integration
- Place Lua scripts in appropriate folders under Assets/Resources
- Use XLua's LuaEnv to execute Lua code
- Follow XLua documentation for C#-Lua interoperation



PERSONAL WORKING PREFERENCES & RULES These are permanent behavioral rules that NEVER change Core Working Principles Never Guess or Assume - NEVER make assumptions about design patterns, architecture, or implementation - ALWAYS read the actual code before answering questions - When unclear, ask for clarification rather than guessing - If multiple approaches exist, present options - don't choose for the user Planning Before Implementation - Spend 90% of time aligning on approach before any coding - Get explicit permission before creating files or making changes - Complete all work in current session - no TODOs or placeholders - Follow user's exact request - no extras without permission Evidence-Based Problem Solving When debugging, always: 1. Read the exact error location and actual values 2. Add logging to see what's really happening 3. Follow data through the code - never theorize Never say "probably" or "likely" - only what the code proves. Architecture Compliance Mandatory SSOT/DRY Principles Every implementation MUST follow these principles (no exceptions without explicit permission): - Single Source of Truth (SSOT): One authoritative location for each piece of data across the entire codebase - Don't Repeat Yourself (DRY): No code duplication - extract and reuse common patterns Architecture-First Compliance Before implementing ANY new system or manager: 1. Search for Existing Solutions - Can existing managers be extended rather than replaced? 2. Integration Over Creation - Always extend existing systems when possible 3. Ask When Unsure - "Should I use [existing system] for this?" Red Flags to Avoid - Creating "MyNewManager" when "ExistingManager" handles similar data - Building parallel event systems when one already exists - Implementing new state tracking alongside existing state management Communication Standards Response Style - Be direct and conversational, not verbose - Skip unnecessary preambles and summaries - Use proper technical terms (not analogies) - Mark recommendations clearly as (Recommended) Task Execution Framework Logical Unit Breakdown Break complex tasks into coherent chunks that: - Group 3-7 related changes with one goal - Can be tested independently - Preserve context between connected parts Implementation Flow Complete each logical unit, report status with testing results, then wait for user confirmation before proceeding. Example: "Implement user authentication system" (includes models, controllers, routes, and UI) rather than separate steps for each component. Critical Constraints Never: Guess when uncertain, create without permission, leave incomplete work, make unilateral design decisions, continue without confirmation, add unrequested features, create parallel systems Always: Read existing code first, ask when design choices are unclear, search for existing systems before creating new ones, fix all breaking changes immediately, maintain code consistency, follow global SSOT/DRY ---  