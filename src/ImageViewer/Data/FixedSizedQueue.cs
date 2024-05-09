using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Data
{
    public class FixedSizedQueue<T>
    {
        private List<T> _values = new List<T>();
        public int Size { get; set; }

        public void Enqueue(T obj)
        {
            _values.Add(obj);
            if (_values.Count > Size)
            {
                _values.RemoveAt(0);
            }
        }

        public int Count()
        {
            return _values.Count;
        }

        public T Dequeue()
        {
            T obj;
            if (_values.Count <= 0)
                return default;

            obj = _values.Last();
            _values.Remove(obj);
            return obj;
        }
    }
}
