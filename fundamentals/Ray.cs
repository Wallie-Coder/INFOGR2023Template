using OpenTK.Mathematics;
using System;

class Ray
{
    // member variables
    Vector3 origin;
    // direction vector of a ray
    Vector3 direction;
    // the calculated color of the pixel the ray was shot from
    Vector3 color;
    // the amount of bounces the ray has done
    int bounces;
    // the amount of bounces after which we stop tracing the ray
    int maxBounces = 10;

    public Vector3 Origin { get { return origin; } set { origin = value; } }

    public Vector3 Direction { get { return direction; } set { direction = value; } }

    public Vector3 Color { get { return color; } set { color = value; } }

    public Ray(Vector3 direction, Vector3 origin)
    {
        this.direction = direction;
        this.direction.Normalize();
        color = new Vector3(0, 0, 0);
        this.origin = origin;
    }
}
