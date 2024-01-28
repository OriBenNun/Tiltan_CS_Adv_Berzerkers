using System;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;

public abstract class RangedUnit : Unit
{
    private int Range { get; set; } = 5;
    private int ProjectileDamage { get; set; } = 2;

    protected override void Attack(Unit target)
    {
        var bullet = new Projectile {Damage = ProjectileDamage};
        if (!bullet.DidHitTarget(Range)) { return; }

        Damage += bullet.Damage;
        base.Attack(target);
    }
}

internal class Projectile
{
    public int Damage { get; set; }

    public bool DidHitTarget(int range)
    {
        var distanceToTarget = new Random().Next(0, 10);

        return range >= distanceToTarget;
    }
}