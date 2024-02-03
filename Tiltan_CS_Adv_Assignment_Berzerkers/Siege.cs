// ---- C# II (Dor Ben Dor) ----
// Ori Ben Nun
// ----------------------------

using System;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;

public abstract class Siege : Unit
{
    private const int ChanceToDoubleAttack = 65;

    protected Siege(Race race, string className, Dice damageDice, Dice defenseDice, Dice hitChanceDice, int hp,
        int capacity) : base(
        race,
        className,
        damageDice,
        defenseDice,
        hitChanceDice,
        hp,
        capacity)
    {
    }

    public override string ToString()
    {
        return base.ToString() +
               "Archetype: Siege\n" +
               $"Double Attack Chance: {ChanceToDoubleAttack}%\n";
    }

    // Has 65% chance to double attack if damage rolled more than the target after the first attack
    protected override void Attack(Unit target)
    {
        base.Attack(target);

        if (GetUnitDamageRoll() <= target.GetUnitDamageRoll() ||
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
    public Giant(string name = null) : base(
        Race.Human,
        "Giant",
        new Dice(1,8,0),
        new Dice(2,12,2),
        new Dice(2,6,-1),
        180,
        100)
    {
        UnitName = GetFixedName(name, ClassName);
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
    public SoulBreaker(int hp = 130, int damageDice = 5, int defenseDice = 2, string name = null) :
        base(Race.Gnome,
            "SoulBreaker",
            new Dice(),
            new Dice(),
            new Dice(),
            130,
            30)
    {
        UnitName = GetFixedName(name, ClassName);
    }

    // SoulBreaker special ability => can defend using either its damage or defense stats
    protected override void Defend(Unit attacker)
    {
        if (attacker.DamageDice < DamageDice)
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

    public Tank(int hp = 210, int damageDice = 2, int defenseDice = 6, string name = null) : base(Race.Gnome, "Tank")
    {
        UnitName = GetFixedName(name, ClassName);
        Hp = hp;
        DamageDice = damageDice;
        DefenseDice = defenseDice;
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
            DamageDice *= CriticalDamageMultiplier;
        }

        base.Attack(target);

        // Resets the state
        if (!criticalHit) return;
        DamageDice /= CriticalDamageMultiplier;
    }
}

// Paladin = Elf Siege
public sealed class Paladin : Siege
{
    public Paladin(string name = null) : base(Race.Elf, "Paladin")
    {
        UnitName = GetFixedName(name, ClassName);
        Hp = 175;
        DamageDice = new Dice();
        DefenseDice = new Dice();
    }

    // Paladin special ability => upon succeeding the Siege Double Attack - the second attack is doubled
    protected override void SiegeDoubleAttack(Unit target)
    {
        DamageDice *= 2;

        Console.WriteLine($"{UnitName} is doubling their damage for the second attack!\n");

        base.SiegeDoubleAttack(target);

        DamageDice /= 2;
    }
}