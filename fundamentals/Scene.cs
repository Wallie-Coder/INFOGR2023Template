using OpenTK.Graphics.OpenGL;
using System.Numerics;
using Template;

namespace RAYTRACER
{
    public class Scene
    {
        List<Light> lights;

        List<Primitive> primitives;

        Vector3 ambientLighting = new Vector3(1, 1, 1);

        public Vector3 AmbientLighting { get { return ambientLighting; } }

        public List<Primitive> Primitives { get { return primitives; } private set { primitives = value; } }

        public List<Light> Lights { get { return lights; } private set { lights = value; } }

        public Scene(Surface screen)
        {
            lights = new List<Light>
        {   // position , intensity
                        // intensity cannot be higher than 1 on any value
            new Light(new Vector3(0, -4, 0), new Vector3(255f, 255f, 255f))
        };
            primitives = new List<Primitive>
        {
            // keep radius small when placing close to camera
            // x, y, z values are related, x and y can be larger when z is larger
            new Sphere(new Vector3(-2, 0, 5), 2f, new Vector3(131,189,125), new Vector3(131,189,125)),
                //new Sphere(new Vector3(5, 0, 0), 2f, new Vector3(0,255,0), new Vector3(0,255,0)),
            new Sphere(new Vector3(2, 0, 5), 2f, new Vector3(255,0,0), new Vector3(131,0,0)),
                //new Sphere(new Vector3(-5, 0, 0), 2f, new Vector3(180,180,255), new Vector3(180,180,255))
            new Plane(new Vector3(0, 1, 0), new Vector3(0, 5, 0), new Vector3 (255, 255, 255), new Vector3(0,0,255))
        };
        }
    }
}