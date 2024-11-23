namespace Schafkopfturnierplanung;

public static class Utility
{
    public static List<T> Shuffle<T>(this List<T> list)
    {
        ArgumentNullException.ThrowIfNull(list);

        var shuffledList = list.ToList();
        var random = new Random();

        int n = shuffledList.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (shuffledList[n], shuffledList[k]) = (shuffledList[k], shuffledList[n]);
        }

        return shuffledList;
    }

    public static List<Player> SortByRoundsSinceTable(this List<Player> players, Table table)
    {
        ArgumentNullException.ThrowIfNull(players);
        ArgumentNullException.ThrowIfNull(table);
        Dictionary<int, List<Player>> dict = [];
        foreach(Player player in players)
        {
            int roundsSinceTable = player.RoundsSinceTable(table);
            dict.TryAdd(roundsSinceTable, []);
            dict[roundsSinceTable].Add(player);
        }
        List<int> distances = [.. dict.Keys];
        distances.Sort();
        distances.Reverse();
        players.Clear();

        foreach(int distance in distances)
            players.AddRange(dict[distance]);
        
        return players;
    }
}