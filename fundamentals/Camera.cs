using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOGR2023Template.fundamentals
{
    internal class Camera
    {
        Vector3 Location;

        Vector3 LookatDirection;

        Vector3 UpDirection;

        internal Camera()
        {
            Location = new Vector3(0, 0, 0);
            LookatDirection = new Vector3(0, 0, 1);
            UpDirection = new Vector3(0, 1, 0);
        }
    }
}
