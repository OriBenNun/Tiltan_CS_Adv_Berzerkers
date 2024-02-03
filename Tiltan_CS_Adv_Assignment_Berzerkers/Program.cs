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
            // DiceTest();

            UnitFightTest();
        }

        private static void DiceTest()
        {
            DiceEqualityTest();
            
            // SingleDieTest();
        }

        private static void DiceEqualityTest()
        {
            var d1 = new Dice(2, 8, 1);
            var d2 = new Dice(2, 8, 1);
            var d3 = new Dice(4, 6, 4);

            Console.WriteLine(d1.Equals(d2));
            Console.WriteLine(d1.Equals(d3));
            Console.WriteLine(d2.GetHashCode() == d3.GetHashCode());

            Console.WriteLine($"D1 rolled: {d1.Roll()}");
            Console.WriteLine($"D2 rolled: {d2.Roll()}");
            Console.WriteLine($"D3 rolled: {d3.Roll()}");
            
        }

        private static void SingleDieTest()
        {
            var d1 = new Dice(1, 6, 0);
            var ones = 0;
            var twos = 0;
            var threes = 0;
            var fours = 0;
            var fives = 0;
            var sixes = 0;

            for (int i = 0; i < 100_000; i++)
            {
                var roll = d1.Roll();

                switch (roll)
                {
                    case 1:
                        ones++;
                        break;
                    case 2:
                        twos++;
                        break;
                    case 3:
                        threes++;
                        break;
                    case 4:
                        fours++;
                        break;
                    case 5:
                        fives++;
                        break;
                    case 6:
                        sixes++;
                        break;
                }
            }

            Console.WriteLine(ones);
            Console.WriteLine(twos);
            Console.WriteLine(threes);
            Console.WriteLine(fours);
            Console.WriteLine(fives);
            Console.WriteLine(sixes);
        }

        private static void UnitFightTest()
        {
            var s = new Tank(name: "Aba");
            var p = new Guardian(name: "Gobo");
            var pp = new Barbarian(name: "Loooko");

            Console.WriteLine(s);
            Console.WriteLine(p);
            Console.WriteLine(pp);

            s.Fight(p);
            // p.Fight(s);
            // s.Fight(p);
            // p.Fight(s);
            // s.Fight(pp);
            // s.Fight(pp);
            // s.Fight(pp);
            // s.Fight(pp);
            // pp.Fight(s);
            // pp.Fight(s);
            // pp.Fight(s);
            // pp.Fight(s);
            // pp.Fight(s);
            // p.Fight(s);
            // s.Fight(p);
            // s.Fight(p);
            // s.Fight(p);
            // s.Fight(p);
            // s.Fight(pp);
            // s.Fight(pp);
            // s.Fight(pp);
            // s.Fight(p);
            // p.Fight(s);
            // p.Fight(s);
            // p.Fight(s);
            // p.Fight(s);
            // p.Fight(s);
            // s.Fight(p);
            // pp.Fight(s);
            // pp.Fight(s);
            // pp.Fight(s);
            // s.Fight(pp);
            // s.Fight(pp);
            // s.Fight(pp);
            // pp.Fight(s);
            // pp.Fight(s);
            // p.Fight(s);
            // s.Fight(p);
            // p.Fight(s);
            // s.Fight(p);
            // p.Fight(s);
            // s.Fight(p);
            // s.Fight(p);
            // pp.Fight(s);
            // pp.Fight(s);
            // pp.Fight(s);
            // s.Fight(pp);
            // s.Fight(pp);
            // s.Fight(pp);
            // pp.Fight(s);
            // pp.Fight(s);
            // s.Fight(p);
            // s.Fight(p);
            // s.Fight(p);
            // p.Fight(s);
            // p.Fight(s);
            // s.Fight(pp);
            // s.Fight(pp);
            // s.Fight(pp);
            // p.Fight(s);
            // p.Fight(s);
            // p.Fight(s);
            // pp.Fight(s);
            // pp.Fight(s);
            // pp.Fight(s);
            // pp.Fight(s);
            // pp.Fight(s);

            Console.WriteLine(s);
            Console.WriteLine(p);
            Console.WriteLine(pp);
        }
    }
}