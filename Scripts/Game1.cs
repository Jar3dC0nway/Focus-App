using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Focus_App.Foundation.Rendering;
using Focus_App.Foundation;
using Focus_App.Scripts;
using Focus_App.Scripts.Foundation.Utility;
using Focus_App.Scripts.Foundation.Rendering.UI;
using Focus_App.Scripts.Foundation.Objects;

namespace Focus_App
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public SpriteBatch spriteBatch;

        Process process;

        bool RUN_CAMERA = false;
        public Renderer renderer;
        Scene scene;
        public static Game1 INSTANCE;
        public SpriteFont font;

        bool sceneSwitched = false;

        private float lastTimeStamp;
        public static float deltaTime;

        private List<GameObject> blackBars = new List<GameObject>();
        private int[] barDimensions = { 21800, 16000};


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            INSTANCE = this;
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
            LevelReader.SetLevel(null); //Create level_data if not found
            spriteBatch = new SpriteBatch(GraphicsDevice);
            renderer = new Renderer(Content, spriteBatch);
            Window.ClientSizeChanged += new EventHandler<EventArgs>(SetWindowSize);
            SetWindowSize(null, null); 
            font = Content.Load<SpriteFont>("Textures/Font");

            scene = new MainMenu();

            for (int i = 0; i < 4; i++) {
                UIPanel bar = new UIPanel();
                bar.S(320, 320).P(barDimensions[0] * (i % 2) - barDimensions[0] / 2, barDimensions[1] * (int)(i / 2) - barDimensions[1] / 2);
                bar.color = new Color(0.15f, 0.1f, 0.3f);
                blackBars.Add(bar);
            }

            // TODO: use this.Content to load your game content here
        }

        public void SetWindowSize(object sender, EventArgs e)
        {
            renderer.SetWindowSize(new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height));
        }
        public Vector2 GetWindowSize() {
            return new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
        }

        public void Quit() {
            process?.CloseMainWindow();
            process?.Kill();
            Exit();
        }

        protected override void Update(GameTime gameTime)
        {
            Input.UpdateInput();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                scene = new MainMenu();
            }

            if (Input.IsKeyJustPressed(Keys.S))
            {
                //if (!(scene is LevelEditor))
                //    scene = (scene is MainScene) ? new LevelSelectScene() : new LevelEditor();
            }

            if (process != null) CameraAsync();

            //Debug.WriteLine(DateTime.Now + " " + process.StandardOutput.ReadLine());

            // TODO: Add your update logic here

            deltaTime = (float)(gameTime.ElapsedGameTime.TotalSeconds - lastTimeStamp);
            lastTimeStamp = (float)gameTime.ElapsedGameTime.TotalSeconds;


            GameObject[] objs = scene.gameObjects.ToArray();
            foreach (GameObject gameObject in objs)
            {
                gameObject.Tick(gameTime);
            }

            scene.Update(gameTime);

            for (int i = 0; i < scene.gameObjects.Count; i++) {
                if (scene.gameObjects[i].markRemoved)
                { 
                    scene.gameObjects.RemoveAt(i);
                    i -= 1;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            renderer.Draw(scene.gameObjects, gameTime);
            scene.Draw(gameTime);
            renderer.Draw(blackBars, gameTime);
            base.Draw(gameTime);
        }

        public Vector2 GetMousePosition()
        {
            Vector2 WindowSize = GetWindowSize();
            return (Mouse.GetState().Position.ToVector2() - new Vector2(WindowSize.X / 2, WindowSize.Y / 2)) * 800 / WindowSize.Y;
        }

        public void SwitchScene(Scene _scene) {
            scene = _scene;
        }
    }
}