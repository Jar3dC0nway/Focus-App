using Focus_App.Foundation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.Utilities.Deflate;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App
{
    public class Stario : GameObject
    {
        float points = 0;

        private Vector2 GetDistance(Game1 caller)
        {
            Vector2 WindowSize = caller.GetWindowSize();
            return (Mouse.GetState().Position.ToVector2() - new Vector2(WindowSize.X / 2, WindowSize.Y / 2)) * 800 / WindowSize.Y - position;
        }

        public override void Tick(GameTime gameTime, Game1 caller)
        {
            if (GetDistance(caller).Length() < 80)
            {
                points++;
            }
            else {
                points--;
            }
            //Debug.WriteLine(points);
            //position = new Vector2(MathF.Cos((float)(gameTime.TotalGameTime.TotalSeconds)) * 100f, MathF.Sin((float)(gameTime.TotalGameTime.TotalSeconds) * 2) * 10f);

            position = LevelReader.GetPosition(gameTime);

            base.Tick(gameTime, caller);
        }

        public float GetPoints() {
            return points;
        }
    }
}
