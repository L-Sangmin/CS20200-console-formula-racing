**Project Title**: Console Formula Racing

**Overview**: This project is a command-line lap racing game inspired by Formula 1 (F1), where 2 to 4 teams race on a linear track. Each team uses tire cards to complete a fixed number of laps as quickly as possible. The game includes simplified rules for pit stops, weather, and passive team abilities.

**Requirements**:
1. When the game starts, the user will choose the total number of teams, from 2 to 4.
2. The user will choose one passive ability for the user’s team.
3. The user will control one team, and all other teams will be controlled by simple AI opponents.
4. The passive abilities of AI opponents will be assigned randomly.
5. Before the race begins, the user will choose one starting tire type - which has different movements according to its type - for the user’s team.
6. After setup is complete, the user will see a linear track in the terminal.
7. At the start of the race, all teams will be shown at the start/finish line.
8. The terminal will show the turn order, live standings, and current weather.
9. On the user’s turn, the user will choose one tire card from the hand and discard it to move forward.
10. The amount of movement from a tire card will depend on the selected tire card and the current weather.
11. After each team completes its turn, the user will see the updated track in the terminal.
12. At the beginning of each round, the weather will be changed randomly - except for the first round; it is fixed as Sunny - to Sunny or Rainy, and the result will be shown in the terminal.
13. If a team has no tire cards left, that team will move with basic movement with less than half of other tire card's.
14. If a team’s movement passes the pit entry during its turn, that team may choose to enter the pit by entering option in the terminal.
15. If a team chooses to enter the pit, that team must stop at the Pit space even if it still has remaining movement.
16. If a team enters the pit, that team will choose a tire type and receive new tire cards of that type.
17. On the next turn in the pit, the team will leave the pit with using a tire card as usual.
18. When a team crosses the start/finish line (include pit), that team’s lap count will increase by 1.
19. The live standings shown in the terminal will be updated when lap counts or finishing order change.
20. When a team completes the target number of laps, that team finishes the race.
21. The race will end when all teams finish, and the terminal will show the final result in finishing order.


**Example Interaction**:

**Setup**: 
The game asks the user to choose the number of teams, a passive ability for the user’s team, and a starting tire type by entering the option in the terminal. The game then assigns AI teams, determines the turn order, and prints the initial track and weather.

**Race**:
The game prints the current track and the user team’s cards. The user chooses a tire card to move forward on their turn by entering the option in the terminal. If the movement can reach the pit entry, the user may choose to enter the pit and stop there. After the move, the game updates the track and standings, the AI teams take their turns, and the weather is updated at the beginning of the next round.