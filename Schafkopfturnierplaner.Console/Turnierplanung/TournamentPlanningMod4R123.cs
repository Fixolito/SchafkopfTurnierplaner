using System.Diagnostics.CodeAnalysis;

namespace Schafkopfturnierplanung;

public partial class TournamentPlanning
{
    public void GenerateNextRound()
    {
        CreateConsistencyForLateJoiningPlayers();
        CreateTableSetup(out List<Player> activePlayers);
        activePlayers.Shuffle();
        CreateMatchups(activePlayers);
        SeatPlayer();
        RoundsGenerated++;
    }

    private void CreateMatchups(List<Player> activePlayers)
    {
        if (TrySimpleRoundCreation(activePlayers, true, out Round? round))
        {
            Rounds.Add(round);
            return;
        }

        throw new NotImplementedException("Complex matchup creation is not implemented.");
    }

    private bool TrySimpleRoundCreation(List<Player> activePlayers, bool shuffle , [NotNullWhen(true)] out Round? round)
    {
        
        int neededTables = activePlayers.Count / 4;
        
        if (shuffle)
            activePlayers.Shuffle();
        
        Dictionary<Table, Matchup> tableMatchups = [];

        for(int i = 0; i < neededTables; i++)
        {
            Console.WriteLine();
            Table table = Tables[i];

            if (TrySimpleMatchupCreation(table, activePlayers, out Matchup? matchup))
            {
                tableMatchups.Add(table, matchup);
                Console.WriteLine($"table {table.ID}: {matchup}");
            }
            else
            {
                round = null;
                return false;
            }

        }

        round = new(RoundsGenerated, tableMatchups);
        return round.ValidateTableSizes();
    }

    private bool TrySimpleMatchupCreation(Table table, List<Player> activePlayers, [NotNullWhen(true)] out Matchup? matchup)
    {
        activePlayers.SortByRoundsSinceTable(table);

        List<Player> players = [];
        HashSet<Player> excluded = [];
        matchup = null;

        for (int seat = 0; seat < table.Size; seat++)
        {
            if (TryFillSeatSimple(activePlayers,excluded, out Player? player))
                players.Add(player);
            else
                return false;
        }

        matchup = new(players);
        return true;

    }

    private static bool TryFillSeatSimple(List<Player> activePlayers, HashSet<Player> excluded, [NotNullWhen(true)] out Player? player)
    {
        for (int i = 0; i < activePlayers.Count; i++)
        {
            player = activePlayers[i];
            if (excluded.Add(player))
            {
                excluded.UnionWith(player.GetFormerOpponents());
                activePlayers.Remove(player);
                return true;
            }
        }
        player = null;
        return false;
    }

}