// ---- C# II (Dor Ben Dor) ----
// Ori Ben Nun
// ----------------------------

#region Classes Overview
// Humans:
// Giant => Human Siege
// Barbarian => Human Warrior
// Knight => Human Warrior

// Gnomes:
// SoulBreaker => Gnome Siege
// Tank => Gnome Siege
// UnderTaker => Gnome Warrior

// Elves:
// Paladin => Elf Siege
// Rebel => Elf Warrior
// Guardian => Elf Warrior 
#endregion

using System;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;

public abstract class Unit
{
    private const int MinRollToHitTargetDefenseFactor = 2;
    
    public string UnitName { get; protected set; } = "Unit";
    protected int Hp { get; private set; }
    protected Dice DamageDice { get; set; }
    protected Dice DefenseDice { get; set; }
    protected string ClassName { get; }
    private Race Race { get; }
    private int Capacity { get; }
    private Dice HitChanceDice { get; }
    
    private bool _isDead;
    public event Action OnUnitDied;

    protected Unit(Race race, string className, Dice damageDice, Dice defenseDice, Dice hitChanceDice, int hp, int capacity)
    {
        Race = race;
        ClassName = className;
        DamageDice = damageDice;
        DefenseDice = defenseDice;
        HitChanceDice = hitChanceDice;
        Hp = hp;
        Capacity = capacity;
    }

    public override string ToString()
    {
        return $"\nUnit Stats:\n" +
               $"Unit Name: {UnitName}\n" +
               $"Class Type: {ClassName}\n" +
               $"Race: {Race}\n" +
               $"Hp: {Hp}\n" +
               $"Defense: {DefenseDice}\n" +
               $"Damage: {DamageDice}\n" +
               $"Capacity: {Capacity}\n";
    }

    protected static string GetFixedName(string name, string className)
    {
        var fixedName = className;
        if (!string.IsNullOrEmpty(name))
        {
            fixedName = $"{name} ({className})";
        }

        return fixedName;
    }

    protected virtual void Attack(Unit target)
    {
        if (target == this)
        {
            Console.WriteLine($"{UnitName} tried to attack itself! aborting action.");
            return;
        }

        if (_isDead)
        {
            Console.WriteLine($"{UnitName} tried to attack but they're dead! aborting action.");
            return;
        }

        var minRollToHit = target.GetUnitDefenseRoll() / MinRollToHitTargetDefenseFactor;

        if (HitChanceDice.Roll(UnitName) < minRollToHit)
        {
            Console.WriteLine($"{UnitName} missed the attack against {target.UnitName}!\n");
            return;
        }

        Console.WriteLine($"{UnitName} is attacking {target.UnitName}!\n");

        target.Defend(this);
    }

    protected virtual void Defend(Unit attacker)
    {
        if (_isDead)
        {
            Console.WriteLine($"{UnitName} tried to defend but they're dead! aborting action.");
            return;
        }

        var attackerDamage = attacker.GetUnitDamageRoll();

        if (attackerDamage <= GetUnitDefenseRoll())
        {
            BlockAttack(attacker);
            return;
        }

        TakeDamage(attackerDamage);
    }

    protected virtual void BlockAttack(Unit attacker)
    {
        Console.WriteLine($"{UnitName} blocked {attacker.UnitName}'s attack!\n");
    }
    
    public int GetUnitDamageRoll() => DamageDice.Roll(UnitName);
    public int GetUnitDefenseRoll() => DefenseDice.Roll(UnitName);

    private void TakeDamage(int damageToTake)
    {
        Hp = Math.Max(0, Hp - damageToTake);

        Console.WriteLine($"{UnitName} received {damageToTake} damage!\n" +
                          $"It has now {Hp} HP\n");

        if (Hp > 0) return;

        Die();
    }

    private void Die()
    {
        Console.WriteLine($"{UnitName} is dead!\n");
        _isDead = true;
        OnUnitDied?.Invoke();
    }
}

public enum Race
{
    Human,
    Gnome,
    Elf
}

public readonly struct Dice : IEquatable<Dice>
{
    public uint Scalar { get; } = 1;
    public uint BaseDie { get; } = 6;
    public int Modifier { get; } = 0;

    public Dice(uint scalar, uint baseDie, int modifier)
    {
        Scalar = scalar;
        BaseDie = baseDie;
        Modifier = modifier;
    }

    public int Roll(string unitName = "[Unknown Unit]")
    {
        var result = 0;
        
        for (var i = 0; i < Scalar; i++)
        {
            var rollResult = RandomChanceChecker.GetRandomInteger(BaseDie + 1, 1);
            result += rollResult;
        }
        
        Console.WriteLine($"[{unitName}] rolled: {result + Modifier}");
        return result + Modifier;
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

        return $"Dice Stats: {Scalar}d{BaseDie} {suffix}";
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

    public static int GetRandomInteger(uint maxValueExclusive, int minValueInclusive = 0)
    {
        return Random.Next(minValueInclusive, (int)maxValueExclusive);
    }
}