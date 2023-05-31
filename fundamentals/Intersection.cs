using System.Numerics;
using System;

namespace RAYTRACER
{
    public class Intersection
    {
        // MEMBER VARIABLES
        private float distance;
        private float t;


        private Vector3 intersection, normal;
        public Vector3 Normal { get { return normal; } }
        public Vector3 IntersectionPoint { get { return intersection; } }


        private Ray ray;
        private Primitive prim;

        public Primitive GetPrimitive { get { return prim; } }

        public float GetT { get { return t; } }

        
        // CONSTRUCTOR
        public Intersection(Ray ray, Primitive prim, float t)
        {
            this.ray = ray;
            this.prim = prim;
            this.t = t;
            CalculateIntersection(ray, t);
            SetNormal();
            CalculateDistance();
        }

        // CLASS METHODS

        // calculates the point of intersection
        void CalculateIntersection(Ray r, float t)
        {
            intersection = r.Origin + t * r.Direction;
        }

        // sets the normal of an intersection
        void SetNormal()
        {
            if (prim is Sphere sphere)
            {
                normal = -sphere.OutsideNormal(intersection);
            }
        }

        // calculates the distance between the origin of the ray and the intersection
        void CalculateDistance()
        {
            distance = Vector3.Distance(intersection,ray.Origin);
        }
    }
}