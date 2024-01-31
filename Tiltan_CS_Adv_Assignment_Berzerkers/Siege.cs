namespace Tiltan_CS_Adv_Assignment_Berzerkers
{
    public abstract class Siege : Unit
    {
        // Double HP
        protected override int Hp => base.Hp * 2;
        
        // Double attack if has more damage than the target
        protected override void Attack(Unit target)
        {
            base.Attack(target);

            if (Damage <= target.Damage) { return; }
            
            base.Attack(target);
        }

        protected Siege(Race race) : base(race) {}
    }

    public sealed class Giant : Siege
    {
        public Giant(int hp = 180, int damage = 2, int defense = 5) : base(Race.Human)
        {
            Hp = hp;
            Damage = damage;
            Defense = defense;
        }

        protected override void BlockAttack(Unit attacker)
        {
            base.BlockAttack(attacker);
            
            // Giant special ability => counter attack upon successful block
            
            Attack(attacker);
        }
    }
    
    public sealed class SoulBreaker : Siege
    {
        public SoulBreaker(int hp = 130, int damage = 4, int defense = 2) : base(Race.Gnome)
        {
            Hp = hp;
            Damage = damage;
            Defense = defense;
        }

        protected override void Defend(Unit attacker)
        {
            // SoulBreaker special ability => can defend using either its damage or defense stats
            
            if (attacker.Damage < Damage)
            {
                BlockAttack(attacker);
                return;
            }
            
            base.Defend(attacker);
        }
    }
}