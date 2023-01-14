using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace GreenTrutle_crossplatform.Sprites
{
    internal class AnimatedSprite : Sprite
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private TimeSpan prevTime;
        public int updateTimer=100;
        public Vector2 origin;
        public AnimatedSprite()
        {

        }

        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
        }

        public void update(GameTime gameTime)
        {

            if (gameTime.TotalGameTime.TotalMilliseconds - prevTime.TotalMilliseconds > updateTimer)
            {
                currentFrame = (currentFrame + 1) % (Columns * Rows);
                prevTime = gameTime.TotalGameTime;
            }
        }
        public Sprite getFrame(GameTime gameTime)
        {
            int width = texture.Width / Columns;
            int height = texture.Height / Rows;
            int row = currentFrame / Columns;
            int column = currentFrame % Columns;

            Sprite frame = new Sprite();

            frame.sourceRectangle = new Rectangle(width * column, height * row, width, height);
            frame.texture = this.texture;
            frame.origin = this.origin;
            update(gameTime);

            return frame;
        }
    }
}
