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
        public ValueTuple<double, float, float> Collision(Ray ray)
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

        // converts 3d sphere coordinates to 2d coordinates
        protected Vector2 To2DSphere(Vector3 input, Vector3 point, float radius)
        {
            float theta = (float)Math.Acos(Math.Clamp((input.Z - point.Z), -radius,radius) / radius);
            float phi = (float)Math.Atan2(input.Y - point.Y, input.X - point.X);

            float u = (float)((phi + Math.PI) / (2 * Math.PI));
            float v = (float)(theta / Math.PI);
            return new Vector2(u, v);
        }

        // use the color from a specific texture map or the regular color 
        public override Vector3 GetDiffuseColor(Vector3 input)
        {
            if (texture == Textures.WeirdLines)
            {
                return WeirdLineSphere(To2DSphere(input, center, radius));
            }

            return diffuseColor;
        }

        public override Vector3 GetSpecularColor(Vector3 input)
        {
            if (texture == Textures.WeirdLines)
            {
                return WeirdLineSphere(To2DSphere(input, center, radius));
            }

            return specularColor;
        }

    }
}
