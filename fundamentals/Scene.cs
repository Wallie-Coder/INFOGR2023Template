using System;
using OpenTK.Graphics.OpenGL;
using System.Numerics;
using Template;

class Scene
{
    List<Light> lights;

    List<Primitive> primitives;

    public List<Primitive> Primitives{ get { return primitives; } private set { primitives = value; } }

    public List<Light> Lights { get { return lights; } private set { lights = value; } }

    public Scene(Surface screen) 
    {
        //lights = new List<Light>
        //{
        //    new Light(new Vector3(5, 5, 5), new Vector3(1, 1, 1))
        //};
        primitives = new List<Primitive>
        {
            // keep radius small when placing close to camera
            // x, y, z values are related, x and y can be larger when z is larger
            new Sphere(new Vector3(0, 0, 5), 0.7f, new Vector3(255,255,255))
        };
    }
}