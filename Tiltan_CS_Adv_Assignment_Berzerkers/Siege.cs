// ---- C# II (Dor Ben Dor) ----
// Ori Ben Nun
// ----------------------------

using System;



public abstract class Siege : Unit
{
    private const int ChanceToDoubleAttack = 65;

    protected Siege(Race race, string className, IRandomProvider damage, IRandomProvider defense, IRandomProvider hitChance, int hp,
        int capacity) : base(race, className, damage, defense, hitChance, hp, capacity)
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
        if (GetIsDead()) { return; }

        base.Attack(target);

        Console.WriteLine($"{UnitName} is checking for a Siege double attack");
        
        if (GetUnitDamageRoll() <= target.GetUnitDamageRoll() ||
            !RandomChanceChecker.DidChanceSucceed(ChanceToDoubleAttack))
        {
            Console.WriteLine($"{UnitName} failed the Siege double attack check");
            return;
        }

        Console.WriteLine($"{UnitName} (Siege) succeeded a double attack check and is attacking {target.UnitName} again!\n");

        SiegeDoubleAttack(target);
    }

    protected virtual void SiegeDoubleAttack(Unit target)
    {
        UnitBasicUncheckedAttack(target);
    }
}

// Giant = Human Siege
public sealed class Giant : Siege
{
    public Giant(string name = null) : base(
        Race.Human,
        "Giant",
        new NumberDice(1,12,0),
        new Bag(10,20),
        new NumberDice(2,8,-1),
        180,
        32)
    {
        UnitName = GetFixedName(name, ClassName);
    }

    // Giant special ability => counter attack upon successful block
    protected override void BlockAttack(Unit attacker)
    {
        base.BlockAttack(attacker);

        UnitBasicUncheckedAttack(attacker);
    }
}

// SoulBreaker = Gnome Siege
public sealed class SoulBreaker : Siege
{
    public SoulBreaker(string name = null) : base(Race.Gnome,
        "SoulBreaker",
        new NumberDice(1, 20, 3),
        new NumberDice(2, 8, 0),
        new NumberDice(1, 12, 1),
        130,
        9)
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
        new NumberDice(1, 10, 0),
        new Bag(8,17),
        new Bag(6,16),
        210,
        30)
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

    // Tank special ability => Has 25% chance to deal a triple attack instead of the siege double attack (if succeed)
    protected override void Attack(Unit target)
    {
        if (GetIsDead()) { return; }
        
        if (RandomChanceChecker.DidChanceSucceed(CriticalHitChance))
        {
            Console.WriteLine($"{UnitName} succeed a triple attack check and will attack {target.UnitName} three times!\n");
            UnitBasicUncheckedAttack(target);
            UnitBasicUncheckedAttack(target);
            UnitBasicUncheckedAttack(target);
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
        new NumberDice(1, 20, 1),
        new NumberDice(2, 10, 2),
        new NumberDice(1, 20, 0),
        175,
        17)
    {
        UnitName = GetFixedName(name, ClassName);
    }

    // Paladin special ability => upon succeeding the Siege Double Attack - the second attack modifier is tripled
    protected override void SiegeDoubleAttack(Unit target)
    {
        var originalModifier = DamageRollModifier; 
        
        DamageRollModifier = originalModifier * 3;

        Console.WriteLine($"{UnitName} is doubling their damage for the Siege second attack!\n");

        base.SiegeDoubleAttack(target);

        DamageRollModifier = originalModifier;
    }
}