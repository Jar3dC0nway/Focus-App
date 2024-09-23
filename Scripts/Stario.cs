﻿using Focus_App.Foundation;
using Focus_App.Foundation.Rendering;
using Focus_App.Scripts.Foundation.Objects;
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
        string STARIO_TEXTURE = "Textures/stario",
        STARIO_BLINK = "Textures/stario_blink",
        STARIO_TALK = "Textures/stario_talk",
        STARIO_TRANSFORM = "Textures/stario_transform",
        STARIO_RING = "Textures/stario_ring";

        float points = 0;

        public Stario() {
            Renderer renderer = Game1.INSTANCE.renderer;
            T(new AnimationCollection(new List<Animation> {
                renderer.CreateAnimation(new List<string> { STARIO_TEXTURE, STARIO_BLINK }, new List<float> { 1.9f, 0.1f }),
                renderer.CreateAnimation(new List<string> { STARIO_TEXTURE, STARIO_TALK }, new List<float> { 0.15f, 0.15f }),
                renderer.CreateAnimation(new List<string> { STARIO_TRANSFORM, STARIO_RING }, new List<float> { 0.10f })
            }));
        }

        private Vector2 GetDistance(Game1 caller)
        {
            Vector2 WindowSize = caller.GetWindowSize();
            return (Mouse.GetState().Position.ToVector2() - new Vector2(WindowSize.X / 2, WindowSize.Y / 2)) * 800 / WindowSize.Y - position;
        }

        public override void Tick(GameTime gameTime)
        {
            if (GetDistance(Game1.INSTANCE).Length() < 80)
            {
                points++;
            }
            else {
                points--;
            }
            //Debug.WriteLine(points);
            //position = new Vector2(MathF.Cos((float)(gameTime.TotalGameTime.TotalSeconds)) * 100f, MathF.Sin((float)(gameTime.TotalGameTime.TotalSeconds) * 2) * 10f);

            position = LevelReader.GetPosition(gameTime);

            base.Tick(gameTime);
        }

        public float GetPoints() {
            return points;
        }
    }
}