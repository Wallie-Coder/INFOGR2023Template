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
        OpenTK.Mathematics.Vector2 lastPos;
        bool firstMove = true;
        int f = 0;

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
            raytracer.Render();
            debugOutput.Draw();
        }

        //public void Rotate()
        //{
        //    window.CursorGrabbed = true;
        //    if (firstMove)
        //    {
        //        lastPos = window.MouseState.Position;
        //        firstMove = false;
        //    }
        //    else
        //    {
        //        float deltaX = window.MouseState.Delta.X;
        //        float deltaY = window.MouseState.Delta.Y;
        //        lastPos = window.MouseState.Position;

        //        raytracer.camera.Yaw += deltaX * raytracer.camera.RotationSpeed;
        //        if (raytracer.camera.Pitch > 89.0f)
        //        {
        //            raytracer.camera.Pitch = 89.0f;
        //        }
        //        else if (raytracer.camera.Pitch < -89.0f)
        //        {
        //            raytracer.camera.Pitch = -89.0f;
        //        }
        //        else
        //        {
        //            raytracer.camera.Pitch -= deltaX * raytracer.camera.RotationSpeed;
        //        }
        //    }

        //    raytracer.camera.screenZ.X = (float)Math.Cos(MathHelper.DegreesToRadians(raytracer.camera.Pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(raytracer.camera.Yaw));
        //    raytracer.camera.screenZ.Y = (float)Math.Sin(MathHelper.DegreesToRadians(raytracer.camera.Pitch));
        //    raytracer.camera.screenZ.Z = (float)Math.Cos(MathHelper.DegreesToRadians(raytracer.camera.Pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(raytracer.camera.Yaw));
        //    raytracer.camera.CalculateBase(new System.Numerics.Vector3(0,0,0), raytracer.camera.screenZ, System.Numerics.Vector3.UnitY);

        //    window.MousePosition = new OpenTK.Mathematics.Vector2(640 / 2, 360 / 2); 

        //}

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