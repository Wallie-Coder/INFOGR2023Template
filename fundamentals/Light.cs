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
}