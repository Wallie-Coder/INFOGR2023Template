using System.Numerics;
using Template;

namespace RAYTRACER
{
    public class Raytracer
    {
        // MEMBER VARIABLES
        private Surface screen;
        private Scene scene;
        public Scene Scene { get { return scene; } }

        private Camera camera;
        public Camera Camera { get { return camera; } }

        // position the camera will look at
        private Vector3 camTarget = new Vector3(0, 0, 1);
        private Vector3 camOrigin = new Vector3(0, 0, 0);


        // vertical fov
        private float FOV = 45;

        // get or set field of view
        public float getsetFOV { get { return FOV; } set { FOV = value; } }


        // samples per pixel
        private const int samplesPerPixel = 3;

        public Vector3 CamTarget { get { return camTarget; } }
        public Vector3 CamOrigin { get { return camOrigin; } }

        // CONSTRUCTOR
        public Raytracer(Surface screen)
        {
            this.screen = screen;
            scene = new Scene();
            camera = new Camera(camOrigin, camTarget, FOV, screen.width, screen.height);
        }

        // CLASS METHODS

        // multithreaded for loop over y-axis
        public void MultithreadedRender()
        {
            Parallel.For(0, camera.ScreenHeight, IterateOverX);
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


        Vector3 CalculateColorByLighting(Intersection intersection, Primitive p, int i1, int j1)
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
                if (p is Sphere sphere)
                {
                    var shadowCollide = sphere.CollisionSphere(shadowRay);

                    if (shadowRay.ConcludeFromCollision(shadowCollide.Item1, shadowCollide.Item2, shadowCollide.Item3))
                    {
                        continue;
                    }
                    else
                    {
                        if (shadowRay.LightSource is Spotlight spotlight)
                        {
                            if (!spotlight.RayInSpotlight(shadowRay))
                            {
                                continue;
                            }
                        }
                        // set the color
                        shadowRay.Color = shadowRay.LightSource.Intensity * (1 / (Vector3.Distance(shadowRay.Origin, shadowRay.LightSource.Location) * Vector3.Distance(shadowRay.Origin, shadowRay.LightSource.Location)));
                        Vector3 R = Vector3.Normalize(shadowRay.Direction - 2 * (Vector3.Dot(shadowRay.Direction, intersection.Normal) * intersection.Normal));
                        Vector3 V = Vector3.Normalize(camera.Origin - intersection.IntersectionPoint);
                        double q = Math.Pow(Vector3.Dot(R, V), 10);
                        pixelColor += shadowRay.Color * (p.GetDiffuseColor(intersection.IntersectionPoint) * Math.Max(0, Vector3.Dot(intersection.Normal, shadowRay.Direction)) + p.GetSpecularColor(intersection.IntersectionPoint) * (float)Math.Max(0, q));
                    }

                    if (i1 == 180 && j1 % 20 == 0)
                        DebugOutput.RayLines.Add((new Vector2(shadowRay.Origin.X, shadowRay.Origin.Z), new Vector2(shadowRay.LightSource.Location.X, shadowRay.LightSource.Location.Z), new Vector3(255, 0, 0)));

                }
                else if (p is Plane plane)
                {

                    (Primitive, float) ClosestPtoLight = (p, plane.CollisionPlane(shadowRay));

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

                    if (ClosestPtoLight.Item1 == plane)
                    {
                        // set the color
                        shadowRay.Color = shadowRay.LightSource.Intensity / (Vector3.Distance(shadowRay.Origin, shadowRay.LightSource.Location) * Vector3.Distance(shadowRay.Origin, shadowRay.LightSource.Location));
                        float dot = Vector3.Dot(intersection.Normal, shadowRay.Direction);
                        Vector3 R = Vector3.Normalize(shadowRay.Direction - 2 * dot * intersection.Normal);
                        Vector3 V = Vector3.Normalize(camera.Origin - intersection.IntersectionPoint);
                        double q = Math.Pow(Vector3.Dot(R, V), 10);
                        pixelColor += shadowRay.Color * ((plane.GetDiffuseColor(intersection.IntersectionPoint) * Math.Max(0, dot)) + p.GetSpecularColor(intersection.IntersectionPoint) * (float)Math.Max(0, q));
                    }
                }
            }

            return pixelColor + p.GetDiffuseColor(intersection.IntersectionPoint) * scene.AmbientLightingIntensity;
        }

        Vector3 ColorFromSamples(Vector3 color, int samplePerPixel)
        {
            float r = color.X;
            float g = color.Y;
            float b = color.Z;

            float sampleScale = 1f / samplePerPixel;
            r = Math.Clamp(r * sampleScale, 0, 1);
            g = Math.Clamp(g * sampleScale, 0, 1);
            b = Math.Clamp(b * sampleScale, 0, 1);
            Vector3 result = new Vector3(r, g, b);
            return result;
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
                if (i == 180 && j % 20 == 0 && (intersection.GetPrimitive is Sphere || ray.Origin != camera.Origin))
                {
                    Vector3 color;
                    if(ray.Origin != camera.Origin)
                    {
                        color = new Vector3(0, 0, 255);
                    }
                    else
                    {
                        color = new Vector3(255, 255, 0);
                    }
                    DebugOutput.RayLines.Add((new Vector2(ray.Origin.X, ray.Origin.Z), new Vector2(intersection.IntersectionPoint.X, intersection.IntersectionPoint.Z), color));

                }
                if (intersection.GetPrimitive.Specular)
                {
                    if(ray.Bounces < Ray.RecursionDepth)
                        finalColor = intersection.GetPrimitive.GetDiffuseColor(intersection.IntersectionPoint) * TraceRay(new Ray(FindReflectionDirection(ray, intersection), intersection.IntersectionPoint, ray.Bounces + 1), i, j, ref finalColor);
                }
                else
                {
                    finalColor = CalculateColorByLighting(intersection, intersection.GetPrimitive, i, j);
                }
            }

            return finalColor;
        }

        Vector3 FindReflectionDirection(Ray incomingRay, Intersection intersection)
        {
            return incomingRay.Direction - 2 * Vector3.Dot(incomingRay.Direction, intersection.Normal) * intersection.Normal;
        }

        Vector3 CalculatePixelColor(int i, int j)
        {
            Vector3 pixelColor = new Vector3(0, 0, 0);

            Ray primaryRay = camera.CalculateRay(j, i);

            pixelColor = TraceRay(primaryRay, i , j, ref pixelColor);

            
            return new Vector3(pixelColor.X, pixelColor.Y, pixelColor.Z);
        }

        // iterates over the x axis
        void IterateOverX(int i)
        {
            // iterate over the x axis
            for(int x = 0; x < camera.ScreenWidth;x++)
            {
                Vector3 pixelColor = new Vector3(0, 0, 0);
                if (MyApplication.AntiAliasing)
                {
                    SampledPixelColor(i, x);
                }
                else
                {
                    pixelColor += CalculatePixelColor(i, x);
                }
                int r = (int)(Math.Clamp(pixelColor.X, 0, 1) * 255);
                int g = (int)(Math.Clamp(pixelColor.Y, 0, 1) * 255);
                int b = (int)(Math.Clamp(pixelColor.Z, 0, 1) * 255);
                screen.Plot(x, i, MyApplication.MixColor(r, g, b));
            }
        }

        // iterates samplePerPixel times over a single pixel for stochastic sampling
        void SampledPixelColor(int i, int j)
        {
            Vector3 sampledColorTotal = new Vector3(0, 0, 0);
            for (int s = 0; s < samplesPerPixel; s++)
            {
                sampledColorTotal += CalculatePixelColor(i, j);
            }

            Vector3 pixelColor = ColorFromSamples(sampledColorTotal, samplesPerPixel);
            int r = (int)(Math.Clamp(pixelColor.X, 0, 1) * 255);
            int g = (int)(Math.Clamp(pixelColor.Y, 0, 1) * 255);
            int b = (int)(Math.Clamp(pixelColor.Z, 0, 1) * 255);
            screen.Plot(j, i, MyApplication.MixColor(r, g, b));
        }

        public void Render()
        {
            // iterate over the y axis
            for (int y = 0; y < camera.ScreenHeight; y++)
            {
                IterateOverX(y);
            }
        }
    }
}
