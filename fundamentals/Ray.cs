using OpenTK.Mathematics;
using System;

class Ray
{
    // direction vector of a ray
    Vector3 direction;
    // the calculated color of the pixel the ray was shot from
    Vector3 color;
    // the amount of bounces the ray has done
    int bounces;
    // the amount of bounces after which we stop tracing the ray
    int maxBounces = 10;
}
