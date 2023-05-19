using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Plane : Primitive
{
    Vector3 Normal;

    float Distance;

    public Plane(Vector3 normal, float distance)
    {
        this.Normal = normal;
        this.Distance = distance;
    }

    public override float Collision()
    {
        return 0f;
    }
}

