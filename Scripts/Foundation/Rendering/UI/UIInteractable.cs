using Focus_App.Scripts.Foundation.Objects;
using Focus_App.Scripts.Foundation.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Scripts.Foundation.Rendering.UI
{
    public static class UIInteractable
    {
        private static bool isInBounds(Vector2 position, Vector2 scale, Vector2 other)
        {
            return other.X < position.X + scale.X * 32.0f && other.X > position.X - scale.X * 32.0f
                && other.Y < position.Y + scale.Y * 32.0f && other.Y > position.Y - scale.Y * 32.0f;
        }

        public static (bool, Color) HandleInteractionsOfGameObject(GameObject gameObject, Action activate, Action deactivate, bool pressedDown, Color color) {
            Vector2 mousePosition = Game1.INSTANCE.GetMousePosition();
            if (isInBounds(gameObject.position, gameObject.scale, mousePosition))
            {
                if ((pressedDown && Input.IsMouseClicked(MouseKeys.Left)) || Input.IsMouseJustClicked(MouseKeys.Left))
                {
                    color = Color.Lerp(color, new Color(0.5f, 0.3f, 0.7f), 0.5f);
                    pressedDown = true;
                }
                else
                {
                    color = Color.Lerp(color, new Color(0.3f, 0.3f, 0.6f), 0.5f);
                    if (pressedDown)
                    {
                        activate.Invoke();
                    }
                }
            }
            else
            {
                color = Color.Lerp(color, new Color(0.5f, 0.8f, 0.9f), 0.5f);

                if (Input.IsMouseClicked(MouseKeys.Left)) {
                    deactivate?.Invoke();
                }
            }
            if (!Input.IsMouseClicked(MouseKeys.Left)) pressedDown = false;

            return (pressedDown, color);
        }
    }
}
