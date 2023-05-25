using System.Numerics;

class Sphere : Primitive
{
    Vector3 location;
    float radius;
    Vector3 color;

    public Vector3 Color { get { return color; } }

    public Vector3 Location
    {
        get { return location; }
        private set { location = value; }
    }


    public Sphere(Vector3 location, float radius, Vector3 color)
    {
        this.location = location;
        this.radius = radius;
        this.color = color;
        
    }
    public override bool Collision(Ray ray)
    {
        Vector3 CenterOrigin = ray.Origin - location;
        float a = Vector3.Dot(ray.Direction,ray.Direction);
        float b = 2 * Vector3.Dot(CenterOrigin,ray.Direction);
        float c = Vector3.Dot(CenterOrigin, CenterOrigin) - (radius * radius);
        double D = b * b - 4 * a * c;
        return D > 0;
    }
}
