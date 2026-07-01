https://github.com/user-attachments/assets/2db22141-3366-48ce-82bd-2a6df4985036

Game Project

Overview

A third-person action game featuring a robot-inspired character equipped with multiple firearms. The player can dual-wield two guns simultaneously but fires with one active weapon at a time, switching between them as needed.

Core Systems

1. Character


Robot-inspired design
Can hold two guns at once (dual wield)
Only one gun is active/firing at a time; the second is held in reserve
Supports switching the active weapon mid-combat


2. Weapon / Gun System


Each gun has its own bullet capacity (magazine size)
Tracks current ammo count per weapon
Handles reload logic once capacity reaches zero
Supports multiple gun types, each with independent stats (capacity, fire rate, etc.)


3. Fire System


Manages firing logic: input handling, fire rate, muzzle spawn point
Spawns bullets from the currently active gun
Reduces ammo count from the gun's capacity on each shot
Triggers reload/empty state when capacity hits zero


4. Bullet Object Pool


Object pooling system to manage bullet instances efficiently
Reuses bullet objects instead of constantly instantiating/destroying them
Improves performance during high fire-rate sequences
Pool grows/shrinks or recycles based on active bullet count


5. Object Interaction


Fired bullets interact with objects in the scene (collision detection)
Determines hit object type and applies appropriate response (damage, physics reaction, destruction, etc.)
Returns bullet to the object pool after impact


6. VFX System


Spawns visual effects on bullet impact (hit sparks, impact decals, particle bursts)
VFX triggered based on the type of object hit (e.g., metal vs. environment)
Effects are pooled or cleaned up after playback to avoid memory overhead
