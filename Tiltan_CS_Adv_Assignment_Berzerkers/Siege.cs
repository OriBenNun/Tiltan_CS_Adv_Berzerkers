// ---- C# II (Dor Ben Dor) ----
// Ori Ben Nun
// ----------------------------

using System;

public abstract class Siege : Unit
{
    private const int ChanceToDoubleAttack = 65;

    protected Siege(Race race, string className, Dice damageDice, Dice defenseDice, Dice hitChanceDice, int hp,
        int capacity) : base(race, className, damageDice, defenseDice, hitChanceDice, hp, capacity)
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
    public SoulBreaker(string name = null) : base(Race.Gnome,
            "SoulBreaker",
            new Dice(1, 20, 1),
            new Dice(2, 6, 0),
            new Dice(1, 12, 1),
            130,
            25)
    {
        UnitName = GetFixedName(name, ClassName);
    }

    // SoulBreaker special ability => can defend using either its damage or defense stats
    protected override void Defend(Unit attacker)
    {
        if (attacker.GetUnitDamageRoll() < GetUnitDamageRoll())
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

    public Tank(string name = null) : base(
        Race.Gnome,
        "Tank",
        new Dice(2, 8, 0),
        new Dice(2, 12, 2),
        new Dice(2, 8 ,0),
        210,
        100)
    {
        UnitName = GetFixedName(name, ClassName);
    }

    public override string ToString()
    {
        return base.ToString() +
               $"{ClassName} Stats:\n" +
               $"Critical Hit Chance: {CriticalHitChance}%\n" +
               $"Critical Damage Multiplier: x{CriticalDamageMultiplier}\n";
    }

    // Tank special ability => Has 25% chance to deal a triple attack
    protected override void Attack(Unit target)
    {
        if (RandomChanceChecker.DidChanceSucceed(CriticalHitChance))
        {
            Console.WriteLine($"{UnitName} succeed a triple attack check and will attack {target.UnitName} three times!\n");
            base.Attack(target);
            base.Attack(target);
            base.Attack(target);
            return;
        }

        base.Attack(target);
    }
}

// Paladin = Elf Siege
public sealed class Paladin : Siege
{
    public Paladin(string name = null) : base(
        Race.Elf,
        "Paladin",
        new Dice(1, 12, 1),
        new Dice(2, 8, 2),
        new Dice(1, 20, 0),
        175,
        60)
    {
        UnitName = GetFixedName(name, ClassName);
    }

    // Paladin special ability => upon succeeding the Siege Double Attack - the second attack modifier is tripled
    protected override void SiegeDoubleAttack(Unit target)
    {
        UpdateDamageDiceModifier(GetDamageDiceModifier() * 3);

        Console.WriteLine($"{UnitName} is doubling their damage for the second attack!\n");

        base.SiegeDoubleAttack(target);

        UpdateDamageDiceModifier(GetDamageDiceModifier() / 3);
    }
}