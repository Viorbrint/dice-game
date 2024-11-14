namespace Dice.Utils;

using Dice.Interfaces;
using Dice.Models;
using Dice.Services;

public class ProbabilityTable
{
    private readonly IConsoleWriter _consoleWriter;
    private readonly List<DiceModel> _dice;
    private List<List<string>> _table;

    public ProbabilityTable(List<DiceModel> dice, IConsoleWriter consoleWriter)
    {
        _consoleWriter = consoleWriter;
        _dice = dice;
        _table = FillTable();
    }

    public void Show()
    {
        _consoleWriter.ShowTable(_table, "Probability Table");
    }

    private List<List<string>> FillTable()
    {
        var probabilities = ProbabilityCalculator.CalculateAllProbabilities(_dice);
        var tableData = new List<List<string>> { GenerateHeaderRow() };

        for (int i = 0; i < _dice.Count; i++)
        {
            tableData.Add(GenerateRow(i, probabilities));
        }

        return tableData;
    }

    private List<string> GenerateHeaderRow()
    {
        var header = new List<string> { "Dice \\ Dice" };
        foreach (var dice in _dice)
        {
            header.Add(dice.ToString());
        }
        return header;
    }

    private List<string> GenerateRow(int rowIndex, Dictionary<(int, int), double> probabilities)
    {
        var row = new List<string> { _dice[rowIndex].ToString() };

        for (int colIndex = 0; colIndex < _dice.Count; colIndex++)
        {
            double probability = probabilities.ContainsKey((rowIndex, colIndex))
                ? probabilities[(rowIndex, colIndex)]
                : 0.0;

            row.Add(FormatProbability(rowIndex, colIndex, probability));
        }

        return row;
    }

    private string FormatProbability(int rowIndex, int colIndex, double probability)
    {
        if (rowIndex == colIndex)
        {
            return $"- ({(probability * 100):F2}%)";
        }
        return $"{(probability * 100):F2}%";
    }
}
