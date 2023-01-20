using GreenTrutle_crossplatform.scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Reflection;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene.Objects;
using System.Reflection.Metadata.Ecma335;
using tainicom.Aether.Physics2D.Common;
using GreenTrutle_crossplatform.player.Human.states;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework.Graphics;

namespace GreenTrutle_crossplatform.player.Human
{
    internal class HumanPlayer : Player
    {
        KeyboardState previousState;
        static int speed = 50;
        Vector2 up = new Vector2(0, -speed);
        Vector2 down = new Vector2(0, speed);
        Vector2 left =  new Vector2(-speed, 0);
        Vector2 right = new Vector2(speed, 0);
        Vector2 still = Vector2.Zero;
        Vector2 pendingMove;

        public HumanPlayer(DrawableGameObject body, Scene theScene) : base(body, theScene)
        {

        }

        private void setDirection(DrawableGameObject body, Vector2 direction)
        {
            IParticle clone = null;
            if (body is IParticle)
                clone = (IParticle)body.Clone();
            else
                return;
            IParticle boodyParticle = (IParticle)body;
            clone.velocity = Globals.getDirection(direction);
            clone.position += clone.velocity;
            if (World.collision(clone))
            {
                clone.rotate();
                if (World.collision(clone))
                {
                    pendingMove = direction;
                    return;
                }
                boodyParticle = (IParticle)body;
                boodyParticle.rotate();
            }
            pendingMove = Vector2.Zero;
            boodyParticle.velocity = direction;
            setRSE(body, direction);
        }
        public void setRSE(DrawableGameObject body, Vector2 direction)
        {
            IRSE clone = null;
            if (body is IRSE)
                clone = (IRSE)body;
            else
                return;
            switch(direction){
                case var _ when direction.Equals(left):
                    clone.effect = SpriteEffects.FlipHorizontally;
                    clone.rotation = 0;
                    break;
                case var _ when direction.Equals(right):
                    clone.effect = SpriteEffects.None;
                    clone.rotation = 0;
                    break;
                case var _ when direction.Equals(up):
                    clone.effect = SpriteEffects.None;
                    clone.rotation = -Globals.ToRadians(90);
                    break;
                case var _ when direction.Equals(down):
                    clone.effect = SpriteEffects.None;
                    clone.rotation = Globals.ToRadians(90);
                    break;
            }
            
        }
        public void Update(GameTime gameTime)
        {

            KeyboardState Kstate = Keyboard.GetState();

            if (keyIsPressed(ref Kstate,Keys.Left)||pendingMove.Equals(left))
            {
                setDirection(body, left);
            }
            if (keyIsPressed(ref Kstate, Keys.Right) || pendingMove.Equals(right))
            {
                setDirection(body, right);
            }
            if (keyIsPressed(ref Kstate, Keys.Up) || pendingMove.Equals(up))
            {
                setDirection(body, up);
            }
            if (keyIsPressed(ref Kstate, Keys.Down) || pendingMove.Equals(down))
            {
                setDirection(body, down);
            }
            if (keyIsPressed(ref Kstate, Keys.Space))
            {
                setDirection(body, still);
            }

            previousState = Kstate;

            MouseState Mstate = Mouse.GetState();

            // Update our sprites position to the current cursor location
            Vector2 position = new Vector2();
            position.X = Mstate.X;
            position.Y = Mstate.Y;

            // Check if Right Mouse Button pressed, if so, exit
            if (Mstate.RightButton == ButtonState.Pressed)
            {
                //something
            }

            bool keyIsPressed(ref KeyboardState Kstate,Keys key)
            {
                return Kstate.IsKeyDown(key) & !previousState.IsKeyDown(
                                key);
            }
        }
    }
}
