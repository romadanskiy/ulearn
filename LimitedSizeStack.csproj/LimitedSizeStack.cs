using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication
{
    public class LimitedSizeStack<T>
    {
        private readonly int size;
        private readonly LinkedList<T> list = new LinkedList<T>();
        public LimitedSizeStack(int limit)
        {
            size = limit;
        }

        public void Push(T item)
        {
            if (list.Count == size)
            {
                list.RemoveFirst();
            }
            list.AddLast(item);
        }

        public T Pop()
        {
            if (list.Count == 0) throw new InvalidOperationException();
            var result = list.Last;
            list.RemoveLast();
            return result.Value;
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }
    }
}
