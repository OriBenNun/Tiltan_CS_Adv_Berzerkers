﻿// ---- C# II (Dor Ben Dor) ----
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
    private const int SunWeatherHitChanceModifierFactor = 4;
    
    public string UnitName { get; protected set; } = "Unit";
    public int Capacity { get; }
    public int Hp { get; private set; }
    protected string ClassName { get; }
    private Dice DamageDice { get; set; }
    private Dice DefenseDice { get; set; }
    private Dice HitChanceDice { get; set; }
    private Race Race { get; }

    private Weather _currentAffectingWeather = Weather.None;
    
    private bool _isDead;

    private Dice _weatherCacheDice;

    protected Unit(Race race, string className, Dice damageDice, Dice defenseDice, Dice hitChanceDice, int hp, int capacity)
    {
        Race = race;
        ClassName = className;
        DamageDice = damageDice;
        DefenseDice = defenseDice;
        HitChanceDice = hitChanceDice;
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
               $"Defense: {DefenseDice}\n" +
               $"Damage: {DamageDice}\n" +
               $"Hit Chance: {HitChanceDice}\n" +
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
                // Unit is temporarily gained SunWeatherHitChanceModifierFactor to its hit chance dice modifier
                UpdateHitChanceDiceModifier(SunWeatherHitChanceModifierFactor, true);
                weatherEffectInfo = $"TEMPORARILY receives +{SunWeatherHitChanceModifierFactor} to their hit chance modifier";
                break;
            case Weather.Rainy:
                // Unit is temporarily losing its defense dice modifier + the scalar drops to 1
                _weatherCacheDice = DefenseDice;
                DefenseDice = new Dice(1, DefenseDice.BaseDie, 0);
                weatherEffectInfo = "TEMPORARILY loses their defense dice modifier and its scalar also drops to 1";
                break;
            case Weather.Snowy:
                // Unit is losing hp equals to half of their capacity (the bigger the unit is the more damage it takes)
                TakeDamage(Capacity / 2);
                weatherEffectInfo = "Taking damage for an amount equals to third of their capacity";
                break;
            case Weather.Gusty:
                // Unit is temporarily gaining +1 to its damage dice scalar
                _weatherCacheDice = DamageDice;
                DamageDice = new Dice(DamageDice.Scalar + 1, DamageDice.BaseDie, DamageDice.Modifier);
                weatherEffectInfo = "TEMPORARILY gaining +1 to its damage dice scalar";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(weather), weather, null);
        }
        Console.WriteLine($"{_currentAffectingWeather} Weather effect on {UnitName} is {didEffectChangedText} active ({weatherEffectInfo})");
    }
    
    public int GetUnitDamageRoll() => DamageDice.Roll(UnitName);
    public int GetUnitDefenseRoll() => DefenseDice.Roll(UnitName);
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
            Console.WriteLine($"{UnitName} tried to attack {target.UnitName} but they're already dead! aborting action.");
            return;
        }
        
        Console.WriteLine($"{UnitName} is rolling an hit check against {target.UnitName}");

        var minRollToHit = target.GetUnitDefenseRoll() / MinRollToHitTargetDefenseFactor;

        if (HitChanceDice.Roll(UnitName) < minRollToHit)
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
    
    protected void UpdateDamageDiceModifier(int damageModifier, bool isAdditive = false)
    {
        var originalDamage = DamageDice;
        var newModifier = isAdditive ? originalDamage.Modifier + damageModifier : damageModifier;
        
        DamageDice = new Dice(originalDamage.Scalar, originalDamage.BaseDie, newModifier);
    }

    protected void UpdateDefenseDiceModifier(int defenseModifier, bool isAdditive = false)
    {
        var originalDefense = DefenseDice;
        var newModifier = isAdditive ? originalDefense.Modifier + defenseModifier : defenseModifier;
        
        DefenseDice = new Dice(originalDefense.Scalar, originalDefense.BaseDie, newModifier);
    }
    
    private void UpdateHitChanceDiceModifier(int hitChanceModifier, bool isAdditive = false)
    {
        var originalHitChance = HitChanceDice;
        var newModifier = isAdditive ? originalHitChance.Modifier + hitChanceModifier : hitChanceModifier;
        
        HitChanceDice = new Dice(originalHitChance.Scalar, originalHitChance.BaseDie, newModifier);
    }

    protected int GetDamageDiceModifier() => DamageDice.Modifier;

    private void ResetCurrentWeatherEffect(Weather newWeather = Weather.None)
    {
        if (_currentAffectingWeather != newWeather)
        {
            Console.WriteLine($"{_currentAffectingWeather} Weather effect on {UnitName} is over");
        }
        switch(_currentAffectingWeather)
        {
            case Weather.None:
                // Nothing to Undo
                break;
            case Weather.Sunny:
                UpdateHitChanceDiceModifier(-SunWeatherHitChanceModifierFactor, true);
                break;
            case Weather.Rainy:
                DefenseDice = _weatherCacheDice;
                // DefenseDice = new Dice(_weatherCacheDice.Scalar, _weatherCacheDice.BaseDie, _weatherCacheDice.Modifier);
                break;
            case Weather.Snowy:
                // Nothing to Undo
                break;
            case Weather.Gusty:
                DamageDice = _weatherCacheDice;
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

public enum Race
{
    Human,
    Gnome,
    Elf
}

public enum Weather
{
    None,
    Sunny,
    Rainy,
    Snowy,
    Gusty
}

public readonly struct Dice : IEquatable<Dice>
{
    public uint Scalar { get; }
    public uint BaseDie { get; }
    public int Modifier { get; }

    public Dice(uint scalar, uint baseDie, int modifier)
    {
        Scalar = scalar;
        BaseDie = baseDie;
        Modifier = modifier;
    }

    public int Roll(string unitName = "[Unknown Unit]")
    {
        var result = 0;
        
        for (var i = 0; i < Scalar; i++)
        {
            var rollResult = RandomChanceChecker.GetRandomInteger((int)BaseDie + 1, 1);
            result += rollResult;
        }
        
        Console.WriteLine($"[{unitName}] rolled: {result + Modifier}");
        return result + Modifier;
    }

    public bool Equals(Dice other)
    {
        return other.Scalar == Scalar &&
               other.BaseDie == BaseDie &&
               other.Modifier == Modifier;
    }

    // I got some help from ChatGPT to better understand
    // the bit manipulation principles and operators, and how to use them correctly to end up with a deterministic func
    public override int GetHashCode()
    {
        var hash = 17; // We start with a prime number

        // Using bit-shifting and XOR (^) to combine hash codes and improve distribution.
        // We use the values' hash codes to avoid operating on 0 or negatives, which can collapse distribution.
        hash = (hash << 7) ^ Modifier.GetHashCode();
        hash = (hash << 7) ^ BaseDie.GetHashCode();
        hash = (hash << 7) ^ Scalar.GetHashCode();

        return hash;
    }

    public override string ToString()
    {
        var suffix = Modifier.ToString();
        switch (Modifier)
        {
            case > 0:
                suffix = $"+{Modifier}";
                break;
            case 0:
                suffix = "";
                break;
        }

        return $"{Scalar}d{BaseDie} {suffix}";
    }
}

public static class RandomChanceChecker
{
    private static readonly Random Random;

    static RandomChanceChecker()
    {
        Random = new Random();
    }

    public static bool DidChanceSucceed(int chancePercents)
    {
        var random = Random.Next(100);

        return random < chancePercents;
    }

    public static int GetRandomInteger(int maxValueExclusive, int minValueInclusive = 0)
    {
        return Random.Next(minValueInclusive, maxValueExclusive);
    }
}