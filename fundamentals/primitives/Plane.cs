using System.Numerics;

namespace RAYTRACER
{
    public class Plane : Primitive
    {
        // MEMBER VARIABLES
        private Vector3 normal;
        public Vector3 GetNormal { get { return normal; } }

        // a given point on the plane
        Vector3 point;


        // CONSTRUCTOR
        public Plane(Vector3 normal, Vector3 point, Vector3 diffuseColor, Vector3 glossyColor, bool specular = false) :base(diffuseColor, glossyColor, specular)
        {
            this.normal = Vector3.Normalize(normal);
            this.point = point;
        }

        // CLASS METHODS
        public override Vector3 OutsideNormal(Vector3 point)
        {
            normal.Y *= -1;
            return normal;
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

        // get the color of a 3D location from a texture or formula
        public Vector3 GetColor(Vector3 location)
        {
            Vector3 intersection = location - point;

            float s = intersection.X / intersection.X / (float)(Math.Sqrt((normal.X * normal.X) + (normal.Y * normal.Y)));

            bool x = false;
            bool y = false;

            if (normal.X == 0 || normal.Y == 0 || normal.Z == 0)
            {
                (bool, bool) xy = Getxy(intersection);
                x = xy.Item1;
                y = xy.Item2;
            }

            if (x && y)
            {
                return new Vector3(1, 1, 1);
            }
            else if (!x && y)
            {
                return new Vector3(0, 0, 0);
            }
            else if (x && !y)
            {
                return new Vector3(0, 0, 0);
            }
            else
            {
                return new Vector3(1, 1, 1);
            }
                
        }


        // NEEDS COMMENT
        public (bool, bool) Getxy(Vector3 intersection)
        {
            bool x = false;
            bool y = false;

            if (normal.X != 0 && normal.Y == 0 && normal.Z != 0)
            {
                x = ((int)Math.Abs(intersection.Y * normal.X)) % 2 == 0;
                y = ((int)Math.Abs(intersection.Z * normal.Z)) % 2 == 0;

                if (intersection.Y < 0)
                {
                    x = !x;
                }
                if (intersection.Z < 0)
                {
                    y = !y;
                }
            }
            if (normal.X == 0 && normal.Y != 0 && normal.Z != 0)
            {
                x = ((int)Math.Abs(intersection.X * normal.Y)) % 2 == 0;
                y = ((int)Math.Abs(intersection.Z * normal.Z)) % 2 == 0;

                if (intersection.X < 0)
                {
                    x = !x;
                }
                if (intersection.Z < 0)
                {
                    y = !y;
                }
            }
            if (normal.X != 0 && normal.Y != 0 && normal.Z == 0)
            {
                x = ((int)Math.Abs(intersection.X * normal.X)) % 2 == 0;
                y = ((int)Math.Abs(intersection.Z * normal.Y)) % 2 == 0;

                if (intersection.X < 0)
                {
                    x = !x;
                }
                if (intersection.Z < 0)
                {
                    y = !y;
                }
            }
            if (normal.X == 0 && normal.Y != 0 && normal.Z == 0)
            {
                x = ((int)Math.Abs(intersection.X)) % 2 == 0;
                y = ((int)Math.Abs(intersection.Z)) % 2 == 0;

                if (intersection.X < 0)
                {
                    x = !x;
                }
                if (intersection.Z < 0)
                {
                    y = !y;
                }
            }
            if (normal.X == 0 && normal.Y == 0 && normal.Z != 0)
            {
                x = ((int)Math.Abs(intersection.X)) % 2 == 0;
                y = ((int)Math.Abs(intersection.Y)) % 2 == 0;

                if (intersection.X < 0)
                {
                    x = !x;
                }
                if (intersection.Y < 0)
                {
                    y = !y;
                }
            }
            if (normal.X != 0 && normal.Y == 0 && normal.Z == 0)
            {
                x = ((int)Math.Abs(intersection.Y)) % 2 == 0;
                y = ((int)Math.Abs(intersection.Z)) % 2 == 0;

                if (intersection.Y < 0)
                {
                    x = !x;
                }
                if (intersection.Z < 0)
                {
                    y = !y;
                }
            }

            return (x, y);
        }
    }
}

