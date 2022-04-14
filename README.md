# Star Fox SNES Clone in Unity
This is a clone fo the SNES version of Star Fox programmed in Unity. ```/Assets/Scripts``` contains all the game scripts.
```/Assets/Models``` contains all the models. The initial project only contained the game scripts however it seemed 
to make aa lot more sense to include everything in the repository. Therefore it was moved to a new one.

## Scene
The player does not actually move forward but the map in negative z-direction creating the illusion 
of a forward flying ship. Some object locations are bound to the moving map (like obstacles and ground enemies)
while some are free (like flying enemies, most projectiles). 

Every object is active only if it is in a certain "box" surrounding 
the player. If the object is too far away in negative z-direction or if the player just passed it the object is
disabled.

The player slightly tilts left and right to give an illusion of floatiness.

## Player
The player works by lineary moving to a target vector depending on the controller stick position. 
She receives damage for colliding with objects, enemies or shots.

## Enemies
### Tanks
Tanks are ground enemies which a shoot a slower directly to the player and keeping the direction.
The slow projectiles compensate with larger damage.

### Flying Enemies
They appear from behind flying in z-direction first then tilting left or right to face the player 
and match her level above the ground. After the enemies shot a short amount of time they face 
to negative z-direction and fly away.

### Boss
The boss has four states:
 - State 1 spawns following enemies that shoot from the bosses "hangar".
 - State 2 spawns following, quick rockets but with less ability to steer.
 - State 3 lets the boss tilt to one side like she is protecting herself. 
   This state only appears if the hangar was destroyed and the rocket launchers are
   intact.
 - State 4 moves to the player and back and forth. While moving towards to the
   player it shoots slow heavy shots to the player
  
   
