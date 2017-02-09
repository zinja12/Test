using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class Camera
    {
        private Matrix transform;
        public Matrix Transform
        {
            get { return transform; }
        }

        private Vector2 center;
        private Viewport viewport;

        private float zoom = 1.0f;
        private float rotation = 0.0f;

        public float X
        {
            get { return center.X; }
            set { center.X = value; }
        }

        public float Y
        {
            get { return center.Y; }
            set { center.Y = value; }
        }

        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                if (zoom < 0.1f)
                    zoom = 0.1f;
            }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
        }

        public void Update(Vector2 position)
        {
            center = position;

            transform = Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0)) *
                                                 Matrix.CreateRotationZ(Rotation) *
                                                 Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                                 Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
        }
    }
}
