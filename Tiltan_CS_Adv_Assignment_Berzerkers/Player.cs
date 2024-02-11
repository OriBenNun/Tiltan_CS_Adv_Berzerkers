using System;
using System.Collections.Generic;

public class Player
{
    public string Name { get; }
    public List<Unit> Units { get; }
    public uint Resources { get; private set; }
    private Race Race { get; }

    public Player(string name, List<Unit> units, uint startingResources, Race race)
    {
        Name = name;
        Units = units;
        Resources = startingResources;
        Race = race;
    }

    public uint StealResources(uint resourcesToSteal)
    {
        if (Resources == 0)
        {
            Console.WriteLine($"Someone tried to steal from {Name}, but they has no resources left!");
            return 0;
        }
        
        var stolenResources = Math.Max(0, Resources - resourcesToSteal);
        Resources -= resourcesToSteal;
        
        Console.WriteLine($"{stolenResources} resources were stolen from {Name}!\n" +
                          $"They have {Resources} left");
        return stolenResources;
    }

    public override string ToString()
    {
        return $"Name: {Name}\n" +
               $"Race: {Race}\n" +
               $"Resources: {Resources}\n" +
               $"Number of Units: {Units.Count}";
    }
}