using System;

namespace Tiltan_CS_Adv_Assignment_Berzerkers
{
    public abstract class Unit
    {
        public virtual string Name { get; protected set; } = "Unit";
        public virtual int Hp { get; protected set; } = 100;
        public virtual int Damage { get; protected set; } = 1;

        public virtual void Attack(Unit target) => target.Defend(this);
        public virtual void Defend(Unit attacker) => TakeDamage(attacker.Damage);

        private void TakeDamage(int damageToTake)
        {
            Hp = Math.Max(0, Hp - damageToTake);
            if (Hp > 0) return;
            
            Die();
        }

        private void Die()
        {
            Console.WriteLine($"Unit named: '{Name}' is dead!");
        }

        public override string ToString()
        {
            return $"Unit Stats:\n" +
                   $"Hp: {Hp}\n" +
                   $"Damage: {Damage}\n";
        }
    }

    public sealed class MeleeUnit : Unit
    {
        public MeleeUnit()
        {
            Name = "Melee Unit";
            Hp = 120;
            Damage = 3;
        }
        public int WeaponBonusModifier { get; set; } = 5;

        public override string ToString()
        {
            return base.ToString() +
                   $"Melee Unit Stats:\n" +
                   $"Weapon Bonus Modifier: {WeaponBonusModifier}\n";
        }

        public override int Damage => base.Damage + WeaponBonusModifier;
    }
}