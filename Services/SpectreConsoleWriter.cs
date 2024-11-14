namespace Dice.Services;

using System.Linq;
using Dice.Interfaces;
using Spectre.Console;

public class SpectreConsoleWriter : IConsoleWriter
{
    public void WriteLine(string message = "")
    {
        AnsiConsole.MarkupLineInterpolated($"{message}");
    }

    public void WritePositiveLine(string message)
    {
        AnsiConsole.MarkupLineInterpolated($"[bold green]{message}[/]");
    }

    public void WriteNegativeLine(string message)
    {
        AnsiConsole.MarkupLineInterpolated($"[bold red]{message}[/]");
    }

    public void WriteHelpLine(string message)
    {
        AnsiConsole.MarkupLineInterpolated($"[bold yellow]{message}[/]");
    }

    public string GetSelection(List<string> choices, string title = "")
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title($"[bold green]{title}[/]").AddChoices(choices)
        );
    }

    public void ShowTable(List<List<string>> data, string title = "")
    {
        var table = new Table();
        table.Border(TableBorder.Ascii);

        foreach (var col in data[0])
        {
            table.AddColumn(new TableColumn(new Markup($"[blue]{col}[/]")));
        }

        foreach (var row in data.Skip(1))
        {
            var rowCells = row.Select(
                    (string cell, int index) =>
                        index == 0 ? new Markup($"[blue]{cell}[/]") : new Markup(cell)
                )
                .ToArray();

            table.AddRow(rowCells);
        }

        WriteHelpLine(title);
        AnsiConsole.Write(table);
    }
}
