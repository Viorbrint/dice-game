# Dice Game

[![Watch the video](https://img.youtube.com/vi/bYB871bkhNo/0.jpg)](https://www.youtube.com/watch?v=bYB871bkhNo)

This is a console dice game where the player and the computer take turns rolling dice and comparing results. The game includes the generation of random numbers for fair rolls using HMAC, as well as a probability table for different dice throws.

## Description

The game involves two players: the computer and the human player. Players take turns selecting dice and rolling them. To ensure fairness, each roll is generated based on random numbers using the cryptographic function HMAC. The results of the rolls are compared, and the winner is determined by the highest result.

**Main stages of the game:**
1. Players select their dice.
2. To determine who goes first, a random number is generated for both the computer and the player.
3. Both players then take turns rolling their selected dice.
4. The results are compared, and the winner is announced based on the higher roll.

## Project Structure

- `Game.cs`: The main game logic that controls the entire gameplay process.
- `Interfaces/`: Interfaces used for implementing console output and random number generation services.
- `Models/`: Data models, including `DiceModel`, that describe the dice.
- `Services/`: Logic for handling probabilities, random number generation, and ensuring fair dice rolls.
- `Utils/`: Helper classes like the menu, probability table, and random number generator.
- `Program.cs`: The entry point of the application.

## Dependencies

The project uses the following dependencies:

- [Spectre.Console](https://spectreconsole.net/): A library for rendering styled and structured console output.

## Running the Project

To run the project, follow these steps:

1. Clone the repository:

    ```bash
    git clone https://github.com/Viorbrint/dice-game

2. Navigate to the project folder:

    ```bash
    cd dice-game

3. Restore dependencies:

    ```bash
    dotnet restore

4. Build and run the project:

   ```bash
   dotnet run
