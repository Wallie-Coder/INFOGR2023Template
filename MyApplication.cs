using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Numerics;
using RAYTRACER;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using System.Reflection.Metadata.Ecma335;

namespace Template
{
    class MyApplication
    {
        // member variables
        public Surface screen;
        Raytracer raytracer;
        GameWindow window;
        bool parallelRendering = false;

        DebugOutput debugOutput;

        // constructor
        public MyApplication(Surface screen, OpenTKApp window)
        {
            this.window = window;
            this.screen = screen;
            raytracer = new Raytracer(screen);

            debugOutput = new DebugOutput(raytracer, raytracer.scene, screen);
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
            if (parallelRendering)
            {
                raytracer.ParallelRender();
            }
            else 
            {
                raytracer.Render();
            }
            debugOutput.Draw();
        }

        ValueTuple<float, float> UnitCirclePositions(float angle)
        {
            angle *= (float)(Math.PI / 180);
            return ((float)Math.Cos(angle), (float)Math.Sin(angle));
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

            //rotate the camera around the x an y axis.
            if (window.IsKeyDown(Keys.Right))
            {
                float yaw = raytracer.camera.Yaw + raytracer.camera.RotationSpeed;
                var coords = UnitCirclePositions(yaw);
                raytracer.camera.LookAt(new System.Numerics.Vector3(coords.Item2, raytracer.camera.LookingAt.Y, coords.Item1)); 
                if (yaw > 359 || yaw < -359)
                {
                    yaw = 0;
                }
                raytracer.camera.Yaw = yaw;
            }
            else if (window.IsKeyDown(Keys.Left))
            {
                float yaw = raytracer.camera.Yaw - raytracer.camera.RotationSpeed;
                var coords = UnitCirclePositions(yaw);
                raytracer.camera.LookAt(new System.Numerics.Vector3(coords.Item2, raytracer.camera.LookingAt.Y, coords.Item1));
                if (yaw > 359 || yaw < -359)
                {
                    yaw = 0;
                }
                raytracer.camera.Yaw = yaw;
            }
            else if(window.IsKeyDown(Keys.Up))
            {
                float pitch = raytracer.camera.Pitch - raytracer.camera.RotationSpeed;
                var coords = UnitCirclePositions(pitch);
                raytracer.camera.LookAt(new System.Numerics.Vector3(raytracer.camera.LookingAt.X, coords.Item2, coords.Item1));
                if (pitch > 359 || pitch < -359)
                {
                    pitch = 0;
                }
                raytracer.camera.Pitch = pitch;
            }
            else if(window.IsKeyDown(Keys.Down))
            {
                float pitch = raytracer.camera.Pitch + raytracer.camera.RotationSpeed;
                var coords = UnitCirclePositions(pitch);
                raytracer.camera.LookAt(new System.Numerics.Vector3(raytracer.camera.LookingAt.X, coords.Item2, coords.Item1));
                if (pitch > 359 || pitch < -359)
                {
                    pitch = 0;
                }
                raytracer.camera.Pitch = pitch;
            }

            // set the camera to the starting base and position.
            if (window.IsKeyDown(Keys.R))
            {
                raytracer.camera.Origin = new System.Numerics.Vector3(0, 0, 0);
                raytracer.camera.CalculateBase(raytracer.CamOrigin, raytracer.CamTarget, raytracer.CamUpView);
                raytracer.camera.Yaw = 0;
                raytracer.camera.Pitch = 0;
            }

            raytracer.camera.CalculatePlane();
        }
    }
}