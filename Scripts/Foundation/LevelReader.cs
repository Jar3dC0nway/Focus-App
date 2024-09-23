using Focus_App.Scripts.Distractions;
using Focus_App.Scripts.Foundation;
using Focus_App.Scripts.Foundation.Objects;
using Focus_App.Scripts.Foundation.Objects.Distractions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Focus_App.Foundation
{
    public static class LevelReader
    {
        private static string JSON_FILENAME = "level_data.json"; 

        private static string level = "", author = "";
        private static JsonArray positions = new JsonArray();
        private static JsonArray distractions = new JsonArray();

        private static int moveIndex = 0;
        private static double animationTime = 0;

        private static float x0, y0, x1, y1;
        private static float? curveOffset;
        private static float frameTime;

        private static int difficulty;

        public static bool SetLevel(String levelName) { 
            level = levelName;
            string jsonData = "{\"tutorial\":{\"author\":\"me\",\"difficulty\":0,\"positions\":[],\"distractions\":[]}}";
            try {
                jsonData = File.ReadAllText(JSON_FILENAME);
            }
            catch {
                File.WriteAllText(JSON_FILENAME, jsonData);
                Debug.WriteLine("File created");
                return false;
            }
            JsonObject leveldata = JsonObject.Parse(jsonData) as JsonObject;
            JsonObject jObject = leveldata[level] as JsonObject;
            if (jObject != null) { //Found level
                author = jObject["author"].GetValue<string>();
                difficulty = jObject["difficulty"].GetValue<int>();
                positions = jObject["positions"] as JsonArray;
                distractions = jObject["distractions"] as JsonArray;
            }
            else {
                return false;
            }
            //Debug.WriteLine($"Data: {author} {difficulty} {positions}");


            moveIndex = 0;
            animationTime = 0;
            UpdatePositionData();

            return true;
        }

        public static void SetDataOfLevel(String levelName, JsonArray positions, JsonArray distractions) {
            string jsonData;
            try
            {
                jsonData = File.ReadAllText(JSON_FILENAME);
            }
            catch {
                return;
            }

            JsonObject levelData = JsonObject.Parse(jsonData) as JsonObject;
            JsonObject level = new JsonObject();
            level.Add("author", "me");
            level.Add("difficulty", 0);
            level.Add("positions", positions);
            level.Add("distractions", distractions);

            if (levelData[levelName] == null)
            {
                levelData.Add(levelName, level);
            }
            else {
                levelData[levelName] = level;
            }

            jsonData = levelData.ToJsonString();
            try {
                File.WriteAllText(JSON_FILENAME, jsonData);
            }
            catch { 
                return; 
            }
        }

        private static void UpdatePositionData() {
            JsonArray frame = positions[moveIndex].AsArray();
            x0 = x1;
            y0 = y1;
            x1 = (float)frame[0].AsValue();
            y1 = (float)frame[1].AsValue();
            frameTime = (float)frame[2].AsValue();
            curveOffset = (frame.Count > 3) ? (float)frame[3].AsValue() : null;
        }

        private static float GetTimeRatio() {
            return (float)(animationTime / frameTime);
        }

        public static JsonArray GetDistractions() {
            return distractions;
        }

        public static List<GameObject> GetDistractionsAsGameObjects() { 
            List<GameObject> gameObjects = new List<GameObject>();
            foreach (JsonArray distraction in distractions.AsArray()) {
                GameObject gameObject = new DCloud()
                    .P((float)distraction[0].AsValue(), (float)distraction[1].AsValue());
                ((Distraction)gameObject).timeStart = (int)distraction[2].AsValue();
                gameObjects.Add(gameObject);
            }
            return gameObjects;
        }

        public static JsonArray GetPositions() {
            return positions;
        }

        public static Vector2 GetPosition(GameTime gameTime) {
            animationTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (positions.Count == 0) return Vector2.Zero;
            if (animationTime >= frameTime) {
                animationTime -= frameTime;
                moveIndex = (moveIndex + 1) % positions.Count;
                UpdatePositionData();
            }
            float ratio = GetTimeRatio();
            if (curveOffset == null) { //Do we jump/delay instead of move?
                return ratio >= 0.5f ? new Vector2(x1, y1) : new Vector2(x0, y0);
            }

            float dx = x1 - x0;
            float dy = y1 - y0;
            Vector2 curve = new Vector2(dy, dx);
            curve.Normalize();

            float offset = (float)(0.25f - Math.Pow(0.5f - ratio, 2));

            return new Vector2(x0 + (x1 - x0) * ratio, y0 + (y1 - y0) * ratio) + curveOffset.Value * offset * curve;
        }

        public static String[] GetLevelNames() {
            String[] levelNames = new String[0];
            string jsonData;
            try
            {
                jsonData = File.ReadAllText(JSON_FILENAME);
            }
            catch {
                return null;
            }

            JsonObject values = JsonObject.Parse(jsonData).AsObject();
            levelNames = new String[values.Count];
            for (int i = 0; i < values.Count; i++) {
                levelNames[i] = values.ElementAt(i).Key.ToString();
            }

            return levelNames;
        }
    }
}
