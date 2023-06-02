using System.Linq;
using System.Numerics;
using Template;

namespace RAYTRACER
{
    public class Camera
    {
        // MEMBER VARIABLES

        private Vector3 origin;
        public Vector3 Origin { get { return origin; } set { origin = value; } }


        private float movementSpeed = 0.2f;
        private float rotationSpeed = 1f;
        public float MovementSpeed { get { return movementSpeed; } }
        public float RotationSpeed { get { return rotationSpeed; } }

        
        private float pitch, yaw;
        public float Pitch { get { return pitch; } set { pitch = value; } }
        public float Yaw { get { return yaw; } set { yaw = value; } }
       
        // the horizontal and vertical vectors of the screen plane
        private Vector3 vertical, horizontal;


        // random for stochastic sampling
        private Random random = new Random();


        // the vectors for movement
        public Vector3 Forward { get { return Vector3.Normalize(screenZ); } }
        public Vector3 Backward { get { return Vector3.Normalize(-screenZ); } }
        public Vector3 Left { get { return Vector3.Normalize(-screenX); } }
        public Vector3 Right { get { return Vector3.Normalize(screenX); } }
        public Vector3 Up { get { return Vector3.Normalize(-screenY); } }
        public Vector3 Down { get { return Vector3.Normalize(screenY); } }

        // aspect ratio of the real screen
        private float aspectRatio;

        // the width and height of the real screen
        private int screenWidth;
        private int screenHeight;
        public int ScreenWidth { get { return screenWidth; } }
        public int ScreenHeight { get { return screenHeight; } }


        // the height and width of the camera screen plane
        private float cameraHeight, cameraWidth;

        // the basis of the camera
        private Vector3 screenZ, screenY, screenX;
        public Vector3 ScreenZ { get { return screenZ; } }
        public Vector3 ScreenY { get { return screenY; } }
        public Vector3 ScreenX { get { return screenX; } }


        // the center and corners of the screen plane
        private Vector3 planeCenter, topRight, topLeft, bottomRight, bottomLeft;
        public Vector3 TopRight { get { return topRight; } }
        public Vector3 TopLeft { get { return topLeft; } }


        // CONSTRUCTOR
        public Camera(Vector3 lookFrom, Vector3 lookAt, float fov, int screenWidth, int screenHeight)
        {
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth / 2;
            aspectRatio = (float)this.screenWidth / this.screenHeight;
            // calculate the screens size based on vertical FOV
            float theta = (float)Math.Tan(fov * (float)(Math.PI / 180) / 2);
            cameraHeight = theta * 2;
            cameraWidth = aspectRatio * cameraHeight;
            CalculateBase(lookFrom, lookAt, Vector3.UnitY);
            CalculatePlane();
            // set pitch and yaw according to the x y and z.
            pitch = (float)Math.Asin(screenZ.Y) * (float)(180 / Math.PI);
            yaw = (float)Math.Atan2(screenZ.X, screenZ.Z) * (float)(180 / Math.PI);
        }

        // CLASS METHODS

        // Calculates the plane center and corners of the plane the camera shows.
        public void CalculatePlane()
        {
            planeCenter = origin + screenZ;
            topRight = origin + horizontal / 2 + vertical / 2 + screenZ;
            topLeft = origin - horizontal / 2 + vertical / 2 + screenZ;
            bottomRight = origin + horizontal / 2 - vertical / 2 + screenZ;
            bottomLeft = origin - horizontal / 2 - vertical / 2 + screenZ;
        }

        // caclulate the screens size base on new vertical FOV
        public void SetFOV(float fov, Vector3 lookFrom, Vector3 lookAt)
        {
            // calculate the screens size based on vertical FOV
            float theta = (float)Math.Tan(fov * (float)(Math.PI / 180) / 2);
            cameraHeight = theta * 2;
            cameraWidth = aspectRatio * cameraHeight;
            horizontal = cameraWidth * screenX;
            vertical = cameraHeight * screenY;
            CalculatePlane();
        }

        // returns a ray through a given pixel on the real screen.
        public Ray CalculateRay(int x, int y)
        {
            float v = x;
            float u = y;
            if (MyApplication.AntiAliasing)
            {
                v += (float)random.NextDouble();
                u += (float)random.NextDouble();
            }
           
            v /= (screenWidth - 1);
            u /= (screenHeight - 1);
            return new Ray(bottomLeft + v * horizontal + u * vertical - origin, origin);
        }

        //  calculates the basis for the camera
        public void CalculateBase(Vector3 lookFrom, Vector3 lookAt, Vector3 viewUp)
        {
            screenZ = Vector3.Normalize(lookAt - lookFrom);
            screenX = Vector3.Normalize(Vector3.Cross(viewUp, screenZ));
            screenY = Vector3.Normalize(Vector3.Cross(screenZ, screenX));
            origin = lookFrom;
            horizontal = cameraWidth * screenX;
            vertical = cameraHeight * screenY;
            yaw = (float)Math.Atan2(screenZ.X, screenZ.Z) * (float)(180 / Math.PI);
            pitch = (float)Math.Asin(screenZ.Y) * (float)(180 / Math.PI);
        }

        // rotates the camera basis according to a pitch and yaw quaternion.
        public void Rotate(Quaternion pitch, Quaternion yaw)
        {
            screenZ = Vector3.Transform(Vector3.UnitZ, pitch);
            screenZ = Vector3.Transform(screenZ, yaw);
            screenX = Vector3.Transform(Vector3.UnitX, pitch);
            screenX = Vector3.Transform(screenX, yaw);
            screenY = Vector3.Transform(Vector3.UnitY, pitch);
            screenY = Vector3.Transform(screenY, yaw);
            // normalize the transformed vectors
            screenZ = Vector3.Normalize(screenZ);
            screenX = Vector3.Normalize(screenX);
            screenY = Vector3.Normalize(screenY);
            //recalculate the screen plane size
            horizontal = cameraWidth * screenX;
            vertical = cameraHeight * screenY;
            CalculatePlane();
        }
    }
}


