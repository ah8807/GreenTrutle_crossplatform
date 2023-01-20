
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using GreenTrutle_crossplatform.scene.Objects;
using GreenTrutle_crossplatform.tools;

namespace GreenTrutle_crossplatform.player.AI
{
    internal class AIPlayer : Player
    {
        KeyboardState previousState;
        static int speed = 50;
        Vector2 up = new Vector2(0, -speed);
        Vector2 down = new Vector2(0, speed);
        Vector2 left = new Vector2(-speed, 0);
        Vector2 right = new Vector2(speed, 0);
        Vector2 still = Vector2.Zero;
        private AStar alg;
        private Timer randomT = new Timer(5 * 1000);

        private static string StateRandom = "Random";
        private static string StateHunt = "Hunt";
        private static string StateSearch = "Search";

        private bool hunting = false;


        private String state = StateRandom;

        List<Vector2> ls = new List<Vector2>();

        List<Vector2> movment = new List<Vector2>();

        Vector2 pendingMove;

        IParticle pBody;

        Trex tBody;

        private Turtle playerBody;

        Random rnd = new Random();

        public AIPlayer(DrawableGameObject body, Scene theScene, DrawableGameObject turtle) : base(body, theScene)
        {
            this.body.velocity *= speed;
            pBody = (IParticle)body;
            tBody = (Trex)body;
            playerBody = (Turtle)turtle;
            alg = new AStar(playerBody, tBody);

            pBody.velocity = left;

            movment.Add(up);
            movment.Add(down);
            movment.Add(left);
            movment.Add(right);

            randomT.oneTime += changeState;
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
            direction = Globals.getDirection(body.velocity);
            switch (direction)
            {
                case var _ when direction.Equals(Globals.getDirection(left)):
                    clone.scale = Vector2.One;
                    clone.effect = SpriteEffects.None;
                    clone.rotation = 0;
                    break;
                case var _ when direction.Equals(Globals.getDirection(right)):
                    clone.scale = Vector2.One;
                    clone.effect = SpriteEffects.FlipHorizontally;
                    clone.rotation = 0;
                    break;
                case var _ when direction.Equals(Globals.getDirection(up)):
                    clone.scale = Vector2.One;
                    clone.effect = SpriteEffects.None;
                    clone.rotation = Globals.ToRadians(90);
                    break;
                case var _ when direction.Equals(Globals.getDirection(down)):
                    clone.scale = Vector2.One;
                    clone.effect = SpriteEffects.None;
                    clone.rotation = -Globals.ToRadians(90);
                    break;
            }

        }

        private void changeState(Object? o, EventArgs args)
        {
            if (state == StateRandom)
            {
                state = StateSearch;
            }
            else if (state == StateHunt)
            {

            }
            else if (state == StateSearch)
            {

            }
        }

        public void Update(GameTime gameTime)
        {
            if (seePlayer() && playerBody.velocity != Vector2.Zero)
            {
                speed = 80;
                Vector2 dir = Globals.getDirection(body.velocity);
                body.velocity = dir * speed;
                hunting = true;
            }
            else
            {
                if (hunting && seePlayer())
                {
                }
                else
                {
                    speed = 50;
                    Vector2 dir = Globals.getDirection(body.velocity);
                    body.velocity = dir * speed;
                    hunting = false;
                }

            }

            Vector2 direction = pBody.velocity;
            if (tBody.collWithTerain)
            {
                if (state == StateRandom)
                {
                    direction = RandomMovement();
                }

                setDirection(body, direction);
                tBody.collWithTerain = false;
            }

            if (state == StateSearch)
            {
                alg.Find(playerBody.position);
                if (alg.directions.Any())
                {
                    if (canMove(body, alg.directions[0] * speed))
                    {
                        direction = alg.directions.First() * speed;
                        alg.directions.RemoveAt(0);
                    }
                }
                else
                {
                    state = StateRandom;
                    randomT.elapsed = false;
                }

                setDirection(body, direction);
            }

            tBody.hunting = hunting;

        }

        private bool seePlayer()
        {
            Vector2 pPosition = new Vector2((int)playerBody.position.X, (int)playerBody.position.Y);
            Vector2 tPosition = new Vector2((int)body.position.X, (int)body.position.Y);
            Vector2 direction = Globals.getDirection(body.velocity);
            switch (direction)
            {
                case var _ when direction.Equals(Globals.getDirection(left)):
                    for (int i = (int)tPosition.X; i >= 0; i--)
                    {
                        if (World.grid[(int)tPosition.Y, i] == 1)
                            return false;
                        if (pPosition == new Vector2(i, tPosition.Y))
                            return true;
                    }

                    break;
                case var _ when direction.Equals(Globals.getDirection(right)):
                    for (int i = (int)tPosition.X; i < 250; i++)
                    {
                        if (World.grid[(int)tPosition.Y, i] == 1)
                            return false;
                        if (pPosition == new Vector2(i, tPosition.Y))
                            return true;
                    }

                    break;
                case var _ when direction.Equals(Globals.getDirection(up)):
                    for (int j = (int)tPosition.Y; j >= 0; j--)
                    {
                        if (World.grid[j, (int)tPosition.X] == 1)
                            return false;
                        if (pPosition == new Vector2(tPosition.X, j))
                            return true;
                    }

                    break;
                case var _ when direction.Equals(Globals.getDirection(down)):
                    for (int j = (int)tPosition.Y; j < 135; j++)
                    {
                        if (World.grid[j, (int)tPosition.X] == 1)
                            return false;
                        if (pPosition == new Vector2(tPosition.X, j))
                            return true;
                    }

                    break;
            }

            return false;
        }

        private Vector2 RandomMovement()
        {
            Vector2 direction;
            bool found;
            int ticks = rnd.Next(0, 20) % 4;
            double chance = rnd.NextDouble();
            direction = Globals.getDirection(body.velocity);
            Vector2 bodyOpppositeDirection = direction * speed * -1;
            if (chance < 0.01)
            {
                direction = bodyOpppositeDirection;
                found = true;
            }
            else
            {
                found = true;
                chance = rnd.NextDouble();
                if (chance < 0.5)
                {
                    direction = new Vector2(direction.Y * speed, direction.X * speed);
                }
                else
                    direction = new Vector2(direction.Y * speed * -1, direction.X * speed * -1);

                if (!canMove(body, direction))
                    direction = direction * -1;
                if (!canMove(body, direction))
                    found = false;
            }

            if (!found)
            {
                direction = bodyOpppositeDirection;
            }

            return direction;
        }

        public void Close()
        {
            randomT.Close();
        }
    }
}


