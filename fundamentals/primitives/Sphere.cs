using System.Numerics;

namespace RAYTRACER
{
    public class Sphere : Primitive
    {
        // MEMBER VARIABLES
        private Vector3 center;
        public Vector3 Center { get { return center; } }


        private float radius;
        public float Radius { get { return radius; } }


        // CONSTRUCTOR
        public Sphere(Vector3 center, float radius, Vector3 diffuseColor, Vector3 glossyColor, bool specular = false, Textures texture = Textures.None) :base(diffuseColor, glossyColor, specular, texture)
        {
            this.center = center;
            this.radius = radius;
        }

        // CLASS METHODS

        // detects collision between a given ray and the sphere if there is a collision and where along the ray it is
        public ValueTuple<double, float, float> CollisionSphere(Ray ray)
        {
            Vector3 CenterOrigin = ray.Origin - center;
            double D;
            float p1 = 0, p2 = 0, a, b, c;
            a = Vector3.Dot(ray.Direction, ray.Direction);
            b = 2.0f * Vector3.Dot(CenterOrigin, ray.Direction);
            c = Vector3.Dot(CenterOrigin, CenterOrigin) - (radius * radius);
            D = b * b - 4 * a * c;
            if (D >= 0)
            {
                p2 = (-b - (float)Math.Sqrt(D)) / (2 * a);
                p1 = (-b + (float)Math.Sqrt(D)) / (2 * a);
            }
            return (D, p1, p2);
        }

        // returns the normal of a point on the sphere that is pointing outwards
        public override Vector3 OutsideNormal(Vector3 point)
        {
            return -Vector3.Normalize(center - point);
        }

        public override Vector3 GetDiffuseColor(Vector3 input)
        {
            if (texture == Textures.Checkerboard)
            {
                return CheckerboardSphere(input, center, radius);
            }

            return diffuseColor;
        }

        public override Vector3 GetSpecularColor(Vector3 input)
        {
            if (texture == Textures.Checkerboard)
            {
                return CheckerboardSphere(input, center, radius);
            }

            return specularColor;
        }

    }
}
