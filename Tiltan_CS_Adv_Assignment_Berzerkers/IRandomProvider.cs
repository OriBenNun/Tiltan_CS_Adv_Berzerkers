using System;
using System.Collections.Generic;
using System.Linq;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;

public class NumberDice : Dice<uint>
{
    private uint Scalar { get; }
    private uint BaseDie { get; }
    private int Modifier { get; }

    public NumberDice(uint scalar, uint baseDie, int modifier) : base(new uint[baseDie])
    {
        Scalar = scalar;
        BaseDie = baseDie;
        Modifier = modifier;

        for (var i = 0; i < baseDie; i++)
        {
            InjectValue((uint)((i + 1) * Scalar + Modifier), i);
        }

        
    }

    public override uint GetRandom(string unitName) => NumberDiceRoll(unitName);

    public bool Equals(NumberDice other)
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

    private uint NumberDiceRoll(string unitName)
    {
        var result = 0;

        for (var i = 0; i < Scalar; i++)
        {
            var rollResult = RandomChanceChecker.GetRandomInteger((int)BaseDie + 1, 1);
            result += rollResult;
        }

        Console.WriteLine($"[{unitName}] rolled: {result + Modifier}");
        return (uint)(result + Modifier);
    }
}

public class Dice<T> where T : IComparable<T>
{
    private T[] DiceFaces { get; set; }
    
    public Dice(T[] diceFaces)
    {
        DiceFaces = diceFaces;
    }

    public virtual T GetRandom(string unitName)
    {
        return Roll();
    }
    
    protected void InjectValue(T val, int index)
    {
        if (index < 0 || index >= DiceFaces.Length)
        {
            throw new IndexOutOfRangeException();
        }
        
        DiceFaces[index] = val;
    }

    private T Roll()
    {
        var randomIndex = RandomChanceChecker.GetRandomInteger(DiceFaces.Length);
        return DiceFaces[randomIndex];
    }
    
}

public readonly struct Deck<T> where T : struct, IComparable<T>
{
    private uint BiggestNumber { get; }
    private uint SmallestNumber { get; }

    private readonly List<uint> _currentBag;

    public Deck(uint smallestNumber, uint biggestNumber)
    {
        if (biggestNumber <= smallestNumber)
        {
            Console.WriteLine(
                $"ATTENTION!! Bag struct was initialized with biggest number that isn't bigger than the smallest number.\n" +
                $"As a result, the biggest number value will be changed to be higher by 1 than the smallest number," +
                $" so the Bag functions properly.");
            biggestNumber = smallestNumber + 1;
        }

        SmallestNumber = smallestNumber;
        BiggestNumber = biggestNumber;

        _currentBag = new List<uint>();

        InitBag();
    }

    public int GetRandom(string unitName = "Unknown Actor")
    {
        return (int)PickFromTopOfBag(unitName);
    }

    public override string ToString()
    {
        return _currentBag.Aggregate("Bag that currently contains the number[s]:\n",
            (total, elem) => total + $"{elem}, ");
    }
    
    private void InitBag()
    {
        ReshuffleBag();
        // Console.WriteLine($"Bag was initialized with numbers between {SmallestNumber} and {BiggestNumber}");
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

    private void ResetBag()
    {
        _currentBag.Clear();

        ReshuffleBag();

        Console.WriteLine($"Bag was reset and reshuffled");
    }

    private void ReshuffleBag()
    {
        var possibleOutcomes = new List<uint>();

        for (uint i = SmallestNumber; i <= BiggestNumber; i++)
        {
            possibleOutcomes.Add(i);
        }

        var cycles = possibleOutcomes.Count;

        for (var i = 0; i < cycles; i++)
        {
            var randomIndex = RandomChanceChecker.GetRandomInteger(possibleOutcomes.Count);
            var randomElementToAdd = possibleOutcomes[randomIndex];
            possibleOutcomes.RemoveAt(randomIndex);
            _currentBag.Add(randomElementToAdd);
        }
    }
}