using OpenTK.Mathematics;
using System;
using System.Reflection.Metadata.Ecma335;

class Intersection
{
    // distance to intersection
    float distance;
    // point of intersection
    Vector3 intersection;
    // the normal of the primitive at the point of intersection
    Vector3 normal;
    // the ray that intersected with a primitive
    Ray ray;
    // the primitive the ray intersected with
    Primitive prim;

    public Intersection(Ray ray, Primitive prim, Vector3 intersection)
    {
        this.ray = ray;
        this.prim = prim;
        this.intersection = intersection;
    }

    void Normal()
    {
        if(prim is Sphere)
        {
            Sphere sphere = prim as Sphere;
            normal = intersection - sphere.Location;
        }
    }
}
