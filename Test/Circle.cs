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
        //Define center and radius of circle
        public Vector2 center { get; set; }
        public float radius { get; set; }

        public Circle(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        //Check distance from point to center
        public bool contains_point(Vector2 point)
        {
            return (Vector2.Distance(point, center) <= radius);
        }

        //Check distance between centers of circles
        public bool intersects_circle(Circle other)
        {
            return (Vector2.Distance(other.center, center) < (other.radius - radius));
        }

        //Check all scenarios where rectangle can intersect a circle
        public bool intersects_rectangle(Rectangle rect)
        {
            float dist_x = Math.Abs(center.X - rect.X - rect.Width / 2);
            float dist_y = Math.Abs(center.Y - rect.Y - rect.Height / 2);

            if (dist_x > (rect.Width / 2 + radius)) { return false; }
            if (dist_y > (rect.Height / 2 + radius)) { return false; }

            if (dist_x <= (rect.Width / 2)) { return true; }
            if (dist_y <= (rect.Height / 2)) { return true; }

            var dx = dist_x - rect.Width / 2;
            var dy = dist_y - rect.Height / 2;
            return (dx * dx + dy * dy <= (radius * radius));
        }
    }
}
