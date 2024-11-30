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
        int playerCount = 27;

        for (int i = 1; i <= playerCount; i++)
            planning.AddPlayer($"Player{i}");

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
}