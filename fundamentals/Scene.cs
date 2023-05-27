using System.Numerics;
using Template;

namespace RAYTRACER
{
    public class Scene
    {
        List<Light> lights;

        List<Primitive> primitives;

        public List<Primitive> Primitives { get { return primitives; } private set { primitives = value; } }

        public List<Light> Lights { get { return lights; } private set { lights = value; } }

        public Scene(Surface screen)
        {
            lights = new List<Light>
        {
            new Light(new Vector3(4, 0, 5), new Vector3(1, 1, 1))
        };
            primitives = new List<Primitive>
        {
            // keep radius small when placing close to camera
            // x, y, z values are related, x and y can be larger when z is larger
            new Sphere(new Vector3(0, 0, 5), 2f, new Vector3(131,189,125))
        };
        }
    }
}