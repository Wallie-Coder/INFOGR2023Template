using System.Numerics;

namespace RAYTRACER
{
    public class Light
    {
        // position of the pointlight
        Vector3 location;
        // intensity of the light
        Vector3 intensity;

        public Vector3 Location { get { return location; } }

        public Light(Vector3 location, Vector3 intensity)
        {
            this.location = location;
            this.intensity = intensity;
        }
    }
}