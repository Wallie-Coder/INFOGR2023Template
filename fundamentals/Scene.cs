using System.Numerics;
using Template;

namespace RAYTRACER
{
    public class Scene
    {
        // MEMBER VARIABLES
        private List<Light> lights; 
        public List<Light> Lights { get { return lights; } }


        List<Primitive> primitives;
        public List<Primitive> Primitives { get { return primitives; } }


        Vector3 ambientLightingIntensity = (new Vector3(15, 15, 15)) / 255;
        public Vector3 AmbientLightingIntensity { get { return ambientLightingIntensity; } }
        
        
        // CONSTRUCTOR
        public Scene()
        {
            lights = new List<Light>
            {
                new Light(new Vector3(0, 0, 0), new Vector3(20, 20, 20)),

                // 2 spotlights, 1 shinning on the red sphere top down, the other on brightly on the left wall.
                new Spotlight(new Vector3(-2, -5, 5), new Vector3(10,10,10), new Vector3(0, 1, 0), 20),
                new Spotlight(new Vector3(0, 0, 0), new Vector3(60,60,60), new Vector3(-1, 0, 0),5)

            };
            
            primitives = new List<Primitive>
            {
                // 3 spheres (1 mirror and 1 with texture)
                new Sphere(new Vector3(2, 0, 5), 1f, new Vector3(150, 150,150), new Vector3(150,150,150), true),
                new Sphere(new Vector3(4, 1, -2), 1f, new Vector3(0,255,0), new Vector3(0,255,0), false, Primitive.Textures.WeirdLines),
                new Sphere(new Vector3(-2, 0, 5), 1f, new Vector3(255,0,0), new Vector3(131,0,0)),
                
                // ceiling and floor plane with texture
                new Plane(new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(0, 4, 0), new Vector3 (0, 0, 0), new Vector3(0,0,255), texture:Primitive.Textures.Checkerboard),
                new Plane(new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, -4, 0), new Vector3 (0, 0, 0), new Vector3(0,0,255), false, Primitive.Textures.Checkerboard),
                
                // wall planes in different colors
                new Plane(new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(-8, 0, 0), new Vector3 (0, 0, 255),  new Vector3(0,0,0)),
                new Plane(new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(8, 0, 0), new Vector3 (255, 0, 0),  new Vector3(0,0,0)),
                new Plane(new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, -8), new Vector3 (0, 255, 0),  new Vector3(0,0,0)),
                new Plane(new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 8), new Vector3 (255, 255, 0),  new Vector3(0,0,0))
            };
        }
    }
}