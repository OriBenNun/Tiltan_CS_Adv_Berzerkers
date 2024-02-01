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
            var g = new Giant();
            var r = new Rebel();

            Console.WriteLine(g);
            Console.WriteLine(r);
            
            g.Attack(r);
            r.Attack(g);
            
            Console.WriteLine(g);
            Console.WriteLine(r);
        }
    }
}