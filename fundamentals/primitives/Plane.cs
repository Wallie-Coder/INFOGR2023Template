using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace RAYTRACER
{
    public class Plane : Primitive
    {
        Vector3 Normal;

        float Distance;

        Vector3 Point;

        public Vector3 GetNormal { get { return Normal; } }

        public Plane(Vector3 normal, Vector3 point, Vector3 diffuseColor, Vector3 glossyColor) :base(diffuseColor, glossyColor)
        {
            this.Normal = normal;
            this.Point = point;
        }

        public override float CollisionPlane(Ray ray)
        {
            float t = 0;
            Vector3 E = ray.Origin;
            Vector3 D = ray.Direction;

            //float zero = (xt - E.X) * Normal.X + (yt - E.Y) * Normal.Y + (zt - E.Z) * Normal.Z;
            //-t((D.X * Normal.X) + (D.Y * Normal.Y) + (D.Y * Normal.Z)) = (E.X - E.X) * Normal.X + (E.Y - E.Y) * Normal.Y + (E.Z - E.Z) * Normal.Z;

            float a = ((D.X * Normal.X) + (D.Y * Normal.Y) + (D.Z * Normal.Z));
            float b = (E.X - Point.X) * Normal.X + (E.Y - Point.Y) * Normal.Y + (E.Z - Point.Z) * Normal.Z;

            t = (-b) / a;

            if (t <= 0.0001)
            {
                return 0;
            }

            float Zero = ((E.X + (t * D.X) - Point.X) * Normal.X) + ((E.Y + (t * D.Y) - Point.Y) * Normal.Y) + ((E.Z + (t * D.Z) - Point.Z) * Normal.Z);

            double xt = E.X + t * D.X;
            double yt = E.Y + t * D.Y;
            double zt = E.Z + t * D.Z;

            if (Zero > -0.01 && Zero < 0.01)
            {
                return t;
            }

            return 0f;

        }
    }
}

