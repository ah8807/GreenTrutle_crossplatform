using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GreenTrutle_crossplatform.Sprites;

public class Sprite
{
    public Texture2D texture { get; set; }
    public Rectangle sourceRectangle { get; set; }
    public Vector2 origin { get; set; }

}