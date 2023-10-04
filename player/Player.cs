using GreenTrutle_crossplatform.scene;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.Physics;
using GreenTrutle_crossplatform.scene.Objects;

namespace GreenTrutle_crossplatform.player
{
    internal class Player
    {
        public DrawableGameObject body;
        protected Scene theScene;
        public Player(DrawableGameObject body,Scene theScene)
        {
            this.body = body;
            this.theScene = theScene;
        }
        
        public void Update(GameTime gametime) {
        }

        protected bool playerCollidesWithWalls(IParticle player)
        {
            if(PhysicsEngine.quadTree is null)
                return false;
            return PhysicsEngine.quadTree.searchAll(player).Any(obj => obj is Brick || obj is Wall);
        }

        protected Boolean canMove(DrawableGameObject body, Vector2 direction)
        {
            IParticle clone = body.Clone() as IParticle;
            IParticle boodyParticle = (IParticle)body;
            
            clone.velocity = Globals.getDirection(direction);
            clone.position += clone.velocity;

            return !playerCollidesWithWalls(clone);
        }
    }
}
