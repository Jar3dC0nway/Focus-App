using Focus_App.Foundation.Rendering;
using Focus_App.Scripts.Foundation.Objects;
using Focus_App.Scripts.Foundation.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Scripts.Foundation.Rendering.UI.Text
{
    public class TextBox : UIElement
    {
        public TextLabel label = new TextLabel();
        private String _text;
        public String text {
            get { return _text; }
            set { _SetText(value); }
        }
        public String defaultText = "";
        public Color boxColor = Color.White;
        public bool pressedDown = false;

        public bool selected = false;

        public InputTypes type = InputTypes.All;

        public TextBox()
        {
            texture_renderable = new Sprite(Registry.BUTTON);
            label.P(position);
            label.centered = true;
        }

        public override GameObject P(float x, float y)
        {
            return P(new Vector2(x, y));
        }
        public override GameObject P(Vector2 other)
        {
            label.P(other);
            return base.P(other);
        }

        private void _SetText(String value) {
            _text = value;
            if (value == null || value.Equals("")) {
                label.text = defaultText;
                label.textColor = Color.LightGray;
            }
            else
            {
                label.text = value;
                label.textColor = Color.Black;
            }
        }

        private void _AddChar(String character) { 
            _text += character;
            _SetText(_text);
        }
        private void _RemoveChar() {
            if (_text.Length > 0)
                _text = _text.Substring(0, _text.Length - 1);
            _SetText(_text);
        }
        private void _Negate() {
            if (_text.Length == 0) _text = "-";
            else if (_text.StartsWith("-")) _text = _text.Substring(1);
            else _text = "-" + _text;
            _SetText(_text);
        }

        private void _SetSelected() { 
            selected = true;
        }

        private void _SetUnselected() { 
            selected = false;
        }

        public override void Tick(GameTime gameTime)
        {
            (pressedDown, boxColor) = UIInteractable.HandleInteractionsOfGameObject(this, _SetSelected, _SetUnselected, pressedDown, boxColor);

            if (selected) {
                KeyInput[] input = Input.GetInput();
                foreach (KeyInput i in input) {
                    if (Input.IsKeyJustPressed(i.Key)) {
                        switch (i.Key)
                        {
                            case Keys.Back:
                                _RemoveChar();
                                break;
                            case (>= Keys.D0 and <= Keys.D9):
                                if (type != InputTypes.Alpha) _AddChar("" + ((int)i.Key - 48));
                                break;
                            case (>= Keys.NumPad0 and <= Keys.NumPad9):
                                if (type != InputTypes.Alpha) _AddChar("" + ((int)i.Key - 96));
                                break;
                            case Keys.Space:
                                if (type != InputTypes.Numeric) _AddChar("_");
                                break;
                            case Keys.OemMinus:
                                if (type != InputTypes.Alpha) _Negate();
                                break;
                            default:
                                if (type != InputTypes.Numeric) _AddChar(i.Key.ToString().ToLower());
                                break;
                        }
                    }
                }
            }

            base.Tick(gameTime);
        }
    }
}
