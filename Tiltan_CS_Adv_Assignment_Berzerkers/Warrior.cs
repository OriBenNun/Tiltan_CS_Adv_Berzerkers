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

    public override void Attack(Unit target)
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

// Barbarian => Human Warrior
public sealed class Barbarian : Warrior
{
    public Barbarian(int hp = 130, int damage = 3, int defense = 2) : base(race: Race.Human, shieldBonusModifier: 0,
        weaponBonusModifier: 1)
    {
        UnitName = "Barbarian";
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
    
    public Knight(int hp = 160, int damage = 2, int defense = 4) : base(race: Race.Human, shieldBonusModifier: 2,
        weaponBonusModifier: 0)
    {
        UnitName = "Knight";
        Hp = hp;
        Damage = damage;
        Defense = defense;
    }

    // Knight special ability => has 30% of making a horse attack as an extra attack
    public override void Attack(Unit target)
    {
        base.Attack(target);
        
        HorseAttack(target);
    }

    private void HorseAttack(Unit target)
    {
        if (!RandomChanceChecker.DidChanceSucceed(HorseAttackChance)) { return; }
        
        Attack(target);
    }
}

// Rebel = Elf Warrior
public sealed class Rebel : Warrior
{

    private const int CounterAttackChance = 50;
    
    public Rebel(int hp = 100, int damage = 2, int defense = 3) : base(race: Race.Elf, shieldBonusModifier: 2,
        weaponBonusModifier: 2)
    {
        UnitName = "Rebel";
        Hp = hp;
        Damage = damage;
        Defense = defense;
    }
    
    // Rebel special ability => has 50% chance to counter attack every time they being attacked
    protected override void Defend(Unit attacker)
    {
        base.Defend(attacker);
        
        if (!RandomChanceChecker.DidChanceSucceed(CounterAttackChance)) { return; }

        Attack(attacker);
    }
}