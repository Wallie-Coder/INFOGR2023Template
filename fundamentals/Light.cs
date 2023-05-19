using OpenTK.Mathematics;
using System;

class Light
{
    // position of the pointlight
    Vector3 position;
    // intensity of the light
    Vector3 intensity;

    public Light(Vector3 position, Vector3 intensity)
    {
        this.position = position;
        this.intensity = intensity;
    }
}