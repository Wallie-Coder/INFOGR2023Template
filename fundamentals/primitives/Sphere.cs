using System.Numerics;

class Sphere : Primitive
{
    Vector3 location;
    float radius;
    

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
    public override ValueTuple<bool, float> Collision(Ray ray)
    {
        Vector3 CenterOrigin = ray.Origin - location;
        float a = Vector3.Dot(ray.Direction,ray.Direction);
        float b = 2 * Vector3.Dot(CenterOrigin,ray.Direction);
        float c = Vector3.Dot(CenterOrigin, CenterOrigin) - (radius * radius);
        double D = b * b - 4 * a * c;
        float p2 = (-b - (float)Math.Sqrt(D)) / 2 * a;
        float p1 = (-b + (float)Math.Sqrt(D)) / 2 * a;
        float t;
        if (p1 < p2) t = p1;
        else t = p2;
        return (D > 0, t);
    }

    public override Vector3 OutsideNormal(Vector3 point)
    {
        return Vector3.Normalize(location - point);
    }
}
