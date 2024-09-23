using Focus_App.Foundation;
using Focus_App.Foundation.Rendering;
using Focus_App.Scripts.Foundation.Objects;
using Focus_App.Scripts.Foundation.Rendering.UI;
using Focus_App.Scripts.Foundation.Rendering.UI.Button;
using Focus_App.Scripts.Foundation.Rendering.UI.Text;
using Focus_App.Scripts.Foundation.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Focus_App.Scripts
{
    public class LevelEditor : Scene
    {
        Stario stario;
        TextBox[] textBoxes = new TextBox[0];
        Button[] addDeleteButtons = new Button[0];
        GameObject[] markers = new GameObject[0];
        GameObject[] distractionObjs = new GameObject[0];

        TextBox levelName;

        List<Position> data_ref = new List<Position>();


        List<Position> s_positions = new List<Position>();
        List<Position> distractions = new List<Position>();




        List<Button> tabButtons = new List<Button>();

        TextLabel pageLabel;

        int pageNumber = 0;
        int PAGE_LENGTH = 8;

        private class Position {
            public short x, y, time;
            public short? offset;

            public Position P(short _x, short _y) {
                x = _x;
                y = _y;
                return this;
            }
            public Position T(short _time) {
                time = _time;
                return this;
            }
            public Position O(short _offset) { 
                offset = _offset;
                return this;
            }

            public short? GetValue(int variable) { 
                switch (variable)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return time;
                    default: return offset;
                }
            }
        }
        private class Distractions {
            public short x, y, time, index;
        }



        public override void Draw(GameTime gameTime)
        {
            //Game1.INSTANCE.renderer.Draw(LevelReader.GetDistractionsAsGameObjects().Item1, gameTime);
        }

        public override void LoadScene()
        {
            CreateUI();
            stario = new Stario();
            gameObjects.Add(stario);
            CreateTextboxes();
        }

        public override void Start()
        {
            
        }

        private short stringToShort(string s) {
            if (s == "-") return 0;
            return short.Parse(s);
        }

        public override void Update(GameTime gameTime)
        {
            stario.position -= new Vector2(250, 0);

            for (int i = 0; i < textBoxes.Length / 4 && i + (PAGE_LENGTH * pageNumber) < data_ref.Count; i++) {
                data_ref[i + (PAGE_LENGTH * pageNumber)].P(stringToShort(textBoxes[i * 4].label.text), stringToShort(textBoxes[i * 4 + 1].label.text));
                data_ref[i + PAGE_LENGTH * pageNumber].T(stringToShort(textBoxes[i * 4 + 2].label.text));
                String offset = textBoxes[i * 4 + 3].text;
                data_ref[i + PAGE_LENGTH * pageNumber].offset = (offset == null || offset == "" || offset == "-" ? null : short.Parse(offset));
            }
        }

        private void ExitToGame() {
            Game1.INSTANCE.SwitchScene(new MainMenu());
        }

        private void CreateTextboxes() {
            foreach (TextBox textBox in textBoxes) {
                gameObjects.Remove(textBox);
            }
            foreach (Button button in addDeleteButtons) {
                gameObjects.Remove(button);
            }
            if (data_ref.Count == 0) {
                data_ref.Add(new Position());
            }

            textBoxes = new TextBox[Math.Min(data_ref.Count, PAGE_LENGTH) * 4];
            addDeleteButtons = new Button[textBoxes.Length / 2];
            for (int i = 0; i < PAGE_LENGTH && i + pageNumber * PAGE_LENGTH < data_ref.Count; i++) {
                for (int j = 0; j < 4; j++) {
                    TextBox textBox = new TextBox();
                    textBox.text = data_ref[i + pageNumber * PAGE_LENGTH].GetValue(j) + "";
                    textBox.defaultText = j < 3 ? "0" : "";
                    textBox.type = InputTypes.Numeric;
                    textBox.P(221 + 80 * j, -120 + i * 58).S(1.2f, 0.7f);
                    gameObjects.Add(textBox);
                    textBoxes[i * 4 + j] = textBox;

                }
                
                Button button = new Button();
                button.text = "+";
                button.color = Color.Green;
                button.P(541, -120 + i * 58);
                button.label.S(1.0f, 0.8f);
                int value = i + PAGE_LENGTH * pageNumber;
                button.func = () => { 
                    data_ref.Insert(value + 1, new Position());
                    CreateTextboxes();
                };
                gameObjects.Add(button);
                addDeleteButtons[i * 2] = button;

                Button delete = new Button();
                delete.text = "x";
                delete.color = Color.Red;
                delete.P(621, -120 + i * 58).S(1.2f, 1.1f);
                delete.func = () => {
                    data_ref.RemoveAt(value);
                    CreateTextboxes();
                };
                gameObjects.Add(delete);
                addDeleteButtons[i * 2 + 1] = delete;
            }
            UpdatePageLabel();
        }

        private void UpdatePageLabel() {
            int pages = (int)MathF.Ceiling(data_ref.Count * 1.0f / PAGE_LENGTH);
            pageLabel.text = (pageNumber + 1) + "/" + pages;
        }

        private void ChangePage(int delta) {
            int pages = (int)MathF.Ceiling(data_ref.Count * 1.0f / PAGE_LENGTH);
            if (pages == 0) pages = 1;
            pageNumber = (pageNumber + pages + delta) % pages;
            UpdatePageLabel();
            CreateTextboxes();
        }

        private void UpdatePoints() {
            JsonArray positions = LevelReader.GetPositions();
            JsonArray distractionData = LevelReader.GetDistractions();
            data_ref = new List<Position>();
            s_positions = data_ref;
            distractions = new List<Position>();
            foreach (TextBox textBox in textBoxes) {
                gameObjects.Remove(textBox);
            }
            foreach (GameObject marker in markers) {
                gameObjects.Remove(marker);
            }
            foreach (GameObject obj in distractionObjs) {
                gameObjects.Remove(obj);
            }
            textBoxes = new TextBox[positions.Count * 4];
            markers = new GameObject[positions.Count];
            distractionObjs = LevelReader.GetDistractionsAsGameObjects().ToArray();
            foreach (GameObject obj in distractionObjs) {
                gameObjects.Add(obj);
                obj.position -= new Vector2(250, 0);
            }
            Sprite markerSprite = Game1.INSTANCE.renderer.CreateSprite("Textures/markers1");
            for (int i = 0; i < positions.Count; i++) { 
                JsonArray positionData = positions[i].AsArray();
                Position pos = new Position().P((short)positionData[0], (short)positionData[1]).T((short)positionData[2]);
                if (positionData.Count > 3 && positionData[3] != null)
                    pos.O((short)positionData[3]);
                s_positions.Add(pos);

                GameObject marker = new GameObject().T(markerSprite).P(new Vector2((float)positions[i][0].AsValue() - 250, (float)positions[i][1].AsValue()));
                gameObjects.Insert(3, marker);
                markers[i] = marker;
            }
            for (int i = 0; i < distractionData.Count; i++) {
                JsonArray distraction = distractionData[i].AsArray();
                Position pos = new Position().P((short)distraction[0], (short)distraction[1]).T((short)distraction[2]);
                if (distraction.Count > 3 && distraction[3] != null)
                    pos.O((short)distraction[3]);
                distractions.Add(pos);
            }

            if (distractions.Count == 0)
                distractions.Add(new Position());
                
            CreateTextboxes();
            ChangePage(0);
        }

        private void LoadLevel() {
            Debug.WriteLine("Loaded level" + levelName.text);
            LevelReader.SetLevel(levelName.text);
            UpdatePoints();
        }

        private JsonArray _CompilePositionData() { 
            JsonArray positionData = new JsonArray();
            for (int i = 0; i < data_ref.Count; i++) {
                JsonArray position = new JsonArray {
                    data_ref[i].x,
                    data_ref[i].y,
                    data_ref[i].time
                };
                if (data_ref[i].offset != null)
                    position.Add(data_ref[i].offset);
                positionData.Add(position);
            }
            return positionData;
        }

        private void SaveLevel() {
            Debug.WriteLine("Saved level " + levelName.text);
            var _ = data_ref;
            data_ref = s_positions;
            JsonArray positionData = _CompilePositionData();
            data_ref = distractions;
            JsonArray distractionData = _CompilePositionData();

            data_ref = _;
            LevelReader.SetDataOfLevel(levelName.text, positionData, distractionData);
        }

        private void CreateUI()
        {
            UIPanel sidePanel = new UIPanel();
            sidePanel.color = new Color(0.2f, 0.25f, 0.25f);
            sidePanel.S(12.0f, 16.0f).P(550, 0);
            gameObjects.Add(sidePanel);

            UIPanel mainScreen = new UIPanel();
            mainScreen.color = new Color(0.7f, 0.95f, 0.95f);
            mainScreen.S(12.0f, 12.0f).P(-250, 0);
            gameObjects.Add(mainScreen);
            UIPanel mainScreenOverlay = new UIPanel();
            mainScreenOverlay.color = new Color(0.5f, 0.75f, 1f);
            mainScreenOverlay.S(11.9f, 11.9f).P(-250, 0);
            gameObjects.Add(mainScreenOverlay);


            Button exitButton = new Button();
            gameObjects.Add(exitButton);
            exitButton.text = "Exit";
            exitButton.P(new Vector2(600, 350));
            exitButton.func = ExitToGame;

            Button saveButton = new Button();
            saveButton.text = "Save";
            saveButton.P(new Vector2(600, -350));
            gameObjects.Add(saveButton);
            saveButton.func = SaveLevel;

            Button loadButton = new Button();
            loadButton.text = "Load";
            loadButton.P(new Vector2(490, -350));
            gameObjects.Add(loadButton);
            loadButton.func = LoadLevel;


            levelName = new TextBox();
            levelName.defaultText = "Level Name";
            levelName.text = "";
            levelName.P(new Vector2(304, -350)).S(3.8f, 0.8f);
            gameObjects.Add(levelName);

            Button nextValues = new Button();
            nextValues.text = ">";
            nextValues.P(new Vector2(475, 350));
            nextValues.func = () => ChangePage(1);
            gameObjects.Add(nextValues);

            Button lastValues = new Button();
            lastValues.text = "<";
            lastValues.P(new Vector2(325, 350));
            lastValues.func = () => ChangePage(-1);
            gameObjects.Add(lastValues);

            pageLabel = new TextLabel();
            pageLabel.text = "1/1";
            pageLabel.P(new Vector2(400, 350));
            pageLabel.centered = true;
            gameObjects.Add(pageLabel);

            UIPanel selector = new UIPanel();
            selector.P(new Vector2(270, -215)).S(1, 0.1f);
            gameObjects.Add(selector);

            Button starioTab = new Button();
            starioTab.text = "Positions";
            starioTab.P(new Vector2(270, -250));
            starioTab.func = () =>
            {
                selector.P(new Vector2(270, -215))
                .S(starioTab.S().X, 0.1f);

                data_ref = s_positions;
                ChangePage(0);
            };
            gameObjects.Add(starioTab);
            tabButtons.Add(starioTab);

            Button distractionsTab = new Button();
            distractionsTab.text = "Distractions";
            distractionsTab.P(new Vector2(530, -250));
            distractionsTab.func = () =>
            {
                selector.P(new Vector2(530, -215))
                .S(distractionsTab.S().X, 0.1f);

                data_ref = distractions;
                ChangePage(0);
            };
            gameObjects.Add(distractionsTab);
            tabButtons.Add(distractionsTab);
        }
    }
}
