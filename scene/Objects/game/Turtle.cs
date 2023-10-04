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
    [Serializable] 
    public class Turtle : DrawableGameObject, IParticle, ICustomCollider, IRSE
    {
        public Rectangle aabb { get; set; }
        public float rotation { get; set; }
        public Vector2 scale { get; set; }
        public SpriteEffects effect { get; set; }
        public List<IEdible> unlocked_edibles { get; set; }
        
        public bool hideing { get; set; }

        public event EventHandler gameOver;

        public bool collWithTerain=false;

        public Turtle() {
            aabb = new Rectangle(0, 0, 14, 14);
            rotation = 0;
            scale = new Vector2(1f,1f);
            effect = SpriteEffects.None;
            unlocked_edibles = new List<IEdible>();
            unlocked_edibles.Add(new Lettuce());
        }

        public bool collidingWithItem(object item)
        {
            bool coll = false;
            if (item is Brick || item is Wall)
            {
                collWithTerain = true;
                velocity=Vector2.Zero;
                coll = true;
            }

            return coll;
        }

        public void collidedWithItem(object item)
        {

        }

    }
}
