using System;
using System.Collections.Generic;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;

internal class Program
{
    public static void Main(string[] args)
    {
        var randomFighter = new RandomFighter();
        
        var deck = new Deck<int>(40);

        for (var i = 0; i < deck.Size(); i++)
        {
            var randomValue = RandomChanceChecker.GetRandomInteger(21, 1);
            deck.InjectValue(randomValue, i);
        }

        var dice = new Dice<int>(20);
        
        for (var i = 0; i < dice.Size(); i++)
        {
            dice.InjectValue(i, i);
        }
        
        randomFighter.Fight(deck, dice);
    }

    private static void StartNewBerzerkersWar()
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
}