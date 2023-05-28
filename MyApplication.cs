using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Numerics;
using RAYTRACER;

namespace Template
{
    class MyApplication
    {
        // member variables
        public Surface screen;
        Raytracer raytracer;
        GameWindow window;
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
            if(window.IsKeyDown(Keys.W))
            {
                //raytracer.camera.Location = Vector3.Lerp(raytracer.camera.Location, raytracer.camera.Location + raytracer.camera.Forward, 0.06f);
                raytracer.camera.Location += 0.2f * raytracer.camera.Forward;
            }
            else if(window.IsKeyDown(Keys.S))
            {
                raytracer.camera.Location += 0.2f * raytracer.camera.Backward;
            }
            else if(window.IsKeyDown(Keys.A))
            {
                raytracer.camera.Location += 0.2f * raytracer.camera.Left;
            }
            else if(window.IsKeyDown(Keys.D))
            {
                raytracer.camera.Location += 0.2f * raytracer.camera.Right;
            }
            else if(window.IsKeyDown(Keys.E))
            {
                raytracer.camera.Location += 0.2f * raytracer.camera.Down;
            }
            else if(window.IsKeyDown(Keys.Q))
            {
                raytracer.camera.Location += 0.2f * raytracer.camera.Up;
            }
            raytracer.camera.CalculatePlane();
        }
    }
}