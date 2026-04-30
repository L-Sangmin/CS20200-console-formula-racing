**Project Title**: Console Formula Racing

**Overview**: This project is a command-line lap racing game inspired by Formula 1 (F1), where 2 to 4 teams race on a linear track. Each team uses tire cards for movement and item cards for various effects to complete a fixed number of laps as quickly as possible. The game includes simplified rules for pit stops, weather, and passive team abilities.

**Requirements**:
1. When the game starts, the user will choose the total number of teams, from 2 to 4.
2. The user will choose one passive ability for the user’s team.
3. The user will control one team, and all other teams will be controlled by simple AI opponents.
4. The passive abilities of AI opponents will be assigned randomly.
5. Before the race begins, the user will choose one starting tire type for the user’s team.
6. After setup is complete, the user will see a linear track in the terminal.
7. At the start of the race, all teams will be shown at the start/finish line.
8. The terminal will show the turn order, live standings, and current weather.
9. On the user’s turn, the user will choose one tire card from the hand and discard it to move forward.
10. The amount of movement from a tire card will depend on the selected tire card and the current weather.
11. On the user’s turn, the user may use item cards if their usage conditions are satisfied.
12. If an item card creates a temporary effect, the game will keep that effect active for the required number of turns and remove it automatically when the effect expires.
13. After each team completes its turn, the user will see the updated track in the terminal.
14. At the beginning of each round, the weather will be changed randomly to Sunny or Rainy, and the result will be shown in the terminal.
15. If a team has no tire cards left, that team will move with basic movement.
16. If a team arrives on an item space, that team will draw one item card.
17. If the user’s team draws an item card, the user will see that card in the terminal.
18. If an AI team draws an item card, the terminal will show only that the AI team drew an item card.
19. If a team’s movement passes the pit entry during its turn, that team may choose to enter the pit.
20. If a team chooses to enter the pit, that team must stop at the Pit space even if it still has remaining movement.
21. If a team enters the pit, that team will choose a tire type and receive new tire cards of that type.
22. On the next turn in the pit, the team will draw one item card and attempt to exit the pit by a random die roll.
23. If the pit exit roll succeeds, the team will immediately leave the pit and move by using a tire card. Otherwise, the team will remain in the pit.
24. When a team crosses the start/finish line, that team’s lap count will increase by 1.
25. The live standings shown in the terminal will be updated when lap counts or finishing order change.
26. When a team completes the target number of laps, that team finishes the race.
27. The race will end when all teams finish, and the terminal will show the final result in finishing order.


**Example Interaction**:

**Setup**: 
The game asks the user to choose the number of teams, a passive ability for the user’s team, and a starting tire type by entering the option in the terminal. The game then assigns AI teams, determines the turn order, and prints the initial track and weather.

**Race**:
The game prints the current track and the user team’s cards. The user may use an item card, then chooses a tire card to move forward on their turn by entering the option in the terminal. If the movement can reach the pit entry, the user may choose to enter the pit and stop there. After the move, the game updates the track and standings, the AI teams take their turns, and the weather is updated at the beginning of the next round.