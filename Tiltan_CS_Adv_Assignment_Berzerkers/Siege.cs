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
}