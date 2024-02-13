using System;
using System.Collections.Generic;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;

public class GameManager
{
    private const int WeatherEffectChancePercent = 30;
    private const int MaxRoundsWeatherEffect = 3;

    private int _currentWeatherRoundsCounter;
    private Weather _currentWeather = Weather.None;

    public void PlayersWar(Player player1, Player player2)
    {
        Console.WriteLine($"Two Brave Players Have Showed Up For An Unbelievable War! May The Best Win!\n\n" +
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
        foreach (var aliveUnit in winnerAliveUnits)
        {
            var amountToSteal = RandomChanceChecker.GetRandomInteger(aliveUnit.Capacity + 1, 1);
            var stolenResources = loser.StealResources((uint)amountToSteal, $"{aliveUnit.UnitName} [{winner.Name}]");

            // Means the loser has no resources left
            if (stolenResources == 0)
            {
                break;
            }

            winner.GainResources(stolenResources);
        }
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
                              $"Team A units alive: {liveUnitsCountTeamA} | " +
                              $"Team B units alive: {liveUnitsCountTeamB}\n");

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

        var winnerString = isTeamAWinner ? "Team A" : "Team B";
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
            Console.WriteLine($"One of the teams has no units alive! aborting the round's fight!");
            return;
        }

        var aliveUnitsTeamA = Unit.GetAliveUnitsList(teamA);
        var aliveUnitsTeamB = Unit.GetAliveUnitsList(teamB);

        var randomUnitTeamA = aliveUnitsTeamA[RandomChanceChecker.GetRandomInteger(aliveUnitsTeamA.Count)];
        var randomUnitTeamB = aliveUnitsTeamB[RandomChanceChecker.GetRandomInteger(aliveUnitsTeamB.Count)];

        randomUnitTeamA.Fight(randomUnitTeamB);
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