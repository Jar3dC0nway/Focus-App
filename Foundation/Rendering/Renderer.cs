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
        private int SCREEN_SCALE = 800;


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
        public Animation CreateAnimation(List<string> paths, List<float> times) {
            List<Sprite> sprites = new List<Sprite>();
            for (int i = 0; i < paths.Count; i++) sprites.Add(CreateSprite(paths[i]));
            return new Animation().S(sprites).T(times);
        }
        public void Draw(List<GameObject> gameObjects, GameTime gameTime)
        {
            spriteBatch.Begin();

            foreach (GameObject gameObject in gameObjects)
            {
                if (!gameObject.isShown)
                    continue;
                if (gameObject.T() is Animation) {
                    (gameObject.T() as Animation).Tick(gameTime);
                }
                else if (gameObject.T() is AnimationCollection) {
                    (gameObject.T() as AnimationCollection).GetAnimation().Tick(gameTime);
                }
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
            Texture2D texture = gameObject.T().GetTexture();
            return new Rectangle(
                (int)(gameObject.position.X * WindowSize.Y / SCREEN_SCALE + WindowSize.X / 2),
                (int)(gameObject.position.Y * WindowSize.Y / SCREEN_SCALE + WindowSize.Y / 2),
                (int)(gameObject.scale.X * texture.Width * WindowSize.Y / SCREEN_SCALE),
                (int)(gameObject.scale.Y * texture.Height * WindowSize.Y / SCREEN_SCALE));
        }
    }
}
