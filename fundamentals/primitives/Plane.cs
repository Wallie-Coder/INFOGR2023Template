using System.Numerics;

namespace RAYTRACER
{
    public class Plane : Primitive
    {
        Vector3 Normal;

        float Distance;

        public Plane(Vector3 normal, float distance)
        {
            this.Normal = normal;
            this.Distance = distance;
        }

        public override ValueTuple<double, float, float> Collision(Ray ray)
        {
            return (0, 0, 0);
        }
    }
}

