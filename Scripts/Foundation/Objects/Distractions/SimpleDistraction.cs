using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Scripts.Foundation.Objects.Distractions
{
    public class SimpleDistraction : Distraction
    {
        public float fadeProperty = 0;
        public float fadeStrength = 2.0f;
        private Vector2 baseScale = new Vector2(1, 1);

        public override void Tick(GameTime gameTime)
        {
            base.Tick(gameTime);
            float delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            float strength = fadeStrength * (isShown ? 4.0f : 0.75f);
            fadeProperty = (1.0f - delta * strength) * fadeProperty + (isShown ? delta * strength : 0.0f);
            scale = baseScale * fadeProperty;
            isShown = fadeProperty > 0.05;
        }
    }
}
