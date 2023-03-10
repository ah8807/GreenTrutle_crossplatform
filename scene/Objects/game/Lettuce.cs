using GreenTrutle_crossplatform.interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenTrutle_crossplatform.Physics;
using Microsoft.VisualBasic.FileIO;

namespace GreenTrutle_crossplatform.scene.Objects
{
    public class Lettuce: DrawableGameObject, IParticle, ICustomCollider
    {
        public static event EventHandler OnPickUp;
        
        private bool deleted = false;
        public Rectangle aabb { get; set; }
        public Lettuce()
        {
            aabb = new Rectangle(0, 0, 13, 13);

        }
        
        

        public bool collidingWithItem(object item)
        {
            return false;
        }

        public void collidedWithItem(object item)
        {
            if (item is Turtle)
            {
                if (!deleted)
                {
                    this.scene.removeItem(this);
                    Globals.eventManager.Trigger("lettucePickup", this,null);
                }
                deleted = true;
            }
            //remove lettuce
        }
    }
}
