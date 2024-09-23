using Focus_App.Foundation.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Scripts.Foundation.Objects
{
    public class GameObject
    {
        public Renderable texture_renderable;
        public Vector2 position;
        public Vector2 scale;
        public float rotation;

        public bool isShown = true;
        public bool markRemoved = false;

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
        public GameObject T(Texture2D texture)
        {
            texture_renderable = new Sprite(texture);
            return this;
        }
        public Renderable T() { return texture_renderable; }
        public virtual GameObject P(float x, float y)
        {
            position = new Vector2(x, y);
            return this;
        }
        public virtual GameObject P(Vector2 other)
        {
            position = other;
            return this;
        }
        public virtual Vector2 P() { return position; }
        public GameObject S(float x, float y)
        {
            scale = new Vector2(x, y);
            return this;
        }
        public GameObject S(Vector2 other)
        {
            scale = other;
            return this;
        }
        public Vector2 S() { return scale; }
        public GameObject R(float r)
        {
            rotation = r;
            return this;
        }
        public float R() { return rotation; }

        public virtual void Tick(GameTime gameTime)
        {

        }
    }
}
