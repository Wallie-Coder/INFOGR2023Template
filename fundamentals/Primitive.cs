using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Numerics;

namespace RAYTRACER
{
    public class Primitive
    {
        // MEMBER VARIABLES
        protected Vector3 diffuseColor, specularColor;


        protected bool specular;

        public bool Specular
        {
            get { return specular; }
        }


        // CONSTRUCTOR
        public Primitive(Vector3 diffuseColor, Vector3 specularColor, bool specular)
        {
            // correct for 0 - 1 range on colors
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

        // CLASS METHODS

        public virtual Vector3 OutsideNormal(Vector3 point)
        {
            return Vector3.Zero;
        }

        // TEXTURE MAP FUNCTIONS
        protected Vector3 Checkerboard(Vector3 input, Vector3 point, Vector3 u, Vector3 v)
        {
            if (this is Plane)
            {
                float x = Vector3.Dot(input - point, u);
                float y = Vector3.Dot(input - point, v);
                float c = ((int)y + (int)x) & 1;
                return new Vector3(c, c, c);
            }

            return diffuseColor;
        }

        public virtual Vector3 GetDiffuseColor(Vector3 input)
        {
            return diffuseColor;
        }

        public virtual Vector3 GetSpecularColor(Vector3 input)
        {
            return specularColor;
        }
    }
}