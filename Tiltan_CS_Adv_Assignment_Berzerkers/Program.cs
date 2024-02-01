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
            var s = new SoulBreaker(name: "Aba");
            var t = new Tank();
            var k = new Knight(name: "");
            var kk = new Knight(name: "TONY");

            Console.WriteLine($"{s}\n");
            Console.WriteLine($"{t}\n");
            Console.WriteLine($"{k}\n");
            Console.WriteLine($"{kk}\n");
            
            s.Attack(t);
            t.Attack(s);
            t.Attack(k);
            k.Attack(s);

            Console.WriteLine($"{s}\n");
            Console.WriteLine($"{t}\n");
            Console.WriteLine($"{k}\n");
            Console.WriteLine($"{kk}\n");
        }
    }
}