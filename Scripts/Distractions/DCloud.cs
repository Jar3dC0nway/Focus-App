using Focus_App.Scripts.Foundation;
using Focus_App.Scripts.Foundation.Objects;
using Focus_App.Scripts.Foundation.Objects.Distractions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Scripts.Distractions
{
    public class DCloud : SimpleDistraction
    {
        float trajectory = 0.0f;
        Vector2? basePos = null;
        public DCloud() { 
            timeLength = 3;
            T(Registry.CLOUD);
            setNewTrajectory();
        }

        public override GameObject P(float x, float y)
        {
            return P(new Vector2(x, y));
        }
        public override GameObject P(Vector2 other)
        {
            basePos = other;
            return base.P(other);
        }

        public override void Tick(GameTime gameTime)
        {
            base.Tick(gameTime);
            if (!isShown)
            {
                position = basePos.Value;
                setNewTrajectory();
            }
            float sec = ((float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f));

            position = new Vector2(P().X + (trajectory < 0.5 ? -1f : 1f) * sec * 20.0f, P().Y);
        }

        private void setNewTrajectory() {
            trajectory = ((float)new Random().NextDouble());
        }
    }
}
