using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Scripts.Foundation
{
    public static class Registry
    {
        public static Texture2D BUTTON = _load("button");
        public static Texture2D CLOUD = _load("cloudsprite");
        public static Texture2D RETICLE = _load("reticle");
        public static String LEADERBOARD_URL = "https://focus-app-leaderboard-default-rtdb.firebaseio.com/";
        public static readonly HttpClient CLIENT = new HttpClient();

        static Texture2D _load(string path)
        {
            return Game1.INSTANCE.Content.Load<Texture2D>("Textures/" + path);
        }
    }
}
