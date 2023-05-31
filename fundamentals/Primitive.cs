using System;
using System.Numerics;

namespace RAYTRACER
{
    public class Primitive
    {
        // MEMBER VARIABLES
        protected Vector3 diffuseColor, specularColor;
        public Vector3 DiffuseColor { get { return diffuseColor; } }
        public Vector3 SpecularColor { get { return specularColor; } }


        protected bool specular;
        public bool Specular { get { return specular; } }


        // CONSTRUCTOR
        public Primitive(Vector3 diffuseColor, Vector3 specularColor, bool specular = false)
        {
            this.diffuseColor = diffuseColor / 255;

            this.specular = specular;
            if (specular)
            {
                this.specularColor = specularColor / 255;
            }
            else
            {
                this.specularColor = diffuseColor / 255;
            }
        }
        public virtual ValueTuple<double, float, float> CollisionSphere(Ray ray)
        {
            return (0, 0, 0);
        }

        public virtual float CollisionPlane(Ray ray)
        {
            return 0f;
        }

        public virtual Vector3 OutsideNormal(Vector3 point)
        {
            return Vector3.Zero;
        }
    }
}