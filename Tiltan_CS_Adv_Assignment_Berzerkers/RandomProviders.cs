using System;
using System.Collections.Generic;

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

    public uint GetRandom(string unitName) => NumberDiceRoll(unitName);

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
    private T[] DiceFaces { get; }
    
    public Dice(int baseDie)
    {
        DiceFaces = new T[baseDie];

        for (var i = 0; i < baseDie; i++)
        {
            DiceFaces[i] = default;
        }
    }

    protected Dice(T[] diceFaces)
    {
        DiceFaces = diceFaces;
    }
    
    public int Size()
    {
        return DiceFaces.Length;
    }

    public void InjectValue(T val, int index)
    {
        if (index < 0 || index >= DiceFaces.Length)
        {
            throw new IndexOutOfRangeException();
        }
        
        DiceFaces[index] = val;
    }

    public T Roll()
    {
        var randomIndex = RandomChanceChecker.GetRandomInteger(DiceFaces.Length);
        return DiceFaces[randomIndex];
    }
}

public class Deck<T> where T : struct, IComparable<T>
{
    private List<T> _availableCards;
    private readonly List<T> _discardedCards = new();
    private int DeckSize { get; }
    public Deck(int size)
    {
        DeckSize = size;
        
        _availableCards = new List<T>();

        for (var i = 0; i < size; i++)
        {
            _availableCards.Add(new T());
        }
    }
    
    public void InjectValue(T val, int index)
    {
        if (index < 0 || index >= _availableCards.Count)
        {
            throw new IndexOutOfRangeException();
        }
        
        _availableCards[index] = val;
    }

    public bool TryDraw(out T card)
    {
        if (_availableCards.Count == 0)
        {
            card = default;
            return false;
        }

        card = DrawFromTop();
        return true;
    }

    private void Shuffle()
    {
        var shuffledList = new List<T>();
        var availableIndices = new List<int>();

        for (var i = 0; i < _availableCards.Count; i++)
        {
            availableIndices.Add(i);
        }

        foreach (var _ in _availableCards)
        {
            var randomIndex = RandomChanceChecker.GetRandomInteger(availableIndices.Count);
            var randomAvailableIndex = availableIndices[randomIndex];
            availableIndices.RemoveAt(randomIndex);

            shuffledList.Add(_availableCards[randomAvailableIndex]);
        }

        _availableCards = shuffledList;
    }

    public T Peek()
    {
        if (_availableCards.Count != 0) return _availableCards[0];
        
        Console.WriteLine("Deck is currently empty. Have you tried reshuffling? Default value will be returned by Peek.");
        return default;
    }

    public int Size() => DeckSize;

    public int Remaining() => _availableCards.Count;
    
    public void PrintDeck()
    {
        foreach (var card in _availableCards)
        {
            Console.WriteLine(card.ToString());
        }
    }

    private void ReShuffle()
    {
        if (_discardedCards.Count == 0)
        {
            return;
        }
        
        foreach (var discardedCard in _discardedCards)
        {
            _availableCards.Add(discardedCard);
        }
        _discardedCards.Clear();
        
        Shuffle();
    }
    
    private T DrawFromTop()
    {
        var card = _availableCards[0];
        _availableCards.RemoveAt(0);
        _discardedCards.Add(card);

        return card;
    }
}