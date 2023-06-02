using System.Numerics;

namespace RAYTRACER
{
    public class Plane : Primitive
    {
        // MEMBER VARIABLES
        private Vector3 normal;
        private Vector3 u;
        private Vector3 v;
        public Vector3 GetNormal { get { return normal; } }

        private Object func;
        // a given point on the plane
        Vector3 point;
        private bool checkerboard;

        // CONSTRUCTOR
        public Plane(Vector3 u, Vector3 v, Vector3 point, Vector3 diffuseColor, Vector3 glossyColor, bool specular = false, Textures texture = Textures.None) :base(diffuseColor, glossyColor, specular, texture)
        {
            this.normal = Vector3.Normalize(Vector3.Cross(u, v));
            this.point = point;
            this.u = u;
            this.v = v;
        }

        // CLASS METHODS
        public override Vector3 OutsideNormal(Vector3 point)
        {
            Vector3 temp = normal;
            temp.Y *= -1;
            return temp;
        }

        // detects collision between a given ray and the plane if there is a collision and where along the ray it is
        public float CollisionPlane(Ray ray)
        {
            float t = 0;
            Vector3 rayOrigin = ray.Origin;
            Vector3 rayDirection = ray.Direction;


            float a = ((rayDirection.X * normal.X) + (rayDirection.Y * normal.Y) + (rayDirection.Z * normal.Z));
            float b = (rayOrigin.X - point.X) * normal.X + (rayOrigin.Y - point.Y) * normal.Y + (rayOrigin.Z - point.Z) * normal.Z;

            t = (-b) / a;

            if (t <= 0.0001)
            {
                return 0;
            }

            float zero = ((rayOrigin.X + (t * rayDirection.X) - point.X) * normal.X) + ((rayOrigin.Y + (t * rayDirection.Y) - point.Y) * normal.Y) + ((rayOrigin.Z + (t * rayDirection.Z) - point.Z) * normal.Z);

            double xt = rayOrigin.X + t * rayDirection.X;
            double yt = rayOrigin.Y + t * rayDirection.Y;
            double zt = rayOrigin.Z + t * rayDirection.Z;

            if (zero > -0.01 && zero < 0.01)
            {
                return t;
            }

            return 0f;
        }

        // converts 3d plane coordinates to 2d coordinates
        private Vector2 To2DPlane(Vector3 input, Vector3 point, Vector3 u, Vector3 v)
        {
            float x = Vector3.Dot(input - point, u);
            float y = Vector3.Dot(input - point, v);
            return new Vector2(x, y);
        }

        // use the color from a specific texture map or the regular color
        public override Vector3 GetDiffuseColor(Vector3 input)
        {
            if (texture == Textures.Checkerboard)
            {
                return CheckerboardPlane(To2DPlane(input, point, u, v));
            }

            return diffuseColor;
        }

        public override Vector3 GetSpecularColor(Vector3 input)
        {
            if (texture == Textures.Checkerboard)
            {
                return CheckerboardPlane(To2DPlane(input, point, u, v));
            }
            return specularColor;
        }
    }
}

