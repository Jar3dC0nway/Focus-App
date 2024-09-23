using Focus_App.Scripts.Foundation.Objects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Foundation
{
    public abstract class Scene
    {
        public List<GameObject> gameObjects = new List<GameObject>();
        public abstract void Start();
        public abstract void Update(GameTime gameTime);
        public abstract void LoadScene();
        public abstract void Draw(GameTime gameTime);

        public Scene() {
            LoadScene();
        }
    }
}
