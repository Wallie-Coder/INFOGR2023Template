using Microsoft.VisualBasic;
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

        public Vector3 GetColor(Vector3 location)
        {
            Vector3 intersection = location - point;


            Vector3 N = Vector3.Normalize(normal);

            //intersection = new Vector3(intersection.X * (1 - N.X), intersection.Y * (1 - N.Y), intersection.Z * (1 - N.Z));

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
                return new Vector3(1, 1, 1);
            else if(!x && y)
                return new Vector3(0, 0, 0);
            else if(x && !y)
                return new Vector3(0, 0, 0);
            else
                return new Vector3(1, 1, 1);


        }

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

