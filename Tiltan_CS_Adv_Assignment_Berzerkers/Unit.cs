// "Berzerkers"
// Created by Ori Ben Nun
// For Dor Ben Dor | CS Advanced class (2024)

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

using System;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;
public abstract class Unit
{
    private const int ChanceToBlock = 50;

    public string UnitName { get; set; } = "Unit";
    public virtual int Damage { get; protected set; } = 1;
    public virtual int Defense { get; protected set; } = 1;
    private Race Race { get; }
    protected virtual int Hp { get; set; } = 100;
    protected string ClassName { get; }

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

    public virtual void Attack(Unit target)
    {
        if (target == this)
        {
            Console.WriteLine($"{UnitName} tried to attack itself! aborting action.");
            return;
        }
        
        target.Defend(this);
    }

    protected virtual void Defend(Unit attacker)
    {
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
                          $"It has now {Hp} HP");

        if (Hp > 0) return;

        Die();
    }

    private void Die()
    {
        Console.WriteLine($"{UnitName} is dead!\n");
    }
}

public enum Race
{
    Human,
    Gnome,
    Elf
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

        Console.WriteLine($"Random number: {random}\n\n");

        return random < chancePercents;
    }
}