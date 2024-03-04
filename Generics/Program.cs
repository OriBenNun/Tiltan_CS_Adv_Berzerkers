using System;

namespace Generics
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var l = new List<int>();
            l.Store(1);
            l.Store(2);
            l.Store(3);

            Console.WriteLine(l.Retrieve(-1));
            Console.WriteLine(l.Retrieve(0));
            Console.WriteLine(l.Retrieve(1));
            Console.WriteLine(l.Retrieve(2));
            Console.WriteLine(l.Retrieve(3));

            Console.WriteLine(l.Unique(0));
            Console.WriteLine(l.Unique(1));
            Console.WriteLine(l.Unique(4));
            
            l.Store(1);
            l.Store(1);
            l.Store(1);
            l.Store(1);
            l.Store(1);
            l.Store(1);
            l.Store(1);
            l.Store(1);
            l.Store(1);
            l.Store(1);
            l.Store(1);
            
            Console.WriteLine(l.Unique(1));
            Console.WriteLine(l.Retrieve(9));
            Console.WriteLine(l.Retrieve(15));
        }
    }

    internal class List<T> : IList<T>
    {

        private readonly T[] _array = new T[10];
        private int _nextFreeIndex;
        
        public void Store(T value)
        {
            if (_nextFreeIndex == _array.Length)
            {
                Console.WriteLine($"List is full! it has only {_array.Length} slots. Aborting.");
                return;
            }

            _array[_nextFreeIndex++] = value;
        }

        public T Retrieve(int index)
        {
            if (index >= _array.Length || index < 0)
            {
                Console.WriteLine($"Index is out of range! the list has only {_array.Length} elements. Aborting.");
                return default;
            }
            
            return _array[index];
        }

        public bool Unique(T value)
        {
            var unique = true;

            foreach (var elem in _array)
            {
                if (!value.Equals(elem)) continue;
                unique = false;
                break;
            }

            return unique;
        }
    }

    interface IList<T>
    {
        void Store(T value);
        T Retrieve(int index);
        bool Unique(T value);
    }
}