using System.Numerics;
using Microsoft.VisualBasic;
using OpenTK.Graphics.ES20;
using OpenTK.Graphics.GL;
using Template;

namespace RAYTRACER
{
    public class Raytracer
    {
        // shadow color p.DiffuseColor * scene.AmbientLighting

        // MEMBER VARIABLES
        private Surface screen;
        private Scene scene;
        public Scene Scene { get { return scene; } }

        private Camera camera;
        public Camera Camera { get { return camera; } }


        private Vector3 camTarget = new Vector3(0, 0, 1);
        private Vector3 camOrigin = new Vector3(0, 0, 0);

        private float FOV = 45;

        public Vector3 CamTarget { get { return camTarget; } }
        public Vector3 CamOrigin { get { return camOrigin; } }

        // CONSTRUCTOR
        public Raytracer(Surface screen)
        {
            this.screen = screen;
            scene = new Scene(screen);
            camera = new Camera(camOrigin, camTarget, FOV);
        }

        // CLASS METHODS

        public void ParallelRender()
        {
            //multithread through the y axis
            Parallel.For(0, camera.ScreenHeight, RenderY);
        }

        Intersection FindClosestIntersection(List<Intersection> intersections)
        {
            Intersection closest = null;
            if (intersections.Count > 0)
            {
                closest = intersections[0];
                foreach (Intersection I in intersections)
                {
                    if (I.GetT < closest.GetT && I.GetT != 0)
                    {
                        closest = I;
                    }
                }
            }
            return closest;
        }


        Vector3 RenderShading(Intersection intersection, Primitive p)
        {
            Vector3 pixelColor = new Vector3(0, 0, 0);
            List<ShadowRay> shadows = new List<ShadowRay>();
            for (int s = 0; s < scene.Lights.Count; s++)
            {
                Light light = scene.Lights[s];
                shadows.Add(new ShadowRay(scene.Lights[s].Location - intersection.IntersectionPoint, intersection.IntersectionPoint, light));
            }
            for(int i = 0; i < shadows.Count; i++)
            {
                ShadowRay shadowRay = shadows[i];
                if (p is Sphere s)
                {
                    var shadowCollide = s.CollisionSphere(shadowRay);

                    if (shadowRay.ConcludeFromCollision(shadowCollide.Item1, shadowCollide.Item2, shadowCollide.Item3))
                    {
                        continue;
                    }
                    else
                    {
                        // set the color
                        shadowRay.Color = shadowRay.LightSource.Intensity * (1 / (Vector3.Distance(shadowRay.Origin, shadowRay.LightSource.Location) * Vector3.Distance(shadowRay.Origin, shadowRay.LightSource.Location)));
                        Vector3 R = Vector3.Normalize(shadowRay.Direction - 2 * (Vector3.Dot(shadowRay.Direction, intersection.Normal) * intersection.Normal));
                        Vector3 V = Vector3.Normalize(camera.Origin - intersection.IntersectionPoint);
                        double q = Math.Pow(Vector3.Dot(R, V), 10);
                        pixelColor += shadowRay.Color * (p.DiffuseColor * Math.Max(0, Vector3.Dot(intersection.Normal, shadowRay.Direction)) + p.SpecularColor * (float)Math.Max(0, q));
                        //shadow color p.DiffuseColor* scene.AmbientLighting
                    }
                }
                else if (p is Plane P)
                {

                    (Primitive, float) ClosestPtoLight = (p, P.CollisionPlane(shadowRay));

                    shadowRay.Direction = Vector3.Normalize(shadowRay.Direction);

                    foreach (Primitive p1 in scene.Primitives)
                    {
                        if (p1 == p)
                        {
                            continue;
                        }

                        if (p1 is Plane)
                        {
                            Plane x = (Plane)p1;

                            if (x.CollisionPlane(shadowRay) > float.Epsilon)
                            {
                                if (x.CollisionPlane(shadowRay) < ClosestPtoLight.Item2)
                                    ClosestPtoLight = (x, x.CollisionPlane(shadowRay));
                            }
                        }

                        if (p1 is Sphere)
                        {
                            Sphere x = (Sphere)p1;
                            var collision = x.CollisionSphere(shadowRay);
                            var conclusion =
                                shadowRay.ConcludeFromCollision(collision.Item1, collision.Item2, collision.Item3);

                            if (conclusion)
                            {
                                if (collision.Item2 > collision.Item3)
                                {
                                    if (collision.Item2 > ClosestPtoLight.Item2)
                                        ClosestPtoLight = (x, collision.Item2);
                                }
                                else
                                {
                                    if (collision.Item3 > ClosestPtoLight.Item2)
                                        ClosestPtoLight = (x, collision.Item3);
                                }
                            }
                        }
                    }

                    if (ClosestPtoLight.Item1 == P)
                    {
                        // set the color

                        // for plane with texture use P.getColor(intersection.intersectionpoint instead of p.DiffuseColor

                        shadowRay.Color = shadowRay.LightSource.Intensity / (Vector3.Distance(shadowRay.Origin, shadowRay.LightSource.Location) * Vector3.Distance(shadowRay.Origin, shadowRay.LightSource.Location));
                        float dot = Vector3.Dot(intersection.Normal, shadowRay.Direction);
                        Vector3 R = Vector3.Normalize(shadowRay.Direction - 2 * dot * intersection.Normal);
                        Vector3 V = Vector3.Normalize(camera.Origin - intersection.IntersectionPoint);
                        double q = Math.Pow(Vector3.Dot(R, V), 10);
                        pixelColor += shadowRay.Color * ((P.GetColor(intersection.IntersectionPoint) * Math.Max(0, dot)) + p.SpecularColor * (float)Math.Max(0, q));
                       
                    }
                }
            }
            return pixelColor + p.DiffuseColor * scene.AmbientLightingIntensity;
        }

        Vector3 TraceRay(Ray ray, int i , int j, ref Vector3 finalColor)
        {
            Intersection intersection = null;
            List<Intersection> result = new List<Intersection>();
            foreach (Primitive p in scene.Primitives)
            {
                if (p is Sphere q)
                {
                    if ((camera.Origin - q.Center).LengthSquared() < q.Radius * q.Radius)
                    {
                        continue;
                    }

                    var collide = q.CollisionSphere(ray);
                    var conclusion = ray.ConcludeFromCollision(collide.Item1, collide.Item2, collide.Item3);
                    if (conclusion.Item1)
                    {
                        intersection = new Intersection(ray, p, conclusion.Item2);

                        result.Add(intersection);
                    }
                }
                if (p is Plane x)
                {

                    if (x.CollisionPlane(ray) == 0)
                    {
                        continue;
                    }

                    intersection = new Intersection(ray, p, x.CollisionPlane(ray));
                    result.Add(intersection);
                }
            }
            intersection = FindClosestIntersection(result);
            
                if (intersection != null)
                {
                    if (i == 180 && j % 10 == 0)
                        DebugOutput.RayLines.Add((new Vector2(camera.Origin.X, camera.Origin.Z), new Vector2(intersection.IntersectionPoint.X, intersection.IntersectionPoint.Z)));
                    if (intersection.GetPrimitive.Specular)
                    {
                        finalColor = intersection.GetPrimitive.DiffuseColor * TraceRay(new Ray(FindReflectionDirection(ray, intersection), intersection.IntersectionPoint), i, j, ref finalColor);
                    }
                else
                {
                    finalColor = RenderShading(intersection, intersection.GetPrimitive);
                }
            }

            return finalColor;
        }

        Vector3 FindReflectionDirection(Ray incomingRay, Intersection intersection)
        {
            return incomingRay.Direction - 2 * Vector3.Dot(incomingRay.Direction, intersection.Normal) * intersection.Normal;
        }

        void RenderX(int i, int j)
        {
            List<ShadowRay> shadows = new List<ShadowRay>();
            Vector3 pixelColor = new Vector3(0, 0, 0);

            Ray primaryRay = camera.CalculateRay(j, i);

            pixelColor = TraceRay(primaryRay, i , j, ref pixelColor);
            

            // change the color of the pixel based on the calculations
            int location = j + i * screen.width;

            int r = (int)(Math.Clamp(pixelColor.X, 0, 1) * 255);
            int g = (int)(Math.Clamp(pixelColor.Y, 0, 1) * 255);
            int b = (int)(Math.Clamp(pixelColor.Z, 0, 1) * 255);
            screen.Plot(j, i, MyApplication.MixColor(r, g, b));
        }

        void RenderY(int i)
        {
            // iterate over the x axis
            for(int x = 0; x < camera.ScreenWidth;x++)
            {
                RenderX(i, x);
            }
        }

        public void Render()
        {
            // iterate over the y axis
            for (int y = 0; y < camera.ScreenHeight; y++)
            {
                RenderY(y);
            }
        }
    }
}
