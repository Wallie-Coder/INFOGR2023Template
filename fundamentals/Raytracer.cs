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


        private Vector3 camTarget = new Vector3(1, 0, 0);
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
                var shadowCollide = p1.Collision(shadowRay);

                // determine if a point on a sphere has light
                if (shadowRay.ConcludeFromCollision(shadowCollide.Item1, shadowCollide.Item2, shadowCollide.Item3))
                {
                    shadowRay.ConcludeFromCollision(shadowCollide.Item1, shadowCollide.Item2, shadowCollide.Item3);
                    p1.Collision(shadowRay);
                    pixelColor = new Vector3(0, 0, 0);
                    return;
                }
                else
                {
                    // set the color
                    shadowRay.Color = shadowRay.LightSource.Intensity * (1 / (Vector3.Distance(shadowRay.Origin, shadowRay.LightSource.Location) * Vector3.Distance(shadowRay.Origin, shadowRay.LightSource.Location)));
                    Vector3 R = Vector3.Normalize(shadowRay.Direction - 2 * (Vector3.Dot(shadowRay.Direction, intersection.Normal) * intersection.Normal));
                    Vector3 V = Vector3.Normalize(camera.Origin - intersection.IntersectionPoint);
                    double q = Math.Pow(Vector3.Dot(R, V), 10);
                    pixelColor = shadowRay.Color *
                        (p.DiffuseColor * Math.Max(0, Vector3.Dot(intersection.Normal, shadowRay.Direction)) +
                        p.SpecularColor * (float)Math.Max(0, q));
                }
            }
        }

       
        void RenderX(int i, int j)
        {
            Vector3 pixelColor = new Vector3(20, 20, 20);
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
                }
                var collide = p.Collision(primaryRay);
                var conclusion = primaryRay.ConcludeFromCollision(collide.Item1, collide.Item2, collide.Item3);
                if (!conclusion.Item1) continue;
                intersection = new Intersection(primaryRay, p, conclusion.Item2);

                //for (int k = 0; k < scene.Lights.Count; k++)
                //{
                // this works for single light environments
                ShadowRay shadowRay = new ShadowRay(scene.Lights[0].Location - intersection.IntersectionPoint, intersection.IntersectionPoint, scene.Lights[0]);
                //}
                RenderShading(ref shadowRay, ref pixelColor, ref intersection, ref prim);

                // add the ray to the DebugOutput
                if (i == 180 && j % 10 == 0)
                    DebugOutput.RayLines.Add((new Vector2(camera.Origin.X, camera.Origin.Z), new Vector2(intersection.IntersectionPoint.X, intersection.IntersectionPoint.Z)));
            }
            // change the color of the pixel based on the calculations
            screen.Plot(j, i, MyApplication.MixColor((int)pixelColor.X, (int)pixelColor.Y, (int)pixelColor.Z));

            if (intersection != null) return;
            // add the ray to the DebugOutput
            if (i == 180 && j % 10 == 0)
                DebugOutput.RayLines.Add((new Vector2(camera.Origin.X, camera.Origin.Z), new Vector2(camera.Origin.X + primaryRay.Direction.X * 200, camera.Origin.Z + primaryRay.Direction.Z * 200)));
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
