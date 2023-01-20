using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenTrutle_crossplatform.scene.Objects
{
    public class Turtle : DrawableGameObject, IParticle, ICustomCollider, IRSE
    {
        public Rectangle aabb { get; set; }
        public float rotation { get; set; }
        public Vector2 scale { get; set; }
        public SpriteEffects effect { get; set; }

        public event EventHandler gameOver;

        public bool collWithTerain=false;

        public Turtle() {
            aabb = new Rectangle(0, 0, 13, 13);
            rotation = 0;
            scale = new Vector2(1f,1f);
            effect = SpriteEffects.None;

        }

        public bool collidingWithItem(object item)
        {
            if (item is World)
                collWithTerain = true;
            
            return true;
        }

        public void collidedWithItem(object item)
        {

        }

    }
}
