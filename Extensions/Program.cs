using System;

namespace Extensions
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var b = true;
            Console.WriteLine(b.BoolAsInt());

            var str = "This    is  4    words";
            Console.WriteLine(str.WordsInString());

            var num = 15;
            num.Factorial();
            Console.WriteLine(num);
            num.Factorial();
            Console.WriteLine(num);
        }
    }

    public static class ExtensionMethods
    {

        public static void Factorial(this ref int num)
        {
            var curr = 1;
            for (var i = 2; i <= num; i++)
            {
                curr *= i;
            }

            num = curr;
        }
        public static int BoolAsInt(this bool b)
        {
            return b ? 1 : 0;
        }

        public static int WordsInString(this string str)
        {
            var result = 1;
            var spaceChar = ' ';
            var lastWasSpace = false;

            foreach (var c in str)
            {
                if (c == spaceChar && !lastWasSpace)
                {
                    lastWasSpace = true;
                    result++;
                    continue;
                }

                if (c != spaceChar)
                {
                    lastWasSpace = false;
                }
                
            }

            return result;
        }
    }
}