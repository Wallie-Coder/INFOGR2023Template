using System.Numerics;
using System;

class Ray
{
    // member variables
    Vector3 origin;
    Vector3 direction;
    Vector3 color;
    int bounces;
    int maxBounces = 10;

    public Vector3 Origin { get { return origin; } set { origin = value; } }

    public Vector3 Direction { get { return direction; } set { direction = value; } }

    public Vector3 Color { get { return color; } set { color = value; } }

    public Ray(Vector3 direction, Vector3 origin)
    {
        this.direction = direction;
        Vector3.Normalize(this.direction);
        color = new Vector3(0, 0, 0);
        this.origin = origin;
    }
}
