# SpookyStudios

Lobatomic Online - Final Delivery Report


Team Members: Marc Escandell, Lluc Estruch, Daniel Manas

Game Premise:
You play as a scientist diving into the minds of patients to eliminate harmful thoughts while preserving the good ones. Your goal is to restore happiness to the patients by destroying negative thought cores without causing excessive damage to the positive ones. In the online version, one of the players focuses on destroying yellow cores while the other one destroys the purple ones.

Controls: WASD, arrow keys, or Xbox controller.


###Features

For this delivery, our team focused on creating artificial jitter and reducing it via position interpolation and apply feedback of previous deliveries.

Here's a breakdown of our work:
- World State Replication Overhaul
- We revamped the World State Replication system to work with tiles positions instead of an ID for each tile.
- This change improved the accuracy of level generation and overall framerate, as the code works faster now, ensuring both clients shared identical environments during gameplay.

Challenges addressed:
- Synchronization of random level generation.
- Faster tile searching and destroying algorythms.
- When the other player leaves the game, a pop-up is shown indicating that them left and loads the lobby again.
- Latency and Jitter Mitigation


To ensure smooth gameplay and handle network latency:

- Lag Simulation:
Artificial lag was added to simulate real-world network conditions, allowing us to rigorously test our mitigation strategies.

- Interpolation:
Implemented interpolation for player positions to create smoother transitions on both sides.

Contributions
- Marc Escandell
Contributed to the redesign and implementation of the World State Replication system.

- Lluc Estruch
Focused on gathering feedback and integrating it into the game.

- Daniel Manas
Worked on lag simulation and interpolation techniques to smooth movement for players.

- Joint Efforts
All members collaborated extensively to debug, test, and refine the networking code, ensuring a polished and synchronized experience.


Improvements From Previous Delivery
- World State Synchronization:
Transitioned from tile replication to position-based replication, solving major issues with desynchronized levels.

- Performance and Responsiveness:
Introduced interpolation to create a seamless and fluid experience.

- Debugging and Testing:
Simulated network lag and jitter to validate mitigation techniques under real-world conditions.

mondongo