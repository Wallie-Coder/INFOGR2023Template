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

        // position the camera is looking at
        Vector3 camTarget = new Vector3(0, 0, 5);
        // position of the camera
        Vector3 camOrigin = new Vector3(0, 0, 0);
        // the "up" direction of the camera used to calculate the basis
        Vector3 camUpView = new Vector3(0, 1, 0);
        // vertical FOV
        float FOV = 90;

        public Vector3 CamTarget { get { return camTarget; } }
        public Vector3 CamOrigin { get { return camOrigin; } }
        public Vector3 CamUpView { get { return camUpView; } }

        public Raytracer(Surface screen)
        {
            this.screen = screen;
            scene = new Scene(screen);
            camera = new Camera(camOrigin, camTarget, camUpView, FOV);
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
            for (int i = 0; i < camera.screenHeight; i++)
            {
                for (int j = 0; j < camera.screenWidth; j++)
                {
                    Vector3 PixelColor = new Vector3(0,0,0);
                    Intersection intersection;
                    Ray ray1 = camera.CalculateRay(j, i);
                    foreach (Primitive p in scene.Primitives)
                    {
                        if (p is Sphere)
                        {
                            Sphere x = (Sphere)p;
                            if((camera.Origin - x.Location).Length() < x.Radius)
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
                                    PixelColor = new Vector3(0, 0, 0);
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
                                    Vector3 V = Vector3.Normalize(camera.Origin - intersection.IntersectionPoint);
                                    double q = Math.Pow(Vector3.Dot(R, V), 10);
                                    PixelColor = ray2.Color * 
                                        (p.DiffuseColor * Math.Max(0, Vector3.Dot(intersection.Normal, ray2.Direction)) + 
                                        p.SpecularColor * (float)Math.Max(0, q));

                                    // shadow color p.DiffuseColor * scene.AmbientLighting

                                }

                            }
                        }
                    }
                    // change the color of the pixel based on the calculations
                    int location = j + i * screen.width;
                    screen.Plot(j, i, MixColor((int)PixelColor.X,(int)PixelColor.Y, (int)PixelColor.Z));
                }
            }
        }
    }
}
