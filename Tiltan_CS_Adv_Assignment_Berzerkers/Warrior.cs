// ---- C# II (Dor Ben Dor) ----
// Ori Ben Nun
// ----------------------------

using System;

public abstract class Warrior : Unit
{
    private int ShieldBonusModifier { get; }
    protected int WeaponBonusModifier { get; set; }
    public override int Defense => base.Defense + ShieldBonusModifier;
    public override int Damage => base.Damage + WeaponBonusModifier;

    protected Warrior(Race race, string className, int shieldBonusModifier, int weaponBonusModifier) : base(race,
        className)
    {
        ShieldBonusModifier = shieldBonusModifier;
        WeaponBonusModifier = weaponBonusModifier;
    }

    public override string ToString()
    {
        return base.ToString() +
               "Archetype: Warrior\n" +
               $"Weapon Bonus Modifier: {WeaponBonusModifier}\n" +
               $"Shield Bonus Modifier: {ShieldBonusModifier}\n";
    }

    // Warrior special ability => makes a shield attack if has higher defense than the target
    protected override void Attack(Unit target)
    {
        base.Attack(target);

        if (Defense <= target.Defense)
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
    public Barbarian(int hp = 145, int damage = 3, int defense = 2, string name = null) :
        base(race: Race.Human,
            "Barbarian",
            shieldBonusModifier: 0,
            weaponBonusModifier: 3)
    {
        UnitName = GetFixedName(name, ClassName);
        Hp = hp;
        Damage = damage;
        Defense = defense;
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

    public Knight(int hp = 160, int damage = 2, int defense = 4, string name = null) : 
        base(race: Race.Human,
        "Knight",
        shieldBonusModifier: 2,
        weaponBonusModifier: 1)
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

    public Rebel(int hp = 100, int damage = 4, int defense = 3, string name = null) :
        base(race: Race.Elf, "Knight", shieldBonusModifier: 2,
            weaponBonusModifier: 1)
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

    public UnderTaker(int hp = 150, int damage = 3, int defense = 3, string name = null) :
        base(race: Race.Gnome, "UnderTaker", shieldBonusModifier: 1,
            weaponBonusModifier: 1)
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
               $"One Shot Attack Chance: {OneShotChance}%\n";
    }

    // UnderTaker special ability => has small chance (10%) to inta kill (one shot) upon attacking!
    protected override void Attack(Unit target)
    {
        var oneShotHit = RandomChanceChecker.DidChanceSucceed(OneShotChance);
        var originalDamage = Damage;
        if (oneShotHit)
        {
            Console.WriteLine($"{UnitName} is about to kill {target.UnitName} with one shot! DAMN!!\n");
            Damage = target.Hp;
        }

        base.Attack(target);

        // Resets the state
        if (!oneShotHit) return;
        Damage = originalDamage;
    }
}

// Guardian = Elf Warrior
public sealed class Guardian : Warrior
{
    private const int PowerShieldAttackChance = 45;
    private const int PowerShieldAttackMultiplier = 4;

    public Guardian(int hp = 150, int damage = 1, int defense = 2, string name = null) :
        base(race: Race.Elf, "Guardian", shieldBonusModifier: 4,
            weaponBonusModifier: 2)
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
               $"Power Shield Attack Chance: {PowerShieldAttackChance}%\n" +
               $"Power Shield Damage Multiplier: x{PowerShieldAttackMultiplier}\n";
    }

    // Guardian special ability => Upon making a shield attack - has a 45% to multiply their weapon damage by 4
    protected override void WarriorShieldAttack(Unit target)
    {
        var powerShieldAttack = RandomChanceChecker.DidChanceSucceed(PowerShieldAttackChance);
        if (powerShieldAttack)
        {
            Console.WriteLine($"{UnitName} multiplied their weapon damage by {PowerShieldAttackMultiplier} for this shield attack!\n");
            WeaponBonusModifier *= PowerShieldAttackMultiplier;
        }

        base.WarriorShieldAttack(target);

        // Resets the state
        if (!powerShieldAttack) return;
        WeaponBonusModifier /= PowerShieldAttackMultiplier;
    }
}