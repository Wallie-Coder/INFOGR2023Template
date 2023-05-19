using OpenTK.Mathematics;
using System;
using System.Reflection.Metadata.Ecma335;

class Intersection
{
    // member variables
    float distance;
    Vector3 intersection;
    Vector3 normal;
    Ray ray;
    Primitive prim;

    public Intersection(Ray ray, Primitive prim, Vector3 intersection)
    {
        this.ray = ray;
        this.prim = prim;
        this.intersection = intersection;
    }

    void SetNormal()
    {
        if(prim is Sphere)
        {
            Sphere sphere = (Sphere)prim;
            normal = intersection - sphere.Location;
        }
    }
}
