using Focus_App.Scripts.Foundation.Objects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Scripts.Foundation.Objects.Distractions
{
    public class Distraction : GameObject
    {
        public int timeLength = 0;
        public int timeStart = 0;

        public static int levelLength = 10;

        public override void Tick(GameTime gameTime)
        {
            float sec = gameTime.TotalGameTime.Seconds % levelLength;
            isShown = sec - timeStart > 0 && sec - timeStart < timeLength;
            base.Tick(gameTime);
        }
    }
}
