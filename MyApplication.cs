namespace Template
{
    class MyApplication
    {
        // member variables
        public Surface screen;
        // constructor
        public MyApplication(Surface screen)
        {
            this.screen = screen;
        }
        // initialize
        public void Init()
        {
             
        }
        // tick: renders one frame
        public void Tick()
        {
            screen.Clear(0);
        }
    }
}