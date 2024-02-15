using System;
using System.Collections.Generic;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;

internal class Program
{
    public static void Main(string[] args)
    {
        StartNewWar();
    }

    private static void StartNewWar()
    {
        const int maxStartingResources = 80;
        const int minStartingResources = 40;
        
        // Player 1:

        var teamAStartingResources =
            RandomChanceChecker.GetRandomInteger(maxStartingResources + 1, minStartingResources);
        var teamA = new List<Unit>
        {
            new Paladin("LichKing"),
            new Guardian("Lombo"),
            new Guardian("Combo"),
            new Rebel("Martin"),
            new Rebel("Lae'zel"),
        };

        var player1 = new Player("Shadowheart", teamA, (uint)teamAStartingResources, Race.Elf);

        // Player 2:
        
        var teamBStartingResources =
            RandomChanceChecker.GetRandomInteger(maxStartingResources + 1, minStartingResources);
        
        var teamB = new List<Unit>
        {
            new Giant("Big-Tony"),
            new Giant("Lil-Tony"),
            new Barbarian("Roku"),
            new Barbarian("Ron"),
            new Knight("Don Kishot"),
            new Knight("Harry"),
        };

        var player2 = new Player("Gale", teamB, (uint)teamBStartingResources, Race.Human);
        
        // GameManager and War initialization:

        var gm = new GameManager();
        
        gm.InitTwoPlayersWar(player1, player2);
    }
    
    private static void BagTest()
    {
        var b = new Bag(4, 10);

        b.GetRandom();
        b.GetRandom();
        b.GetRandom();
        b.GetRandom();
        b.GetRandom();
        b.GetRandom();
        b.GetRandom();
        b.GetRandom();
        b.GetRandom();
        b.GetRandom();
        b.GetRandom();
        b.GetRandom();

        Console.WriteLine(b);
    }
}