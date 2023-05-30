using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using Template;
namespace RAYTRACER
{
    public class Camera
    {
        // member variables
        Vector3 origin;

        float sensitivity = 0.2f;
        float pitch = 0;
        float yaw = 0;
        float rotationSpeed = 1f;

        public float Sensitivity { get { return sensitivity; } }
        public float Pitch { get { return pitch; } set { pitch = value; } }
        public float Yaw { get { return yaw; } set { yaw = value; } }
        public float RotationSpeed { get { return rotationSpeed; } }

        public Vector3 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Vector3 vertical;

        public Vector3 horizontal;

        public Vector3 Forward { get { return Vector3.Normalize(screenZ); } }
        public Vector3 Backward { get { return Vector3.Normalize(-screenZ); } }
        public Vector3 Left { get { return Vector3.Normalize(-screenX); } }
        public Vector3 Right { get { return Vector3.Normalize(screenX); } }
        public Vector3 Up { get { return Vector3.Normalize(screenY); } }
        public Vector3 Down { get { return Vector3.Normalize(-screenY); } }



        float aspectRatio = (float)16 / 9;

        public float AspectRatio { get { return aspectRatio; } }

        public int screenWidth = 640;
        public int screenHeight = 360;

        public Vector3 planeCenter;

        float cameraHeight;
        float cameraWidth;
        float planeDistance = -1;

        public Vector3 screenZ, screenY, screenX;
        public float PlaneDistance
        {
            get { return planeDistance; }
        }

        Vector3 p0; // top right
        Vector3 p1; // top left
        Vector3 p2; // bottom right
        Vector3 p3; // bottom left

        public Vector3 TopRight { get { return p0; } }
        public Vector3 TopLeft { get { return p1; } }
        public Vector3 BottomRight { get { return p2; } }
        public Vector3 BottomLeft { get { return p3; } }

        Vector3 lookingAt;
        public Vector3 LookingAt { get { return lookingAt; } }
        //constructor
        public Camera(Vector3 lookFrom, Vector3 lookAt, Vector3 viewUp, float fov)
        {
            float theta = (float)Math.Tan(fov * (float)(Math.PI / 180) / 2);
            cameraHeight = theta * 2;
            cameraWidth = aspectRatio * cameraHeight;
            this.lookingAt = lookAt;
            CalculateBase(lookFrom, lookAt, viewUp);
            CalculatePlane();
            // set pitch and yaw according to the x y and z.
            pitch = (float)Math.Asin(screenZ.Y);
            yaw = (float)Math.Atan2(screenZ.X, screenZ.Z);
        }

        // Calculates the plane center and corners of the plane the camera shows.
        public void CalculatePlane()
        {
            planeCenter = origin + screenZ;
            p0 = origin + horizontal / 2 + vertical / 2 + screenZ;
            p1 = origin - horizontal / 2 + vertical / 2 + screenZ;
            p2 = origin + horizontal / 2 - vertical / 2 + screenZ;
            p3 = origin - horizontal / 2 - vertical / 2 + screenZ;
        }

        public Ray CalculateRay(int x, int y)
        {
            float v = (float)x / (screenWidth - 1);
            float u = (float)y / (screenHeight - 1);
            return new Ray(BottomLeft + v * horizontal + u * vertical - origin, origin);
        }

        public void CalculateBase(Vector3 lookFrom, Vector3 lookAt, Vector3 viewUp)
        {
            screenZ = Vector3.Normalize(lookAt - lookFrom);
            screenX = Vector3.Normalize(Vector3.Cross(viewUp, screenZ));
            screenY = Vector3.Normalize(Vector3.Cross(screenZ, screenX));
            origin = lookFrom;
            this.lookingAt = lookAt;
            horizontal = cameraWidth * screenX;
            vertical = cameraHeight * screenY;
        }

        public void LookAt(Vector3 direction)
        {
            
            screenZ = Vector3.Normalize(direction);
            screenX = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, screenZ));
            screenY = Vector3.Normalize(Vector3.Cross(screenZ, screenX));
            lookingAt = direction;
            horizontal = cameraWidth * screenX;
            vertical = cameraHeight * screenY;
            CalculatePlane();
        }

        public void LookFrom(Vector3 origin)
        {
            CalculateBase(origin, lookingAt, Vector3.UnitY);
            CalculatePlane();
        }
    }
}


