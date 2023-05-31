using System.Numerics;

namespace RAYTRACER
{
    public class Plane : Primitive
    {
        // MEMBER VARIABLES
        private Vector3 normal;


        private float distance;


        // CONSTRUCTOR
        public Plane(Vector3 normal, float distance) :base(new Vector3(0,0,0), new Vector3(0,0,0))
        {
            this.normal = normal;
            this.distance = distance;
        }
        
        // CLASS METHODS

        public override ValueTuple<double, float, float> Collision(Ray ray)
        {
            return (0, 0, 0);
        }
    }
}

