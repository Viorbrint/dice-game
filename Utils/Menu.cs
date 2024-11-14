namespace Dice.Utils;

using System;
using System.Collections.Generic;
using Dice.Interfaces;
using Dice.Models;

public class Menu
{
    private readonly ProbabilityTable _probabilityTable;
    private readonly IConsoleWriter _consoleWriter;
    private readonly List<string> _constOptions = new() { "Help", "Exit" };

    public Menu(IConsoleWriter consoleWriter, List<DiceModel> dice)
    {
        _consoleWriter = consoleWriter ?? throw new ArgumentNullException(nameof(consoleWriter));
        _probabilityTable = new ProbabilityTable(dice, _consoleWriter);
    }

    public DiceModel GetDiceSelection(List<DiceModel> dice)
    {
        var choices = new List<string>();
        for (int i = 0; i < dice.Count; i++)
        {
            choices.Add($"Dice {i + 1}: {string.Join(", ", dice[i].Sides)}");
        }

        choices.AddRange(_constOptions);
        int selectionIndex = GetSelection(choices, "Select your dice");
        return dice[selectionIndex];
    }

    public int GetNumberSelection()
    {
        var choices = new List<string> { "0", "1", "2", "3", "4", "5" };
        choices.AddRange(_constOptions);
        int selection = GetSelection(choices, "Select a number");

        _consoleWriter.WriteLine($"Your selection: {selection}");
        return selection;
    }

    public int GetFirstMoveGuess()
    {
        var choices = new List<string> { "0", "1" };
        choices.AddRange(_constOptions);

        int guess = GetSelection(choices, "Try to guess my selection");
        _consoleWriter.WriteLine($"Your guess: {guess}");
        return guess;
    }

    private int GetSelection(List<string> choices, string title = "")
    {
        var choice = _consoleWriter.GetSelection(choices, title);

        switch (choice)
        {
            case "Help":
                ShowHelp();
                return GetSelection(choices, title);
            case "Exit":
                ExitGame();
                Environment.Exit(0);
                break;
        }

        return choices.IndexOf(choice);
    }

    private void ShowHelp()
    {
        _consoleWriter.WriteLine();
        DisplayInstructions();
        _consoleWriter.WriteLine();
        _probabilityTable.Show();

        var options = new List<string> { "Continue", "Exit" };
        GetSelection(options, "Help menu");
    }

    public void DisplayInstructions()
    {
        _consoleWriter.WriteLine("Welcome to the Non-Transitive Dice Game!");
        _consoleWriter.WriteLine();
        _consoleWriter.WriteHelpLine("Game Instructions:");

        _consoleWriter.WriteHelpLine("Determine Who Chooses First:");
        _consoleWriter.WriteLine(
            "1. The computer will select a hidden random value (0 or 1) and provide its HMAC hash."
        );
        _consoleWriter.WriteLine(
            "   You need to guess this value. If you guess correctly, you get to choose your dice first; otherwise, the computer chooses first."
        );
        _consoleWriter.WriteLine(
            "   After your guess, the computer reveals the chosen value and key to verify fairness."
        );
        _consoleWriter.WriteLine();

        _consoleWriter.WriteHelpLine("Gameplay:");
        _consoleWriter.WriteLine(
            "2. The game proceeds in two rounds, with each player making a dice throw in their turn:"
        );
        _consoleWriter.WriteLine(
            "   - For each throw, The computer selects a random number from 0 to 5 and displays its HMAC hash for fairness."
        );
        _consoleWriter.WriteLine("   - Then the player also selects a number from 0 to 5.");
        _consoleWriter.WriteLine(
            "   - Once both numbers are chosen, they are added together, and the result is taken modulo 6."
        );
        _consoleWriter.WriteLine(
            "   - This final result is the index of the dice side that determines the outcome of the throw."
        );
        _consoleWriter.WriteLine();

        _consoleWriter.WriteHelpLine("Determine the Winner:");
        _consoleWriter.WriteLine(
            "3. After both players have completed their throws, the results are compared."
        );
        _consoleWriter.WriteLine("   The player with the higher rolled value wins the round.");
        _consoleWriter.WriteLine();

        _consoleWriter.WriteHelpLine("Extra Commands:");
        _consoleWriter.WriteLine(" - 'Help': Display this help information.");
        _consoleWriter.WriteLine(" - 'Exit': Quit the game at any time.");
        _consoleWriter.WriteLine();

        _consoleWriter.WriteLine("Good luck, and may the best strategy win!");
    }

    private void ExitGame()
    {
        _consoleWriter.WriteNegativeLine("Exiting the game...");
    }
}
