namespace Schafkopfturnierplanung;

public class Program
{
    static readonly List<string> Namen =
    [
            "Alexander", "Barbara", "Christian", "Daniela", "Erik", "Franziska", "Gustav", "Hannah", "Isabel", "Johannes",
            "Katharina", "Lukas", "Marie", "Niklas", "Oliver", "Patricia", "Quentin", "Rebecca", "Stefan", "Theresa",
            "Andreas", "Birgit", "Carsten", "Diana", "Elias", "Florian", "Greta", "Heinrich", "Ingrid", "Julia", "Klaus",
            "Laura", "Michael", "Nina", "Otto", "Peter", "Richard", "Sabine", "Thomas", "Ursula", "Annette", "Lukas", "Tina",
            "Felix", "Markus"
        ];

    static void Main()
    {
        TournamentPlanning planning = new();
        
        Console.WriteLine(string.Join(", ", Namen));
        List<string> names = GetCommaSeparatedInput();

        /* for (int i = 1; i <= playerCount; i++)
            planning.AddPlayer($"Player{i}"); */

        foreach (string name in names)
            planning.AddPlayer(name);

        Console.WriteLine($"Player participating: {planning.Players.Count}");
        planning.GenerateGeneral3RoundMethod();
        planning.GenerateGeneral3RoundMethod();
        planning.GenerateGeneral3RoundMethod();
        planning.ShuffleSeating();
        ConsoleColor def = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        planning.PrintRounds();
        Console.ForegroundColor = def;
    }

    public static List<string> GetCommaSeparatedInput()
    {
        Console.WriteLine("Please enter comma-separated values:");
        string input = Console.ReadLine() ?? string.Empty;
        
        // Split the string and trim whitespace
        List<string> values = input
            .Split(',')
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
            
        return values;
    }
}