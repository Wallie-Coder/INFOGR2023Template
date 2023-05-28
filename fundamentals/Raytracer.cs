using System.Numerics;
using System.Security.Claims;
using Template;

namespace RAYTRACER
{
    public class Raytracer
    {
        Surface screen;

        Scene scene;

        public Camera camera;

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
            if (p is Sphere)
            {

            }
        }

        //  CHANGE FOREACH TO FOR LOOPS, BETTER PERFORMANCE
        public void Render()
        {
            int r = 0, g = 0, b = 0;
            for (int i = 0; i < camera.screenHeight; i++)
            {
                int n = 0;
                for (int j = 0; j < camera.screenWidth; j++)
                {
                    Vector3 PixelColor = new Vector3(0,0,0);
                    Intersection intersection;
                    float v = (float)j / (camera.screenWidth - 1);
                    float u = (float)i / (camera.screenHeight - 1);
                    Ray ray1 = new Ray(camera.p3 + v * camera.rightDirection + u * camera.upDirection - camera.Location, camera.Location);
                    foreach (Primitive p in scene.Primitives)
                    {
                        if (p is Sphere)
                        {
                            Sphere x = (Sphere)p;
                            if((camera.Location - x.Location).Length() < x.Radius)
                            {
                                continue;
                            }
                        }
                        var collide = p.Collision(ray1);
                        var conclusion = ray1.ConcludeFromCollision(collide.Item1, collide.Item2, collide.Item3);
                        if (conclusion.Item1)
                        {
                            intersection = new Intersection(ray1, p, conclusion.Item2);

                            //for (int k = 0; k < scene.Lights.Count; k++)
                            //{
                            // this works for single light environments
                            ShadowRay ray2 = new ShadowRay(scene.Lights[0].Location - intersection.IntersectionPoint, intersection.IntersectionPoint, scene.Lights[0]);
                            //}
                            foreach (Primitive p1 in scene.Primitives)
                            {
                                var shadowCollide = p1.Collision(ray2);

                                if (ray2.ConcludeFromCollision(shadowCollide.Item1, shadowCollide.Item2, shadowCollide.Item3))
                                {
                                    ray2.ConcludeFromCollision(shadowCollide.Item1, shadowCollide.Item2, shadowCollide.Item3);
                                    p1.Collision(ray2);
                                    r = 0;
                                    g = 0;
                                    b = 0;
                                    break;
                                }
                                else
                                {
                                    // temp
                                    ray2.ConcludeFromCollision(shadowCollide.Item1, shadowCollide.Item2, shadowCollide.Item3);
                                    p1.Collision(ray2);
                                    // set the color
                                    
                                    ray2.Color = ray2.LightSource.Intensity * (1 / (Vector3.Distance(ray2.Origin, ray2.LightSource.Location) * Vector3.Distance(ray2.Origin, ray2.LightSource.Location) ));
                                    Vector3 R =  Vector3.Normalize(ray2.Direction - 2 * (Vector3.Dot(ray2.Direction, intersection.Normal) * intersection.Normal));
                                    Vector3 V = Vector3.Normalize(camera.Location - intersection.IntersectionPoint);
                                    double q = Math.Pow(Vector3.Dot(R, V), 10);
                                    PixelColor = ray2.Color * 
                                        (p.DiffuseColor * Math.Max(0, Vector3.Dot(intersection.Normal, ray2.Direction)) + 
                                        p.SpecularColor * (float)Math.Max(0, q));

                                }

                            }
                        }
                    }
                    // change the color of the pixel based on the calculations
                    int location = j + i * screen.width;
                    screen.Plot(j, i, MixColor((int)PixelColor.X,(int)PixelColor.Y, (int)PixelColor.Z));
                    r = 0;
                    g = 0;
                    b = 0;
                }
            }
        }
    }
}
