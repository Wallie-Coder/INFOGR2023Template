using OpenTK.Mathematics;
using System;
using System.Reflection.Metadata.Ecma335;

class Intersection
{
    // distance to intersection
    float distance;
    // point of intersection
    Vector3 location;
    // the normal of the primitive at the point of intersection
    Vector3 normal;
    // the ray that intersected with a primitive
    Ray ray;
    // the primitive the ray intersected with
    Primitive prim;

    public Intersection(Ray ray, Primitive prim)
    {
        this.ray = ray;
        this.prim = prim;
    }

    Vector3 Normal()
    {
        if(prim.GetType() == Sphere)
        {

        }
    }
}
