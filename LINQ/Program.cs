using System;
using System.Collections;
using System.Linq;

namespace LINQ
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // DistinctAscendingOrder();

            var s1 = "four four four four four"; // 4
            var s2 = "two two two two two"; // 3
            var s3 = "two four two four"; // 14/4 = 3.5

            // s1.Select()
            // Console.WriteLine(res);
        }

        private static void DistinctAscendingOrder()
        {
            var enum1 = new string[] {"Dog", "Cat", "Dog", "Bird", "Cat", "Ape", "Koko Bongo"};
            foreach (var s in enum1)
            {
                Console.WriteLine(s);
            }
            
            Console.WriteLine();
            Console.WriteLine();

            var orderedEnumerable = enum1.Distinct().OrderBy(s => s);
            foreach (var s in orderedEnumerable)
            {
                Console.WriteLine(s);
            }
        }
    }
}