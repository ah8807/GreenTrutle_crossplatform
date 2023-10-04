using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenTrutle_crossplatform.Graphics;
using GreenTrutle_crossplatform.scene.Objects;
using GreenTrutle_crossplatform.sound;
using GreenTrutle_crossplatform.tools;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Fluids;

namespace GreenTrutle_crossplatform
{
    internal class Globals
    {
        public static GraphicsDeviceManager graphics { get; set; }
        public static DebugRenderer debugRenderer;
        public static List<Level> levels { get; set; } = new List<Level>();
        public static GameTime gameTime { get; set; }
        public static SpriteFont font { get; set; }

        public static Vector2 gameSize = new Vector2(242, 136);

        public static Game game;
        public static SpriteBatch spriteBatch;
        public static int ScreenWidth = 1920/2 ;
        public static int ScreenHeight = 1080/2 ;
        public static EventManager eventManager=EventManager.Instance;
        public static String appDataFilePath;
        public static Save save;
        public static SoundControl soundControl;
        public static Vector2 calcMovment(IMovable movable, GameTime gameTime)
        {
            return movable.position+movable.velocity*(float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        
        public static Vector2 getDirection(Vector2 velocity)
        {
            velocity.Normalize();
            return velocity;
        }

        public static float ToRadians(double angle)
        {
            return (float)((Math.PI / 180) * angle);
        }
    }
}
