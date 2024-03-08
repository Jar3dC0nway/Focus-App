using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Foundation.Rendering
{
    public class Animation : Renderable
    {
        List<Sprite> sprites = new List<Sprite>();
        List<float> time_step = new List<float>();
        float time = 0;
        int frame = 0;
        public Animation()
        {

        }
        public Animation S(List<Sprite> sprites)
        {
            this.sprites = sprites;
            return this;
        }
        public Animation T(List<float> times)
        {
            time_step = times;
            return this;
        }

        public int Frame() { return frame; }
        public Sprite GetSprite(int index) { return sprites[index]; }

        public Sprite Tick(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            if (time >= time_step[frame])
            {
                frame = (frame + 1) % time_step.Count;
                time = 0;
            }
            return sprites[frame];
        }
        public override Texture2D GetTexture()
        {
            return sprites[frame].GetTexture();
        }
    }
}
