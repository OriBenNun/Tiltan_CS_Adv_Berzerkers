// ---- C# II (Dor Ben Dor) ----
// Ori Ben Nun
// ----------------------------

#region Classes Overview

// Humans:
// Giant => Human Siege
// Barbarian => Human Warrior
// Knight => Human Warrior

// Gnomes:
// SoulBreaker => Gnome Siege
// Tank => Gnome Siege
// UnderTaker => Gnome Warrior

// Elves:
// Paladin => Elf Siege
// Rebel => Elf Warrior
// Guardian => Elf Warrior 

#endregion

using System;
using System.Collections.Generic;

namespace Tiltan_CS_Adv_Assignment_Berzerkers;

public abstract class Unit
{
    private const int MinRollToHitTargetDefenseFactor = 2;
    private const int MinRollToBlockDefenseFactor = 2;
    
    private const int RainyWeatherDefenseModifierEffect = -3;
    private const int GustyWeatherDamageModifierEffect = 5;
    private const int SunnyWeatherHitChanceModifierEffect = 4;

    public string UnitName { get; protected set; } = "Unit";
    public int Capacity { get; }
    public int Hp { get; private set; }
    protected string ClassName { get; }
    private IRandomProvider Damage { get; set; }
    private IRandomProvider Defense { get; set; }
    private IRandomProvider HitChance { get; set; }
    private Race Race { get; }

    private Weather _currentAffectingWeather = Weather.None;

    private bool _isDead;

    protected int DamageRollModifier { get; set; }
    protected int DefenseRollModifier { get; set; }
    protected int HitChanceRollModifier { get; set; }

    protected Unit(Race race, string className, IRandomProvider damage, IRandomProvider defense,
        IRandomProvider hitChance, int hp, int capacity)
    {
        Race = race;
        ClassName = className;
        Damage = damage;
        Defense = defense;
        HitChance = hitChance;
        Hp = hp;
        Capacity = capacity;
    }

    public override string ToString()
    {
        return $"\nUnit Stats:\n" +
               $"Unit Name: {UnitName}\n" +
               $"Class Type: {ClassName}\n" +
               $"Race: {Race}\n" +
               $"Hp: {Hp}\n" +
               $"Defense: {Defense}\n" +
               $"Damage: {Damage}\n" +
               $"Hit Chance: {HitChance}\n" +
               $"Capacity: {Capacity}\n" +
               $"Weather Effect: {_currentAffectingWeather}\n";
    }

    public static List<Unit> GetAliveUnitsList(List<Unit> units)
    {
        if (units.Count == 0)
        {
            return null;
        }

        var aliveUnits = new List<Unit>();

        foreach (var unit in units)
        {
            if (unit.GetIsDead())
            {
                continue;
            }

            aliveUnits.Add(unit);
        }

        return aliveUnits;
    }

    public static int GetAliveUnitsCount(List<Unit> units)
    {
        if (units.Count == 0)
        {
            return 0;
        }

        var counter = 0;

        foreach (var unit in units)
        {
            if (unit.GetIsDead())
            {
                continue;
            }

            counter++;
        }

        return counter;
    }

    public void Fight(Unit target)
    {
        Attack(target);
    }

    public void WeatherEffect(Weather weather)
    {
        var didEffectChangedText = _currentAffectingWeather == weather ? "still" : "now";

        if (_currentAffectingWeather != Weather.None)
        {
            ResetCurrentWeatherEffect(weather);
        }

        _currentAffectingWeather = weather;

        string weatherEffectInfo;

        switch (_currentAffectingWeather)
        {
            case Weather.None:
                return;
            case Weather.Sunny:
                // Unit is temporarily gaining +4 point to their hit chance rolls (modifier)
                HitChanceRollModifier += SunnyWeatherHitChanceModifierEffect;
                weatherEffectInfo = $"TEMPORARILY adding {SunnyWeatherHitChanceModifierEffect} points to its hit chance rolls";
                break;
            case Weather.Rainy:
                // Unit is losing temporarily -3 points from their defense rolls (modifier) 
                DefenseRollModifier += RainyWeatherDefenseModifierEffect;
                weatherEffectInfo = $"TEMPORARILY loses {RainyWeatherDefenseModifierEffect} points from their defense rolls.";
                break;
            case Weather.Snowy:
                // Unit is losing hp equals to half of their capacity (the bigger the unit is the more damage it takes)
                TakeDamage(Capacity / 2);
                weatherEffectInfo = "Taking damage for an amount equals to third of their capacity";
                break;
            case Weather.Gusty:
                // Unit is temporarily gaining +5 to its damage rolls (modifier)
                DamageRollModifier += GustyWeatherDamageModifierEffect;
                weatherEffectInfo = $"TEMPORARILY adding {GustyWeatherDamageModifierEffect} points to its damage rolls";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(weather), weather, null);
        }

        Console.WriteLine(
            $"{_currentAffectingWeather} Weather effect on {UnitName} is {didEffectChangedText} active ({weatherEffectInfo})");
    }

    public int GetUnitDamageRoll() => Damage.GetRandom(UnitName) + DamageRollModifier;
    public int GetUnitDefenseRoll() => Defense.GetRandom(UnitName) + DefenseRollModifier;
    protected bool GetIsDead() => _isDead;
    protected static string GetFixedName(string name, string className)
    {
        var fixedName = className;
        if (!string.IsNullOrEmpty(name))
        {
            fixedName = $"{name} ({className})";
        }

        return fixedName;
    }

    protected virtual void Attack(Unit target)
    {
        if (target == this)
        {
            Console.WriteLine($"{UnitName} tried to attack itself! aborting action.");
            return;
        }

        if (_isDead)
        {
            Console.WriteLine($"{UnitName} tried to attack but they're dead! aborting action.");
            return;
        }

        if (target.GetIsDead())
        {
            Console.WriteLine(
                $"{UnitName} tried to attack {target.UnitName} but they're already dead! aborting action.");
            return;
        }

        Console.WriteLine($"{UnitName} is rolling an hit check against {target.UnitName}");

        var minRollToHit = target.GetUnitDefenseRoll() / MinRollToHitTargetDefenseFactor;

        if (GetUnitHitChanceRoll() < minRollToHit)
        {
            Console.WriteLine($"{UnitName} missed the hit against {target.UnitName}!\n");
            return;
        }

        Console.WriteLine($"{UnitName} succeeded hit check and is attacking {target.UnitName}!\n");

        target.Defend(this);
    }

    // This method is used by the unit classes to be able to "go around" their archetype Attack overriden method, when needed  
    protected void UnitBasicUncheckedAttack(Unit target)
    {
        if (target == this)
        {
            Console.WriteLine($"{UnitName} tried to attack itself! aborting action.");
            return;
        }

        if (_isDead)
        {
            Console.WriteLine($"{UnitName} tried to attack but they're dead! aborting action.");
            return;
        }

        target.Defend(this);
    }

    protected virtual void Defend(Unit attacker)
    {
        if (_isDead)
        {
            Console.WriteLine($"{UnitName} tried to defend but they're dead! aborting action.");
            return;
        }

        var attackerDamage = attacker.GetUnitDamageRoll();

        // In case the delta between the attacker damage roll and the defender defense roll is too drastic - the attack is completely blocked
        if (attackerDamage <= GetUnitDefenseRoll() / MinRollToBlockDefenseFactor)
        {
            BlockAttack(attacker);
            return;
        }

        TakeDamage(attackerDamage);
    }

    protected virtual void BlockAttack(Unit attacker)
    {
        Console.WriteLine($"{UnitName} blocked {attacker.UnitName}'s attack!\n");
    }
    
    private int GetUnitHitChanceRoll() => HitChance.GetRandom(UnitName) + HitChanceRollModifier;

    private void ResetCurrentWeatherEffect(Weather newWeather = Weather.None)
    {
        if (_currentAffectingWeather != newWeather)
        {
            Console.WriteLine($"{_currentAffectingWeather} Weather effect on {UnitName} is over");
        }

        switch (_currentAffectingWeather)
        {
            case Weather.None:
                // Nothing to Undo
                break;
            case Weather.Sunny:
                HitChanceRollModifier -= SunnyWeatherHitChanceModifierEffect;
                break;
            case Weather.Rainy:
                DefenseRollModifier -= RainyWeatherDefenseModifierEffect;
                break;
            case Weather.Snowy:
                // Nothing to Undo
                break;
            case Weather.Gusty:
                DamageRollModifier -= GustyWeatherDamageModifierEffect;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _currentAffectingWeather = Weather.None;
    }

    private void TakeDamage(int damageToTake)
    {
        Hp = Math.Max(0, Hp - damageToTake);

        Console.WriteLine($"{UnitName} received {damageToTake} damage!\n" +
                          $"It has now {Hp} HP\n");

        if (Hp > 0) return;

        Die();
    }

    private void Die()
    {
        Console.WriteLine($"{UnitName} is dead!\n");
        _isDead = true;
    }
}