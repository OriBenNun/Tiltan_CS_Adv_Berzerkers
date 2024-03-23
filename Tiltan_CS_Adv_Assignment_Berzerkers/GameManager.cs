using System;
using System.Collections.Generic;
using Tiltan_CS_Adv_Assignment_Berzerkers;

public class GameManager
{
    private const int WeatherEffectChancePercent = 30;
    private const int MaxRoundsWeatherEffect = 3;

    private int _currentWeatherRoundsCounter;
    private Weather _currentWeather = Weather.None;

    public void InitTwoPlayersWar(Player player1, Player player2)
    {
        Console.WriteLine($"\nTwo Brave Players Have Showed Up For An Unbelievable War! May The Best Player Win!\n\n" +
                          $"Player #1 Information:\n" +
                          $"{player1}\n\n" +
                          $"Player #2 Information:\n" +
                          $"{player2}\n\n" +
                          $"LET THE WAR BEGIN!!");

        var fightResult = UnitsFight(player1.Units, player2.Units);

        var message = "";
        switch (fightResult)
        {
            case FightResult.Draw:
                message = $"No resources were stolen, as no units left alive.\n";
                break;
            case FightResult.TeamAWon:
                MoveResourcesFromLoserToWinner(player1, player2);
                break;
            case FightResult.TeamBWon:
                MoveResourcesFromLoserToWinner(player2, player1);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        message += $"Player #1 Information:\n" +
                   $"{player1}\n\n" +
                   $"Player #2 Information:\n" +
                   $"{player2}\n\n" +
                   $"THE WAR IS OVER FOR TODAY. LET THE TROOPS REST UNTIL NEXT TIME!";

        Console.WriteLine(message);
    }

    private void MoveResourcesFromLoserToWinner(Player winner, Player loser)
    {
        var winnerAliveUnits = Unit.GetAliveUnitsList(winner.Units);
        uint totalStolen = 0;
        foreach (var aliveUnit in winnerAliveUnits)
        {
            var amountToSteal = RandomChanceChecker.GetRandomInteger(aliveUnit.Capacity + 1, 1);
            var stolenResources = loser.StealResources((uint)amountToSteal, $"{aliveUnit.UnitName} [{winner.Name}]");

            // Means the loser has no resources left
            if (stolenResources == 0)
            {
                break;
            }

            totalStolen += stolenResources;
            winner.GainResources(stolenResources);
        }

        Console.WriteLine(
            $"\n{winner.Name}'s unit[s] stole a total of {totalStolen} resource[s] from {loser.Name} after winning the war!\n");
    }

    /// <summary>
    /// The algorithm for the actual units fight, which ends when one (or both) of the teams has no units left.
    /// Note that at every step of the algo, a unit from (<paramref name="teamA"/>) will try to attack first a unit from (<paramref name="teamB"/>),
    /// so there is some significant to the order by which the lists are being passed. 
    /// </summary>
    /// <param name="teamA">List of Unit objects that represents the first team</param>
    /// <param name="teamB">List of Unit objects that represents the second team</param>
    /// <returns>The result of the units fight</returns>
    private FightResult UnitsFight(List<Unit> teamA, List<Unit> teamB)
    {
        Console.WriteLine($"-------THE UNITS FIGHT HAS STARTED-------");
        var roundCounter = 1;
        var liveUnitsCountTeamA = Unit.GetAliveUnitsCount(teamA);
        var liveUnitsCountTeamB = Unit.GetAliveUnitsCount(teamB);

        while (liveUnitsCountTeamA > 0 && liveUnitsCountTeamB > 0)
        {
            Console.WriteLine($"\n\nRound #{roundCounter} started.\n\n" +
                              $"Player 1 unit[s] alive: {liveUnitsCountTeamA} | " +
                              $"Player 2 unit[s] alive: {liveUnitsCountTeamB}\n");

            if (_currentWeatherRoundsCounter == 0)
            {
                _currentWeatherRoundsCounter = RandomChanceChecker.GetRandomInteger(MaxRoundsWeatherEffect + 1, 1);
                _currentWeather = SelectNewWeatherWithChance();
            }

            ApplyWeatherEffectToUnitList(teamA, _currentWeather);
            ApplyWeatherEffectToUnitList(teamB, _currentWeather);
            _currentWeatherRoundsCounter--;

            RoundFight(teamA, teamB);

            liveUnitsCountTeamA = Unit.GetAliveUnitsCount(teamA);
            liveUnitsCountTeamB = Unit.GetAliveUnitsCount(teamB);

            Console.WriteLine($"\nRound #{roundCounter} ended.\n");
            
            roundCounter++;
        }

        // Handling the edge case where the 2 teams has no live units left (most likely because of a weather event)
        if (liveUnitsCountTeamA == 0 && liveUnitsCountTeamB == 0)
        {
            Console.WriteLine($"\n\nThe fight is over at round #{roundCounter}!\n" +
                              $"The two teams has no live unit left!\n" +
                              $"Therefore, the fight ended with no winner. Only losers.\n" +
                              $"-------THE FIGHT IS OVER-------");
            return FightResult.Draw;
        }

        var isTeamAWinner = liveUnitsCountTeamA > 0;

        var winnerString = isTeamAWinner ? "Player 1" : "Player 2";
        var winnerTeamAliveCount = isTeamAWinner ? liveUnitsCountTeamA : liveUnitsCountTeamB;

        Console.WriteLine($"\n\nThe fight is over at round #{roundCounter}!\n" +
                          $"The winner is: {winnerString}!!\n" +
                          $"They won having {winnerTeamAliveCount} alive unit[s] left.\n" +
                          $"-------THE UNITS FIGHT IS OVER-------");

        return isTeamAWinner ? FightResult.TeamAWon : FightResult.TeamBWon;
    }

    private void RoundFight(List<Unit> teamA, List<Unit> teamB)
    {
        if (Unit.GetAliveUnitsCount(teamA) == 0 || Unit.GetAliveUnitsCount(teamB) == 0)
        {
            Console.WriteLine($"One of the players has no units alive! aborting the round's fight!");
            return;
        }

        var aliveUnitsTeamA = Unit.GetAliveUnitsList(teamA);
        var aliveUnitsTeamB = Unit.GetAliveUnitsList(teamB);

        var randomUnitTeamA = aliveUnitsTeamA[RandomChanceChecker.GetRandomInteger(aliveUnitsTeamA.Count)];
        var randomUnitTeamB = aliveUnitsTeamB[RandomChanceChecker.GetRandomInteger(aliveUnitsTeamB.Count)];
        
        Console.WriteLine($"\n[Player 1]'s random unit to fight this round:\n" +
                          $"{randomUnitTeamA}\n");
        
        Console.WriteLine($"[Player 2]'s random unit to fight this round:\n" +
                          $"{randomUnitTeamB}\n\n");

        Console.WriteLine($"\n[Player 1] {randomUnitTeamA.UnitName}'s turn to attack\n");
        randomUnitTeamA.Fight(randomUnitTeamB);
        
        Console.WriteLine($"\n[Player 2] {randomUnitTeamB.UnitName}'s turn to attack\n");
        randomUnitTeamB.Fight(randomUnitTeamA);
    }

    private static void ApplyWeatherEffectToUnitList(List<Unit> teamA, Weather newWeather)
    {
        teamA.ForEach((unit) => unit.WeatherEffect(newWeather));
    }

    private Weather SelectNewWeatherWithChance()
    {
        var didSucceed = RandomChanceChecker.DidChanceSucceed(WeatherEffectChancePercent);
        if (!didSucceed)
        {
            return Weather.None;
        }

        var randomWeather = RandomChanceChecker.GetRandomInteger(
            Enum.GetValues(typeof(Weather)).Length, 1);
        return (Weather)randomWeather;
    }

    private enum FightResult
    {
        Draw,
        TeamAWon,
        TeamBWon,
    }
}

public static class RandomChanceChecker
{
    private static readonly Random Random;

    static RandomChanceChecker()
    {
        Random = new Random();
    }

    public static bool DidChanceSucceed(int chancePercents)
    {
        var random = Random.Next(100);

        return random < chancePercents;
    }

    public static int GetRandomInteger(int maxValueExclusive, int minValueInclusive = 0)
    {
        return Random.Next(minValueInclusive, maxValueExclusive);
    }
}

public enum Race
{
    Human,
    Gnome,
    Elf
}

public enum Weather
{
    None,
    Sunny,
    Rainy,
    Snowy,
    Gusty
}