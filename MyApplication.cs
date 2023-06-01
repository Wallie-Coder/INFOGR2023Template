using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Numerics;
using RAYTRACER;

namespace Template
{
    class MyApplication
    {
        // MEMBER VARIABLES
        public Surface screen;
        private Raytracer raytracer;
        private GameWindow window;
        private static bool multithreading = true;
        public static bool Multithreading { get { return multithreading; } }

        DebugOutput debugOutput;

        // CONSTRUCTOR
        public MyApplication(Surface screen, OpenTKApp window)
        {
            this.window = window;
            this.screen = screen;
            raytracer = new Raytracer(screen);

            debugOutput = new DebugOutput(raytracer, raytracer.Scene, screen);
        }

        // CLASS METHODS

        // renders one frame
        public void Tick()
        {
            screen.Clear(0);
            Input();
            if (multithreading)
            {
                raytracer.MultithreadedRender();
            }
            else 
            {
                raytracer.Render();
            }
            debugOutput.Draw();
        }

        // mix a color to 24-bit from rgb: from P0
        public static int MixColor(int red, int green, int blue) { return (red << 16) + (green << 8) + blue; }

        // handle all input from the user
        public void Input()
        {
            // moving forward, backward, right, left, up and down
            if (window.IsKeyDown(Keys.W))
            {
                raytracer.Camera.Origin += raytracer.Camera.MovementSpeed * raytracer.Camera.Forward;
            }
            else if (window.IsKeyDown(Keys.S))
            {
                raytracer.Camera.Origin += raytracer.Camera.MovementSpeed * raytracer.Camera.Backward;
            }
            else if (window.IsKeyDown(Keys.A))
            {
                raytracer.Camera.Origin += raytracer.Camera.MovementSpeed * raytracer.Camera.Left;
            }
            else if (window.IsKeyDown(Keys.D))
            {
                raytracer.Camera.Origin += raytracer.Camera.MovementSpeed * raytracer.Camera.Right;
            }
            else if (window.IsKeyDown(Keys.E))
            {
                raytracer.Camera.Origin += raytracer.Camera.MovementSpeed * raytracer.Camera.Down;
            }
            else if (window.IsKeyDown(Keys.Q))
            {
                raytracer.Camera.Origin += raytracer.Camera.MovementSpeed * raytracer.Camera.Up;
            }


            //rotate the camera around the x an y axis.
            if (window.IsKeyDown(Keys.Right))
            {
                float yaw = (raytracer.Camera.Yaw + raytracer.Camera.RotationSpeed) * (float)(Math.PI / 180);
                float pitch = raytracer.Camera.Pitch * (float)(Math.PI / 180);
                Quaternion qPitch = Quaternion.CreateFromYawPitchRoll(0, pitch, 0);
                Quaternion qYaw = Quaternion.CreateFromYawPitchRoll(yaw, 0, 0);
                raytracer.Camera.Rotate(qPitch, qYaw);
                raytracer.Camera.Yaw += raytracer.Camera.RotationSpeed;
                if (raytracer.Camera.Yaw > 359 || raytracer.Camera.Yaw < -359)
                {
                    raytracer.Camera.Yaw = 0;
                }
            }
            else if (window.IsKeyDown(Keys.Left))
            {
                float yaw = (raytracer.Camera.Yaw - raytracer.Camera.RotationSpeed) * (float)(Math.PI / 180);
                float pitch = raytracer.Camera.Pitch * (float)(Math.PI / 180);
                Quaternion qPitch = Quaternion.CreateFromYawPitchRoll(0, pitch, 0);
                Quaternion qYaw = Quaternion.CreateFromYawPitchRoll(yaw, 0, 0);
                raytracer.Camera.Rotate(qPitch, qYaw);
                raytracer.Camera.Yaw -= raytracer.Camera.RotationSpeed;
                if (raytracer.Camera.Yaw > 359 || raytracer.Camera.Yaw < -359)
                {
                    raytracer.Camera.Yaw = 0;
                }
                
            }
            else if(window.IsKeyDown(Keys.Up))
            {
                float pitch = (raytracer.Camera.Pitch + raytracer.Camera.RotationSpeed) * (float)(Math.PI / 180);
                float yaw = raytracer.Camera.Yaw * (float)(Math.PI / 180);
                Quaternion qPitch = Quaternion.CreateFromYawPitchRoll(0, pitch, 0);
                Quaternion qYaw = Quaternion.CreateFromYawPitchRoll(yaw, 0, 0);
                raytracer.Camera.Rotate(qPitch, qYaw);
                raytracer.Camera.Pitch += raytracer.Camera.RotationSpeed;
                if (raytracer.Camera.Pitch > 359 || raytracer.Camera.Pitch < -359)
                {
                    raytracer.Camera.Pitch = 0;
                }
            }
            else if(window.IsKeyDown(Keys.Down))
            {
                float pitch = (raytracer.Camera.Pitch - raytracer.Camera.RotationSpeed) * (float)(Math.PI / 180);
                float yaw = raytracer.Camera.Yaw * (float)(Math.PI / 180);
                Quaternion qPitch = Quaternion.CreateFromYawPitchRoll(0, pitch, 0);
                Quaternion qYaw = Quaternion.CreateFromYawPitchRoll(yaw, 0, 0);
                raytracer.Camera.Rotate(qPitch, qYaw);
                raytracer.Camera.Pitch -= raytracer.Camera.RotationSpeed;
                if (raytracer.Camera.Pitch > 359 || raytracer.Camera.Pitch < -359)
                {
                    raytracer.Camera.Pitch = 0;
                }
                
            }

            // set the Camera to the starting orientation and position.
            if (window.IsKeyDown(Keys.R))
            {
                // reset the FOV
                raytracer.getsetFOV = 45;
                raytracer.Camera.SetFOV(raytracer.getsetFOV, raytracer.CamOrigin, raytracer.CamTarget);

                // reset the camera
                raytracer.Camera.Origin = new System.Numerics.Vector3(0, 0, 0);
                raytracer.Camera.CalculateBase(raytracer.CamOrigin, raytracer.CamTarget, Vector3.UnitY);
                raytracer.Camera.Yaw = (float)Math.Atan2(raytracer.Camera.ScreenZ.X, raytracer.Camera.ScreenZ.Z) * (float)(180 / Math.PI);
                raytracer.Camera.Pitch = (float)Math.Asin(raytracer.Camera.ScreenZ.Y) * (float)(180 / Math.PI);
            }

            // increase the FOV of the camera
            if (window.IsKeyDown(Keys.U))
            {
                raytracer.getsetFOV = raytracer.getsetFOV + 2;
                if (raytracer.getsetFOV > 150)
                    raytracer.getsetFOV = 150;
                raytracer.Camera.SetFOV(raytracer.getsetFOV, raytracer.CamOrigin, raytracer.CamTarget);
            }

            // decrease the FOV of the camera
            if (window.IsKeyDown(Keys.I))
            {
                raytracer.getsetFOV = raytracer.getsetFOV - 2;
                if (raytracer.getsetFOV < 30)
                    raytracer.getsetFOV = 30;
                raytracer.Camera.SetFOV(raytracer.getsetFOV, raytracer.CamOrigin, raytracer.CamTarget);
            }

            // factor in the changed base and position of the camera to recalculate the camera screen
            raytracer.Camera.CalculatePlane();
        }
    }
}