namespace Schafkopfturnierplanung;

public class Matchup
{
    public Player[] Players;
    public int PlayerCount => Players.Length;
    private bool AllSlotsFilled = false;

    public Matchup(List<Player> players)
    {
        if (players.Count != 4 && players.Count != 5)
            throw new ArgumentOutOfRangeException($"A matchup has 4 or 5 players, but was {players.Count}");

        Players = [.. players];
        AllSlotsFilled = HasAllSlotsFilled();
    }

    public Matchup(Player[] players)
    {
        if (players.Length != 4 && players.Length != 5)
            throw new ArgumentOutOfRangeException($"A matchup has 4 or 5 players, but was {players.Length}");

        Players = players;
        AllSlotsFilled = HasAllSlotsFilled();
    }

    public Matchup(Table table)
    {
        Player[] players = new Player[table.Size];
        if (players.Length != 4 && players.Length != 5)
            throw new ArgumentOutOfRangeException($"A matchup has 4 or 5 players, but was {players.Length}");

        Players = players;
        AllSlotsFilled = HasAllSlotsFilled();
    }

    public void FillSeats(List<Player> players, int first)
    {
        for(int i = 0; i < PlayerCount; i++)
            Players[i] = players[first + i];

        Console.WriteLine(this);
        AllSlotsFilled = HasAllSlotsFilled();
    }



    public override string ToString()
    {
        return string.Join<Player>(", ", Players);
    }

    public HashSet<Player> GetOpponentsForPlayer(Player player)
    {
        HashSet<Player> opponents = [..Players];
        opponents.Remove(player);
        
        if (opponents.Count == PlayerCount)
            throw new ArgumentException("Player is not part of matchup.");
        
        return opponents;
    }

    private bool HasAllSlotsFilled()
    {
        for (int i = 0; i < PlayerCount; i++)
        {
            if (Players[i] is null)
                return false;
        }
        return true;
    }
}