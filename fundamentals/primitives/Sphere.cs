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


        public Sphere(Vector3 center, float radius, Vector3 color)
        {
            this.center = center;
            this.radius = radius;
            this.color = color;

        }
        public override ValueTuple<double, float, float> Collision(Ray ray)
        {
            Vector3 CenterOrigin = ray.Origin - center;
            float a = Vector3.Dot(ray.Direction, ray.Direction);
            float b = 2.0f * Vector3.Dot(CenterOrigin, ray.Direction);
            float c = Vector3.Dot(CenterOrigin, CenterOrigin) - (radius * radius);
            double D = b * b - 4 * a * c;
            float p1 = 0, p2 = 0;
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
