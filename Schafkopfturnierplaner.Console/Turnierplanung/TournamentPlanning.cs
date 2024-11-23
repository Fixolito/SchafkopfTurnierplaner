using System.Diagnostics.CodeAnalysis;

namespace Schafkopfturnierplanung;

public partial class TournamentPlanning
{
    public Table FillerTable = new(-1);
    public Matchup FillerMatchup = new (new Player[4]);
    public List<Player> Players = [];
    public List<Table> Tables = [];
    public List<Round> Rounds = [];
    private int RoundsGenerated = 0;

    public Dictionary<Round, Dictionary<Player, HashSet<Player>>> Problems = [];
    
    public void AddPlayer(string name)
    {
        Player player = new(name);
        Players.Add(player);
    }

    private void SeatPlayer()
    {
        Rounds[^1].SeatPlayers();
    }

    private bool GenerateFirstRound()
    {
        if (Rounds.Count != 0)
            return false;

        CreateTableSetup(out List<Player> activePlayers);

        if (activePlayers.Count < 4 || (activePlayers.Count / 4 < activePlayers.Count % 4))
            throw new Exception($"{activePlayers.Count} players cannont be seated on 4/5player tables.");

        Dictionary<Table, Matchup> seating = [];
        int i = 0;
        foreach(Table table in Tables)
        {
            if (i >= activePlayers.Count)
                break;

            Matchup matchup = new Matchup(table);
            matchup.FillSeats(activePlayers, i);
            i += table.Size;
            seating.Add(table, matchup);
        }

        Round round = new(RoundsGenerated, seating);
        round.Validate();
        RoundsGenerated++;
        Rounds.Add(round);
        return true;
    }


    private void CreateTableSetup(out List<Player> activePlayers)
    {
        activePlayers = [];
        foreach (Player player in Players)
        {
            if (player.IsActive)
                activePlayers.Add(player);
        }
        int neededTables = activePlayers.Count / 4;
        int extraSeatedTables = activePlayers.Count % 4;

        for (int i = 0; i < neededTables; i++)
        {
            if (Tables.Count < neededTables)
                Tables.Add(new Table(i));
            
            if (extraSeatedTables > 0)
            {
                Tables[i].Size = 5;
                extraSeatedTables--;
            }
            else
                Tables[i].Size = 4;
        }
        PrintTables();
    }

    private void CreateConsistencyForLateJoiningPlayers()
    {
        foreach (Player player in Players)
        {
            while (player.PlayedTables.Count < RoundsGenerated)
                player.PlayedTables.Add(FillerTable);

            while (player.Matchups.Count < RoundsGenerated)
                player.Matchups.Add(FillerMatchup);
        }
    }

    public void AnalyzeProblem(Round round)
    {
        Dictionary<Player, HashSet<Player>> problems = [];

        foreach(Matchup matchup in round.Seatings.Values)
        {
            foreach(Player player in matchup.Players)
            {
                HashSet<Player> currentOpponents = matchup.GetOpponentsForPlayer(player);
                HashSet<Player> formerOpponents = player.GetFormerOpponents();
                currentOpponents.IntersectWith(formerOpponents);
                problems.Add(player, currentOpponents);
            }
        }

        Problems.Add(round, problems);
    }

    public void PrintProblems(Round round)
    {
        ConsoleColor before = Console.ForegroundColor;
        if (!Problems.ContainsKey(round))
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Round {round.Number} has no problems");
            Console.ForegroundColor = before;
            return;
        }

        Dictionary<Player, HashSet<Player>> problems = Problems[round];
        Console.WriteLine($"Round {round.Number} has problems");
        foreach (Player player in problems.Keys)
        {
            HashSet<Player> playerProblems = problems[player];

            Console.ForegroundColor = playerProblems.Count > 0 ? ConsoleColor.DarkRed : ConsoleColor.DarkGreen;
            Console.WriteLine($"{player}: [{string.Join(", ", playerProblems)}]");
        }
        Console.ForegroundColor = before;
    }

    public void PrintTables()
    {
        Console.WriteLine("Tables:");
        foreach (Table table in Tables)
            Console.WriteLine($"   - Table {table.ID} - Size: {table.Size}");
    }

    public void TryReassigningTables3Rounds()
    {
        if (Rounds.Count != 3)
            return;

        

        Round firstRound = Rounds[0];
        List<Matchup> secondRound4Players = [];
        List<Matchup> secondRound5Players = [];
        List<Matchup> thirdRound4Players = [];
        List<Matchup> thirdRound5Players = [];

    }
}