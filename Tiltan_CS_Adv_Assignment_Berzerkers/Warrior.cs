// ---- C# II (Dor Ben Dor) ----
// Ori Ben Nun
// ----------------------------

using System;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;

public abstract class Warrior : Unit
{
    protected int WeaponBonusModifier { get; }
    private int ShieldBonusModifier { get; }

    protected Warrior(Race race, string className, int shieldBonusModifier, int weaponBonusModifier,
        Dice damageDice, Dice defenseDice ,Dice hitChanceDice, int hp, int capacity) : base(
        race, className, damageDice, defenseDice, hitChanceDice, hp, capacity)
    {
        ShieldBonusModifier = shieldBonusModifier;
        UpdateDefenseDiceModifier(ShieldBonusModifier);

        WeaponBonusModifier = weaponBonusModifier;
        UpdateDamageDiceModifier(WeaponBonusModifier);
    }

    public override string ToString()
    {
        return base.ToString() +
               "Archetype: Warrior\n" +
               $"Weapon Bonus Modifier: {WeaponBonusModifier}\n" +
               $"Shield Bonus Modifier: {ShieldBonusModifier}\n";
    }

    // Warrior special ability => makes a second shield attack if defense rolling higher than the target after the first attack
    protected override void Attack(Unit target)
    {
        base.Attack(target);

        if (GetUnitDefenseRoll() <= target.GetUnitDefenseRoll())
        {
            return;
        }

        WarriorShieldAttack(target);
    }

    protected virtual void WarriorShieldAttack(Unit target)
    {
        // If the warrior has no shield, shield attack is canceled
        if (ShieldBonusModifier <= 0)
        {
            return;
        }

        base.Attack(target);
    }
}

// Barbarian => Human Warrior
public sealed class Barbarian : Warrior
{
    public Barbarian(string name = null) : base(
        race: Race.Human, 
        "Barbarian",
        shieldBonusModifier: 0,
        weaponBonusModifier: 3,
        new Dice(2,8,2),
        new Dice(1,8,-1),
        new Dice(2,6,1),
        145,
        60)
    {
        UnitName = GetFixedName(name, ClassName);
    }

    // Barbarian special ability => can make a second attack even if they has no shield (modifier is 0)
    protected override void WarriorShieldAttack(Unit target)
    {
        Attack(target);
    }
}

// Knight => Human Warrior
public sealed class Knight : Warrior
{
    private const int HorseAttackChance = 30;

    public Knight(string name = null) : base(
        race: Race.Human,
        "Knight",
        shieldBonusModifier: 2,
        weaponBonusModifier: 1,
        new Dice(1, 8,1),
        new Dice(2,12,2),
        new Dice(2,8,0),
        160,
        70)
    {
        UnitName = GetFixedName(name, ClassName);
    }

    public override string ToString()
    {
        return base.ToString() +
               $"{ClassName} Stats:\n" +
               $"Horse Attack Chance: {HorseAttackChance}%\n";
    }

    // Knight special ability => has 30% of making a horse attack as an extra attack
    protected override void Attack(Unit target)
    {
        base.Attack(target);

        HorseAttack(target);
    }

    private void HorseAttack(Unit target)
    {
        if (!RandomChanceChecker.DidChanceSucceed(HorseAttackChance))
        {
            return;
        }

        Console.WriteLine($"{UnitName} succeed a horse attack and is attacking {target.UnitName} again!\n");

        base.Attack(target);
    }
}

// Rebel = Elf Warrior
public sealed class Rebel : Warrior
{
    private const int CounterAttackChance = 50;

    public Rebel(string name = null) : base(
        race: Race.Elf, 
        "Knight",
        shieldBonusModifier: 2,
        weaponBonusModifier: 1,
        new Dice(2,8,1),
        new Dice(1,8,0),
        new Dice(3,8,2),
        100,
        35)
    {
        UnitName = GetFixedName(name, ClassName);
    }

    public override string ToString()
    {
        return base.ToString() +
               $"{ClassName} Stats:\n" +
               $"Counter Attack Chance: {CounterAttackChance}%\n";
    }

    // Rebel special ability => has 50% chance to counter attack every time they being attacked
    protected override void Defend(Unit attacker)
    {
        base.Defend(attacker);

        if (!RandomChanceChecker.DidChanceSucceed(CounterAttackChance))
        {
            return;
        }

        Console.WriteLine($"{UnitName} succeed a counter attack and is attacking {attacker.UnitName} back!\n");

        Attack(attacker);
    }
}

// UnderTaker = Gnome Warrior
public sealed class UnderTaker : Warrior
{
    private const int OneShotChance = 10;

    public UnderTaker(string name = null) : base(
        race: Race.Gnome, 
        "UnderTaker",
        shieldBonusModifier: 2,
        weaponBonusModifier: 2,
        new Dice(1,8,1),
        new Dice(2, 8, 1),
        new Dice(1, 12, 1),
        150,
        45)
    {
        UnitName = GetFixedName(name, ClassName);
    }

    public override string ToString()
    {
        return base.ToString() +
               $"{ClassName} Stats:\n" +
               $"One Shot Attack Chance: {OneShotChance}%\n";
    }

    // UnderTaker special ability => has small chance (10%) to inta kill (one shot) upon attacking!
    protected override void Attack(Unit target)
    {
        var oneShotHit = RandomChanceChecker.DidChanceSucceed(OneShotChance);
        var originalModifier = GetDamageDiceModifier();
        if (oneShotHit)
        {
            Console.WriteLine($"{UnitName} is about to kill {target.UnitName} with one shot! DAMN!!\n");
            UpdateDamageDiceModifier(target.Hp);
        }

        base.Attack(target);

        // Resets the state
        if (!oneShotHit) return;
        UpdateDamageDiceModifier(originalModifier);
    }
}

// Guardian = Elf Warrior
public sealed class Guardian : Warrior
{
    private const int PowerShieldAttackChance = 45;
    private const int PowerShieldAttackMultiplier = 4;

    public Guardian(string name = null) : base(
        race: Race.Elf, 
        "Guardian",
        shieldBonusModifier: 4,
        weaponBonusModifier: 2,
        new Dice(1, 8, 0),
        new Dice(2, 12, 2),
        new Dice(2, 8, 1),
        150,
        75)
    {
        UnitName = GetFixedName(name, ClassName);
    }

    public override string ToString()
    {
        return base.ToString() +
               $"{ClassName} Stats:\n" +
               $"Power Shield Attack Chance: {PowerShieldAttackChance}%\n" +
               $"Power Shield Damage Multiplier: x{PowerShieldAttackMultiplier}\n";
    }

    // Guardian special ability => Upon making a shield attack - has a 45% to multiply their weapon damage by 4
    protected override void WarriorShieldAttack(Unit target)
    {
        var powerShieldAttack = RandomChanceChecker.DidChanceSucceed(PowerShieldAttackChance);
        if (powerShieldAttack)
        {
            Console.WriteLine(
                $"{UnitName} multiplied their weapon damage by {PowerShieldAttackMultiplier} for this shield attack!\n");
            UpdateDamageDiceModifier(WeaponBonusModifier * PowerShieldAttackMultiplier);
        }

        base.WarriorShieldAttack(target);

        // Resets the state
        if (!powerShieldAttack) return;
        UpdateDamageDiceModifier(WeaponBonusModifier / PowerShieldAttackMultiplier);
    }
}