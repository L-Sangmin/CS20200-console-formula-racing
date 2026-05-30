**Project Title**: Console Formula Racing

**Overview**: This project is a command-line lap racing game inspired by Formula 1 (F1), where 2 to 4 teams race on a linear track. Each team uses tire cards to complete a fixed number of laps as quickly as possible. The game includes simplified rules for pit stops, weather, and passive team abilities.

**Requirements**:
1. When the game starts, the user will choose the total number of teams, from 2 to 4.

2. The user will choose one passive ability for the user's team. The four passive abilities are:
   - **Warm Tires**: +2 movement when playing a Soft tire card, in any weather.
   - **Rain Engineer**: +2 movement when playing any tire card in Rainy weather.
   - **Pit Crew**: receive 2 additional tire cards when replenishing in the pit.
   - **Endurance**: +2 movement when using basic movement (i.e., no tire cards remaining).

3. The user will control one team, and all other teams will be controlled by simple AI opponents.

4. The passive abilities of AI opponents will be assigned randomly.

5. Before the race begins, the user will choose one starting tire type for the user's team. The three tire types, their movement values, and starting card counts are:

   | Tire   | Sunny move | Rainy move | Cards |
   |--------|------------|------------|-------|
   | Soft   | 7          | 4          | 3     |
   | Medium | 6          | 3          | 5     |
   | Hard   | 5          | 3          | 10    |

   Each tire card has a pre-determined movement variance of −1, 0, or +1 (with 1:2:1 probability) applied only in Sunny weather. In Rainy weather, variance is ignored and only the base tire movement applies.

6. After setup is complete, the user will see a linear track in the terminal.

7. At the start of the race, all teams will be shown at the start/finish line.

8. The terminal will show the turn order, live standings, and current weather.

9. On the user's turn, the user will choose one tire card from the hand and discard it to move forward. The hand is displayed with each card's move value for the current weather before the user picks. Cards are numbered and the user selects by entering a number.

10. The amount of movement from a tire card will depend on the selected tire card and the current weather. Movement = base tire value ± variance (Sunny only) + passive bonus (if applicable), with a minimum of 1.

11. After each team completes its turn, the user will see the updated track in the terminal. To reduce excessive output, AI turns are executed silently and their summaries are displayed together at the top of the next human player's turn. The track is fully updated and shown at the start of each human turn, reflecting all moves since the last display.

12. At the beginning of each round, the weather will be changed randomly — except for the first round, which is fixed as Sunny — to Sunny or Rainy, and the result will be shown in the terminal.

13. If a team has no tire cards left, that team will move with basic movement: 3 squares in Sunny weather and 2 squares in Rainy weather. The Endurance passive adds +2 to basic movement. No card is discarded on a basic movement turn.

14. If a team's movement passes the pit entry during its turn, that team may choose to enter the pit by entering an option in the terminal. The pit entry is located at position 18 on the track.

15. If a team chooses to enter the pit, that team must stop at the Pit space even if it still has remaining movement.

16. If a team enters the pit, that team will choose a tire type and receive new tire cards of that type. The number of new cards equals the card count for the chosen tire type (see Req 5 table), plus 2 if the team has the Pit Crew passive. Any tire cards already in the team's hand are kept; the new cards are added to the hand.

17. On the next turn in the pit, the team will leave the pit: the team is automatically placed at position 0 (the start/finish line), gaining +1 lap, and then immediately plays a tire card from their hand to move forward from position 0, as in a normal turn.

18. When a team crosses the start/finish line (including pit exit), that team's lap count will increase by 1.

19. The live standings shown in the terminal will be updated when lap counts or finishing order change.

20. When a team completes 3 laps, that team finishes the race. Finished teams are no longer shown on the track and their final rank is recorded and displayed in standings.

21. The race will end when all teams finish, and the terminal will show the final result in finishing order.


**Example Interaction**:

**Setup**:
The game asks the user to choose the number of teams (1–3 AI opponents), a passive ability for the user's team (four options numbered 1–4), and a starting tire type (three options with movement values shown). The game then assigns AI teams random passives and tires, determines the turn order randomly, and prints the initial track and weather (Sunny).

**Race**:
The game prints the current track, standings, and the user team's cards with their movement values for the current weather. The user enters a card number to move forward. If the move passes position 18, the user is offered the option to enter the pit. After the user's move, AI teams take their turns silently. At the start of the user's next turn, a summary of all AI moves since the last display is shown, followed by the updated track. Weather is re-rolled at the beginning of each new round.
