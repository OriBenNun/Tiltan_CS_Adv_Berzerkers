using System;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;

public class RandomFighter
{
    public void Fight<T>(Deck<T> deck, Dice<T> dice) where T : struct, IComparable<T>
    {
        var deckWins = 0;
        var diceWins = 0;
        var ties = 0;

        while (deck.TryDraw(out var card))
        {
            var diceRoll = dice.Roll();
            switch (diceRoll.CompareTo(card))
            {
                case < 0:
                    deckWins++;
                    break;
                case > 0:
                    diceWins++;
                    break;
                default:
                    ties++;
                    break;
            }
        }

        var winner = deckWins > diceWins ? "Deck" : diceWins > deckWins ? "Dice" : "Nobody (a tie)";
        
        Console.WriteLine($"Here are the fight results:\n" +
                          $"Deck Wins: {deckWins}\n" +
                          $"Dice Wins: {diceWins}\n" +
                          $"Ties: {ties}\n" +
                          $"\nThe Winner is: {winner}\n");
    }
}