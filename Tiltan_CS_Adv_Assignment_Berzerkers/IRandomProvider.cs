using System;

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