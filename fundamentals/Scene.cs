using OpenTK.Graphics.OpenGL;
using System.Numerics;
using Template;

namespace RAYTRACER
{
    public class Scene
    {
        // member variables
        List<Light> lights;
        List<Primitive> primitives;
        Vector3 ambientLightingIntensity = new Vector3(1, 1, 1);

        public Vector3 AmbientLightingIntensity { get { return ambientLightingIntensity; } }
        public List<Primitive> Primitives { get { return primitives; } }
        public List<Light> Lights { get { return lights; } }

        public Scene(Surface screen)
        {
            lights = new List<Light>
        {   
            new Light(new Vector3(0, 0, 0), new Vector3(1f, 1f, 1f))
        };
            primitives = new List<Primitive>
        {
            new Sphere(new Vector3(0, 5, 0), 2f, new Vector3(131,189,125), new Vector3(131,189,125)),
            new Sphere(new Vector3(5, 0, 0), 2f, new Vector3(0,255,0), new Vector3(0,255,0)),
            new Sphere(new Vector3(0, -5, 0), 2f, new Vector3(255,0,0), new Vector3(131,0,0)),
            new Sphere(new Vector3(2.5f, 0.25f, 0.25f), 0.5f, new Vector3(180,180,255), new Vector3(180,180,255))
        };
        }
    }
}