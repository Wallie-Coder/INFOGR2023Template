using OpenTK.Mathematics;

class Sphere : Primitive
{
    Vector3 location;
    float radius;

    public Vector3 Location
    {
        get { return location; }
        private set { location = value; }
    }


    public Sphere(Vector3 location, float radius)
    {
        this.location = location;
        this.radius = radius;

        
    }
    public override float Collision(Ray ray)
    {
        // calculates the smallest t at which there is an intersection and returns this.

        float r = radius;

        float a = Vector3.Dot(ray.Direction,ray.Direction);
        float b = 2 * Vector3.Dot(ray.Direction, ray.Origin - location);
        float c = Vector3.Dot(ray.Origin - location, ray.Origin - location) - (r * r);

        float t1;
        float t2;


        double D = b * b - 4 * a * c; 
        if(D > 0)
        {
            t1 = -b + (float)Math.Sqrt(D) / (2 * a);
            t2 = -b - (float)Math.Sqrt(D) / (2 * a);
            if (t1 < t2) return t1;
            else return t2;
        }
        else if(D == 0) 
        {
            t2 = -b / 2 * a;
            return t2;
        }
        else
        {
            return 0;
        }

    }
}
