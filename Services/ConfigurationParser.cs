namespace Dice.Services;

using System.Text.RegularExpressions;
using Dice.Models;
using Spectre.Console;

public class ConfigurationParser
{
    public List<DiceModel> Parse(string[] args)
    {
        if (args.Length == 0)
        {
            throw new ArgumentException(
                "Please specify dice configurations via command line arguments. Example: dotnet run 2,2,4,4,9,9 6,8,1,1,8,6 7,5,3,7,5,3"
            );
        }
        if (args.Length < 3)
        {
            throw new ArgumentException(
                "Please specify at least three dice configurations. Example: dotnet run 2,2,4,4,9,9 6,8,1,1,8,6 7,5,3,7,5,3"
            );
        }

        var dice = new List<DiceModel>();

        for (int i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            if (!IsIncorrectChar(arg))
            {
                throw new ArgumentException(
                    $"Invalid dice configuration at position {i + 1}: \"{arg}\". Configuration contains invalid characters. Check that all sides are integers, e.g., \"1,2,3,4,5,6\""
                );
            }
            if (!IsValidDiceConfig(arg))
            {
                throw new ArgumentException(
                    $"Invalid dice configuration at position {i + 1}: \"{arg}\". Each side of the dice must be an integer, e.g., \"1,2,3,4,5,6\""
                );
            }
            var sides = ParseDice(arg);
            if (!IsSixSides(sides))
            {
                throw new ArgumentException(
                    $"Invalid dice configuration at position {i + 1}: \"{arg}\". Each dice should have 6 sides, e.g., \"1,2,3,4,5,6\""
                );
            }
            dice.Add(new DiceModel(sides));
        }

        return dice;
    }

    private bool IsIncorrectChar(string s)
    {
        string pattern = @"^[0-9,]+$";
        return Regex.IsMatch(s, pattern);
    }

    private bool IsSixSides(int[] sides)
    {
        return sides.Length == 6;
    }

    private bool IsValidDiceConfig(string config)
    {
        return config.Split(',').All(item => int.TryParse(item, out _));
    }

    private int[] ParseDice(string config)
    {
        return config.Split(',').Select(int.Parse).ToArray();
    }
}
