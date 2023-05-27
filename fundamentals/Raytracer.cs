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

    public void DebugRay(Ray r)
    {

    }

    public void DebugPrimitive(Primitive p)
    {
        if(p is Sphere)
        {

        }
    }

    //  CHANGE FOREACH TO FOR LOOPS, BETTER PERFORMANCE
    public void Render()
    {
        int r = 0, g = 0, b = 0;
        for (int i = screen.height-1; i >= 0; i--)
        {
            int n = 0;
            for(int j = 0; j < screen.width; j++) 
            {
                Intersection intersection;
                float v = (float)j / (screen.width - 1);
                float u = (float)i / (screen.height - 1);
                Ray ray = new Ray(camera.p3 + v * camera.rightDirection + u * camera.upDirection - camera.Location, camera.Location);
                foreach (Primitive p in scene.Primitives)
                {
                    var collide = p.Collision(ray);
                    if (collide.Item1)
                    {
                        //intersection = new Intersection(ray, p, collide.Item2);

                        //for (int k = 0; k < scene.Lights.Count; k++)
                        //{
                        //    // this works for single light environments
                        //    ray.Direction = Vector3.Normalize(scene.Lights[0].Location - intersection.IntersectionPoint);
                        //    ray.Origin = intersection.IntersectionPoint;
                        //    ray.Bounces += 1;
                        //    ray.ShadowRay = true;
                        //}

                        // dit is temp
                        int location = j + i * screen.width;
                        screen.Plot(j, i, MixColor(r, g, b));


                        // // color the ray returns
                    }
                    //else
                    //{
                    //    continue;
                    //}
                    //foreach(Primitive p1 in scene.Primitives)
                    //{
                    //    var shadowCollide = p1.Collision(ray);

                    //    if (shadowCollide.Item1)
                    //    {
                    //        if (Math.Acos(Vector3.Dot(p1.OutsideNormal(intersection.IntersectionPoint), ray.Direction)) * (180 / Math.PI) > 89)
                    //        {
                    //            r = 0;
                    //            g = 0;
                    //            b = 0;
                    //        }
                    //        else
                    //        {
                    //            r = (int)p.Color.X;
                    //            b = (int)p.Color.Y;
                    //            g = (int)p.Color.Z;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        r = (int)p.Color.X;
                    //        b = (int)p.Color.Y;
                    //        g = (int)p.Color.Z;
                    //    }

                    //}
                    
                }
            }
        }
    }
}