using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Numerics;
using RAYTRACER;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace Template
{
    class MyApplication
    {
        // member variables
        public Surface screen;
        Raytracer raytracer;
        GameWindow window;
        int f = 0;
        int g = 0;
        // constructor
        public MyApplication(Surface screen, OpenTKApp window)
        {
            this.window = window;
            this.screen = screen;
            raytracer = new Raytracer(screen);
        }

        // initialize
        public void Init()
        {

        }
        // tick: renders one frame
        public void Tick()
        {
            screen.Clear(0);
            Input();
            raytracer.Render();
        }

        public void Input()
        {
            // moving left, rigth, forward, backward, up and down
            if (window.IsKeyDown(Keys.W))
            {
                raytracer.camera.Origin += raytracer.camera.Sensitivity * raytracer.camera.Forward;
            }
            else if (window.IsKeyDown(Keys.S))
            {
                raytracer.camera.Origin += raytracer.camera.Sensitivity * raytracer.camera.Backward;
            }
            else if (window.IsKeyDown(Keys.A))
            {
                raytracer.camera.Origin += raytracer.camera.Sensitivity * raytracer.camera.Left;
            }
            else if (window.IsKeyDown(Keys.D))
            {
                raytracer.camera.Origin += raytracer.camera.Sensitivity * raytracer.camera.Right;
            }
            else if (window.IsKeyDown(Keys.E))
            {
                raytracer.camera.Origin += raytracer.camera.Sensitivity * raytracer.camera.Down;
            }
            else if (window.IsKeyDown(Keys.Q))
            {
                raytracer.camera.Origin += raytracer.camera.Sensitivity * raytracer.camera.Up;
            }

            // Cycle the camera horizontally.
            if (window.IsKeyPressed(Keys.F))
            {
                f++;
                if (f == 7)
                {
                    raytracer.camera.LookAt(new System.Numerics.Vector3(1, raytracer.camera.LookingAt.Y, 1));
                }
                else if (f == 6)
                {
                    raytracer.camera.LookAt(new System.Numerics.Vector3(1, raytracer.camera.LookingAt.Y, 0));
                }
                else if (f == 5)
                {
                    raytracer.camera.LookAt(new System.Numerics.Vector3(1, raytracer.camera.LookingAt.Y, -1));
                }
                else if (f == 4)
                {
                    raytracer.camera.LookAt(new System.Numerics.Vector3(0, raytracer.camera.LookingAt.Y, -1));
                }
                else if (f == 3)
                {
                    raytracer.camera.LookAt(new System.Numerics.Vector3(-1, raytracer.camera.LookingAt.Y, -1));
                }
                else if (f == 2)
                {
                    raytracer.camera.LookAt(new System.Numerics.Vector3(-1, raytracer.camera.LookingAt.Y, 0));
                }
                else if (f == 1)
                {
                    raytracer.camera.LookAt(new System.Numerics.Vector3(-1, raytracer.camera.LookingAt.Y, 1));
                }
                else if (f > 7)
                {
                    f = 0;
                    raytracer.camera.LookAt(new System.Numerics.Vector3(0, raytracer.camera.LookingAt.Y, 1));
                }
            }


            // set the camera to the starting base and position.
            if (window.IsKeyDown(Keys.R))
            {
                raytracer.camera.Origin = new System.Numerics.Vector3(0, 0, 0);
                raytracer.camera.CalculateBase(raytracer.CamOrigin, raytracer.CamTarget, raytracer.CamUpView);
                f = 0;
            }

            raytracer.camera.CalculatePlane();
        }
    }
}