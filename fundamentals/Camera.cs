using System.Numerics;
using Template;
namespace RAYTRACER {
    public class Camera
    {
        // member variables
        Vector3 location;

        public Vector3 Location
        {
            get { return location; }
            private set { location = value; }
        }

        public Vector3 forwardDirection;

        public Vector3 upDirection;

        public Vector3 rightDirection;

        float aspectRatio = (float)16 / 9;

        public int screenWidth = 640;
        public int screenHeight = 360;

        public Vector3 planeCenter;

        float cameraHeight = 2.0f;
        float cameraWidth;
        float planeDistance = -1;

        public float PlaneDistance
        {
            get { return planeDistance; }
        }

        Vector3 p0; // top right
        Vector3 p1; // top left
        Vector3 p2; // bottom right
        public Vector3 p3; // bottom left



        //constructor
        public Camera(Surface screen)
        {
            cameraWidth = aspectRatio * cameraHeight;
            location = new Vector3(0, 0, 0);
            forwardDirection = new Vector3(0, 0, planeDistance);
            upDirection = -1 * new Vector3(0, cameraHeight, 0);
            rightDirection = new Vector3(cameraWidth, 0, 0);
            CalculatePlane();
        }

        // Calculates the plane center and corners of the plane the camera shows.
        void CalculatePlane()
        {
            planeCenter = location + forwardDirection;
            p3 = location - rightDirection / 2 - upDirection / 2 - forwardDirection;
        }


    }
}


