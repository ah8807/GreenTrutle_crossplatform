
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using GreenTrutle_crossplatform.scene.Objects;

namespace GreenTrutle_crossplatform.player.AI
{
    internal class AIPlayer:Player
    {
        KeyboardState previousState;
        static int speed = 50;
        Vector2 up = new Vector2(0, -speed);
        Vector2 down = new Vector2(0, speed);
        Vector2 left = new Vector2(-speed, 0);
        Vector2 right = new Vector2(speed, 0);
        Vector2 still = Vector2.Zero;

        List<Vector2> ls = new List<Vector2>();

        List<Vector2> movment = new List<Vector2>(); 

        Vector2 pendingMove;

        IParticle pBody;

        Trex tBody;

        Random rnd = new Random();
        public AIPlayer(DrawableGameObject body, Scene theScene) : base(body, theScene)
        {
            pBody = (IParticle)body;
            tBody = (Trex)body;

            pBody.velocity = left;

            movment.Add(up);
            movment.Add(down);
            movment.Add(left);
            movment.Add(right);
        }

        private Boolean canMove(DrawableGameObject body, Vector2 direction)
        {
            IParticle clone = null;
            if (body is IParticle)
                clone = (IParticle)body.Clone();
            else
                return false;
            IParticle boodyParticle = (IParticle)body;
            clone.velocity = Globals.getDirection(direction);
            clone.position += clone.velocity;
            if (World.collision(clone))
            {
                clone.rotate();
                if (World.collision(clone))
                {
                    return false;
                }
                boodyParticle = (IParticle)body;
                boodyParticle.rotate();
            }
            return true;
        }
        private void setDirection(DrawableGameObject body, Vector2 direction)
        {
            if (canMove(body, direction))
            {
                pendingMove = Vector2.Zero;
                body.velocity = direction;
                setRSE(body, direction);
            }
            else
                pendingMove = direction;

        }
        public void setRSE(DrawableGameObject body, Vector2 direction)
        {
            IRSE clone = null;
            if (body is IRSE)
                clone = (IRSE)body;
            else
                return;
            switch (direction)
            {
                case var _ when direction.Equals(left):
                    clone.scale = Vector2.One;
                    clone.effect = SpriteEffects.None;
                    clone.rotation = 0;
                    break;
                case var _ when direction.Equals(right):
                    clone.scale = Vector2.One;
                    clone.effect = SpriteEffects.FlipHorizontally;
                    clone.rotation = 0;
                    break;
                case var _ when direction.Equals(up):
                    clone.scale = Vector2.One;
                    clone.effect = SpriteEffects.None;
                    clone.rotation = Globals.ToRadians(90);
                    break;
                case var _ when direction.Equals(down):
                    clone.scale = Vector2.One;
                    clone.effect = SpriteEffects.None;
                    clone.rotation = -Globals.ToRadians(90);
                    break;
            }

        }
        public void Update(GameTime gameTime)
        {
            bool found = false;
            if (tBody.collWithTerain)
            {
                
                int ticks = rnd.Next(0, 20)%4;
                double chance = rnd.NextDouble();
                Vector2 bodyOpppositeDirection = new Vector2(body.velocity.X * -1, body.velocity.Y * -1);
                Vector2 direction = movment.ElementAt(ticks);
                if (chance < 0.01)
                {
                    direction = bodyOpppositeDirection;
                    found = true;
                }
                else
                {
                    /* for (int i = 1; i < 5; i++)
                     {
                         if (!direction.Equals(body.velocity) && !direction.Equals(bodyOpppositeDirection) && canMove(body, direction))
                         {
                             found = true;
                             break;
                         }
                         direction = movment.ElementAt((ticks + i) % 4);
                     }*/
                    found = true;
                    chance = rnd.NextDouble();
                    if (chance < 0.5)
                    {
                        direction = new Vector2(body.velocity.Y, body.velocity.X);
                    }else
                        direction = new Vector2(body.velocity.Y*-1, body.velocity.X*-1);
                    if (!canMove(body, direction))
                        direction = direction * -1;
                    if (!canMove(body, direction))
                        found = false;

                }
                if(!found)
                {
                    direction = bodyOpppositeDirection;
                }
                setDirection(body, direction);
                tBody.collWithTerain = false;
            } 

        }
    }
}

