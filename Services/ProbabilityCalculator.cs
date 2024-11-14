using Dice.Models;

namespace Dice.Services;

public static class ProbabilityCalculator
{
    private const int TotalComparisons = 6 * 6;

    private static double CalculateProbability(DiceModel dice1, DiceModel dice2)
    {
        int wins = 0;

        foreach (var side1 in dice1.Sides)
        {
            foreach (var side2 in dice2.Sides)
            {
                if (side1 > side2)
                {
                    wins++;
                }
            }
        }

        return (double)wins / TotalComparisons;
    }

    public static Dictionary<(int Dice1Index, int Dice2Index), double> CalculateAllProbabilities(
        List<DiceModel> dice
    )
    {
        var probabilities = new Dictionary<(int Dice1Index, int Dice2Index), double>();

        for (int i = 0; i < dice.Count; i++)
        {
            for (int j = 0; j < dice.Count; j++)
            {
                double winProbability = CalculateProbability(dice[i], dice[j]);
                probabilities[(Dice1Index: i, Dice2Index: j)] = winProbability;
            }
        }

        return probabilities;
    }
}
