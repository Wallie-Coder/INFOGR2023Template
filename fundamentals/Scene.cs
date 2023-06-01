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
            lights = new List<Light>
            {
                //new Light(new Vector3(0, -2, 0), new Vector3(10f, 10f, 10f)),
                new Light(new Vector3(0, -5, 5), new Vector3(15, 15, 15))
            };
            
            primitives = new List<Primitive>
            {
                new Sphere(new Vector3(2, 0, 5), 2f, new Vector3(150, 150,150), new Vector3(150,150,150)),
                //new Sphere(new Vector3(5, 0, 0), 2f, new Vector3(0,255,0), new Vector3(0,255,0)),
                new Sphere(new Vector3(-2, 0, 5), 2f, new Vector3(255,0,0), new Vector3(131,0,0)),
                //new Sphere(new Vector3(-5, 0, 0), 2f, new Vector3(180,180,255), new Vector3(180,180,255))
                new Plane(new Vector3(0, 1, 0), new Vector3(0, 2, 0), new Vector3 (255, 255, 255), new Vector3(0,0,255))
            };
        }
    }
}