using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Foundation.Rendering
{
    public class Sprite : Renderable
    {
        Texture2D texture2D;
        public Sprite(Texture2D _texture) => texture2D = _texture;

        public override Texture2D GetTexture()
        {
            return texture2D;
        }
    }
}
