using System.Numerics;
using Template;

class Raytracer
{
    Surface screen;

    Scene scene;

    Camera camera;


    public Raytracer(Surface screen)
    {
        this.screen = screen;
        scene = new Scene(screen);
        camera = new Camera(screen);
    }

    int MixColor(int red, int green, int blue) { return (red << 16) + (green << 8) + blue; }

    public void Render()
    {
        for(int i = screen.height-1; i >= 0; i--)
        {
            int n = 0;
            for(int j = 0; j < screen.width; j++) 
            {
                Intersection intersection;
                float v = (float)j / (screen.width - 1);
                float u = (float)i / (screen.height - 1);
                Ray ray = new Ray(camera.p3 + v * camera.rightDirection + u * camera.upDirection - camera.Location, camera.Location);
                Vector3 temp = ray.Direction;
                Vector3.Normalize(temp);
                float t = 0.5f * (temp.Y + 1);
                Vector3 color = (1 - t) * new Vector3(1, 1, 1) + t * new Vector3(0.2f, 0.5f, 1);
                screen.Plot(j, i, MixColor((int)(color.X * 255), (int)(color.Y * 255), (int)(color.Z * 255)));
                foreach (Primitive p in scene.Primitives)
                {
                    if (p.Collision(ray))
                    {
                        //        //intersection = new Intersection(ray, p, t);
                        //        //List<Ray> shadowRays = new List<Ray>();
                        //        //for(int k = 0; k < scene.Lights.Count; k++)
                        //        //{
                        //        //    shadowRays.Add(new Ray(intersection.IntersectionPoint - scene.Lights[i].Location, intersection.IntersectionPoint));
                        //        //}

                        //        //foreach(Primitive P in scene.Primitives)
                        //        //{
                        //        //    for (int l = 0; l < shadowRays.Count; l++)
                        //        //    {
                        //        //        if (P.Collision(shadowRays[l]) == 0) 
                        //        //        {

                        //        //        }
                        //        //    }
                        //        //}
                        int location = j + i * screen.width;

                        screen.Plot(j, i, MixColor(255,0,0)); // color the ray returns
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