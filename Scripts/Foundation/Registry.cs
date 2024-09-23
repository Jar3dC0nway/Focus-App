using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Scripts.Foundation
{
    public static class Registry
    {
        public static Texture2D BUTTON = _load("button");
        public static Texture2D CLOUD = _load("cloudsprite");


        static Texture2D _load(string path)
        {
            return Game1.INSTANCE.Content.Load<Texture2D>("Textures/" + path);
        }
    }
}
