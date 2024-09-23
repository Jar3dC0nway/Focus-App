using Focus_App.Foundation.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Scripts.Foundation.Rendering.UI
{
    public class UIPanel : UIElement
    {
        public Color color = Color.White;

        public UIPanel()
        {
            texture_renderable = new Sprite(Registry.BUTTON);
        }
    }
}
