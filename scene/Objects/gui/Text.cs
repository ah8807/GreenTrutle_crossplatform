using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GreenTrutle_crossplatform.scene.Objects;

public class Text : DrawableGameObject
{
    public String text { get; set; }
    private SpriteFont font {
        get {return Globals.font;}
    }
    

    public Vector2 textMiddlePoint
    {
        get
        {
            return font.MeasureString(text) / 2;
        }
    }
    public void draw(Vector2 hudScale)
    {
        Globals.spriteBatch.DrawString(font, text, position, Color.White, 0, textMiddlePoint, 1.0f*hudScale,
        SpriteEffects.None, 0.5f);
    }

    public Text()
    {
        text = "Turtle";
    }
}