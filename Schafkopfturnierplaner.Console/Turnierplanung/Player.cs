using System.Data;

namespace Schafkopfturnierplanung;

public class Player(string name)
{
    private static int PlayerID = 0;

    public readonly string Name = name;
    public int ID = PlayerID++;
    public bool IsActive = true;
    public List<Table> PlayedTables = [];
    public List<Matchup> Matchups = [];

    public HashSet<Player> GetFormerOpponents(int numberOfRounds = 0)
    {
        HashSet<Player> players = [];

        if (numberOfRounds == 0)
            numberOfRounds = Matchups.Count;

        for (int i = Matchups.Count - numberOfRounds; i < Matchups.Count; i++)
            players.UnionWith(Matchups[i].Players);
        
        return players;
    }

    public bool HasPlayedTable(Table table)
    {
        return PlayedTables.Contains(table);
    }

    public void SeatPlayer(Table table, Matchup matchup)
    {
        if (PlayedTables.Count != Matchups.Count)
            throw new ConstraintException("Number of played tables is not the same as matchups.");
        
        PlayedTables.Add(table);
        Matchups.Add(matchup);
    }

    public int RoundsSinceTable(Table table)
    {
        for (int i = 0; i < PlayedTables.Count; i++)
        {
            if (ReferenceEquals(PlayedTables[PlayedTables.Count - 1 - i], table))
                return i;
        }

        return PlayedTables.Count;
    }

    public override string ToString()
    {
        return Name;
    }

    
}