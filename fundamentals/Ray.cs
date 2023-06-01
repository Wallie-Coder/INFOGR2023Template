using System.Numerics;
using System;

namespace RAYTRACER
{
    public class Ray
    {
        // MEMBER VARIABLES
        protected Vector3 origin, direction, color;
        public Vector3 Origin { get { return origin; } set { origin = value; } }
        public Vector3 Direction { get { return direction; } set { direction = value; } }
        public Vector3 Color { get { return color; } set { color = value; } }

        protected int recursionDepth = 10;

        protected float epsilon = 0.0001f;


        // CONSTRUCTOR
        public Ray(Vector3 direction, Vector3 origin)
        {
            this.direction = direction;
            this.direction = Vector3.Normalize(this.direction);
            color = new Vector3(0, 0, 0);
            this.origin = origin;
        }

        // CLASS METHODS

        // decides wether a collision with a sphere is valid or not.
        public ValueTuple<bool, float> ConcludeFromCollision(double D, float p1, float p2)
        {
            // check if there is a possible collision, and the resulting t
            if (p1 < epsilon && p2 < epsilon) D = -1;
            return (D > 0, p1 < p2 ? p1 : p2);
        }
    }

    public class ShadowRay : Ray
    {
        // MEMBER VARIABLES
        Light light;
        public Light LightSource { get { return light; } }


        // CONSTRUCTOR
        public ShadowRay(Vector3 direction, Vector3 origin, Light light) : base(direction, origin)
        {
            this.light = light;
        }
    
        // CLASS METHODS

        // decides wether a collision with a sphere is valid or not
        public new bool ConcludeFromCollision(double D, float p1, float p2)
        {
            float t = Vector3.Distance(origin, light.Location);
            
            // make sure p1 and p2 are actually between the point on the sphere and the light source
            if (p1 > epsilon && p1 < t - epsilon)
            {
                return true;
            }
            else if (p2 > epsilon && p2 < t - epsilon)
            {
                return true;
            }
            return false;
        }
    }
}