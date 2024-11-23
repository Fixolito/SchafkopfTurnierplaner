namespace Schafkopfturnierplanung;

public class UndirectedGraph
{
    private Dictionary<Player, HashSet<Player>> PlayedAgainst = [];

    public void AddPlayer(Player player)
    {
        PlayedAgainst.Add(player, []);
    }

    public void AddEdge(Player playerA, Player playerB)
    {
        if (ReferenceEquals(playerA, playerB))
            throw new ArgumentException("Players must be different to each other");

        PlayedAgainst[playerA].Add(playerB);
        PlayedAgainst[playerB].Add(playerA);
    }
}
