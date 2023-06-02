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


        Vector3 ambientLightingIntensity = (new Vector3(60, 60, 60)) / 255;
        public Vector3 AmbientLightingIntensity { get { return ambientLightingIntensity; } }
        
        
        // CONSTRUCTOR
        public Scene()
        {
            lights = new List<Light>
            {
                new Light(new Vector3(0, -2, 0), new Vector3(10f, 10f, 10f)),
                //new Light(new Vector3(-2, -4, 5), new Vector3(10f, 10f, 10f)),
                //new Light(new Vector3(0,-9,0), new Vector3(70,70,70))
                //new Spotlight(new Vector3(2,-2,3), new Vector3(10,10,10), new Vector3(0,2,2), 5)
            };
            
            primitives = new List<Primitive>
            {
                new Sphere(new Vector3(2, 0, 5), 2f, new Vector3(150, 150,150), new Vector3(150,150,150), true),
                //new Sphere(new Vector3(5, 0, 0), 2f, new Vector3(0,255,0), new Vector3(0,255,0)),
                new Sphere(new Vector3(-2, 0, 5), 2f, new Vector3(255,0,0), new Vector3(131,0,0)),
                //new Sphere(new Vector3(-5, 0, 0), 2f, new Vector3(180,180,255), new Vector3(180,180,255))
                new Plane(new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(0, 4, 0), new Vector3 (0, 0, 0), new Vector3(0,0,255), texture:Primitive.Textures.Checkerboard),
                new Plane(new Vector3(-1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, -10, 0), new Vector3 (0, 0, 255), new Vector3(0,170,255)),
                //new Plane(new Vector3(1,1,1), new Vector3(1,1,-1), new Vector3(-2,0,0), new Vector3 (0, 0, 255),  new Vector3(0,0,0), true)
            };
        }
    }
}