using System;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;

public abstract class Warrior : Unit
{
    private int ShieldBonusModifier { get; }
    private int WeaponBonusModifier { get; }
    public override int Defense => base.Defense + ShieldBonusModifier;
    public override int Damage => base.Damage + WeaponBonusModifier;

    protected Warrior(Race race, int shieldBonusModifier, int weaponBonusModifier) : base(race)
    {
        ShieldBonusModifier = shieldBonusModifier;
        WeaponBonusModifier = weaponBonusModifier;
    }

    protected override void Attack(Unit target)
    {
        base.Attack(target);

        // Warrior special ability => makes a shield attack if has higher defense than the target
        if (Defense <= target.Defense) { return; }

        ShieldAttack(target);
    }

    protected virtual void ShieldAttack(Unit target)
    {
        // If the warrior has no shield, shield attack is canceled
        if (ShieldBonusModifier <= 0) { return; }
            
        Attack(target);
    }

    public override string ToString()
    {
        return base.ToString() +
               $"Melee Unit Stats:\n" +
               $"Weapon Bonus Modifier: {WeaponBonusModifier}\n" +
               $"Shield Bonus Modifier: {ShieldBonusModifier}\n";
    }
}

public sealed class Barbarian : Warrior
{
    public Barbarian(int hp = 130, int damage = 3, int defense = 2) : base(race: Race.Human, shieldBonusModifier: 0,
        weaponBonusModifier: 1)
    {
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
    
public sealed class Knight : Warrior
{

    private const int HorseAttackChance = 30;
    
    public Knight(int hp = 160, int damage = 2, int defense = 4) : base(race: Race.Human, shieldBonusModifier: 2,
        weaponBonusModifier: 0)
    {
        Hp = hp;
        Damage = damage;
        Defense = defense;
    }

    protected override void Attack(Unit target)
    {
        base.Attack(target);
        
        // Knight special ability => has 30% of making a horse attack as an extra attack

        HorseAttack(target);
    }

    private void HorseAttack(Unit target)
    {
        var random = new Random().Next(0, 100);

        if (random < HorseAttackChance) { return; }
        
        Attack(target);
    }
}