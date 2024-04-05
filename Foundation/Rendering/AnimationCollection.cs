using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Foundation.Rendering
{
    public class AnimationCollection : Renderable
    {
        List<Animation> animations;
        int currentAnimation = 0;
        public AnimationCollection(List<Animation> animations) { 
            this.animations = animations;
        }
        public override Texture2D GetTexture()
        {
            return animations[currentAnimation].GetTexture();
        }
        public void SetAnimation(int animation) {
            if (currentAnimation == animation) return;
            animations[currentAnimation].SetFrame(0);
            currentAnimation = animation;
        }
        public Animation GetAnimation() { return animations[currentAnimation]; }
    }
}
