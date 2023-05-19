using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOGR2023Template.fundamentals.primitives
{
    internal class Sphere : Primitive
    {
        Vector3 Location;
        float Radius;


        internal Sphere(Vector3 location, float radius)
        {
            this.Location = location;
            this.Radius = radius;
        }
    }
}
