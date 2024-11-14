using Dice.Interfaces;
using Dice.Models;
using Dice.Utils;

public class Game
{
    private readonly Menu _menu;
    private readonly List<DiceModel> _dice;
    private readonly IConsoleWriter _consoleWriter;
    private readonly IFairnessService _fairnessService;
    private readonly Random _random;

    public Game(
        List<DiceModel> dice,
        IConsoleWriter consoleWriter,
        IFairnessService fairnessService
    )
    {
        _dice = dice ?? throw new ArgumentNullException(nameof(dice));
        _consoleWriter = consoleWriter ?? throw new ArgumentNullException(nameof(consoleWriter));
        _fairnessService =
            fairnessService ?? throw new ArgumentNullException(nameof(fairnessService));
        _random = new Random();
        _menu = new Menu(_consoleWriter, dice);
    }

    public void Play()
    {
        var (computerChoice, guess) = DetermineFirstMove();
        var (computerDice, playerDice) = SelectDice(computerChoice, guess);

        _consoleWriter.WriteLine("It's time for my throw.");
        int computerThrow = PerformThrow(computerDice);
        _consoleWriter.WriteNegativeLine($"My throw is {computerThrow}.");
        _consoleWriter.WriteLine();

        _consoleWriter.WriteLine("It's time for your throw.");
        int playerThrow = PerformThrow(playerDice);
        _consoleWriter.WritePositiveLine($"Your throw is {playerThrow}.");
        _consoleWriter.WriteLine();

        DetermineWinner(computerThrow, playerThrow);
    }

    private (int computerChoice, int playerGuess) DetermineFirstMove()
    {
        _consoleWriter.WritePositiveLine("Let's determine who makes the first move.");

        var (computerChoice, key, hmac) = _fairnessService.GenerateFairNumber(1);
        _consoleWriter.WriteLine($"I selected a random value in the range 0..1 (HMAC={hmac}).");

        int guess = _menu.GetFirstMoveGuess();

        _consoleWriter.WriteLine($"My selection: {computerChoice} (KEY={key}).");
        _consoleWriter.WriteLine();

        return (computerChoice, guess);
    }

    private (DiceModel computerDice, DiceModel playerDice) SelectDice(int computerChoice, int guess)
    {
        DiceModel computerDice;
        DiceModel playerDice;

        if (guess == computerChoice)
        {
            _consoleWriter.WritePositiveLine("You guessed correctly! You make the first move.");
            playerDice = GetPlayerDice();
            computerDice = GetComputerDice();
            _consoleWriter.WriteLine($"My dice: {computerDice}.");
        }
        else
        {
            computerDice = GetComputerDice();
            _consoleWriter.WriteNegativeLine(
                $"You guessed incorrectly! I make the first move and choose {computerDice} dice."
            );
            playerDice = GetPlayerDice();
        }
        _consoleWriter.WriteLine();

        return (computerDice, playerDice);
    }

    private int PerformThrow(DiceModel dice)
    {
        var (computerNumber, key, hmac) = _fairnessService.GenerateFairNumber(6);
        _consoleWriter.WriteLine($"I selected a random value in the range 0..5 (HMAC={hmac}).");
        _consoleWriter.WriteLine("Add your number modulo 6.");

        var playerNumber = _menu.GetNumberSelection();
        _consoleWriter.WriteLine($"My value: {computerNumber} (KEY={key}).");

        var result = (computerNumber + playerNumber) % 6;
        _consoleWriter.WriteLine(
            $"The result is: ({computerNumber} + {playerNumber}) mod 6 = {result}."
        );

        return dice.Sides[result];
    }

    private DiceModel GetComputerDice()
    {
        int randomIndex = _random.Next(_dice.Count);
        var dice = _dice[randomIndex];
        _dice.Remove(dice);
        return dice;
    }

    private DiceModel GetPlayerDice()
    {
        DiceModel playerChoice = _menu.GetDiceSelection(_dice);
        _consoleWriter.WriteLine($"You chose the {playerChoice} dice.");
        return playerChoice;
    }

    private void DetermineWinner(int computerThrow, int playerThrow)
    {
        if (playerThrow > computerThrow)
        {
            _consoleWriter.WritePositiveLine(
                $"You won! You rolled {playerThrow} and I rolled {computerThrow}."
            );
        }
        else
        {
            _consoleWriter.WriteNegativeLine(
                $"You lost! You rolled {playerThrow} and I rolled {computerThrow}."
            );
        }
    }
}
