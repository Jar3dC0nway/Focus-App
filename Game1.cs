using Focus_App.Foundation;
using Focus_App.Foundation.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Focus_App
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Process process;

        bool RUN_CAMERA = false;

        Renderer renderer;

        GameObject Stario;
        string STARIO_TEXTURE = "stario";

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

            Stario = new GameObject().T(renderer.CreateSprite(STARIO_TEXTURE));

            // TODO: use this.Content to load your game content here
        }

        public void SetWindowSize(object sender, EventArgs e)
        {
            renderer.SetWindowSize(new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                process.CloseMainWindow();
                process?.Kill();
                Exit();
            }

            if (process != null) CameraAsync();

            //Debug.WriteLine(DateTime.Now + " " + process.StandardOutput.ReadLine());

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            renderer.Draw(new System.Collections.Generic.List<GameObject> { Stario });

            base.Draw(gameTime);
        }
    }
}