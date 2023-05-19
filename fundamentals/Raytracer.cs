using System;
using OpenTK.Mathematics;
using Template;

class Raytracer
{
    Surface screen;

    Scene scene;

    Camera camera;


    public Raytracer(Surface screen)
    {
        this.screen = screen;
        scene = new Scene();
        camera = new Camera(screen);
    }

    int MixColor(int red, int green, int blue) { return (red << 16) + (green << 8) + blue; }

    public void Render()
    {
        for(int i = 0; i < screen.height; i++)
        {
            for(int j = 0; j < screen.width; j++) 
            {
                Intersection intersection;
                Ray ray = new Ray(new Vector3((float)(i + 1) / screen.height, (float)(j + 1)  / screen.width, 1) - camera.Location, camera.Location); 
                foreach(Primitive p in scene.Primitives)
                {
                    float t = p.Collision(ray);
                    if (t != 0)
                    {
                        //intersection = new Intersection(ray, p, t);
                        //List<Ray> shadowRays = new List<Ray>();
                        //for(int k = 0; k < scene.Lights.Count; k++)
                        //{
                        //    shadowRays.Add(new Ray(intersection.IntersectionPoint - scene.Lights[i].Location, intersection.IntersectionPoint));
                        //}

                        //foreach(Primitive P in scene.Primitives)
                        //{
                        //    for (int l = 0; l < shadowRays.Count; l++)
                        //    {
                        //        if (P.Collision(shadowRays[l]) == 0) 
                        //        {

                        //        }
                        //    }
                        //}
                        int location = j + i * screen.width;
                        
                        screen.pixels[location] = MixColor(255,255,255); // color the ray returns
                    }
                    else
                    {
                        continue;
                    }
                }
                
            }
        }
    }
}