using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Foundation.Rendering
{
    public class Renderer
    {
        ContentManager contentManager;
        SpriteBatch spriteBatch;
        Vector2 WindowSize = new Vector2();
        private int SCREEN_SCALE = 10;


        public Renderer(ContentManager content, SpriteBatch spriteBatch)
        {
            contentManager = content;
            this.spriteBatch = spriteBatch;
        }
        public Sprite CreateSprite(string path)
        {
            Texture2D texture = contentManager.Load<Texture2D>(path);
            Sprite sprite = new Sprite(texture);
            return sprite;
        }
        public void Draw(List<GameObject> gameObjects)
        {
            spriteBatch.Begin();

            foreach (GameObject gameObject in gameObjects)
            {
                Texture2D texture = gameObject.T().GetTexture();
                var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
                spriteBatch.Draw(gameObject.T().GetTexture(),
                    _GetGameObjectLocationAndScale(gameObject),
                    null,
                    Color.White, gameObject.rotation, origin, SpriteEffects.None, 0);
            }
            //spriteBatch.Draw();
            spriteBatch.End();
        }

        public void SetWindowSize(Vector2 size)
        {
            WindowSize = size;
        }

        private Rectangle _GetGameObjectLocationAndScale(GameObject gameObject)
        {
            return new Rectangle(
                (int)(gameObject.position.X + WindowSize.X / 2),
                (int)(gameObject.position.Y + WindowSize.Y / 2),
                (int)gameObject.scale.X * (int)WindowSize.Y / SCREEN_SCALE,
                (int)gameObject.scale.Y * (int)WindowSize.Y / SCREEN_SCALE);
        }
    }
}
