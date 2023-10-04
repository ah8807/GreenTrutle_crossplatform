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
    public class Trex : DrawableGameObject, IParticle, ICustomCollider, IRSE, IEdible
    {
        public Rectangle aabb { get; set; }
        public float rotation { get; set; }
        public Vector2 scale { get; set; }
        public SpriteEffects effect { get; set; }

        public bool collWithTerain = false;

        public bool hunting = false;

        public Trex()
        {
            aabb = new Rectangle(0, 0, 14, 14);
            rotation = 0;
            scale = Vector2.One;
            effect = SpriteEffects.None;

        }

        public bool collidingWithItem(object item)
        {
            if (item is Wall||item is Brick)
            {
                collWithTerain = true;
                return true;
            }

            if (item is Turtle)
            {
                if (((Turtle)item).unlocked_edibles.OfType<Trex>().Any())
                {
                    scene.removeItem(this);
                    return false;
                }

                if (hunting)
                {
                    Globals.eventManager.Trigger("killTurtle", this,
                        new Dictionary<string, object>() { { "Turtle", item } });
                }
            }
            return false;
        }

        public void collidedWithItem(object item)
        {

        }
    }
}
