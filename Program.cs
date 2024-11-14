using Dice.Interfaces;
using Dice.Services;

public class Program
{
    public static void Main(string[] args)
    {
        IConsoleWriter consoleWriter = new SpectreConsoleWriter();
        try
        {
            ConfigurationParser parser = new ConfigurationParser();
            var dice = parser.Parse(args);

            IFairnessService fairnessService = new FairnessService();
            Game game = new Game(dice, consoleWriter, fairnessService);
            game.Play();
        }
        catch (ArgumentException e)
        {
            consoleWriter.WriteNegativeLine("Error: " + e.Message);
        }
        catch
        {
            Console.WriteLine("An unexpected error occurred. Please try again.");
        }
    }
}
