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

public abstract class Unit
{
    private const int ChanceToBlock = 50;

    public string UnitName { get; protected set; } = "Unit";
    public virtual int Hp { get; protected set; } = 100;
    public virtual int Damage { get; protected set; } = 1;
    public virtual int Defense { get; protected set; } = 1;
    protected string ClassName { get; }
    private Race Race { get; }

    private bool _isDead;

    protected Unit(Race race, string className)
    {
        Race = race;
        ClassName = className;
    }

    public override string ToString()
    {
        return $"\nUnit Stats:\n" +
               $"Unit Name: {UnitName}\n" +
               $"Class Type: {ClassName}\n" +
               $"Race: {Race}\n" +
               $"Hp: {Hp}\n" +
               $"Defense: {Defense}\n" +
               $"Damage: {Damage}\n" +
               $"Chance To Block: {ChanceToBlock}%\n";
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

        Console.WriteLine($"{UnitName} is attacking {target.UnitName}\n");

        target.Defend(this);
    }

    protected virtual void Defend(Unit attacker)
    {
        if (_isDead)
        {
            Console.WriteLine($"{UnitName} tried to defend but they're dead! aborting action.");
            return;
        }

        var attackerDamage = attacker.Damage;

        if (attackerDamage < Defense &&
            RandomChanceChecker.DidChanceSucceed(ChanceToBlock))
        {
            BlockAttack(attacker);
            return;
        }

        TakeDamage(attacker.Damage);
    }

    protected virtual void BlockAttack(Unit attacker)
    {
        Console.WriteLine($"{UnitName} blocked {attacker.UnitName}'s attack!\n");
    }

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
    }
}

public enum Race
{
    Human,
    Gnome,
    Elf
}

public struct Dice : IEquatable<Dice>
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

    public bool Equals(Dice other)
    {
        return other.Scalar == Scalar &&
               other.BaseDie == BaseDie &&
               other.Modifier == Modifier;
    }

    // Got some help from Google and GPT to better understand
    // the bit manipulation operators and how to use them correctly
    public override int GetHashCode()
    {
        var hash = 19; // We start with a prime number

        // Using bit-shifting and XOR (^) to combine hash codes and improve diversity
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
}