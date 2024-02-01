using System;

namespace Tiltan_CS_Adv_Assignment_Berzerkers
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Test();
        }

        private static void Test()
        {
            var s = new Barbarian(name: "Aba");
            var p = new Paladin(name: "Bobo");
            
            Console.WriteLine(s);
            Console.WriteLine(p);
            
            s.Attack(p);
            p.Attack(s);
            s.Attack(p);
            p.Attack(s);
            s.Attack(p);
            p.Attack(s);
            s.Attack(p);
            s.Attack(p);
            s.Attack(p);
            s.Attack(p);
            s.Attack(p);
            p.Attack(s);
            p.Attack(s);
            p.Attack(s);
            p.Attack(s);
            p.Attack(s);
            
            Console.WriteLine(s);
            Console.WriteLine(p);
        }
    }
}