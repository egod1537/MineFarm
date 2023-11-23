using UnityEngine;

public class IntRange
{
    public int l, r;
    public int value { get => Random.Range(l, r); }

    public IntRange(int value)
    {
        this.l = value;
        this.r = value;
    }
    public IntRange(int l, int r)
    {
        this.l = l;
        this.r = r;
    }
}
