using Focus_App.Foundation;
using Focus_App.Scripts.Foundation.Rendering.UI.Button;
using Focus_App.Scripts.Foundation.Rendering.UI.Text;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Scripts
{
    public class MainMenu : Scene
    {
        public override void Draw(GameTime gameTime)
        {
        }

        public override void LoadScene()
        {
            TextLabel textLabel = new TextLabel();
            textLabel.P(0, -350);
            textLabel.centered = true;
            textLabel.text = "Unnamed Focus App";
            gameObjects.Add(textLabel);

            Button levelSelect = new Button();
            levelSelect.P(0, 0);
            levelSelect.text = "Level Select";
            levelSelect.func = () =>
            {
                Game1.INSTANCE.SwitchScene(new LevelSelectScene());
            };
            gameObjects.Add(levelSelect);
            Button levelEdit = new Button();
            levelEdit.P(0, -100);
            levelEdit.text = "Level Editor";
            levelEdit.func = () =>
            {
                Game1.INSTANCE.SwitchScene(new LevelEditor());
            };
            gameObjects.Add(levelEdit);
            Button quit = new Button();
            quit.P(0, -200);
            quit.text = "Quit";
            quit.func = () =>
            {
                Game1.INSTANCE.Quit();
            };
            gameObjects.Add(quit);
        }

        public override void Start()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
