namespace Tiltan_CS_Adv_Assignment_Berzerkers
{
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

            if (Defense <= target.Defense)
            {
                return;
            }

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

    public sealed class Barbarian : Warrior
    {
        public Barbarian(int hp = 130, int damage = 2, int defense = 1) : base(race: Race.Human, shieldBonusModifier: 0,
            weaponBonusModifier: 1)
        {
            Hp = hp;
            Damage = damage;
            Defense = defense;
        }
    }
}