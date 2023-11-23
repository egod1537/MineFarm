using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
//Max Heap
public class PriorityQueue<T> : List<T> where T : IComparable
{
    public T Dequeue()
    {
        T ret = Peek();
        (this[0], this[Count - 1]) = (this[Count - 1], this[0]);
        this.RemoveAt(Count - 1);

        int pos = 0;
        while (pos < Count)
        {
            int l = 2 * pos + 1, r = 2 * pos + 2;
            if (l >= Count) break;
            if (r < Count && this[l].CompareTo(this[r]) < 0)
                l = r;
            if (this[pos].CompareTo(this[l]) < 0)
                (this[pos], this[l]) = (this[l], this[pos]);
            pos = l;
        }
        return ret;
    }
    public void Enqueue(T element)
    {
        this.Add(element);

        int pos = Count - 1;
        while(pos > 0)
        {
            int parent = (pos-1) / 2;
            if (this[parent].CompareTo(this[pos]) < 0)
                (this[parent], this[pos]) = (this[pos], this[parent]);
            else break;
            pos = parent;
        }
    }
    public T Peek()
    {
        return this[0];
    }
}
