using System;
using System.Numerics;

class Primitive
{
    protected Vector3 color;

    public Vector3 Color { get { return color; } }

    public Primitive()
    {

        
    }
    public virtual ValueTuple<bool, float> Collision(Ray ray)
    {
        return (false, 0);
    }

    public virtual Vector3 OutsideNormal(Vector3 point)
    {
        return Vector3.Zero;
    }
}
