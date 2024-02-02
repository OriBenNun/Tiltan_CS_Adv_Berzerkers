using System;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;

public abstract class Siege : Unit
{
    private const int ChanceToDoubleAttack = 65;

    protected Siege(Race race, string className) : base(race, className) { }

    public override string ToString()
    {
        return base.ToString() +
               "Archetype: Siege\n" +
               $"Double Attack Chance: {ChanceToDoubleAttack}%\n";
    }

    // Has 65% chance tp double attack if has more damage than the target
    protected override void Attack(Unit target)
    {
        base.Attack(target);

        if (Damage <= target.Damage ||
            !RandomChanceChecker.DidChanceSucceed(ChanceToDoubleAttack))
        {
            return;
        }

        SiegeDoubleAttack(target);
    }

    protected virtual void SiegeDoubleAttack(Unit target)
    {
        Console.WriteLine($"{UnitName} (Siege) succeeded a double attack on {target.UnitName}!\n");

        base.Attack(target);
    }
}

// Giant = Human Siege
public sealed class Giant : Siege
{
    public Giant(int hp = 180, int damage = 2, int defense = 5, string name = null) : base(Race.Human, "Giant")
    {
        UnitName = GetFixedName(name, ClassName);
        Hp = hp;
        Damage = damage;
        Defense = defense;
    }

    // Giant special ability => counter attack upon successful block
    protected override void BlockAttack(Unit attacker)
    {
        base.BlockAttack(attacker);

        Attack(attacker);
    }
}

// SoulBreaker = Gnome Siege
public sealed class SoulBreaker : Siege
{
    public SoulBreaker(int hp = 130, int damage = 5, int defense = 2, string name = null) :
        base(Race.Gnome,
            "SoulBreaker")
    {
        UnitName = GetFixedName(name, ClassName);
        Hp = hp;
        Damage = damage;
        Defense = defense;
    }

    // SoulBreaker special ability => can defend using either its damage or defense stats
    protected override void Defend(Unit attacker)
    {
        if (attacker.Damage < Damage)
        {
            BlockAttack(attacker);
            return;
        }

        base.Defend(attacker);
    }
}

// Tank = Gnome Siege
public sealed class Tank : Siege
{
    private const int CriticalHitChance = 25;
    private const int CriticalDamageMultiplier = 3;

    public Tank(int hp = 210, int damage = 2, int defense = 6, string name = null) : base(Race.Gnome, "Tank")
    {
        UnitName = GetFixedName(name, ClassName);
        Hp = hp;
        Damage = damage;
        Defense = defense;
    }

    public override string ToString()
    {
        return base.ToString() +
               $"{ClassName} Stats:\n" +
               $"Critical Hit Chance: {CriticalHitChance}%\n" +
               $"Critical Damage Multiplier: x{CriticalDamageMultiplier}\n";
    }

    // Tank special ability => Has 25% chance to deal a critical damage (triple damage)
    protected override void Attack(Unit target)
    {
        var criticalHit = RandomChanceChecker.DidChanceSucceed(CriticalHitChance);
        if (criticalHit)
        {
            Console.WriteLine($"{UnitName} tripled their damage because of a critical hit!\n");
            Damage *= CriticalDamageMultiplier;
        }

        base.Attack(target);

        // Resets the state
        if (!criticalHit) return;
        Damage /= CriticalDamageMultiplier;
    }
}

// Paladin = Elf Siege
public sealed class Paladin : Siege
{
    public Paladin(int hp = 175, int damage = 4, int defense = 3, string name = null) :
        base(Race.Elf,
            "Paladin")
    {
        UnitName = GetFixedName(name, ClassName);
        Hp = hp;
        Damage = damage;
        Defense = defense;
    }

    // Paladin special ability => upon succeeding the Siege Double Attack - the second attack is doubled
    protected override void SiegeDoubleAttack(Unit target)
    {
        Damage *= 2;
        
        Console.WriteLine($"{UnitName} is doubling their damage for the second attack!\n");
        
        base.SiegeDoubleAttack(target);
        
        Damage /= 2;
    }
}