using INFOGR2023Template.Fundamentals;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOGR2023Template.Fundamentals
{
    internal class Plane : Primitive
    {
        Vector3 Normal;

        float Distance;

        internal Plane(Vector3 normal, float distance)
        {
            this.Normal = normal;
            this.Distance = distance;
        }

        internal override void Collision()
        {
            base.Collision();
        }
    }
}
