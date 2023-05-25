using System.Numerics;

class Plane : Primitive
{
    Vector3 Normal;

    float Distance;

    public Plane(Vector3 normal, float distance)
    {
        this.Normal = normal;
        this.Distance = distance;
    }

    public override ValueTuple<bool,float> Collision(Ray ray)
    {
        return (false, 0);
    }
}

