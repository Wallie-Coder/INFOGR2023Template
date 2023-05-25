using System.Numerics;
using System;


class Intersection
{
    // member variables
    float distance;
    float t;
    Vector3 intersection;
    Vector3 normal;
    Ray ray;
    Primitive prim;

    public Vector3 IntersectionPoint { get { return intersection;} }

    public Intersection(Ray ray, Primitive prim, float t)
    {
        this.ray = ray;
        this.prim = prim;
        this.t = t;
        t -= 0.0001f; // make sure it doesnt intersect with itself -- only works if its this high? probably did something wrong
        CalculateIntersection(ray, t);
        SetNormal();
    }

    void CalculateIntersection(Ray r, float t)
    {
        intersection = r.Origin + t * r.Direction;
    }

    void SetNormal()
    {
        if(prim is Sphere)
        {
            Sphere sphere = (Sphere)prim;
            normal = intersection - sphere.Location;
            Vector3.Normalize(normal);
        }
    }
}
