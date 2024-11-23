using System.Text;

namespace Schafkopfturnierplanung;

public class Round(int number, Dictionary<Table,Matchup> roundSeating)
{
    public readonly int Number = number;
    public Dictionary<Table, Matchup> Seatings = roundSeating;
    public int TableCount => Seatings.Count;
    public bool IsValidated { get; private set; } = false;


    public bool Validate()
    {
        return ValidateTableSizes() && ValidateNewMatchupsOnly();
    }

    public int GetPlayerCount()
    {
        int playerCount = 0;

        foreach (Matchup matchup in Seatings.Values)
            playerCount += matchup.PlayerCount;
        
        return playerCount;
    }
    public bool ValidateTableSizes()
    {
        if (IsValidated)
            return true;
        
        foreach (Table table in Seatings.Keys)
        {
            if (table.Size != Seatings[table].PlayerCount)
            {
                Console.WriteLine($"Table {table.ID}({table.Size}): {Seatings[table]}");
                return false;
            }
        }
        IsValidated = true;
        return true;
    }

    public bool ValidateNewMatchupsOnly()
    {
        foreach(Matchup matchup in Seatings.Values)
        {
            foreach (Player player in matchup.Players)
            {
                HashSet<Player> opponents = matchup.GetOpponentsForPlayer(player);
                HashSet<Player> formerOppenents = player.GetFormerOpponents();
                if (opponents.Overlaps(formerOppenents))
                    return false;
            }
        }
        return true;
    }

    public void SeatPlayers()
    {
        foreach (Table table in Seatings.Keys)
        {
            Matchup matchup = Seatings[table];

            foreach (Player player in matchup.Players)
                player.SeatPlayer(table, matchup);
        }
    }

    public override string ToString()
    {
        StringBuilder builder = new();
        builder.AppendLine($"Round: {Number}");
        foreach (Table table in Seatings.Keys)
        {
            builder.AppendLine($"    Table {table.ID}:");
            builder.AppendLine($"        {Seatings[table]}");
        }
        return builder.ToString();
    }


}