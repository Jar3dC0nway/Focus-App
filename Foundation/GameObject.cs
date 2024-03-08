using Focus_App.Foundation.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Foundation
{
    public class GameObject
    {
        public Renderable texture_renderable;
        public Vector2 position;
        public Vector2 scale;
        public float rotation;

        public GameObject()
        {
            texture_renderable = new Sprite(null);
            position = new Vector2();
            scale = new Vector2(1, 1);
            rotation = 0;
        }
        public GameObject T(Renderable renderable)
        {
            texture_renderable = renderable;
            return this;
        }
        public Renderable T() { return texture_renderable; }
        public GameObject P(float x, float y)
        {
            position = new Vector2(x, y);
            return this;
        }
        public Vector2 P() { return position; }
        public GameObject S(float x, float y)
        {
            scale = new Vector2(x, y);
            return this;
        }
        public Vector2 S() { return scale; }
        public GameObject R(float r)
        {
            rotation = r;
            return this;
        }
        public float R() { return rotation; }
    }
}
