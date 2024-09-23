using Focus_App.Foundation;
using Focus_App.Foundation.Rendering;
using Focus_App.Scripts.Foundation.Objects;
using Focus_App.Scripts.Foundation.Rendering.UI.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Focus_App.Scripts
{
    public class MainScene : Scene
    {
        Stario stario;
        GameObject Textbox;
        GameObject[] distractionObjs = new GameObject[0];
        TextLabel TextLabel;
        string TEXTBOX = "Textures/textbox";
        Vector2 textLoc = new Vector2(80, -300);

        SoundEffect[] talk;

        Song song;

        float speakTimer = 0;
        bool talking = false;
        bool ringMode = true;
        bool switchButton = false;

        public override void Draw(GameTime gameTime)
        {
            //TODO: Make text a gameobject as well
        }

        public override void LoadScene()
        {
            Renderer renderer = Game1.INSTANCE.renderer;
            ContentManager Content = Game1.INSTANCE.Content;
            stario = new Stario();
            Textbox = new GameObject().T(renderer.CreateSprite(TEXTBOX)).P(200, -200);
            TextLabel = new TextLabel().P(Textbox.P()) as TextLabel;
            gameObjects.Add(stario);
            gameObjects.Add(Textbox);
            gameObjects.Add(TextLabel);
            talk = new SoundEffect[] { Content.Load<SoundEffect>("Sound/speak1"), Content.Load<SoundEffect>("Sound/speak2"), Content.Load<SoundEffect>("Sound/speak3") };//, Content.Load<SoundEffect>("Sound/speak2"), Content.Load<SoundEffect>("Sound/speak3") };
            song = Content.Load<Song>("Sound/Music/focus");
            MediaPlayer.IsRepeating = true;
            distractionObjs = LevelReader.GetDistractionsAsGameObjects().ToArray();
            foreach (var obj in distractionObjs) { gameObjects.Add(obj); }
            Debug.WriteLine(LevelReader.GetDistractions().Count);
            //MediaPlayer.Play(song);
            //LevelReader.SetLevel("level1");
        }
        public override void Start()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                talking = true;
                speakTimer -= gameTime.ElapsedGameTime.Milliseconds / 1000f;
                if (speakTimer <= 0)
                {
                    speakTimer = 0.18f;
                    talk[new Random().Next(talk.Length)].Play(1, (float)(new Random().NextDouble()), 1);
                }
                Textbox.isShown = true;
                TextLabel.isShown = true;
            }
            else
            {
                talking = false;
                speakTimer = 0;

                Textbox.isShown = false;
                TextLabel.isShown = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A) != switchButton)
            {
                if (switchButton) ringMode = !ringMode;
                switchButton = !switchButton;
            }
            (stario.T() as AnimationCollection).SetAnimation(ringMode ? 2 : talking ? 1 : 0);

            Textbox.position = new Vector2(200, -200) + stario.position;
            TextLabel.position = new Vector2(80, -300) + stario.position;
            TextLabel.text = "Points: " + stario.GetPoints();


            for (int i = 0; i < distractionObjs.Length; i++)
            {
                //distractionObjs[i].isShown = (distractions[i] < (gameTime.ElapsedGameTime.TotalSeconds % 5));
            }
        }
    }
}
