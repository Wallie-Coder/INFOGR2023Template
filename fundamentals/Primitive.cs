using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Numerics;
using Template;

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

        // enum for textures
        public enum Textures  {
            Checkerboard,
            WeirdLines,
            None
        }

        // the texture of the primitive
        protected Textures texture;

        // CONSTRUCTOR
        public Primitive(Vector3 diffuseColor, Vector3 specularColor, bool specular, Textures texture)
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

            this.texture = texture;
        }

        // CLASS METHODS

        // returns the normal pointing outwards
        public virtual Vector3 OutsideNormal(Vector3 point)
        {
            return Vector3.Zero;
        }

        // TEXTURE MAP FUNCTIONS
        protected Vector3 CheckerboardPlane(Vector2 point)
        {
            float c = ((int)point.Y + (int)point.X) & 1;
            return new Vector3(c, c, c);

        }

        protected Vector3 WeirdLineSphere(Vector2 point)
        {
            float c = (int)(point.X * 100 + point.Y * 100) & 1;
            if(c == 1)
            {
                int x = 10;
            }
            return new Vector3(c, c, c);
        }

        // color getters with possibility to override with texture map
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