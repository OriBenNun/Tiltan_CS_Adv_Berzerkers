using System;
using System.Collections.Generic;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;

public interface IRandomProvider
{
    public int GetRandom(string unitName = "[Unknown Unit]");
}

public readonly struct Dice : IEquatable<Dice>, IRandomProvider
{
    private uint Scalar { get; }
    private uint BaseDie { get; }
    private int Modifier { get; }

    public Dice()
    {
        Scalar = 1;
        BaseDie = 6;
        Modifier = 0;
    }

    public Dice(uint scalar, uint baseDie, int modifier)
    {
        Scalar = scalar;
        BaseDie = baseDie;
        Modifier = modifier;
    }

    public int GetRandom(string unitName = "[Unknown Unit]")
    {
        return Roll(unitName);
    }

    public bool Equals(Dice other)
    {
        return other.Scalar == Scalar &&
               other.BaseDie == BaseDie &&
               other.Modifier == Modifier;
    }

    // I got some help from ChatGPT to better understand
    // the bit manipulation principles and operators, and how to use them correctly to end up with a deterministic func
    public override int GetHashCode()
    {
        var hash = 17; // We start with a prime number

        // Using bit-shifting and XOR (^) to combine hash codes and improve distribution.
        // We use the values' hash codes to avoid operating on 0 or negatives, which can collapse distribution.
        hash = (hash << 7) ^ Modifier.GetHashCode();
        hash = (hash << 7) ^ BaseDie.GetHashCode();
        hash = (hash << 7) ^ Scalar.GetHashCode();

        return hash;
    }

    public override string ToString()
    {
        var suffix = Modifier.ToString();
        switch (Modifier)
        {
            case > 0:
                suffix = $"+{Modifier}";
                break;
            case 0:
                suffix = "";
                break;
        }

        return $"{Scalar}d{BaseDie} {suffix}";
    }

    private int Roll(string unitName)
    {
        var result = 0;

        for (var i = 0; i < Scalar; i++)
        {
            var rollResult = RandomChanceChecker.GetRandomInteger((int)BaseDie + 1, 1);
            result += rollResult;
        }

        Console.WriteLine($"[{unitName}] rolled: {result + Modifier}");
        return result + Modifier;
    }
}

public readonly struct Bag : IRandomProvider
{
    private uint BiggestNumber { get; }
    private uint SmallestNumber { get; }

    private readonly List<uint> _currentBag;

    public Bag(uint biggestNumber, uint smallestNumber)
    {
        if (biggestNumber <= smallestNumber)
        {
            Console.WriteLine(
                $"ATTENTION!! Bag struct was initialized with biggest number that isn't bigger than the smallest number.\n" +
                $"Changing the biggest number value to be higher by 1 than the smallest number, so the bag functions properly.");
            biggestNumber = smallestNumber + 1;
        }

        BiggestNumber = biggestNumber;
        SmallestNumber = smallestNumber;

        _currentBag = new List<uint>();

        ResetBag();
    }

    private void ResetBag()
    {
        _currentBag.Clear();

        var possibleOutcomes = new List<uint>();

        for (uint i = SmallestNumber; i <= BiggestNumber; i++)
        {
            possibleOutcomes.Add(i);
        }

        for (var i = 0; i < possibleOutcomes.Count; i++)
        {
            var randomIndex = RandomChanceChecker.GetRandomInteger(possibleOutcomes.Count);
            var randomElementToAdd = possibleOutcomes[randomIndex];
            possibleOutcomes.RemoveAt(randomIndex);
            _currentBag.Add(randomElementToAdd);
        }
        
        Console.WriteLine($"Bag was reset and reshuffled");
    }

    public int GetRandom(string unitName = "[Unknown Unit]")
    {
        return (int)PickFromTopOfBag(unitName);
    }

    private uint PickFromTopOfBag(string unitName)
    {
        if (_currentBag.Count == 0)
        {
            ResetBag();
        }
        
        var result = _currentBag[0];
        _currentBag.RemoveAt(0);
        
        Console.WriteLine($"[{unitName}] picked from bag: {result}");

        return result;
    }
}