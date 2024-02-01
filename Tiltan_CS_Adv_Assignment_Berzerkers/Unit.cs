using System;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;

public abstract class Unit
{
    private const int ChanceToBlock = 50;

    protected string UnitName { get; set; } = "Unit";
    protected virtual int Hp { get; set; } = 100;
    public virtual int Damage { get; protected set; } = 1;
    public virtual int Defense { get; protected set; } = 1;
    private Race Race { get; }

    protected Unit(Race race)
    {
        Race = race;
    }

    public override string ToString()
    {
        return $"Unit Stats:\n" +
               $"Unit Name: {UnitName}\n" +
               $"Race: {Race}\n" +
               $"Hp: {Hp}\n" +
               $"Defense: {Defense}\n" +
               $"Damage: {Damage}\n";
    }

    public virtual void Attack(Unit target) => target.Defend(this);

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
        Console.WriteLine($"{UnitName} blocked {attacker.UnitName}'s attack!");
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
        Console.WriteLine($"{UnitName} is dead!");
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
    private static readonly Random Random = new();

    public static bool DidChanceSucceed(int chancePercents)
    {
        var random = Random.Next(100);

        Console.WriteLine($"Random number: {random}");

        return random < chancePercents;
    }
}