﻿using System.Numerics;
using Template;
using System.Diagnostics;
using SixLabors.ImageSharp;

namespace RAYTRACER
{
    class DebugOutput
    {

        private static List<(Vector2, Vector2, Vector3)> rayLines = new List<(Vector2 origin, Vector2 end, Vector3 color)>();
        public static List<(Vector2, Vector2, Vector3)> RayLines { get { return rayLines; } }

        private List<(Vector2, float)> circles = new List<(Vector2 center, float radius)>();

        private Surface screen;
        private Raytracer raytracer;

        private float xScale;
        private float yScale;


        private Stopwatch t = new Stopwatch();
        private float fpsCounter = 0;
        private float fps = 0;


        // CONSTRUCTOR
        public DebugOutput(Raytracer raytracer, Scene scene, Surface screen)
        {
            this.screen = screen;
            this.raytracer = raytracer;

            foreach(Primitive p in scene.Primitives)
            {
                if(p is Sphere sphere)
                {

                    circles.Add((new Vector2(sphere.Center.X, sphere.Center.Z), sphere.Radius));
                }
            }

            xScale = 1 / 16f * screen.width / 4;
            yScale = 1 / 9f * screen.height / 2;

            t.Start();
        }

        // CLASS METHODS

        // draws the sphere and rays from the Camera in the debug screen
        public void Draw()
        {
            foreach((Vector2, Vector2, Vector3) r in  rayLines) 
            {
                PlotLine(new Vector2(r.Item1.X * xScale, r.Item1.Y * yScale), new Vector2(r.Item2.X * xScale, r.Item2.Y * yScale), MyApplication.MixColor((int)r.Item3.X, (int)r.Item3.Y, (int)r.Item3.Z));
            }

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

            // draw a pixel for the camera
            SetPixel((int)(raytracer.Camera.Origin.X * xScale), (int)(raytracer.Camera.Origin.Z * yScale), MyApplication.MixColor(0, 0, 255));
            DebugInfo();

            int o = MyApplication.MixColor(255, 255, 255);

            // draw the screen plane
            PlotLine(new Vector2(raytracer.Camera.TopLeft.X * xScale, raytracer.Camera.TopLeft.Z * yScale), new Vector2(raytracer.Camera.TopRight.X * xScale, raytracer.Camera.TopRight.Z * yScale), MyApplication.MixColor(255, 0, 255));

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

        // Plots a Line on the debug screen, but cuts the line off on the left size, so the line does not overlap the scene.
        void PlotLine(Vector2 origin, Vector2 end, int color)
        {
            origin.Y = -origin.Y;
            end.Y = -end.Y;

            Vector2 direction = end - origin;

            // clamp rays 
            if (end.X < -screen.width / 4 && origin.X > -screen.width / 4)
            {
                float lessenwith = -screen.width / 4 - end.X;
                direction = new Vector2(direction.X / direction.X, direction.Y / direction.X);
                end.X = (int)(end.X + (direction.X * lessenwith));
                end.Y = (int)(end.Y + (direction.Y * lessenwith));
            }

            if (origin.X < -screen.width / 4 && end.X < -screen.width / 4)
            {
                return;
            }

            if (origin.X < -screen.width / 4 && end.X > -screen.width / 4)
            {
                float lessenwith = -screen.width / 4 - origin.X;
                direction = new Vector2(direction.X / direction.X, direction.Y / direction.X);
                origin.X = (int)(origin.X + (direction.X * lessenwith));
                origin.Y = (int)(origin.Y + (direction.Y * lessenwith));
            }

            // Draw the cut rays.
            screen.Line((int)(origin.X + screen.width / 4 + screen.width / 2), (int)(origin.Y + screen.height / 2), (int)(end.X + screen.width / 4 + screen.width / 2), (int)(end.Y + screen.height / 2), color);
        }

        // prints info about the camera etc. on the debugscreen
        void DebugInfo()
        {
            screen.Print("CamZ = " + raytracer.Camera.ScreenZ.ToString(), screen.width / 2, screen.height - 20, MyApplication.MixColor(255, 255, 255));
            screen.Print("CamY = " + raytracer.Camera.ScreenY.ToString(), screen.width / 2, screen.height - 40, MyApplication.MixColor(255, 255, 255));
            screen.Print("CamX = " + raytracer.Camera.ScreenX.ToString(), screen.width / 2, screen.height - 60, MyApplication.MixColor(255, 255, 255));
            screen.Print("CamOrigin = " + raytracer.Camera.Origin.ToString(), screen.width / 2, screen.height - 80, MyApplication.MixColor(255, 255, 255));
            screen.Print("Yaw = " + raytracer.Camera.Yaw.ToString(), screen.width / 2, screen.height - 100, MyApplication.MixColor(255, 255, 255));
            screen.Print("Pitch = " + raytracer.Camera.Pitch.ToString(), screen.width / 2, screen.height - 120, MyApplication.MixColor(255, 255, 255));

            // update every second, always a whole number.
            if (t.ElapsedMilliseconds >= 1000)
            {
                t.Restart();
                fps = fpsCounter;
                fpsCounter = 0;
            }

            screen.Print("fps: " + fps.ToString(), screen.width / 2, screen.height - 140, MyApplication.MixColor(255, 255, 255));
            screen.Print("FOV = " + raytracer.getsetFOV.ToString(), screen.width / 2, screen.height - 180, MyApplication.MixColor(255, 255, 255));
            screen.Print("multithreading: " + MyApplication.Multithreading.ToString().ToLower(), screen.width / 2, screen.height - 160, MyApplication.MixColor(255, 255, 255));
            fpsCounter++;
        }
    }
}

