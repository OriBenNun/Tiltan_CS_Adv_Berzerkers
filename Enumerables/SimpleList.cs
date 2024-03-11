using System.Collections;
using System.Collections.Generic;

namespace Enumerables
{
    public class SimpleList<T> : IEnumerable<T>
    {
        private readonly List<T> _list;

        public SimpleList(List<T> list)
        {
            _list = list;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SimpleListEnumerator<T>(_list.ToArray());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public struct SimpleListEnumerator<T> : IEnumerator<T>
    {
        private readonly T[] _arr;
        private int _index;
        
        public SimpleListEnumerator(T[] array)
        {
            _index = 0;
            _arr = array;
        }

        public bool MoveNext()
        {
            _index++;
            return IsIndexValid();
        }

        public void Reset()
        {
            throw new System.NotSupportedException();
        }
        
        public T Current => IsIndexValid() ? _arr[_index] : default;

        private bool IsIndexValid()
        {
            return _index >= 0 && _index < _arr.Length;
        }

        object IEnumerator.Current => Current;

        public void Dispose() { }
    }
}