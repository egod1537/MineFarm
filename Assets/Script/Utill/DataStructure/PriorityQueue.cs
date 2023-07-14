using System;
using System.Collections.Generic;

public class PriorityQueue<T> : List<T> where T : IComparable
{
    public T Dequeue()
    {
        T ret = Peek();
        this.RemoveAt(Count - 1);
        return ret;
    }
    public void Enqueue(T element)
    {
        this.Add(element);
    }
    public T Peek()
    {
        return this[Count - 1];
    }
}
