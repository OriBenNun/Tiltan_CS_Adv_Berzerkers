using System;
using System.Collections.Generic;

public class GameManager
{
    private const int WeatherEffectChancePercent = 30;
    private const int MaxRoundsWeatherEffect = 3;

    private int _currentWeatherRoundsCounter;
    private Weather _currentWeather = Weather.None;
    
    public void UnitsFight(List<Unit> teamA, List<Unit> teamB)
    {
        Console.WriteLine($"-------THE FIGHT HAS STARTED-------");
        var roundCounter = 1;
        var liveUnitsCountTeamA = GetAliveUnitsCount(teamA);
        var liveUnitsCountTeamB = GetAliveUnitsCount(teamB);
        
        while (liveUnitsCountTeamA > 0 && liveUnitsCountTeamB > 0)
        {
            Console.WriteLine($"\n\nRound #{roundCounter} started.\n\n" +
                              $"Team A units alive: {liveUnitsCountTeamA} | " +
                              $"Team B units alive: {liveUnitsCountTeamB}");

            if (_currentWeatherRoundsCounter == 0)
            {
                _currentWeatherRoundsCounter = RandomChanceChecker.GetRandomInteger(MaxRoundsWeatherEffect + 1, 1);
                _currentWeather = SelectNewWeatherWithChance();
            }

            ApplyWeatherEffectToUnitList(teamA, _currentWeather);
            ApplyWeatherEffectToUnitList(teamB, _currentWeather);
            _currentWeatherRoundsCounter--;

            RoundFight(teamA, teamB);
            
            liveUnitsCountTeamA = GetAliveUnitsCount(teamA);
            liveUnitsCountTeamB = GetAliveUnitsCount(teamB);
            roundCounter++;
        }

        var winnerString = liveUnitsCountTeamA == 0 ? "Team B" : "Team A";
        var winnerTeamAliveCount = liveUnitsCountTeamA == 0 ? liveUnitsCountTeamB : liveUnitsCountTeamA;
        // TODO add stealing loot according to the loser resources, and the capacity of the winner's alive units (each steals a random amount if the loser has resources left)
        Console.WriteLine($"\n\nThe fight is over at round #{roundCounter}!\n" +
                          $"The winner is: {winnerString}!!\n" +
                          $"They won having {winnerTeamAliveCount} alive units left.\n" +
                          $"-------THE FIGHT IS OVER-------");
    }

    private void RoundFight(List<Unit> teamA, List<Unit> teamB)
    {
        if (GetAliveUnitsCount(teamA) == 0 || GetAliveUnitsCount(teamB) == 0)
        {
            Console.WriteLine($"One of the teams has no units alive! aborting the round's fight!");
            return;
        }
        
        var aliveUnitsTeamA = GetAliveUnitsList(teamA);
        var aliveUnitsTeamB = GetAliveUnitsList(teamB);

        var randomUnitTeamA = aliveUnitsTeamA[RandomChanceChecker.GetRandomInteger(aliveUnitsTeamA.Count)];
        var randomUnitTeamB = aliveUnitsTeamB[RandomChanceChecker.GetRandomInteger(aliveUnitsTeamB.Count)];
        
        randomUnitTeamA.Fight(randomUnitTeamB);
        randomUnitTeamB.Fight(randomUnitTeamA);
    }
    
    private List<Unit> GetAliveUnitsList(List<Unit> units)
    {
        if (units.Count == 0)
        {
            return null;
        }
        
        var aliveUnits = new List<Unit>();

        foreach (var unit in units)
        {
            if (unit.GetIsDead())
            {
                continue;
            }

            aliveUnits.Add(unit);
        }

        return aliveUnits;
    }

    private int GetAliveUnitsCount(List<Unit> units)
    {
        if (units.Count == 0)
        {
            return 0;
        }
        
        var counter = 0;

        foreach (var unit in units)
        {
            if (unit.GetIsDead())
            {
                continue;
            }

            counter++;
        }

        return counter;
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
}