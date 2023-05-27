using System;
using System.Numerics;

namespace RAYTRACER
{
    public class Primitive
    {
        protected Vector3 color;

        public Vector3 Color { get { return color; } }

        public Primitive()
        {


        }
        public virtual ValueTuple<double, float, float> Collision(Ray ray)
        {
            return (0, 0,0);
        }

        public virtual Vector3 OutsideNormal(Vector3 point)
        {
            return Vector3.Zero;
        }
    }
}