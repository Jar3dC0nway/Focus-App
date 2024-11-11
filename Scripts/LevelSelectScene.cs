using Focus_App.Foundation;
using Focus_App.Scripts.Foundation;
using Focus_App.Scripts.Foundation.Rendering.UI;
using Focus_App.Scripts.Foundation.Rendering.UI.Button;
using Focus_App.Scripts.Foundation.Rendering.UI.Text;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Focus_App.Scripts
{
    public class LevelSelectScene : Scene
    {
        static String leaderboard = "Loading...";
        Button[] levelButtons = new Button[0];
        static TextLabel[] leaderboardLabels = new TextLabel[0];
        static String[] levelNames = new String[0];

        public override void Draw(GameTime gameTime) {
            
        }

        private void SetRankFromScore(TextLabel rank, float score) {
            switch (score) {
                case > 0.95f:
                    rank.text = "S";
                    rank.textColor = Color.Magenta;
                    break;
                case > 0.85f:
                    rank.text = "A";
                    rank.textColor = Color.Red;
                    break;
                case > 0.75f:
                    rank.text = "B";
                    rank.textColor = Color.Orange;
                    break;
                case > 0.5f:
                    rank.text = "C";
                    rank.textColor = Color.Lerp(Color.Yellow, Color.DarkOrange, 0.5f);
                    break;
                case > 0.0f:
                    rank.text = "D";
                    rank.textColor = Color.Lime;
                    break;
                default:
                    rank.text = "-";
                    rank.textColor = Color.Blue;
                    break;
            }
        }

        static String GetScoreAsPercent(float levelScore) {
            return (MathF.Round(levelScore * 1000) / 10) + "%";
        }

        static async void GetLeaderboard() {
            if (false) return;
            try {
                leaderboard = await Registry.CLIENT.GetStringAsync(Registry.LEADERBOARD_URL + ".json");
                JsonObject data = JsonObject.Parse(leaderboard).AsObject();
                Debug.Write(leaderboard);
                for (int i = 0; i < levelNames.Length; i++) {
                    String name = levelNames[i];
                    TextLabel label = leaderboardLabels[i];
                    if (data[name] != null) {
                        try {
                            JsonObject levelData = data[name].AsObject();
                            var scoreData = levelData.ToArray();
                            var top3 = scoreData
                                .OrderByDescending(score => (float)score.Value.AsValue())
                                .Take(scoreData.Length < 3 ? scoreData.Length : 3);
                            String text = "";
                            foreach (var score in top3)
                            {
                                text += score.Key + ": " + GetScoreAsPercent((float)score.Value) + "\n";
                            }

                            label.text = text;
                            label.centered = false;
                            label.P(label.P() - new Vector2(100, 0));
                        }
                        catch (Exception e) {
                            label.text = "No scores yet";
                        }
                    } 
                    else {
                        label.text = "No scores yet";
                    }
                }
            } catch (Exception _) {
                Debug.WriteLine(_);
            }
        }

        private void InitLevel(float levelScore, int i) {
            TextLabel rank = new TextLabel();
            UIPanel rankPanel = new UIPanel();
            UIPanel rankOutline = new UIPanel();
            Button levelButton = new Button();
            UIPanel levelDetail = new UIPanel();
            TextLabel leaderboardData = new TextLabel();
            levelButton.text = levelNames[i] + "\n" + GetScoreAsPercent(levelScore);
            levelButton.P(-390 + (275 * (i % 4)), -300 + ((i / 4) * 400));
            levelButton.S(3.8f, 1.8f);
            rank.P(levelButton.P() + new Vector2(100, -60)).S(2, 2);
            rank.centered = true;
            SetRankFromScore(rank, levelScore);
            rankPanel.P(rank.P()).S(1.125f, 1.125f);
            rankOutline.P(rankPanel.P()).S(rankPanel.S() + new Vector2(0.125f, 0.125f));
            rankOutline.color = rank.textColor;
            rankPanel.color = new Color(0.95f, 0.99f, 1.0f);
            levelDetail.color = Color.DarkSlateGray;
            levelDetail.P(levelButton.P() + new Vector2(0, 100)).S(4, 6);
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
            leaderboardData.P(levelButton.P() + new Vector2(0, 100)).S(1, 1);
            leaderboardData.centered = true;
            leaderboardData.text = leaderboard;
            leaderboardData.textColor = rankPanel.color;
            levelButtons[i] = levelButton;
            leaderboardLabels[i] = leaderboardData;
            gameObjects.Add(levelDetail);
            gameObjects.Add(levelButton);
            gameObjects.Add(rankOutline);
            gameObjects.Add(rankPanel);
            gameObjects.Add(rank);
            gameObjects.Add(leaderboardData);
        }

        public override void LoadScene() {
            levelNames = LevelReader.GetLevelNames();
            float[] levelScores = LevelReader.GetLevelScores();
            if (levelNames == null) {
                MainMenu menu = new MainMenu();
                //TextLabel levelsNotFound = new TextLabel();
                //levelsNotFound.text = "No levels in level_data.json, try adding one in level editor first";
                //levelsNotFound.P(-500, 350);
                //menu.gameObjects.Add(levelsNotFound);
                Game1.INSTANCE.SwitchScene(menu);
                return;
            }


            leaderboardLabels = new TextLabel[levelNames.Length];
            levelButtons = new Button[levelNames.Length];
            for (int i = 0; i < levelNames.Length; i++) {
                InitLevel(levelScores[i], i);
            }
            GetLeaderboard();
        }

        public override void Start() {
            
        }

        public override void Update(GameTime gameTime) {
            
        }
    }
}
