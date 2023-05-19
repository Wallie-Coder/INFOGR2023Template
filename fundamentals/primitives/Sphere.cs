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
}
