namespace Tiltan_CS_Adv_Assignment_Berzerkers
{
    public abstract class Warrior : Unit
    {
        protected int ShieldBonusModifier { get; set; } = 1;
        protected int WeaponBonusModifier { get; set; } = 2;
        public override int Defense => base.Defense + ShieldBonusModifier;
        public override int Damage => base.Damage + WeaponBonusModifier;

        public override void Attack(Unit target)
        {
            base.Attack(target);

            if (Defense <= target.Defense) { return; }
            
            ShieldAttack(target);
        }

        private void ShieldAttack(Unit target)
        {
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
}