namespace Dice.Interfaces;

public interface IConsoleWriter
{
    void WritePositiveLine(string message);
    void WriteNegativeLine(string message);
    void WriteHelpLine(string message);
    void WriteLine(string message = "");
    string GetSelection(List<string> choices, string title = "");
    void ShowTable(List<List<string>> data, string title = "");
}
