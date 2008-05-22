*** GAMMA DRACONIS ***
     Readme.txt

Overview
========

Gamma Draconis is built on Microsoft XNA Game Studio 2.0, with LuaInterface 2.0.1,
a C# wrapper for Lua 5.1, used for scripting tracks, weapons, ships, and the HUD.


Project Structure
=================

Core/
- Contains core game components, including the engine, renderer, input manager,
  sound manager, race manager, and Lua handler components.

Interfaces/
- Includes Lua definitions for the HUD and a debugging interface.

Lua/
- InterfaceComponents includes the ship positioning arrows and status bar HUD component
  definitions
- MapBuilders contains utilities for automating elements of track creation.
- Ships contains Lua definitions of our two ships, Raptor and Thor.
- Things contains Lua definitions of various components seen in our tracks, such as
  asteroids, checkpoints, planets, and dust.
- Weapons contains definitions of our laser guns, mines, and missiles.

Maps/
- Contains each level in it's own subdirectory.
- Somebody's Refuge is our primary track, featuring an "indoor" element as well as lots
  of things in open space.
- Circle Track was our test track, it is just a basic loop. 

Resources/
- Contains sounds. music, textures, and effects.

Screens/
- Contains the various "screens" used by our engine. GameScreen is the screen used
  while playing, other screens exist for menus, loading, displaying race results,
  and displaying control schemes.


Credits
=======

Joe Andrusyszyn	- Track design, engine development, collision, HUD arrow.
	joseph.andrusyszyn@gmail.com

Pat Dobson	- Track design, HUD elements, track creation tools.
	pjd6457@rit.edu

Josh Gruenberg	- Player join screen, HUD elements, reset button.
	jrg0805@rit.edu

John Reese	- Physics engine, rendering system, 3D models, weapons, audio engine.
	jreese@leetcode.net

Brian Schroth	- Collision detection, checkpoint management.
	bcs5888@rit.edu

Matt Sokol	- Octree implementation, sound effects, HUD arrow.
	viperxpyro@gmail.com

--

Volition Inc. / Freespace2 - Music tracks
	http://www.freespace2.com (currently unavailable)

SoundSnap.com	- Original sound effects
	http://www.soundsnap.com