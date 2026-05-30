# CS20200-console-formula-racing
This repository contains console based racing game which is motivated with Formula 1 (shortly, F1). Use various card to complete designated lap counts the fastest among the opponents!

## How to Run

Requires [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
git clone https://github.com/L-Sangmin/CS20200-console-formula-racing.git
cd CS20200-console-formula-racing
dotnet run
```

Enter `!q` at any prompt to quit.

## How to Play

All choices are made by entering a single natural number. Enter `!q` at any prompt to quit.

**Setup** (done once before the race):

1. **Number of AI opponents** — choose 1, 2, or 3 (total teams: 2–4)
2. **Passive ability** — choose one of four abilities that applies a bonus throughout the race:
   - Warm Tires (+2 move with Soft tires), Rain Engineer (+2 move in Rainy), Pit Crew (+2 cards on pit stop), Endurance (+2 basic move when hand is empty)
3. **Starting tire type** — choose Soft (fast, few cards), Medium (balanced), or Hard (slow, many cards); move values for each weather are shown

After you finished the settings, you may see the terminal interface as below.
```text
--------------------------------------------------
  Turn order
--------------------------------------------------
  Redbull [CPU]
  You [YOU]
--------------------------------------------------

 · Redbull's turn... done

------------------------------------------------------------------------------------------
 Round 1  | Weather: Sunny | Turn: You (YOU)
------------------------------------------------------------------------------------------
 AI turns:
  Redbull    [Random|Rain Engineer]: pos 5 (Lap 0/3)
------------------------------------------------------------------------------------------
 TRACK:
        │ pit exit (+1 lap)
        ▼
     ┌──00──┐┌──01──┐┌──02──┐┌──03──┐┌──04──┐┌──05──┐┌──06──┐┌──07──┐┌──08──┐┌──09──┐    
     │      ││      ││      ││      ││      ││      ││      ││      ││      ││      │    
 ┌─▶ │ A    ││ .... ││ .... ││ .... ││ .... ││ B    ││ .... ││ .... ││ .... ││ .... │ ──┐
 │   │      ││      ││      ││      ││      ││      ││      ││      ││      ││      │   │
 │   └──────┘└──────┘└──────┘└──────┘└──────┘└──────┘└──────┘└──────┘└──────┘└──────┘   │
 │                                                                                      │
─┼─ Start/Finish (+1 lap if you cross it)                                               │
 │                                                                                      │
 │   ┌──19──┐┌──18──┐┌──17──┐┌──16──┐┌──15──┐┌──14──┐┌──13──┐┌──12──┐┌──11──┐┌──10──┐   │
 │   │      ││      ││      ││      ││      ││      ││      ││      ││      ││      │   │
 └── │ .... ││ .... ││ .... ││ .... ││ .... ││ .... ││ .... ││ .... ││ .... ││ .... │ ◀─┘
     │      ││      ││      ││      ││      ││      ││      ││      ││      ││      │    
     └──────┘└──────┘└──────┘└──────┘└──────┘└──────┘└──────┘└──────┘└──────┘└──────┘    
                │
                ▼  pit entry
------------------------------------------------------------------------------------------
STANDINGS:
  1. Redbull[CPU]                  Lap 0/3  Pos  5  Hard x9
  2. You                           Lap 0/3  Pos  0  Soft x3
------------------------------------------------------------------------------------------
 YOUR HAND [+2 for Soft tires]
  Soft[7] +2 | Soft[7] +2 | Soft[6] +2
------------------------------------------------------------------------------------------
```

**Race loop** (repeats until all teams finish 3 laps):

4. **View the track** — the current positions of all teams, live standings, weather, and your hand are shown
5. **Pick a card** — choose a numbered card from your hand; each card shows its exact move value for the current weather
6. **Pit entry** — if your move passes position 18, you are asked whether to enter the pit (y/n)
   - If yes: choose a tire type and receive new cards; your existing cards are kept
   - On your next turn, you automatically exit the pit to position 0 (+1 lap) and play a card as normal
7. **Basic move** — if your hand is empty, you move automatically with basic movement (no card discarded)
8. **AI turns** — AI teams move silently; their summaries appear at the top of your next turn

In every turn of yours, you can see the updated terminal as below.
```text
*** Round 2 begins — Weather: Sunny ***

 · Redbull's turn... done

------------------------------------------------------------------------------------------
 Round 2  | Weather: Sunny | Turn: You (YOU)
------------------------------------------------------------------------------------------
 AI turns:
  Redbull    [Random|Rain Engineer]: pos 10 (Lap 0/3)
------------------------------------------------------------------------------------------
 TRACK:
        │ pit exit (+1 lap)
        ▼
     ┌──00──┐┌──01──┐┌──02──┐┌──03──┐┌──04──┐┌──05──┐┌──06──┐┌──07──┐┌──08──┐┌──09──┐    
     │      ││      ││      ││      ││      ││      ││      ││      ││      ││      │    
 ┌─▶ │ .... ││ .... ││ .... ││ .... ││ .... ││ .... ││ .... ││ .... ││ .... ││ A    │ ──┐
 │   │      ││      ││      ││      ││      ││      ││      ││      ││      ││      │   │
 │   └──────┘└──────┘└──────┘└──────┘└──────┘└──────┘└──────┘└──────┘└──────┘└──────┘   │
 │                                                                                      │
─┼─ Start/Finish (+1 lap if you cross it)                                               │
 │                                                                                      │
 │   ┌──19──┐┌──18──┐┌──17──┐┌──16──┐┌──15──┐┌──14──┐┌──13──┐┌──12──┐┌──11──┐┌──10──┐   │
 │   │      ││      ││      ││      ││      ││      ││      ││      ││      ││      │   │
 └── │ .... ││ .... ││ .... ││ .... ││ .... ││ .... ││ .... ││ .... ││ .... ││ B    │ ◀─┘
     │      ││      ││      ││      ││      ││      ││      ││      ││      ││      │    
     └──────┘└──────┘└──────┘└──────┘└──────┘└──────┘└──────┘└──────┘└──────┘└──────┘    
                │
                ▼  pit entry
------------------------------------------------------------------------------------------
STANDINGS:
  1. Redbull[CPU]                  Lap 0/3  Pos 10  Hard x8
  2. You                           Lap 0/3  Pos  9  Soft x2
------------------------------------------------------------------------------------------
 YOUR HAND [+2 for Soft tires]
  Soft[7] +2 | Soft[6] +2
------------------------------------------------------------------------------------------
```

**Ranking and end condition:**

- A team finishes when it completes **3 laps** by crossing the start/finish line
- Finish rank is assigned in the order teams complete their third lap
- The race ends when **all teams** finish; the final standings are shown in finishing order

## Requirements Changes

The original proposal (`requirements-original.md`) was written at the planning stage and describes all game rules and interactions correctly at a high level. The final `requirements.md` adds concrete details that were decided as the game was built. No original requirements were removed or weakened.

| Req | What was finalized during implementation | Reason |
|-----|------------------------------------------|--------|
| 2   | Named the four passive abilities and defined their exact effects. | The abilities were designed and balanced during implementation; specific names and bonuses were finalized through iterative testing. |
| 5   | Added tire movement value table (Soft 7/4, Medium 6/3, Hard 5/3), card counts, and variance rule (±1, 1:2:1 distribution, Sunny only). | Exact values were determined through playtesting for balance; variance was added to make Sunny-weather card selection more meaningful. |
| 9   | Added that each card's move value for the current weather is shown before selection. | Displaying computed values in-hand was a UI decision that emerged when implementing the card picker. |
| 10  | Stated the movement formula (base ± variance + passive bonus, min 1). | Formula emerged from combining the tire value table, variance system, and passive ability mechanics designed during implementation. |
| 11  | AI turns are executed silently and summarized at the next human turn rather than rendering the full track after every AI move. | Rendering after each AI turn floods the terminal with redundant output and makes the game harder to follow; decided during implementation. All information is still displayed, just batched for readability. |
| 13  | Replaced "less than half of other tire card's" with exact values (3 Sunny, 2 Rainy). | Exact values were determined when the tire movement table was finalized during implementation. |
| 16  | Added card counts per tire type and that existing cards are preserved on pit entry. | Card preservation was a gameplay mechanic decided while implementing the pit lane to reward strategic pitting timing. |
| 17  | Described the two-step pit exit (auto-place at pos 0 + lap increment, then normal card play). | The exact sequence was determined while implementing the pit logic to keep pit exit consistent with the lap counting rules. |
| 20  | Specified target lap count as 3. | Finalized through playtesting for appropriate game length. |

## LLM Usage

Claude Code (claude-sonnet-4-6) was used as a coding assistant during implementation.

**My role in the project:**
- Designed and built the original game as a physical card game during a Freshman Program Designer (FPD) project; this implementation is a direct adaptation of that design (see Acknowledgement)
- Defined all game rules, tire values, passive abilities, variance system, and lap structure through the requirements document and iterative decisions during development
- Designed the circuit layout with pit lane, start/finish line, and the visual style of the terminal interface (box-drawing cells, connectors, track orientation)
- Directed all major design choices: card-level variance, rainy weather suppressing variance, card preservation on pit entry, AI summary batching, hand sort order, standings format
- Manually edited rendering code in the IDE when the generated layout did not match the intended visual design, and directed all corrections

**What the LLM was used for:**
- Translating the game rules and design decisions into F# modules (`Types.fs`, `Movement.fs`, `Pit.fs`, `Race.fs`, `Render.fs`, `Input.fs`, `AI.fs`, `Setup.fs`, `Program.fs`)
- Iterating on terminal layout details (cell sizing, connector characters, column alignment) under my direction
- Implementing the three AI strategy variants (Greedy, RandomPlay, Precise) from my behavioral descriptions

**What required manual correction or reprompting:**
- Track cell border widths: the LLM did not match the character counts I set manually, requiring several correction rounds
- Pit exit arrow alignment above position 00 needed multiple iterations to match the intended visual
- Rainy weather variance suppression: the LLM kept variance active until explicitly instructed otherwise
- Card preservation on pit entry: initial version replaced the hand; corrected after I specified append behavior

**What the LLM was not able to do correctly on its own:**
- After I directly edited `Render.fs` in the IDE to adjust the track layout, the LLM's subsequent edits to the same file would sometimes revert or mismatch the widths I had set, requiring me to re-correct the layout manually

## Acknowledgement
This game is distilled version of a game I implemented physically in this year as a Freshman Program Designer(FPD). The game was for the first program of Happy College Life(HCL), and there is no specific modeled board game for it - in other words, it was our, FPD's new idea. I sincerely appriciate to my teammates who worked for the program with me and gave permission to use the game as this term project's subject.

## Implementation History in Glance

**April 30th, 2026: Initial commit**
- Created repository and modified `README.md`.
- Created `requirements.md`.
  - I used LLMs to make first draft of requirements smoothen and fix the grammer error.
- Submitted GitHub repository link and `requirements.md` as a PDF.

**May 4th, 2026: Revised Requirements**
- Added/removed some requirements for better game organization.
- Submitted revised `requirements.md` as a PDF again.

**May 23rd, 2026: Basic Interface and Type Logics**
- Made terminal interface with multiple space of tracks
- Add pit entry, exit, space
- Add Leaderboard

**May 30th, 2026: Full Game Implementation**
- Added `Input.fs` and `Setup.fs` for interactive setup (team count, passive ability, starting tire)
- Added `AI.fs` with three distinct AI strategies: Greedy (best card, early pit), RandomPlay (random card and pit), Precise (random card, distance-based pit calculation)
- Added `AiAlgorithm` discriminated union; AI teams are assigned a random algorithm at setup
- Implemented full race loop in `Program.fs`: turn order, weather change per round, human/AI turn dispatch, finish detection, final standings
- Tire cards carry pre-rolled variance (±1 with 1:2:1 distribution); variance applies in Sunny only
- Passive abilities affect per-card movement (Warm Tires, Rain Engineer) or pit replenishment (Pit Crew); displayed in hand header
- Existing cards are preserved when entering the pit; new tire cards are appended to hand
- AI turns are batched silently and summarized below the round header on the next human turn
- Hand display sorts cards by type (Soft→Medium→Hard) then by move value descending; picker shows 5 cards per row with fixed-width columns
- `!q` at any input prompt exits the game cleanly