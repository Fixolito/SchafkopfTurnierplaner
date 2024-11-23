using System.Diagnostics.CodeAnalysis;

namespace Schafkopfturnierplanung;

public partial class TournamentPlanning
{
    public bool GenerateNextRoundMod4R0()
    {
        if (Players.Count % 4 != 0)
            throw new ArgumentOutOfRangeException("This algorithm expects a multiple of 4 as palyer count.");

        if (GenerateFirstRoundMod4R0())
        {
            SeatPlayer();
            return true;
        }

        if (GenerateFollowUpRoundMod4R0(out Round round))
        {
            SeatPlayer();
            return true;
        }
        else
        {
            AnalyzeProblem(round);
            PrintProblems(round);
        }

        Console.WriteLine($"Round {RoundsGenerated} could not be generated.");
        return false;
    }

    private bool GenerateFollowUpRoundMod4R0(out Round round)
    {
        Round lastRound = Rounds[^1];

        Dictionary<Table, Matchup> seatings = [];
        
        foreach(Table table in Tables)
            seatings.Add(table,new(table));

        for (int tableID = 0; tableID < Tables.Count; tableID++)
        {
            Table table = Tables[tableID];
            Matchup lastMatchup = lastRound.Seatings[table];

            if (lastMatchup.PlayerCount != 4)
                throw new ArgumentOutOfRangeException($"Last match on table {table.ID} did not have 4 players but {lastMatchup.PlayerCount}.");

            for (int seat = 0; seat < lastMatchup.PlayerCount; seat++)
            {
                Player player = lastMatchup.Players[seat]; 
                Table nextTable = Tables[(tableID + seat) % Tables.Count];
                seatings[nextTable].Players[seat] = player;
            }
        }

        round = new(RoundsGenerated, seatings);
        
        if (round.Validate())
        {
            Rounds.Add(round);
            RoundsGenerated++;
            Console.WriteLine("Round generation success using: Mod4R0");
            Console.WriteLine(round);

            return true;
        }
        return false;
    }

    private bool GenerateFirstRoundMod4R0()
    {
        if (Rounds.Count != 0)
            return false;

        CreateTableSetup(out List<Player> activePlayers);
        
        if (activePlayers.Count/ 4 == 0)
            return false;

        Dictionary<Table, Matchup> seating = [];
        
        for(int i = 0; i < activePlayers.Count; i +=4)
        {
            Table table = Tables[i/4];
            Matchup matchup = new(activePlayers[i..(i + 4)]);
            seating.Add(table, matchup);
        }

        Round round = new(RoundsGenerated, seating);
        round.Validate();
        RoundsGenerated++;

        Rounds.Add(round);
        return true;
    }
}