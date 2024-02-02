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
            var s = new Tank(name: "Aba");
            var p = new Guardian(name: "Gobo");
            var pp = new Guardian(name: "Loooko");
            
            Console.WriteLine(s);
            Console.WriteLine(p);
            Console.WriteLine(pp);
            
            s.Attack(p);
            p.Attack(s);
            s.Attack(p);
            p.Attack(s);
            s.Attack(pp);
            s.Attack(pp);
            s.Attack(pp);
            s.Attack(pp);
            pp.Attack(s);
            pp.Attack(s);
            pp.Attack(s);
            pp.Attack(s);
            pp.Attack(s);
            p.Attack(s);
            s.Attack(p);
            s.Attack(p);
            s.Attack(p);
            s.Attack(p);
            s.Attack(pp);
            s.Attack(pp);
            s.Attack(pp);
            s.Attack(p);
            p.Attack(s);
            p.Attack(s);
            p.Attack(s);
            p.Attack(s);
            p.Attack(s);
            s.Attack(p);
            pp.Attack(s);
            pp.Attack(s);
            pp.Attack(s);
            s.Attack(pp);
            s.Attack(pp);
            s.Attack(pp);
            pp.Attack(s);
            pp.Attack(s);
            p.Attack(s);
            s.Attack(p);
            p.Attack(s);
            s.Attack(p);
            p.Attack(s);
            s.Attack(p);
            s.Attack(p);
            pp.Attack(s);
            pp.Attack(s);
            pp.Attack(s);
            s.Attack(pp);
            s.Attack(pp);
            s.Attack(pp);
            pp.Attack(s);
            pp.Attack(s);
            s.Attack(p);
            s.Attack(p);
            s.Attack(p);
            p.Attack(s);
            p.Attack(s);
            s.Attack(pp);
            s.Attack(pp);
            s.Attack(pp);
            p.Attack(s);
            p.Attack(s);
            p.Attack(s);
            pp.Attack(s);
            pp.Attack(s);
            pp.Attack(s);
            pp.Attack(s);
            pp.Attack(s);
            
            Console.WriteLine(s);
            Console.WriteLine(p);
            Console.WriteLine(pp);
        }
    }
}