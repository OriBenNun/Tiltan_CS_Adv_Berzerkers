using System;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;

public abstract class Warrior : Unit
{
    private int ShieldBonusModifier { get; }
    private int WeaponBonusModifier { get; }
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
    public override void Attack(Unit target)
    {
        base.Attack(target);

        if (Defense <= target.Defense)
        {
            return;
        }

        ShieldAttack(target);
    }

    protected virtual void ShieldAttack(Unit target)
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
    public Barbarian(int hp = 130, int damage = 3, int defense = 2, string name = null) :
        base(race: Race.Human,
            "Barbarian",
            shieldBonusModifier: 0,
            weaponBonusModifier: 1)
    {
        UnitName = GetFixedName(name, ClassName);
        Hp = hp;
        Damage = damage;
        Defense = defense;
    }

    protected override void ShieldAttack(Unit target)
    {
        // Barbarian special ability => can make a second attack even if they has no shield (modifier is 0)
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
        weaponBonusModifier: 0)
    {
        UnitName = GetFixedName(name, ClassName);
        Hp = hp;
        Damage = damage;
        Defense = defense;
    }

    public override string ToString()
    {
        return base.ToString() +
               "Knight Stats:\n" +
               $"Horse Attack Chance: {HorseAttackChance}%\n";
    }

    // Knight special ability => has 30% of making a horse attack as an extra attack
    public override void Attack(Unit target)
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

        Console.WriteLine($"{UnitName} succeed a horse attack and is attacking {target} again!\n");

        base.Attack(target);
    }
}

// Rebel = Elf Warrior
public sealed class Rebel : Warrior
{
    private const int CounterAttackChance = 50;

    public Rebel(int hp = 100, int damage = 2, int defense = 3, string name = null) :
        base(race: Race.Elf, "Knight", shieldBonusModifier: 2,
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
               "Rebel Stats:\n" +
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

        Console.WriteLine($"{UnitName} succeed a counter attack and is attacking {attacker} back!\n");

        Attack(attacker);
    }
}