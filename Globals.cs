using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Fluids;

namespace GreenTrutle_crossplatform
{
    internal class Globals
    {
        public static GraphicsDeviceManager graphics { get; set; }
        public static Level currLevel { get; set; }
        public static List<Level> levels { get; set; } = new List<Level>();
        public static Game game;
        public static SpriteBatch spriteBatch;
        public static int ScreenWidth = 1920 / 2;
        public static int ScreenHeight = 1080 / 2;
        public static EventManager eventManager = new EventManager();
        public static Vector2 calcMovment(IMovable movable, GameTime gameTime)
        {
            float x = (float)(movable.position.X + gameTime.ElapsedGameTime.TotalSeconds * movable.velocity.X);
            float y = (float)(movable.position.Y + gameTime.ElapsedGameTime.TotalSeconds * movable.velocity.Y);
            return new Vector2(x, y);
        }

        public static void func(Object o, Dictionary<string, object> args)
        {
            //score.incScore(); soundEffect.Play();
            
        }
        public static Vector2 getDirection(Vector2 velocity)
        {
            int x = (int)velocity.X;
            x = x != 0 ? x / Math.Abs(x) : 0;
            int y = (int)velocity.Y;
            y = y != 0 ? y / Math.Abs(y) : 0;
            return new Vector2(x, y);
        }

        public static float ToRadians(double angle)
        {
            return (float)((Math.PI / 180) * angle);
        }
    }
    internal class World
    {
        public static int[,] grid;
        public static bool collision(IParticle particle)
        {
            Rectangle rect = particle.getRect();

            for (int i = rect.X; i <= rect.Width; i++)
            {
                for (int j = rect.Y; j <= rect.Height; j++)
                {
                    if (i > 240 || i < 0 || j > 135 || j < 0)
                    {
                        return true;
                    }

                    if (grid[j, i] == 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
