Scope for Lecture-Note-Based Implementation
This project should primarily follow the functional-first programming style taught in the CS20200 lecture notes, while allowing a small amount of imperative or object-oriented code only when it clearly improves readability or console-game implementation.

1. Core concepts to use
Lec 2 - Abstraction
Use small, well-named functions to separate game logic into meaningful units, such as setup, movement calculation, pit handling, weather update, lap update, and terminal rendering.

The implementation should avoid one huge function that mixes all game rules together, because abstraction is one of the main design principles emphasized in the course.

Lec 4 - Recursion
Recursion may be used for repeated game flow, such as the main race loop, round progression, or repeated user input until valid input is given.

Tail-recursive structure is preferred when recursion directly represents repeated game progression, because the lecture notes emphasize both recursive thinking and efficiency trade-offs.

Lec 6 - Data Abstraction, Records, Discriminated Unions, Pattern Matching
This is the most important lecture for the project structure.

Game state, team state, weather, tire type, passive ability, and turn phase should be modeled using records and discriminated unions, and game logic should rely heavily on pattern matching rather than ad hoc integer or string encodings.

Recommended usage:

records for TeamState, GameState, and track-related data

discriminated unions for Weather, TireType, PassiveAbility, and command or action types

pattern matching for turn resolution and rule branching

Lec 7 - Lists
Lists are appropriate for ordered collections such as teams in turn order, standings, tire hands, and finished teams.

List-processing style should be preferred when updating multiple teams or traversing standings, because this matches the course’s functional treatment of collections.

Lec 8 - Option and Result
Option should be used when something may or may not exist, such as a pit entry decision, a selected tire card, or a winning rank that is not yet assigned.

If input parsing or setup validation is made explicit, Result may also be used to represent success or failure instead of relying on exceptions.

Lec 9 - Higher-Order Functions
Higher-order functions are useful for concise collection processing, especially for filtering finished teams, mapping team states for rendering, or computing standings.

However, they should be used only when they improve readability; for core turn flow, plain recursive or direct code may be clearer.

Lec 10 - Built-in Higher-Order Functions, Set, Map
List.map, List.filter, List.fold, and possibly Map are appropriate for updating collections and looking up team-related information.

If the implementation needs a mapping from team identifiers to states or standings metadata, Map is a reasonable course-aligned choice.

Lec 11 - Imperative Programming
Limited imperative features are acceptable for console interaction, random-number generation, or local mutable counters when they simplify implementation.

However, mutable global game state should be avoided unless clearly justified, because the lecture notes explicitly warn that mutable module state behaves like hidden global state and makes code harder to reason about and test.

Lec 12 - Modules and Namespaces
The project should be split into multiple files and modules rather than keeping everything in one file.

The lecture notes strongly emphasize organizing functions into modules, keeping data transparent, and separating behavior from data, so a module-based design is highly recommended.

Suggested module split:

Domain or Types - records and discriminated unions

Setup - initialization logic

Movement - tire/weather/basic movement logic

Pit - pit entry and replenishment logic

Race - turn and round progression

Render - terminal output

Input - user input parsing and validation

Lec 13 - Active Patterns and I/O
Console I/O is directly relevant, because this is a terminal game.

If input parsing becomes repetitive, active patterns may be used to make parsing commands or menu selections more readable, but they are optional rather than mandatory for this project.

2. Concepts that may be used carefully
Lec 14 - Objects
A small amount of OOP may be used only if there is a clear reason, such as wrapping RNG behavior or separating a console renderer behind an object-like interface.

However, the overall design should still remain functional-first, since the lecture notes explicitly describe F# as a hybrid but functional-first language.

Lec 15 - Polymorphism
Generic or polymorphic design may be used only when it naturally simplifies code, not just to make the implementation look advanced.

For this project, over-generalizing game logic is usually unnecessary, so polymorphism should be used sparingly.

Lec 16 - Interfaces
Interfaces may be useful if the implementation wants to abstract over AI strategy, input source, or renderer behavior.

Still, the lecture notes also emphasize small, behavior-focused interfaces and warn against unnecessary complexity, so interfaces should be introduced only when they genuinely improve extensibility or testing.

3. Concepts that are probably unnecessary
The following later concepts are likely outside the practical scope of this project unless there is a very specific reason:

lazy evaluation

streams

async programming

monads beyond ordinary Option or Result usage

parser combinators or advanced parser design

These topics are valuable in general, but for a small command-line racing game they are likely to increase complexity more than they improve the design.

4. Design preference for this project
The implementation should prefer:

transparent immutable records for game state,

discriminated unions for finite game-rule categories,

pattern matching for rule execution,

modules for organization and separation of concerns,

lists, folds, maps, and options for routine state manipulation,

only small and controlled imperative code for console I/O, randomness, or local convenience.

The implementation should avoid:

large hidden mutable global state,

unnecessary inheritance-heavy OOP structure,

over-engineered abstractions that are not justified by the current requirements,

advanced language features that do not directly help the final game requirements.

5. Practical recommendation for Claude
Claude should read the lecture notes with the following priority:

Lec 6 - Data Abstraction, Records, Discriminated Unions, Pattern Matching

Lec 12 - Modules and Namespaces

Lec 13 - Active Patterns and I/O

Lec 7 and Lec 8 - Lists, Option, Result

Lec 9 and Lec 10 - Higher-Order Functions, Map, Fold

Lec 11 - Imperative Programming

Lec 14 and Lec 16 only if a small abstraction boundary is needed