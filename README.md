# breakout_clone

This is just a simple Breakout clone I built from scratch in C# and WinForms. It was my second game project; something I just used to practice core programming projects.

Features
========

- Paddle and ball mechanics with custom collision handling
- Brick grid with point values per row
- Simple sound effects (paddle, wall, and brick hits)
- Two rounds
- A wave-like brick draw animation
- Score and life tracking
- Pause functionality
- Ability to restart game upon losing or winning

Controls
========

- Space — Launch ball to start round
- Left/Right arrow keys — Move paddle
- Esc — Pause/Resume

Thing I learned
================

- How to resolve overlaps with collision detection
- Handling game states (rounds, lives, restart)
- How small issues like DPI scaling can impact player experience
- To get the game in a playable state before coding half of it...

Comments
========

It's not a perfect Breakout game—there are issues like he ball occasionally breaking through multiple layers of bricks despite specifically addressing hat in the code and some text scaling issues on sysetms with scaled resolutions—but the real goal of this was to learn, not to win Game of the Year. I might make one more game from scratch before I start using Unity. I also planned to include music and so I have a bit of code for controlling volume in it (just one line I think—I didn't program actual volume controls) though I ended up not doing it. I'll add it to the next game project.
