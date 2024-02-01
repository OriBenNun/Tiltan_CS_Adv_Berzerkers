using System;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;

public abstract class Siege : Unit
{
    private const int ChanceToDoubleAttack = 65;

    protected Siege(Race race) : base(race) { }

    // Has 65% chance tp double attack if has more damage than the target
    public override void Attack(Unit target)
    {
        base.Attack(target);

        if (Damage <= target.Damage ||
            !RandomChanceChecker.DidChanceSucceed(ChanceToDoubleAttack))
        {
            return;
        }

        Console.WriteLine($"{UnitName} (Siege) succeeded a double attack on {target.UnitName}!");
        
        base.Attack(target);
    }
}

// Giant = Human Siege
public sealed class Giant : Siege
{
    public Giant(int hp = 180, int damage = 2, int defense = 5) : base(Race.Human)
    {
        UnitName = "Giant";
        Hp = hp;
        Damage = damage;
        Defense = defense;
    }

    // Giant special ability => counter attack upon successful block
    protected override void BlockAttack(Unit attacker)
    {
        base.BlockAttack(attacker);

        Attack(attacker);
    }
}

// SoulBreaker = Gnome Siege
public sealed class SoulBreaker : Siege
{
    public SoulBreaker(int hp = 130, int damage = 4, int defense = 2) : base(Race.Gnome)
    {
        UnitName = "SoulBreaker";
        Hp = hp;
        Damage = damage;
        Defense = defense;
    }

    // SoulBreaker special ability => can defend using either its damage or defense stats
    protected override void Defend(Unit attacker)
    {
        if (attacker.Damage < Damage)
        {
            BlockAttack(attacker);
            return;
        }

        base.Defend(attacker);
    }
}