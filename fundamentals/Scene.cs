using System;
using OpenTK.Mathematics;

class Scene
{
    List<Light> lights;

    List<Primitive> primitives;

    public List<Primitive> Primitives{ get { return primitives; } private set { primitives = value; } }

    public List<Light> Lights { get { return lights; } private set { lights = value; } }

    public Scene() 
    {
        lights = new List<Light>
        {
            new Light(new Vector3(5, 5, 5), new Vector3(1, 1, 1))
        };
        primitives = new List<Primitive>
        {
            new Sphere(new Vector3(0,0,15), 1000)
        };
    }
}