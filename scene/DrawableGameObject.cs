using GreenTrutle_crossplatform.interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenTrutle_crossplatform.scene
{
    public class DrawableGameObject : IMovable, IScene, ICloneable
    {
        public Vector2 velocity { get; set; }
        public Vector2 position { get; set; }

        public Scene scene {get; set;}
        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
