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
    public override float Collision()
    {
        // calculates the smalles intersection point t an returns this.
        // dont know what happens if there is no intersection.

        base.Collision();

        Vector3 rayDirection = new Vector3(1,0,0);
        Vector3 rayOrigin = new Vector3(-2, 0, 0);

        float dx = rayDirection.X;
        float dy = rayDirection.Y;
        float dz = rayDirection.Z;

        float Ex = rayOrigin.X;
        float Ey = rayOrigin.Y;
        float Ez = rayOrigin.Z;

        float X0 = location.X;
        float Y0 = location.Y;
        float Z0 = location.Z;

        float r = radius;

        float a = dx + dy + dz;
        float b = Ex - X0 + Ey - Y0 + Ez - Z0;
        float c = 2 * Ex * X0 + (X0 * X0) + (Ex * Ex) + 2 * Ey * Y0 + (Y0 * Y0) + (Ey * Ey) + 2 * Ez * Z0 + (Z0 * Z0) + (Ez * Ez) - 3 * (r * r);

        float t1 = 0;
        float t2 = 0;

        try
        {
            t1 = -b + (float)Math.Sqrt((double)((b * b) + 4 * a * c)) / 2 * a;
        }
        catch
        {
            t1 = 0;
        }

        try
        {
            t2 = -b + (float)Math.Sqrt((double)((b * b) + 4 * a * c)) / 2 * a;
        }
        catch
        {
            t2 = 0;
        }

        if(t1 < t2)
        {
            return t1;
        }
        else
        {
            return t2;
        }

    }
}
