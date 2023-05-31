using System.Numerics;

namespace RAYTRACER
{
    public class Sphere : Primitive
    {
        Vector3 center;
        float radius;

        public Vector3 Location
        {
            get { return center; }
            private set { center = value; }
        }

        public float Radius { get { return radius; } }
        public Vector3 Center { get { return center; } }


        public Sphere(Vector3 center, float radius, Vector3 diffuseColor, Vector3 glossyColor) :base(diffuseColor, glossyColor)
        {
            this.center = center;
            this.radius = radius;
        }
        public override ValueTuple<double, float, float> CollisionSphere(Ray ray)
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

        public override Vector3 OutsideNormal(Vector3 point)
        {
            return Vector3.Normalize(center - point);
        }
    }
}
