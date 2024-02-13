using System;
using System.Collections.Generic;

public class Player
{
    public string Name { get; }
    public List<Unit> Units { get; }
    private uint Resources { get; set; }
    private Race Race { get; }

    public Player(string name, List<Unit> units, uint startingResources, Race race)
    {
        Name = name;
        Units = units;
        Resources = startingResources;
        Race = race;
    }

    public uint StealResources(uint resourcesToSteal, string stealerName)
    {
        if (Resources == 0)
        {
            Console.WriteLine($"{stealerName} tried to steal from {Name}, but they has no resources left!");
            return 0;
        }

        var actualStolenResources = resourcesToSteal > Resources ? Resources : resourcesToSteal;
        
        Resources -= actualStolenResources;

        Console.WriteLine($"{actualStolenResources} resource[s] were stolen from {Name} by {stealerName}!\n");
        
        return actualStolenResources;
    }
    
    public void GainResources(uint resourcesToGain)
    {
        Resources += resourcesToGain;

        Console.WriteLine($"{Name} gained {resourcesToGain} resource[s]\n");
    }

    public override string ToString()
    {
        return $"Name: {Name}\n" +
               $"Race: {Race}\n" +
               $"Resources: {Resources}\n" +
               $"Number of Units Alive: {Unit.GetAliveUnitsCount(Units)}";
    }
}