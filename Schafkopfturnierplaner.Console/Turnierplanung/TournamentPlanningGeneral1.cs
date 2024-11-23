using System.Diagnostics.CodeAnalysis;

namespace Schafkopfturnierplanung;

public partial class TournamentPlanning
{
    public void GenerateGeneral3RoundMethod()
    {

        if (GenerateFirstRound())
        {
            AnalyzeProblem(Rounds[^1]);
            SeatPlayer();
            Console.WriteLine(Rounds[^1]);
            return;
        }
        
        if (Rounds.Count == 1)
        {
            GenerateSecondRoundGeneral1();
            Console.WriteLine(Rounds[^1]);
            SeatPlayer();
            return;
        }

        if (Rounds.Count == 2)
        {
            GenerateThirdRoundGeneral1();
            Console.WriteLine(Rounds[^1]);
            SeatPlayer();
            return;
        }
        throw new Exception("Only 3 rounds are supposed to be generated this way.");
    }

    private void GenerateSecondRoundGeneral1()
    {
        Round before = Rounds[0];
        Dictionary<Table, Matchup> seating = [];

        foreach(Table table in before.Seatings.Keys)
            seating.Add(table, new(table));

        int playerCount = before.GetPlayerCount();
        int tableCount = playerCount / 4;
        int currentTableID = 0;
        int currentSeat = 0;

        for (int i = 0; i < playerCount; i++)
        {
            Table tableToSeatTo = Tables[currentTableID];
            
            if (currentSeat >= tableToSeatTo.Size)
            {
                currentTableID++;
                currentSeat = 0;
                tableToSeatTo = Tables[currentTableID];
            }
            Matchup matchup = seating[tableToSeatTo];
            int tableLastRound = i % tableCount;
            int seatLastRound = i / tableCount;

            Table tableToSeatFrom = Tables[tableLastRound];
            Player player = before.Seatings[tableToSeatFrom].Players[seatLastRound];
            matchup.Players[currentSeat] = player;
            currentSeat++;
        }
        Round newRound = new(RoundsGenerated++, seating);
        newRound.Validate();
        AnalyzeProblem(newRound);
        //PrintProblems(newRound);
        Rounds.Add(newRound);
    }

    private void GenerateThirdRoundGeneral1()
    {
        Round before = Rounds[0];
        Stack<Table> tables4Seats = [];
        Stack<Table> tables5Seats = [];
        Dictionary<Table, Matchup> seating = [];
        
        foreach (Table table in before.Seatings.Keys)
        {
            if (table.Size == 4)
                tables4Seats.Push(table);
            else
                tables5Seats.Push(table);
        }

        for(int tableIndex = 0; tableIndex < before.TableCount; tableIndex++)
        {
            List<Player> newMatchup = [];
            for (int seat = 0; seat < 5; seat++)
            {
                int oldTableIndex = (tableIndex - seat + before.TableCount) % before.TableCount;
                Table oldTable = Tables[oldTableIndex];
                
                if (seat >= oldTable.Size)
                    break;

                newMatchup.Add(before.Seatings[oldTable].Players[seat]);
            }
            if (newMatchup.Count == 4)
            {
                Table table = tables4Seats.Pop();
                Matchup matchup = new(table);
                matchup.FillSeats(newMatchup, 0);
                seating.Add(table, matchup);
            }
            else
            {
                Table table = tables5Seats.Pop();
                Matchup matchup = new(table);
                matchup.FillSeats(newMatchup, 0);
                seating.Add(table, matchup);
            }
        }
        Round round = new(RoundsGenerated++, seating);
        round.Validate();
        AnalyzeProblem(round);
        //PrintProblems(round);
        Rounds.Add(round);

    }
}