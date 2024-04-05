using Focus_App.Foundation;
using Focus_App.Foundation.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Focus_App
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        Process process;

        bool RUN_CAMERA = false;

        Renderer renderer;

        Stario stario;
        string STARIO_TEXTURE = "Textures/stario",
               STARIO_BLINK = "Textures/stario_blink",
               STARIO_TALK = "Textures/stario_talk",
               STARIO_TRANSFORM = "Textures/stario_transform",
               STARIO_RING = "Textures/stario_ring";
        GameObject Textbox;
        string TEXTBOX = "Textures/textbox";

        SoundEffect[] talk;

        Song song;

        List<GameObject> gameObjects = new List<GameObject>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            if (RUN_CAMERA)
            {
                // TODO: Add your initialization logic here
                //https://stackoverflow.com/questions/4291912/process-start-how-to-get-the-output
                //Cited to figure out structure of process and the argument field
                process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "example.exe",
                        Arguments = "/c DIR",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
            }
            //process.BeginOutputReadLine();
            base.Initialize();
        }

        private async Task CameraAsync() {
            await Task.Run(() =>
            {
                Debug.WriteLine(DateTime.Now + " " + process.StandardOutput.ReadLine());
            });
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            renderer = new Renderer(Content, _spriteBatch);
            Window.ClientSizeChanged += new EventHandler<EventArgs>(SetWindowSize);
            SetWindowSize(null, null);

            stario = new Stario().T(new AnimationCollection(new List<Animation> {
                renderer.CreateAnimation(new List<string> { STARIO_TEXTURE, STARIO_BLINK }, new List<float> { 1.9f, 0.1f }),
                renderer.CreateAnimation(new List<string> { STARIO_TEXTURE, STARIO_TALK }, new List<float> { 0.15f, 0.15f }),
                renderer.CreateAnimation(new List<string> { STARIO_TRANSFORM, STARIO_RING }, new List<float> { 0.10f })
            })) as Stario;
            Textbox = new GameObject().T(renderer.CreateSprite(TEXTBOX)).P(200, -200);

            gameObjects.Add(stario);
            gameObjects.Add(Textbox);

            talk = new SoundEffect[] { Content.Load<SoundEffect>("Sound/talk_new") };//, Content.Load<SoundEffect>("Sound/speak2"), Content.Load<SoundEffect>("Sound/speak3") };
            _font = Content.Load<SpriteFont>("Textures/Font");

            song = Content.Load<Song>("Sound/Music/focus");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);


            LevelReader.SetLevel("tutorial");

            // TODO: use this.Content to load your game content here
        }

        public void SetWindowSize(object sender, EventArgs e)
        {
            renderer.SetWindowSize(new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height));
        }
        public Vector2 GetWindowSize() {
            return new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
        }

        float speakTimer = 0;
        bool talking = false;
        bool ringMode = false;
        bool switchButton = false;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                process?.CloseMainWindow();
                process?.Kill();
                Exit();
            }

            if (process != null) CameraAsync();

            //Debug.WriteLine(DateTime.Now + " " + process.StandardOutput.ReadLine());

            // TODO: Add your update logic here

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                talking = true;
                speakTimer -= gameTime.ElapsedGameTime.Milliseconds/1000f;
                if (speakTimer <= 0)
                {
                    speakTimer = 0.15f;
                    talk[new Random().Next(talk.Length)].Play(1, (float)(new Random().NextDouble()), 1);
                }
                Textbox.isShown = true;
            }
            else {
                talking = false;
                speakTimer = 0;

                Textbox.isShown = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A) != switchButton) {
                if (switchButton) ringMode = !ringMode;
                switchButton = !switchButton;
            }
            (stario.T() as AnimationCollection).SetAnimation(ringMode ? 2 : talking ? 1 : 0);

            Textbox.position = new Vector2(200, -200) + stario.position;
            textLoc = new Vector2(80, -300) + stario.position;

            foreach (GameObject gameObject in gameObjects) {
                gameObject.Tick(gameTime, this);
            }

            base.Update(gameTime);
        }

        Vector2 textLoc = new Vector2(80, -300);

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            renderer.Draw(gameObjects, gameTime);

            //TODO: Make text a gameobject as well
            _spriteBatch.Begin();
            if (Textbox.isShown) { 
            _spriteBatch.DrawString(_font, "Points: " + stario.GetPoints(),
                new Vector2(
                    textLoc.X * Window.ClientBounds.Height / 800 + Window.ClientBounds.Width / 2,
                    textLoc.Y * Window.ClientBounds.Height / 800 + Window.ClientBounds.Height / 2),
                Color.Black, 0, new Vector2(), Window.ClientBounds.Height / 800f, SpriteEffects.None, 0);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}