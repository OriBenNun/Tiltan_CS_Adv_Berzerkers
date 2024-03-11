using System;
using System.Collections.Generic;

namespace Enumerables
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var demoList = new List<int>() { 1, 2, 4, 6, 8 };
            
            var list = new SimpleList<int>(demoList);

            foreach (var elem in list)
            {
                Console.WriteLine(elem);
            }
        }
    }
}