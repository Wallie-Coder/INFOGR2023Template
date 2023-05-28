using System;
using System.Numerics;

namespace RAYTRACER
{
    public class Primitive
    {
        protected Vector3 diffuseColor;
        protected Vector3 specularColor;

        public Vector3 DiffuseColor { get { return diffuseColor; } }

        public Vector3 SpecularColor { get { return specularColor; } }

        public Primitive(Vector3 diffuseColor, Vector3 specularColor)
        {
            this.diffuseColor = diffuseColor;
            this.specularColor = specularColor;
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