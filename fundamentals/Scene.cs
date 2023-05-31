using OpenTK.Graphics.OpenGL;
using System.Numerics;
using Template;

namespace RAYTRACER
{
    public class Scene
    {
        // MEMBER VARIABLES
        private List<Light> lights; 
        public List<Light> Lights { get { return lights; } }


        List<Primitive> primitives;
        public List<Primitive> Primitives { get { return primitives; } }


        Vector3 ambientLightingIntensity = new Vector3(1, 1, 1);
        public Vector3 AmbientLightingIntensity { get { return ambientLightingIntensity; } }
        
        
        // CONSTRUCTOR
        public Scene(Surface screen)
        {
            // the color of both lights and spheres can range from 0 to 255 on all RGB values
            // the RGB values are converted to an RGB value between 0 - 1 in the constructors of their object class
            lights = new List<Light>
            {   
                new Light(new Vector3(0, 0, 0), new Vector3(255, 255, 255)),
                //new Light(new Vector3(0, 2, 0), new Vector3(255, 0f, 0f))
            };
            primitives = new List<Primitive>
            {
                new Sphere(new Vector3(5, 0, 5), 2f, new Vector3(255,0,0), new Vector3(255,0,0)), 
                new Sphere(new Vector3(5, 0, 0), 2f, new Vector3(0,0,255), new Vector3(0,255,0)),
                //new Sphere(new Vector3(0, -5, 0), 2f, new Vector3(255,0,0), new Vector3(255,0,0)),
                new Sphere(new Vector3(0, 0, 5), 1f, new Vector3(0,255,0), new Vector3(0,255,0))
            };
        }
    }
}