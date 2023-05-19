using System;
using OpenTK.Mathematics;
using Template;

class Raytracer
{
    Surface screen;

    Scene scene;

    Camera camera;

    List<Light> lights;

    List<Primitive> primitives;

    public Raytracer(Surface screen, Scene scene)
    {
        this.screen = screen;
        this.scene = scene;
        camera = new Camera(screen);
        lights = new List<Light>
        {
            new Light(new Vector3(5, 5, 5), new Vector3(1, 1, 1))
        };
    }

    public void Render()
    {
        for(int i = 0; i < screen.height; i++)
        {
            for(int j = 0; j < screen.width; j++) 
            {
                Ray ray = new Ray(new Vector3(i / screen.height, j  / screen.width, 0) - camera.Location);

                int location = i + j * screen.width;
                screen.pixels[location] = 1;
            }
        }
    }
}
