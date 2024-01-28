namespace Tiltan_CS_Adv_Assignment_Berzerkers;

public abstract class MeleeUnit : Unit
{
    private int WeaponBonusModifier { get; set; } = 2;
    private int ShieldBonusModifier { get; set; } = 1;
    public override int Defense => base.Defense + ShieldBonusModifier;
    protected override int Damage => base.Damage + WeaponBonusModifier;

    protected override void Attack(Unit target)
    {
        base.Attack(target);

        if (Defense > target.Defense)
        {
            ShieldAttack(target);
        }
    }

    private void ShieldAttack(Unit target)
    {
        Attack(target);
    }

    public override string ToString()
    {
        return base.ToString() +
               "Melee Unit Stats:\n" +
               $"Weapon Bonus Modifier: {WeaponBonusModifier}\n" +
               $"Shield Bonus Modifier: {ShieldBonusModifier}\n";
    }
}