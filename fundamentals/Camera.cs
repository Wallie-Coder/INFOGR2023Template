using OpenTK.Mathematics;

class Camera
{
    Vector3 location;

    Vector3 lookatDirection;

    Vector3 upDirection;

    internal Camera()
    {
        location = new Vector3(0, 0, 0);
        lookatDirection = new Vector3(0, 0, 1);
        upDirection = new Vector3(0, 1, 0);
    }
}

