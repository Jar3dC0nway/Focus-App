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
        
        private static int moveIndex = 0;
        private static double animationTime = 0;

        private static float x0, y0, x1, y1;
        private static float? curveOffset;
        private static float frameTime;

        private static int difficulty;

        public static void SetLevel(String levelName) { 
            level = levelName;
            string jsonData = "{\"tutorial\":{\"author\":\"me\",\"difficulty\":0,\"positions\":[]}}";
            try
            {
                jsonData = File.ReadAllText(JSON_FILENAME);
            }
            catch {
                File.WriteAllText(JSON_FILENAME, jsonData);
                Debug.WriteLine("File created");
                return;
            }
            JsonObject leveldata = JsonObject.Parse(jsonData) as JsonObject;
            JsonObject jObject = leveldata[level] as JsonObject;
            if (jObject != null) { //Found level
                author = jObject["author"].GetValue<string>();
                difficulty = jObject["difficulty"].GetValue<int>();
                positions = jObject["positions"] as JsonArray;
            }

            //Debug.WriteLine($"Data: {author} {difficulty} {positions}");


            moveIndex = 0;
            animationTime = 0;
            UpdatePositionData();
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

        public static Vector2 GetPosition(GameTime gameTime) {
            animationTime += gameTime.ElapsedGameTime.TotalSeconds;
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
    }
}
