using Focus_App.Scripts.Foundation.Objects;
using Focus_App.Scripts.Foundation.Rendering.UI;
using Focus_App.Scripts.Foundation.Rendering.UI.Button;
using Focus_App.Scripts.Foundation.Rendering.UI.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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

        private void _DrawText(TextLabel text) {
            Vector2 offset = text.centered ? _PositionInScreenSpace(Game1.INSTANCE.font.MeasureString(text.text) * 0.5f) 
                - new Vector2(WindowSize.X / 2.0f, WindowSize.Y / 2.0f) : Vector2.Zero;

            Game1.INSTANCE.spriteBatch.DrawString(Game1.INSTANCE.font, text.text,
                _GetGameObjectLocation(text) - offset,
                    text.textColor, 0, new Vector2(), Game1.INSTANCE.Window.ClientBounds.Height / 800f, SpriteEffects.None, 0);
        }

        private void _DrawTextBox(TextBox textBox, GameTime gameTime)
        {
            _DrawGameObject(textBox.S(textBox.S() + Vector2.One * 0.1f), gameTime, textBox.selected ? Color.LightCoral : textBox.boxColor);
            _DrawGameObject(textBox.S(textBox.S() - Vector2.One * 0.1f), gameTime, Color.White);

            _DrawText(textBox.label);
        }

        private void _DrawButton(Button button, GameTime gameTime) {
            button.S(Game1.INSTANCE.font.MeasureString(button.text) * 0.016f + Vector2.One * 0.3f);
            _DrawGameObject(button, gameTime, button.color);

            _DrawText(button.label);
        }

        private void _DrawUI(UIElement uIElement, GameTime gameTime) {
            switch (uIElement) {
                case TextLabel e:
                    _DrawText(e);
                    break;
                case TextBox e:
                    _DrawTextBox(e, gameTime);
                    break;
                case Button e:
                    _DrawButton(e, gameTime);
                    break;
                case UIPanel e:
                    _DrawGameObject(e, gameTime, e.color);
                    break;
            }
        }

        private void _DrawGameObject(GameObject gameObject, GameTime gameTime, Color color) {
            if (!gameObject.isShown) return;
            if (gameObject.T() is Animation)
            {
                ((Animation)gameObject.T()).Tick(gameTime);
            }
            else if (gameObject.T() is AnimationCollection)
            {
                ((AnimationCollection)gameObject.T()).GetAnimation().Tick(gameTime);
            }
            Texture2D texture = gameObject.T().GetTexture();
            var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            spriteBatch.Draw(gameObject.T().GetTexture(),
                _GetGameObjectLocationAndScale(gameObject),
                null,
                color, gameObject.rotation, origin, SpriteEffects.None, 0);
        }

        public void Draw(List<GameObject> gameObjects, GameTime gameTime)
        {
            spriteBatch.Begin();

            foreach (GameObject gameObject in gameObjects)
            {
                if (!gameObject.isShown)
                    continue;
                if (gameObject is UIElement)
                {
                    _DrawUI((UIElement)gameObject, gameTime);
                }
                else { 
                    _DrawGameObject(gameObject, gameTime, Color.White);
                }
            }
            //spriteBatch.Draw();
            spriteBatch.End();
        }

        public void SetWindowSize(Vector2 size)
        {
            WindowSize = size;
        }

        private Vector2 _PositionInScreenSpace(Vector2 position) {
            return new Vector2(
                (int)(position.X * WindowSize.Y / SCREEN_SCALE + WindowSize.X / 2),
                (int)(position.Y * WindowSize.Y / SCREEN_SCALE + WindowSize.Y / 2)
                );
        }

        private Vector2 _GetGameObjectLocation(GameObject gameObject) {
            return _PositionInScreenSpace(gameObject.position);
        }

        private Rectangle _GetGameObjectLocationAndScale(GameObject gameObject)
        {
            Texture2D texture = gameObject.T().GetTexture();
            Vector2 position = _PositionInScreenSpace(gameObject.position);
            return new Rectangle(
                (int)(position.X),
                (int)(position.Y),
                (int)(gameObject.scale.X * texture.Width * WindowSize.Y / SCREEN_SCALE),
                (int)(gameObject.scale.Y * texture.Height * WindowSize.Y / SCREEN_SCALE));
        }
    }
}
