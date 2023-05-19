using OpenTK.Mathematics;
using System;

class Light
{
    Vector3 position;
    Vector3 color;
    int intensity;

    public Light(Vector3 position, Vector3 color, int intensity)
    {
        this.position = position;
        this.color = color;
        this.intensity = intensity;
    }
}