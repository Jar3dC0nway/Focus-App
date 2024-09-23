using Focus_App.Foundation;
using Focus_App.Scripts.Foundation.Rendering.UI.Button;
using Focus_App.Scripts.Foundation.Rendering.UI.Text;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Scripts
{
    public class LevelSelectScene : Scene
    {
        Button[] levelButtons = new Button[0];

        public override void Draw(GameTime gameTime) {
            
        }

        public override void LoadScene() {
            String[] levelNames = LevelReader.GetLevelNames();
            levelButtons = new Button[levelNames.Length];
            for (int i = 0; i < levelNames.Length; i++) {
                Button levelButton = new Button();
                levelButton.text = levelNames[i];
                levelButton.P(-300 + (200 * (i % 4)), -300 + ((i / 4) * 100));
                String value = levelNames[i];
                levelButton.func = () =>
                {
                    LevelReader.SetLevel(value);
                    MainScene level = new MainScene();
                    TextLabel levelChosenText = new TextLabel();
                    levelChosenText.text = value + " loaded";
                    levelChosenText.P(-500, 350);
                    level.gameObjects.Add(levelChosenText);
                    Game1.INSTANCE.SwitchScene(level);
                };
                levelButtons[i] = levelButton;
                gameObjects.Add(levelButton);
            }
        }

        public override void Start() {
            
        }

        public override void Update(GameTime gameTime) {
            
        }
    }
}
