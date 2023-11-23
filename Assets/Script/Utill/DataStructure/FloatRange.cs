using UnityEngine;
public class FloatRange
{
    public float l, r;
    public float value { get => Random.Range(l, r); }

    public FloatRange(float value)
    {
        this.l = value;
        this.r = value;
    }
    public FloatRange(float l, float r)
    {
        this.l = l;
        this.r = r;
    }
}