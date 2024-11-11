using Focus_App.Foundation.Rendering;
using Focus_App.Scripts.Foundation.Objects;
using Focus_App.Scripts.Foundation.Rendering.UI.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Scripts.Foundation.Rendering.UI.Button
{
    public class Button : UIElement
    {
        public TextLabel label = new TextLabel();
        public Color color = Color.White;
        public bool pressedDown = false;
        public bool click = false;
        public Action func = () => { };
        
        public String text
        {
            get { return label.text; }
            set { 
                label.text = value;
                S(Game1.INSTANCE.font.MeasureString(text) * 0.016f + Vector2.One * 0.3f);
            }
        }

        public Button() {
            texture_renderable = new Sprite(Registry.BUTTON);
            label.P(position);
            label.centered = true;
        }

        public override GameObject P(float x, float y) { 
            return P(new Vector2(x, y));
        }
        public override GameObject P(Vector2 other) {
            label.P(other);
            return base.P(other);
        }

        public override void Tick(GameTime gameTime)
        {
            (pressedDown, color) = UIInteractable.HandleInteractionsOfGameObject(this, func, null, pressedDown, color);

            base.Tick(gameTime);
        }
    }
}
