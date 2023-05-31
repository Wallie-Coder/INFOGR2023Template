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
        {   // position , intensity
                        // intensity cannot be higher than 1 on any value
            new Light(new Vector3(0, 0, 0), new Vector3(10f, 10f, 10f))
        };
            primitives = new List<Primitive>
        {
            // keep radius small when placing close to camera
            // x, y, z values are related, x and y can be larger when z is larger
            new Sphere(new Vector3(-2, 0, 5), 2f, new Vector3(0,255,0), new Vector3(131,189,125)),
                //new Sphere(new Vector3(5, 0, 0), 2f, new Vector3(0,255,0), new Vector3(0,255,0)),
            new Sphere(new Vector3(2, 0, 5), 2f, new Vector3(255,0,0), new Vector3(131,0,0)),
                //new Sphere(new Vector3(-5, 0, 0), 2f, new Vector3(180,180,255), new Vector3(180,180,255))
            new Plane(new Vector3(0, 1, 0), new Vector3(0, 2, 0), new Vector3 (255, 255, 255), new Vector3(0,0,255))
        };
        }
    }
}