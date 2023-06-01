using System.Numerics;

namespace RAYTRACER
{
    public class Light
    {
        // MEMBER VARIABLES
        Vector3 location, intensity;
        public Vector3 Location { get { return location; } }
        public Vector3 Intensity { get { return intensity; } }


        // CONSTRUCTOR
        public Light(Vector3 location, Vector3 intensity)
        {
            this.location = location;
            this.intensity = intensity;
        }
    }

    public class Spotlight : Light
    {
        // MEMBER VARIABLES
        private Vector3 centerDirection;
        private float cosAngle;
        public Spotlight(Vector3 location, Vector3 intensity, Vector3 centerDirection, int openingAngle) : base(location, intensity)
        {
            this.centerDirection = Vector3.Normalize(centerDirection);
            cosAngle = (float)Math.Cos(openingAngle * (Math.PI / 180));
        }

        public bool RayInSpotlight(Ray ray)
        {
            float rayCos = Vector3.Dot(ray.Direction, centerDirection);
            if (rayCos < 0 - cosAngle || rayCos > 0 + cosAngle)
            {
                return false;
            }
            return true;
        }
    }
}