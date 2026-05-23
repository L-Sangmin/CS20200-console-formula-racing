# Console Formula Racing - Implementation Guide

This document extracts only the rules needed to implement **Console Formula Racing** from the original K.F.C. rulebook, based on the final selected requirements.[1][2]

## Purpose

This guide is intended for implementation. It removes rules that are not required by the final requirements, such as battle, roulette, and complex team-specific rule exceptions, and keeps only the parts that are necessary for the command-line version described in the requirements.[1][2]

## Scope

The final game is a command-line lap racing game where 2 to 4 teams race on a linear track, use tire cards to move, may enter the pit to replenish tires, are affected by Sunny or Rainy weather, and finish after completing the target number of laps.[2]

## Rules to Keep

### 1. Game setup

- The game asks the user to choose the total number of teams, from 2 to 4.[2]
- The user chooses one passive ability for the user team.[2]
- The user controls one team, and all other teams are AI teams.[2]
- The passive abilities of AI teams are assigned randomly.[2]
- Before the race begins, the user chooses one starting tire type for the user team.[2]
- After setup is complete, all teams are placed at the start-finish line, and the game prints the initial track, turn order, live standings, and current weather.[2]

### 2. Track model

- The track is linear.[2]
- The terminal must show the track at all times after setup and after each team finishes a turn.[2]
- The track contains at least these special locations: start-finish line, pit entry and pit space, and normal spaces.[1][2]
- All teams start on the start-finish line.[2]

### 3. Weather

- Weather is either Sunny or Rainy.[1][2]
- The first round is always Sunny.[1][2]
- At the beginning of each later round, the weather changes randomly to Sunny or Rainy, and the result is printed in the terminal.[1][2]
- Tire movement depends on the current weather.[1][2]

### 4. Tire types and movement

- Tire types are Soft, Medium, and Hard.[1]
- At the start and whenever a team replenishes in the pit, that team chooses one tire type and receives tire cards of that type.[1]
- Each tire type has different movement values depending on weather.[1][2]
- On a team turn, if the team has any tire cards, it uses one tire card and discards it to move forward.[1][2]
- If a team has no tire cards left, that team moves with basic movement instead.[1][2]
- In the original rulebook, basic movement is 3 in Sunny and 2 in Rainy.[1]

### 5. Recommended tire numbers

The original rulebook provides these average movement and replenishment counts.[1]

| Tire Type | Avg. Movement in Sunny | Avg. Movement in Rainy | Cards Received |
|---|---:|---:|---:|
| Soft | 9 | 4 | 3 |
| Medium | 7 | 4 | 5 |
| Hard | 6 | 3 | 10 |

The original rulebook also distinguishes Fresh, Used, and Old cards within one tire type, but the final requirements do not require this extra layer.[1][2]
So for implementation, it is reasonable to simplify each tire type into a single movement profile unless a more detailed tire deck is intentionally added.[1][2]

### 6. Pit rules

- If a team’s movement passes the pit entry while moving forward during its turn, that team may choose to enter the pit.[2]
- This follows the original rulebook idea that the pit is accessed through a pit entry path rather than only by exact landing on a pit tile.[1]
- If a team chooses to enter the pit, that team must stop at the pit space even if it still has remaining movement.[1][2]
- When a team enters the pit, that team chooses a tire type and receives new tire cards of that type.[1][2]
- On the next turn in the pit, the team leaves the pit by using a tire card as usual, according to the final requirements.[2]

### 7. Lap counting and standings

- When a team crosses the start-finish line, that team’s lap count increases by 1.[2]
- This includes the case where the team leaves the pit and crosses the line as part of its movement.[2]
- The live standings shown in the terminal are updated whenever lap counts or finishing order change.[2]
- When a team completes the target number of laps, that team finishes the race.[2]
- The race ends when all teams finish, and the terminal prints the final result in finishing order.[2]

### 8. Terminal output requirements

The terminal output should always make the game state observable for evaluation.[2]
The implementation should therefore show at least the following information clearly:[2]

- current weather
- current turn order
- live standings
- each team position on the track
- each team lap count
- the user team’s tire cards
- messages for pit entry, pit replenishment, lap completion, and final ranking

## Rules to Remove

The following original K.F.C. rules are **not needed** for the final selected requirements and should be omitted from implementation unless intentionally added later as an extra feature.[1][2]

### 1. Item system

- The original rulebook contains item spaces, item cards, timing rules, target rules, and a large card guidebook.[1]
- The final selected requirements do not include item drawing, item usage, temporary item effects, or hidden AI item information.[2]
- Therefore, the item system should be removed entirely from the implementation scope.[1][2]

### 2. Roulette and event spaces

- The original rulebook includes roulette or event spaces with die-based effects such as movement reduction or multi-turn bonuses.[1]
- These are not required by the final selected requirements.[2]
- Therefore, roulette or event spaces should be omitted.[1][2]

### 3. Battle system

- The original rulebook includes battle when multiple teams occupy the same space, using rock-paper-scissors and attacker-defender rules.[1]
- The final selected requirements do not mention battle.[2]
- Therefore, multiple teams may simply coexist on the same track position in the command-line implementation.[1][2]

### 4. Complex team-specific abilities

- The original rulebook defines six named teams with highly specific special abilities.[1]
- The final requirements only require that the user choose one passive ability and AI teams receive random passive abilities.[2]
- Therefore, only a simplified passive ability system is needed, not the full original team table.[1][2]

### 5. Pit exit die roll system

- The original rulebook says that, on the next turn in the pit, a team draws an item card and attempts to exit by rolling a die, with 4 or higher as the first success threshold.[1]
- The final selected requirements simplified this and state that, on the next turn in the pit, the team leaves the pit with using a tire card as usual.[2]
- Therefore, the die-roll pit exit system should be removed in the final implementation if the implementation is intended to follow the final requirements exactly.[1][2]

## Simplified implementation rules

If the goal is to follow the final requirements exactly, the implementation can be summarized like this:[2]

1. Setup asks for team count, user passive ability, and starting tire type.[2]
2. All teams start at the start-finish line, and the first round begins in Sunny weather.[2]
3. On each turn, a team uses one tire card to move, or basic movement if no tire cards remain.[1][2]
4. Tire movement depends on both tire type and weather.[1][2]
5. If movement passes the pit entry, the team may enter the pit and stop there immediately.[1][2]
6. A team that enters the pit chooses a tire type and receives new tire cards.[1][2]
7. On the next turn in the pit, the team exits and moves using a tire card normally.[2]
8. If a team crosses the start-finish line, its lap count increases by 1.[2]
9. Standings are updated whenever lap counts or finishing order change.[2]
10. The race ends when all teams complete the target number of laps.[2]

## Suggested implementation constants

These values are consistent with the original rulebook and can be used directly unless intentionally adjusted.[1]

- Lap target: 3
- Weather values: Sunny or Rainy
- First round weather: Sunny
- Basic movement: Sunny = 3, Rainy = 2
- Soft: Sunny = 9, Rainy = 4, cards = 3
- Medium: Sunny = 7, Rainy = 4, cards = 5
- Hard: Sunny = 6, Rainy = 3, cards = 10

## Example implementation flow

### Setup

The game asks the user to choose the number of teams, a passive ability for the user’s team, and a starting tire type by entering the option in the terminal.[2]
The game then assigns AI teams, determines the turn order, and prints the initial track and weather.[2]

### Race

The game prints the current track and the user team’s cards.[2]
The user chooses a tire card to move forward on their turn by entering the option in the terminal.[2]
If the movement can reach the pit entry, the user may choose to enter the pit and stop there.[2]
After the move, the game updates the track and standings, the AI teams take their turns, and the weather is updated at the beginning of the next round.[2]