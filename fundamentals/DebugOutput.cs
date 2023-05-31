using System.Numerics;
using Template;
using System.Diagnostics;

namespace RAYTRACER
{
    class DebugOutput
    {

        static public List<(Vector2, Vector2)> rayLines = new List<(Vector2 origin, Vector2 end)>();
        static public List<(Vector2, int)> Pixels = new List<(Vector2 Location, int Color)>();
        List<(Vector2, float)> circles = new List<(Vector2 center, float radius)>();

        Surface screen;
        Raytracer raytracer;


        Stopwatch t = new Stopwatch();
        float fpsCounter = 0;
        float FPS = 0;

        private Surface screen;
        private Raytracer raytracer;


        private Vector2 SceneSize = new Vector2(20, 20);


        // CONSTRUCTOR
        public DebugOutput(Raytracer raytracer, Scene scene, Surface screen)
        {
            this.screen = screen;
            this.raytracer = raytracer;

            foreach(Primitive p in scene.Primitives)
            {
                if(p.GetType() == typeof(Sphere))
                {
                    Sphere P = (Sphere)p;

                    circles.Add((new Vector2(P.Center.X, P.Center.Z), P.Radius));
                }
            }
            t.Start();
        }

        // CLASS METHODS

        // draws the sphere and rays from the camera in the debug screen
        public void Draw()
        {
            float xScale = 1 / 16f * screen.width / 4;
            float yScale = 1 / 9f * screen.height / 2;

            foreach((Vector2, Vector2) r in  rayLines) 
            {
                PlotLine(new Vector2(r.Item1.X * xScale, r.Item1.Y * yScale), new Vector2(r.Item2.X * xScale, r.Item2.Y * yScale), MyApplication.MixColor(255, 255, 0));
            }

            //PlotLine(new Vector2(rayLines[rayLines.Count/4].Item1.X * xScale, rayLines[rayLines.Count/4].Item1.Y * yScale), new Vector2(rayLines[rayLines.Count - 1].Item2.X * xScale, rayLines[rayLines.Count - 1].Item2.Y * yScale), MyApplication.ixColor(255, 255, 0));

            // Draw each circle
            foreach ((Vector2 center, float radius) c in circles)
            {
                for (float i = 0; i < 360; i += 0.5f)
                {
                    float r = (int)((float)(c.radius * xScale));
                    float x = MathF.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(i));
                    float y = MathF.Sin(OpenTK.Mathematics.MathHelper.DegreesToRadians(i));

                    x = x * r;
                    y = y * r;

                    Vector2 center = new Vector2(c.center.X * xScale, c.center.Y * yScale);


                    SetPixel((int)(x + center.X), (int)(y + center.Y), MyApplication.MixColor(255, 255, 255));
                }
            }

            // draw pixels
            foreach((Vector2, int) p in Pixels)
            {
                //SetPixel((int)(p.Item1.X * xScale), (int)(p.Item1.Y * yScale), p.Item2);
            }    

            // Draw a pixel for the camera
            SetPixel((int)(raytracer.camera.Origin.X * xScale), (int)(raytracer.camera.Origin.Z * yScale), MixColor(0, 0, 255));
            DebugInfo();


            // Draw the screen plane
            PlotLine(new Vector2(raytracer.camera.TopLeft.X * xScale, raytracer.camera.TopLeft.Z * yScale), new Vector2(raytracer.camera.TopRight.X * xScale, raytracer.camera.TopRight.Z * yScale), MixColor(0, 0, 255));

            rayLines.Clear();
        }

        // sets a pixel color as long as its in the debug screen
        void SetPixel(int x, int y, int color)
        {

            y = -y;
            if (x < screen.width / 4 + screen.width / 2 && x > -screen.width/4 && y < screen.height && y > -screen.height / 2)
            {
                screen.Plot(x + screen.width / 2 + screen.width / 4, y + screen.height / 2, color);
            }

        }

        // NEEDS COMMENT
        void PlotLine(Vector2 origin, Vector2 end, int color)
        {
            Vector2 Origin = origin;
            Vector2 End = end;

            Origin.Y = -Origin.Y;
            End.Y = -End.Y;

            origin.Y = -origin.Y;
            end.Y = -end.Y;

            Vector2 Direction = end - origin;

            if (End.X < -screen.width / 4 && origin.X > -screen.width / 4)
            {
                float lessenwith = -screen.width / 4 - End.X;
                Direction = new Vector2(Direction.X / Direction.X, Direction.Y / Direction.X);
                End.X = (int)(End.X + (Direction.X * lessenwith));
                End.Y = (int)(End.Y + (Direction.Y * lessenwith));
            }


            // Draw the original rays.
            //screen.Line((int)(origin.X + screen.width / 4 + screen.width / 2), (int)(origin.Y + screen.height / 2), (int)(end.X + screen.width / 4 + screen.width / 2), (int)(end.Y + screen.height / 2), MyApplication.MixColor(255, 0, 0));

            // Draw the cut rays.
            screen.Line((int)(Origin.X + screen.width / 4 + screen.width / 2), (int)(Origin.Y + screen.height / 2), (int)(End.X + screen.width / 4 + screen.width / 2), (int)(End.Y + screen.height / 2), color);
        }

        // prints info about the Camera on the debugscreen
        void DebugInfo()
        {
            screen.Print("CamZ = " + raytracer.camera.screenZ.ToString(), screen.width / 2, screen.height - 20, MixColor(255, 255, 255));
            screen.Print("CamY = " + raytracer.camera.screenY.ToString(), screen.width / 2, screen.height - 40, MixColor(255, 255, 255));
            screen.Print("CamX = " + raytracer.camera.screenX.ToString(), screen.width / 2, screen.height - 60, MixColor(255, 255, 255));
            screen.Print("CamOrigin = " + raytracer.camera.Origin.ToString(), screen.width / 2, screen.height - 80, MixColor(255, 255, 255));
            screen.Print("Yaw = " + raytracer.camera.Yaw.ToString(), screen.width / 2, screen.height - 100, MixColor(255, 255, 255));
            screen.Print("Pitch = " + raytracer.camera.Pitch.ToString(), screen.width / 2, screen.height - 120, MixColor(255, 255, 255));

            // print and calculate FPS

            // update every second, always whole number.
            if (t.ElapsedMilliseconds >= 1000)
            {
                t.Restart();
                FPS = fpsCounter;
                fpsCounter = 0;
            }

            // update everey frame
            //FPS = (1000f / t.ElapsedMilliseconds);
            //t.Restart();

            screen.Print("FPS: " + FPS.ToString(), 640, screen.height - 140, MixColor(255, 255, 255));
            fpsCounter++;
        }
    }
}

