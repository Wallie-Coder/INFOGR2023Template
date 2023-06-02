using System.Numerics;

namespace RAYTRACER
{
    public class Intersection
    {
        // MEMBER VARIABLES
        private float t;
        public float GetT { get { return t; } }


        private Vector3 intersection, normal;
        public Vector3 Normal { get { return normal; } }
        public Vector3 IntersectionPoint { get { return intersection; } }


        private Ray ray;
        private Primitive primitive;

        public Primitive GetPrimitive { get { return primitive; } }

        
        // CONSTRUCTOR
        public Intersection(Ray ray, Primitive primitive, float t)
        {
            this.ray = ray;
            this.primitive = primitive;
            this.t = t;
            CalculateIntersection();
            SetNormal();
        }

        // CLASS METHODS

        // calculates the point of intersection
        void CalculateIntersection()
        {
            intersection = ray.Origin + t * ray.Direction;
        }

        // sets the normal of an intersection
        void SetNormal()
        {
            if (primitive is Sphere sphere)
            {
                normal = sphere.OutsideNormal(intersection);
            }
            else if (primitive is Plane plane)
            {
                normal = plane.OutsideNormal(intersection);
            }
        }
    }
}