using System;

namespace Tiltan_CS_Adv_Assignment_Berzerkers
{
    public abstract class Unit
    {
        protected virtual string UnitName { get; set; } = "Unit";
        protected virtual int Hp { get; set; } = 100;
        public virtual int Damage { get; protected set; } = 1;
        public virtual int Defense { get; protected set; } = 1;
        protected Race Race { get; }

        protected Unit(Race race)
        {
            Race = race;
        }


        public virtual void Attack(Unit target) => target.Defend(this);

        protected virtual void Defend(Unit attacker)
        {
            var attackerDamage = attacker.Damage;

            if (attackerDamage < Defense)
            {
                Console.WriteLine($"{UnitName} blocked {attacker.UnitName}'s attack!");
                return;
            }

            TakeDamage(attacker.Damage);
        }

        private void TakeDamage(int damageToTake)
        {
            Hp = Math.Max(0, Hp - damageToTake);
        
            Console.WriteLine($"{UnitName} received {damageToTake} damage!\n" +
                              $"It has now {Hp} HP");
        
            if (Hp > 0) return;
        
            Die();
        }

        private void Die()
        {
            Console.WriteLine($"{UnitName} is dead!");
        }

        public override string ToString()
        {
            return $"Unit Stats:\n" +
                   $"Unit Name: {UnitName}\n" +
                   $"Race: {Race}\n" +
                   $"Hp: {Hp}\n" +
                   $"Defense: {Defense}\n" +
                   $"Damage: {Damage}\n";
        }
    }

    public enum Race
    {
        Human,
        Gnome,
        Elf
    }
}