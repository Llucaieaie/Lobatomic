# SpookyStudios

Lobatomic Online - Final Delivery Report


Team Members: Marc Escandell, Lluc Estruch, Daniel Manas

Game Premise:
You play as a scientist diving into the minds of patients to eliminate harmful thoughts while preserving the good ones. Your goal is to restore happiness to the patients by destroying negative thought cores without causing excessive damage to the positive ones. In the online version, one of the players focuses on destroying yellow cores while the other one destroys the purple ones.


Controls: WASD, arrow keys, or Xbox controller.


Networking Features

For this final delivery, our team focused on creating a robust and playable online multiplayer experience for Lobatomic, attempting to implement all the key networking concepts we covered in class.

Here's a breakdown of our work:

- World State Replication Overhaul
- We revamped the World State Replication system to synchronize the positions of each tile instead of replicating entire tiles.
- This change improved the accuracy of level generation and ensured both clients shared identical environments during gameplay.

Challenges addressed:
- Real-time synchronization of random level generation.
- Avoiding inconsistencies in tile layout between clients.
- Latency and Jitter Mitigation


To ensure smooth gameplay and handle network latency:

- Interpolation:
Implemented interpolation for player and object positions to create smoother transitions on the client side.

- Client-Side Prediction:
Used predictive techniques to maintain responsive controls even with minor delays in packet delivery.

- Lag Simulation:
Artificial lag was added to simulate real-world network conditions, allowing us to rigorously test our mitigation strategies.

- Matchmaking
A simple matchmaking system was created to allow players to find opponents in 1v1 sessions.
Players are paired based on availability, and sessions are established dynamically.


Contributions
- Marc Escandell
Contributed to the redesign and implementation of the World State Replication system.
Worked on interpolation techniques to smooth movement for both players and objects.

- Lluc Estruch
Focused on matchmaking implementation and integrating it into the overall game flow.
Helped fine-tune latency simulation and validate the effectiveness of lag mitigation strategies.

- Daniel Manas
Led the client-side prediction development to enhance the responsiveness of game actions.
Worked on integrating the replicated tile system into the game engine and ensuring consistency across clients.

- Joint Efforts
All members collaborated extensively to debug, test, and refine the networking code, ensuring a polished and synchronized experience.


Improvements From Previous Delivery
- World State Synchronization:
Transitioned from tile replication to position-based replication, solving major issues with desynchronized levels.

- Performance and Responsiveness:
Introduced client-side prediction and interpolation to create a seamless and fluid experience.

- Debugging and Testing:
Simulated network lag and jitter to validate mitigation techniques under real-world conditions.