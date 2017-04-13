using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class Circle
    {
        public Vector2 center { get; set; }
        public float radius { get; set; }

        public Circle(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public bool contains_point(Vector2 point)
        {
            return (Vector2.Distance(point, center) <= radius);
        }

        public bool intersects_circle(Circle other)
        {
            return (Vector2.Distance(other.center, center) < (other.radius - radius));
        }
    }
}
