using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace RAYTRACER
{
    public class Plane : Primitive
    {
        // MEMBER VARIABLES
        private Vector3 normal;


        Vector3 point;

        public Vector3 GetNormal { get { return normal; } }

        public Plane(Vector3 normal, Vector3 point, Vector3 diffuseColor, Vector3 glossyColor, bool specular = false) :base(diffuseColor, glossyColor, specular)
        {
            this.normal = normal;
            this.point = point;
        }

        public float CollisionPlane(Ray ray)
        {
            float t = 0;
            Vector3 E = ray.Origin;
            Vector3 D = ray.Direction;

            //float zero = (xt - E.X) * Normal.X + (yt - E.Y) * Normal.Y + (zt - E.Z) * Normal.Z;
            //-t((D.X * Normal.X) + (D.Y * Normal.Y) + (D.Y * Normal.Z)) = (E.X - E.X) * Normal.X + (E.Y - E.Y) * Normal.Y + (E.Z - E.Z) * Normal.Z;

            float a = ((D.X * normal.X) + (D.Y * normal.Y) + (D.Z * normal.Z));
            float b = (E.X - point.X) * normal.X + (E.Y - point.Y) * normal.Y + (E.Z - point.Z) * normal.Z;

            t = (-b) / a;

            if (t <= 0.0001)
            {
                return 0;
            }

            float Zero = ((E.X + (t * D.X) - point.X) * normal.X) + ((E.Y + (t * D.Y) - point.Y) * normal.Y) + ((E.Z + (t * D.Z) - point.Z) * normal.Z);

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

