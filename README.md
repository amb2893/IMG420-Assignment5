# IMG420 - Assignment 5

A small but refined tech demo built with **Godot**.  



## How to Play  
- **Movement** Use the **Arrow Keys** (← →) to move .  

## Installation  
1. Download or clone this repository.  
2. Open **Godot**.  
3. Import the project:  
   - Either select the **project.godot** file  
   - Or import directly from the downloaded ZIP.  
4. Run the project to start playing.  


## Assignment goals 
### Main Goals

## Part 1 – Shader & Particles (3 points)
- **Custom shader correctly applied** ✅  
  - `ParticleController` assigns `custom_particle.gdshader` to the `GpuParticles2D` material.  
- **Wave distortion working** ✅  
  - The shader uses `uv.x += sin(uv.y * 10.0 + TIME) * wave_intensity;` to create horizontal waves along the vertical axis.  
  - `wave_intensity` is animated in `_Process()`, producing dynamic motion over time.  
- **Color gradient implemented** ✅  
  - `mix(color_start, color_end, uv.y)` produces a vertical gradient from top (`color_start`) to bottom (`color_end`).  
- **Time-based animation** ✅  
  - The shader uses `TIME` in the sine function, and the particle controller updates `wave_intensity` each frame, creating continuous animation.

---

## Part 2 – Physics & Joints (3.5 points)
- **Multiple rigid bodies created** ✅  
  - `_segments` list instantiates multiple `RigidBody2D` nodes as chain segments.  
- **Joints properly configured** ✅  
  - Each segment is connected via `PinJoint2D` to the previous segment or the static `_anchor`.  
- **Realistic physics behavior** ✅  
  - Segments have `GravityScale`, `LinearDamp`, and `AngularDamp` set for smooth hanging motion.  
  - The top `_anchor` is a `StaticBody2D`, preventing the chain from falling.  
- **Player interaction working** ✅  
  - `ApplyForceAtPoint()` applies impulses to nearby segments.  
  - Neighboring segments are also nudged to make the chain move cohesively when pushed.

---

## Part 3 – Raycasting (3.5 points)
- **Raycast correctly implemented** ✅  
  - `LaserDetector` creates a `RayCast2D`, updates it each `_PhysicsProcess()`, and checks collisions with the player.  
- **Visual laser beam** ✅  
  - `Line2D` updates dynamically to show the ray and collision point.  
- **Player detection working** ✅  
  - The raycast detects `_player` and triggers the alarm.  
- **Alarm system functional** ✅  
  - `TriggerAlarm()` changes the laser color and prints an alert; `_alarmTimer` resets the laser to normal after 1 second.

---


## Video of Game
Here is the URL of my recording of me playing the game. [Zoom Recording Link](https://nau.zoom.us/rec/share/D2V_0p38S_gMCOsQTHQh8EBXSFCavVMFbTUMkfB1pOxtaFTYjIxOm1EiPOXA9ZS7.v4kQcWxIY8CpLJqV?startTime=1762308624000)
## Credits  
Developed as part of **IMG420 - Assignment 5**.  
Built with the [Godot Engine](https://godotengine.org/).  
