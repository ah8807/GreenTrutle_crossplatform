
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GreenTrutle_crossplatform.Physics;
using tainicom.Aether.Physics2D.Dynamics;
using GreenTrutle_crossplatform.scene.Objects;
using GreenTrutle_crossplatform.tools;
using tainicom.Aether.Physics2D.Fluids;
using World = GreenTrutle_crossplatform.Physics.World;

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
        private Timer moving_in_same_direction_for_n_seconds = new Timer(20);
        private Timer between_time = new Timer(20);
        // private Timer isStuck = new Timer(1000);
        private bool oneTime = false;

        private static string StateRandom = "Random";
        private static string StateHunt = "Hunt";
        private static string StateSearch = "Search";
        private static string StateRun = "Run";

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
            alg = new AStar(playerBody, tBody,(int)Globals.gameSize.X, (int)Globals.gameSize.Y);

            pBody.velocity = left;

            movment.Add(up);
            movment.Add(down);
            movment.Add(left);
            movment.Add(right);

            randomT.oneTime += changeState;
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
            {
                pendingMove = direction;
            }

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
                if (!oneTime)
                {
                    World.grid=World.CreateGrid(this, null);
                    oneTime = true;
                }
                if (!alg.Find(playerBody.position))
                {
                    randomT.elapsed = false;
                    return;
                }
                
                state = StateSearch;
                body.velocity = alg.directions.First() * speed;
                alg.directions.RemoveAt(0);
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
            if (playerBody.unlocked_edibles.OfType<Trex>().Any())
                state = StateRun;
            if(state == StateRandom||state == StateHunt)
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
            }

            Vector2 direction = pBody.velocity;

            if (state == StateRandom)
            {
                if (randomT.elapsed)
                {
                    state=StateSearch;
                }
                direction = RandomMovement()*speed;
                setDirection(body, direction);
                tBody.collWithTerain = false;
            }

            if (state == StateRun)
            {
                if (!seePlayer())
                {
                    direction = RandomMovement() * speed;
                    setDirection(body, direction);
                    tBody.collWithTerain = false;
                }
                else
                {
                    setDirection(body,body.velocity*-1);
                }
            }

            if (state == StateSearch)
            {
                if (alg.wayPoints.Any())
                {
                    Globals.debugRenderer.wayPoints.AddRange(alg.wayPoints);
                    direction = pendingMove == Vector2.Zero ? body.velocity : pendingMove;
                    if(Vector2.Dot((alg.wayPoints[0] - body.position), Globals.getDirection(direction)) <= 2)
                    {
                        direction = alg.directions.First() * speed;
                        alg.directions.RemoveAt(0);
                        alg.wayPoints.RemoveAt(0);
                        
                        // else
                        // {
                        //     if (!alg.Find(playerBody.position))
                        //     {
                        //         state = StateRandom;
                        //     }
                        //         
                        // }
                    }
                    // if (canMove(body, alg.directions[0] * speed)&&moving_in_same_direction_for_n_seconds.elapsed)
                    // {
                    //     moving_in_same_direction_for_n_seconds.elapsed = false;
                    //     direction = alg.directions.First() * speed;
                    //     alg.directions.RemoveAt(0);
                    // }
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
            Vector2 pPosition = World.worldScordsToTile(playerBody.position);
            Vector2 tPosition = World.worldScordsToTile(body.position);
            Vector2 direction = Globals.getDirection(body.velocity);
            List<Vector2> lines = CastRay.pointAToPointB(tPosition, pPosition);
            if (lines.Count<2 ||lines[1] - lines[0] != direction)
                return false;
            foreach (Vector2 v in CastRay.pointAToPointB(tPosition, pPosition))
            {
                bool value;
                if (!World.grid.TryGetValue(v,out value)||value)
                {
                    return false;
                }
            }
            return true;
            // switch (direction)
            // {
            //     case var _ when direction.Equals(Globals.getDirection(left)):
            //         for (int i = (int)tPosition.X; i >= 0; i--)
            //         {
            //             if (PhysicsEngine.grid[(int)tPosition.Y, i] == 1)
            //                 return false;
            //             if (pPosition == new Vector2(i, tPosition.Y))
            //                 return true;
            //         }
            //
            //         break;
            //     case var _ when direction.Equals(Globals.getDirection(right)):
            //         for (int i = (int)tPosition.X; i < 250; i++)
            //         {
            //             if (PhysicsEngine.grid[(int)tPosition.Y, i] == 1)
            //                 return false;
            //             if (pPosition == new Vector2(i, tPosition.Y))
            //                 return true;
            //         }
            //
            //         break;
            //     case var _ when direction.Equals(Globals.getDirection(up)):
            //         for (int j = (int)tPosition.Y; j >= 0; j--)
            //         {
            //             if (PhysicsEngine.grid[j, (int)tPosition.X] == 1)
            //                 return false;
            //             if (pPosition == new Vector2(tPosition.X, j))
            //                 return true;
            //         }
            //
            //         break;
            //     case var _ when direction.Equals(Globals.getDirection(down)):
            //         for (int j = (int)tPosition.Y; j < 135; j++)
            //         {
            //             if (PhysicsEngine.grid[j, (int)tPosition.X] == 1)
            //                 return false;
            //             if (pPosition == new Vector2(tPosition.X, j))
            //                 return true;
            //         }
            //
            //         break;
            // }
            //
            // return false;
        }

        private Vector2 RandomMovement()
        {
            Vector2 direction;
            bool found=false;
            int ticks = rnd.Next(0, 20) % 4;
            double chance = rnd.NextDouble();
            direction = Globals.getDirection(body.velocity);
            Vector2 forward = Globals.getDirection(body.velocity);
            Vector2 backwards = new Vector2(direction.X * -1,direction.Y * -1);
            Vector2 left =  new Vector2(direction.Y, direction.X);
            Vector2 right = new Vector2(direction.Y * -1, direction.X * -1);
            if (!between_time.elapsed)
            {
                return forward;
            }
            between_time.elapsed = false;
            // 0.01 chance da gre ob koliziji v rikverc
            if (chance < 0.00 && tBody.collWithTerain)
            {
                direction = backwards;
                tBody.collWithTerain = false;
                return direction;
            }
            float chance_to_move=tBody.collWithTerain?0.5f:0.33f;
            if((tBody.collWithTerain || moving_in_same_direction_for_n_seconds.elapsed))
            {
                found = true;
                between_time.elapsed = false;

                chance = rnd.NextDouble();
                if (chance < chance_to_move)
                {
                    direction = new Vector2(direction.Y, direction.X );
                }
                else if (chance >= chance_to_move && chance <= chance_to_move * 2)
                {
                    direction = new Vector2(direction.Y  * -1, direction.X  * -1);

                }


                if (!canMove(body, direction) && chance < chance_to_move * 2)
                {
                    direction = direction * -1;
                    if (!canMove(body, direction))
                        found = false;
                }

                if (chance >= chance_to_move * 2)
                    found = false;
            }

            if (!found)
            {
                direction = forward;
                if (chance >= chance_to_move * 2)
                {
                    chance = rnd.NextDouble();
                    if (canMove(body, forward))
                        direction = forward;
                    else
                    {
                        direction = right;
                        if (chance < 50)
                            direction = left;
                        if (!canMove(body, direction))
                        {
                            direction = direction * -1;
                            if (!canMove(body, direction))
                                direction = backwards;
                        }
                    }
            }else
                //če ne more nadaljevati v isti smeri ga obrne
                if (!canMove(body, direction)&&tBody.collWithTerain)
                {
                    direction = backwards;
                }
            }
            if (moving_in_same_direction_for_n_seconds.elapsed&&!tBody.collWithTerain)
            {
                moving_in_same_direction_for_n_seconds.elapsed = false;
            }
            tBody.collWithTerain = false;
            return direction;
        }

        public void Close()
        {
            randomT.Close();
            moving_in_same_direction_for_n_seconds.Close();
            between_time.Close();
        }
    }
}


