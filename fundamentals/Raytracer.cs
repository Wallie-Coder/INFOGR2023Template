using OpenTK.Graphics.ES20;
using System.ComponentModel;
using System.Numerics;
using System.Security.Claims;
using Template;
using static System.Formats.Asn1.AsnWriter;

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
        private Vector3 camUp = new Vector3(0, 1, 0);

        private float FOV = 90;

        public Vector3 CamTarget { get { return camTarget; } }
        public Vector3 CamOrigin { get { return camOrigin; } }
        public Vector3 CamUp { get { return camUp; } }

        // CONSTRUCTOR
        public Raytracer(Surface screen)
        {
            this.screen = screen;
            scene = new Scene(screen);
            camera = new Camera(camOrigin, camTarget, camUp, FOV);
        }

        // CLASS METHODS

        //  CHANGE FOREACH TO FOR LOOPS, BETTER PERFORMANCE
        public void ParallelRender()
        {
            //multithread through the y axis
            Parallel.For(0, camera.ScreenHeight, RenderY);

        }

        void RenderShading(ref ShadowRay shadowRay, ref Vector3 pixelColor, ref Intersection intersection, ref Primitive p)
        {
            for (int l = 0; l < scene.Primitives.Count; l++)
            {
                Primitive p1 = scene.Primitives[l];

                if (p1 is Sphere)
                {
                    Sphere s = (Sphere)p1;
                    var shadowCollide = s.CollisionSphere(shadowRay);

                    if (shadowRay.ConcludeFromCollision(shadowCollide.Item1, shadowCollide.Item2, shadowCollide.Item3))
                    {
                        shadowRay.ConcludeFromCollision(shadowCollide.Item1, shadowCollide.Item2, shadowCollide.Item3);
                        s.CollisionSphere(shadowRay);
                        pixelColor = new Vector3(0, 0, 0);
                        return;
                    }
                    else
                    {
                        // temp
                        shadowRay.ConcludeFromCollision(shadowCollide.Item1, shadowCollide.Item2, shadowCollide.Item3);
                        s.CollisionSphere(shadowRay);
                        // set the color
                        shadowRay.Color = shadowRay.LightSource.Intensity * (1 / (Vector3.Distance(shadowRay.Origin, shadowRay.LightSource.Location) * Vector3.Distance(shadowRay.Origin, shadowRay.LightSource.Location)));
                        Vector3 R = Vector3.Normalize(shadowRay.Direction - 2 * (Vector3.Dot(shadowRay.Direction, intersection.Normal) * intersection.Normal));
                        Vector3 V = Vector3.Normalize(camera.Origin - intersection.IntersectionPoint);
                        double q = Math.Pow(Vector3.Dot(R, V), 10);
                        pixelColor = shadowRay.Color *
                            (p.DiffuseColor * Math.Max(0, Vector3.Dot(intersection.Normal, shadowRay.Direction)) +
                            p.SpecularColor * (float)Math.Max(0, q));

                        //shadow color p.DiffuseColor* scene.AmbientLighting

                    }
                } 
            }
            if (p is Plane)
            {
                Plane P = (Plane)p;
                //list of t of lighrays.
                (Primitive, float) ClosestPtoLight = (p, P.CollisionPlane(shadowRay));


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
                        var conclusion = shadowRay.ConcludeFromCollision(collision.Item1, collision.Item2, collision.Item3);

                        if (conclusion)
                        {
                            if (collision.Item2 < collision.Item3)
                            {
                                if (collision.Item2 < ClosestPtoLight.Item2)
                                    ClosestPtoLight = (x, collision.Item2);
                            }
                            else
                            {
                                if (collision.Item3 < ClosestPtoLight.Item2)
                                    ClosestPtoLight = (x, collision.Item3);
                            }
                        }
                    }
                }

                if (ClosestPtoLight.Item1 == P)
                {
                    Plane x = (Plane)p;

                    float ab = (x.GetNormal.X * shadowRay.Direction.X) + (x.GetNormal.Y * shadowRay.Direction.Y) + (x.GetNormal.Z * shadowRay.Direction.Z);
                    float A = (float)Math.Sqrt((x.GetNormal.X * x.GetNormal.X) + (x.GetNormal.Y * x.GetNormal.Y) + (x.GetNormal.Z * x.GetNormal.Z));
                    float B = (float)Math.Sqrt((shadowRay.Direction.X * shadowRay.Direction.X) + (shadowRay.Direction.Y * shadowRay.Direction.Y) + (shadowRay.Direction.Z * shadowRay.Direction.Z));

                    //pixelColor = p.DiffuseColor * scene.Lights[0].Intensity * (1 / Vector3.Distance(scene.Lights[0].Location, intersection.IntersectionPoint) * Math.Cos(ab / (A * B));
                    //pixelColor = new Vector3(255, 255, 255);

                    // set the color
                    shadowRay.Color = shadowRay.LightSource.Intensity * (1 / (Vector3.Distance(shadowRay.Origin, shadowRay.LightSource.Location) * Vector3.Distance(shadowRay.Origin, shadowRay.LightSource.Location)));
                    Vector3 R = Vector3.Normalize(shadowRay.Direction - 2 * (Vector3.Dot(shadowRay.Direction, intersection.Normal) * intersection.Normal));
                    Vector3 V = Vector3.Normalize(camera.Origin - intersection.IntersectionPoint);
                    double q = Math.Pow(Vector3.Dot(R, V), 10);
                    pixelColor = shadowRay.Color *
                        (p.DiffuseColor * Math.Max(0, Vector3.Dot(intersection.Normal, shadowRay.Direction)) +
                        p.SpecularColor * (float)Math.Max(0, q));

                }
                else
                {
                    pixelColor = new Vector3(0, 0, 0);
                }
            }
        }

       
        void RenderX(int i, int j)
        {
            Vector3 pixelColor = new Vector3(20, 20, 20);
            List<Intersection> intersections = new List<Intersection>();
            Intersection intersection = null;
            Ray primaryRay = camera.CalculateRay(j, i);
            foreach (Primitive p in scene.Primitives)
            {
                Primitive prim = p;
                if (p is Sphere q)
                {
                    if ((camera.Origin - q.Center).LengthSquared() < q.Radius * q.Radius)
                    {
                        continue;
                    }

                    var collide = q.CollisionSphere(primaryRay);
                    var conclusion = primaryRay.ConcludeFromCollision(collide.Item1, collide.Item2, collide.Item3);
                    if (conclusion.Item1)
                    {
                        intersection = new Intersection(primaryRay, p, conclusion.Item2);

                        intersections.Add(intersection);
                    }
                }
                if (p is Plane)
                {
                    Plane x = (Plane)p;
                    if(x.CollisionPlane(primaryRay) == 0)
                    {
                        continue;
                    }
                    else
                    {
                        intersection = new Intersection(primaryRay, p, x.CollisionPlane(primaryRay));
                        intersections.Add(intersection);
                    }
                }
            }

            Intersection Closest = null;
            if (intersections.Count > 0)
            {
                Closest = intersections[0];
                foreach (Intersection I in intersections)
                {
                    if (I.GetT < Closest.GetT && I.GetT != 0)
                    {
                        Closest = I;
                    }
                }
            }

            if (Closest != null)
            {
                Primitive prim = Closest.GetPrimitive;
                if (Closest.GetPrimitive is Sphere)
                {
                    //for (int k = 0; k < scene.Lights.Count; k++)
                    //{
                    // this works for single light environments
                    ShadowRay shadowRay = new ShadowRay(scene.Lights[0].Location - Closest.IntersectionPoint, Closest.IntersectionPoint, scene.Lights[0]);
                    //}
                    RenderShading(ref shadowRay, ref pixelColor, ref Closest, ref prim);

                    // add the ray to the DebugOutput
                    if (i == 180 && j % 10 == 0)
                        DebugOutput.RayLines.Add((new Vector2(camera.Origin.X, camera.Origin.Z), new Vector2(Closest.IntersectionPoint.X, Closest.IntersectionPoint.Z)));
                }
                if (Closest.GetPrimitive is Plane)
                {
                    //pixelColor = Closest.GetPrimitive.DiffuseColor;
                    ShadowRay shadowRay = new ShadowRay(scene.Lights[0].Location - Closest.IntersectionPoint, Closest.IntersectionPoint, scene.Lights[0]);

                    RenderShading(ref shadowRay, ref pixelColor, ref Closest, ref prim);

                    // add the ray to the DebugOutput
                    if (i == 180 && j % 10 == 0)
                        DebugOutput.RayLines.Add((new Vector2(camera.Origin.X, camera.Origin.Z), new Vector2(Closest.IntersectionPoint.X, Closest.IntersectionPoint.Z)));

                }
            }

            // change the color of the pixel based on the calculations
            int location = j + i * screen.width;

            int r = (int)(Math.Clamp(pixelColor.X, 0, 1) * 255);
            int g = (int)(Math.Clamp(pixelColor.Y, 0, 1) * 255);
            int b = (int)(Math.Clamp(pixelColor.Z, 0, 1) * 255);
            screen.Plot(j, i, MyApplication.MixColor(r, g, b));

            if (intersection == null)
            {
                // add the ray to the DebugOutput
                //if (i == 180 && j % 10 == 0)
                    //DebugOutput.rayLines.Add((new Vector2(camera.Origin.X, camera.Origin.Z), new Vector2(camera.Origin.X + primaryRay.Direction.X * 200, camera.Origin.Z + primaryRay.Direction.Z * 200)));
            }
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
