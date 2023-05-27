using RAYTRACER;

namespace Template
{
    class MyApplication
    {
        // member variables
        public Surface screen;
        Raytracer raytracer;
        // constructor
        public MyApplication(Surface screen)
        {
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
            raytracer.Render();
        }
    }
}