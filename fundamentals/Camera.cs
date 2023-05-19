﻿using OpenTK.Mathematics;
using Template;

class Camera
{
    // member variables
    Vector3 location;

    public Vector3 Location
    {
        get { return location; }
        private set { location = value; }
    }

    public Vector3 forwardDirection;

    public Vector3 upDirection;

    public Vector3 rightDirection;

    public Vector3 planeCenter;

    

    float cameraHeight = 2.0f;
    float cameraWidth;
    float planeDistance = 1;

    public float PlaneDistance
    {
        get { return planeDistance; }
    }

    Vector3 p0; // top right
    Vector3 p1; // top left
    Vector3 p2; // bottom right
    public Vector3 p3; // bottom left

    float aspectRatio;

    //constructor
    public Camera(Surface screen)
    {
        aspectRatio = screen.width / screen.height;
        cameraWidth = aspectRatio * screen.width;
        location = new Vector3(0, 0, 0);
        forwardDirection = new Vector3(0, 0, -1);
        upDirection = new Vector3(0, 1, 0);
        rightDirection = new Vector3(1, 0, 0);
        CalculatePlane();
    }

    // Calculates the plane center and corners of the plane the camera shows.
    void CalculatePlane()
    {
        planeCenter = planeDistance * forwardDirection;
        p0 = planeCenter + upDirection - rightDirection * aspectRatio;
        p1 = planeCenter + upDirection + rightDirection * aspectRatio;
        p2 = planeCenter - upDirection - rightDirection * aspectRatio;
        p3 = planeCenter - upDirection + rightDirection * aspectRatio;
    }

    Vector2 PointOnPlane(float a, float b)
    {
        Vector3 u = p1 - p0;
        Vector3 v = p2 - p0;
        Vector3 temp = p0 + aspectRatio * u + aspectRatio * v;
        return new Vector2(temp.X, temp.Y);
    }
}

