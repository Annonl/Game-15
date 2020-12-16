using System;
using System.Collections.Generic;
using System.Linq;

namespace Игра_в_15
{
    class PriorityQueue<TItem, TPriority> where TPriority : IComparable
    {
        private SortedList<TPriority, Queue<TItem>> pq = new SortedList<TPriority, Queue<TItem>>();
        public int Count { get; private set; }

        public void Enqueue(TItem item, TPriority priority)
        {
            ++Count;
            if (!pq.ContainsKey(priority)) pq[priority] = new Queue<TItem>();
            pq[priority].Enqueue(item);
        }

        public TItem Dequeue()
        {
            --Count;
            var queue = pq.ElementAt(0).Value;
            if (queue.Count == 1) pq.RemoveAt(0);
            return queue.Dequeue();
        }
        public bool Empty()
        {
            return Count == 0;
        }
        public void Clear()
        {
            Count = 0;
            pq.Clear();
        }
    }
}
