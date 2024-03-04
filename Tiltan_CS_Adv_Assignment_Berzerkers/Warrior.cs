// ---- C# II (Dor Ben Dor) ----
// Ori Ben Nun
// ----------------------------

using System;



public abstract class Warrior : Unit
{
    protected int WeaponBonusModifier { get; }
    private int ShieldBonusModifier { get; }

    protected Warrior(Race race, string className, int shieldBonusModifier, int weaponBonusModifier,
        IRandomProvider damage, IRandomProvider defense ,IRandomProvider hitChance, int hp, int capacity) : base(
        race, className, damage, defense, hitChance, hp, capacity)
    {
        ShieldBonusModifier = shieldBonusModifier;
        DefenseRollModifier += ShieldBonusModifier;

        WeaponBonusModifier = weaponBonusModifier;
        DamageRollModifier += WeaponBonusModifier;
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
        if (GetIsDead()) { return; }
        
        base.Attack(target);
        
        Console.WriteLine($"{UnitName} is trying to make a warrior shield attack against {target.UnitName}");

        if (GetUnitDefenseRoll() <= target.GetUnitDefenseRoll())
        {
            Console.WriteLine($"{UnitName} missed the shield attack against {target.UnitName}");
            return;
        }

        WarriorShieldAttack(target);
    }

    protected virtual void WarriorShieldAttack(Unit target)
    {
        // If the warrior has no shield, shield attack is canceled
        if (ShieldBonusModifier <= 0)
        {
            Console.WriteLine($"{UnitName} doesn't have a shield, so their shield attack against {target.UnitName} is canceled.");
            return;
        }
        
        Console.WriteLine($"{UnitName} didn't miss the shield attack is attacking {target.UnitName} again");

        UnitBasicUncheckedAttack(target);
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
        new Dice(2,12,2),
        new Bag(6,14),
        new Dice(2,8,1),
        145,
        20)
    {
        UnitName = GetFixedName(name, ClassName);
    }

    // Barbarian special ability => can make a second attack even if they has no shield (modifier is 0)
    protected override void WarriorShieldAttack(Unit target)
    {
        UnitBasicUncheckedAttack(target);
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
        new Dice(2, 8,1),
        new Dice(2,12,2),
        new Dice(1,20,1),
        160,
        25)
    {
        UnitName = GetFixedName(name, ClassName);
    }

    public override string ToString()
    {
        return base.ToString() +
               $"{ClassName} Stats:\n" +
               $"Horse Attack Chance: {HorseAttackChance}%\n";
    }

    // Knight special ability => has 30% of making a horse attack as an extra attack (on top of the Warrior Shield Attack)
    protected override void Attack(Unit target)
    {
        if (GetIsDead()) { return; }

        base.Attack(target);

        HorseAttack(target);
    }

    private void HorseAttack(Unit target)
    {
        Console.WriteLine($"{UnitName} is rolling a check for a Horse Attack (Knight ability) against {target.UnitName}\n");

        if (!RandomChanceChecker.DidChanceSucceed(HorseAttackChance))
        {
            Console.WriteLine($"{UnitName} failed the Horse Attack check against {target.UnitName}\n");
            return;
        }

        Console.WriteLine($"{UnitName} succeed the Horse Attack check and is trying to attack {target.UnitName} again!\n");

        UnitBasicUncheckedAttack(target);
    }
}

// Rebel = Elf Warrior
public sealed class Rebel : Warrior
{
    private const int CounterAttackChance = 50;

    public Rebel(string name = null) : base(
        race: Race.Elf, 
        "Rebel",
        shieldBonusModifier: 1,
        weaponBonusModifier: 2,
        new Dice(2,10,1),
        new Dice(2,6,0),
        new Bag(14,20),
        115,
        8)
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

        Console.WriteLine($"{UnitName} succeed a counter attack check (Rebel ability) and will try to attack {attacker.UnitName} back!\n");

        UnitBasicUncheckedAttack(attacker);
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
        new Dice(1,8,0),
        new Dice(2, 10, 1),
        new Dice(3, 6, 1),
        150,
        15)
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
        if (GetIsDead()) { return; }

        var oneShotHit = RandomChanceChecker.DidChanceSucceed(OneShotChance);
        var originalModifier = DamageRollModifier;
        if (oneShotHit)
        {
            Console.WriteLine($"{UnitName} is about to kill {target.UnitName} with one shot! DAMN!!\n");
            DamageRollModifier = target.Hp;
        }

        base.Attack(target);

        // Resets the state
        if (!oneShotHit) return;
        
        DamageRollModifier = originalModifier;
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
        new Dice(1, 12, 0),
        new Dice(1, 20, 1),
        new Dice(1, 8, 3),
        150,
        25)
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
        var originalModifier = DamageRollModifier;
        if (powerShieldAttack)
        {
            Console.WriteLine(
                $"{UnitName} succeeded a Power Shield Attack check (Guardian ability) which multiplied their weapon damage by {PowerShieldAttackMultiplier} for this shield attack check!\n");
            DamageRollModifier = WeaponBonusModifier * PowerShieldAttackMultiplier;
        }

        base.WarriorShieldAttack(target);

        // Resets the state
        if (!powerShieldAttack) return;
        
        DamageRollModifier = originalModifier;
    }
}