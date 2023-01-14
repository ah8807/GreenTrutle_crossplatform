using GreenTrutle_crossplatform.scene;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GreenTrutle_crossplatform.player
{
    internal class Player
    {
        protected  DrawableGameObject body;
        protected Scene theScene;
        public Player(DrawableGameObject body,Scene theScene)
        {
            this.body = body;
            this.theScene = theScene;
        }

        public void Update(GameTime gametime) {
        }

    }
}
