using OpenTK.Mathematics;

class Sphere : Primitive
{
    Vector3 Location;
    float Radius;


    internal Sphere(Vector3 location, float radius)
    {
        this.Location = location;
        this.Radius = radius;
    }
}
