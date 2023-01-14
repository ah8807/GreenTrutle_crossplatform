using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GreenTrutle_crossplatform.Graphics;

public class RenderBase: DrawableGameComponent
{
    protected Texture2D textureWhite;
    protected Texture2D textureRed;
    protected SpriteBatch spriteBatch;

    
    protected void DrawRectangle(Rectangle rect, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(textureRed, new Vector2(rect.X, rect.Y), null,
            Color.Red, 0f, Vector2.Zero, new Vector2(rect.Width, 1), SpriteEffects.None, 0f);
        
        spriteBatch.Draw(textureRed, new Vector2(rect.X, rect.Y + rect.Height), null, 
            Color.Red, 0f, Vector2.Zero, new Vector2(rect.Width, 1), SpriteEffects.None, 0f);

        spriteBatch.Draw(textureRed, new Vector2(rect.X, rect.Y), null, 
            Color.Red, (float)Math.PI / 2, Vector2.Zero, new Vector2(rect.Height, 1), SpriteEffects.None, 0f);
        
        spriteBatch.Draw(textureRed, new Vector2(rect.X + rect.Width, rect.Y), null, 
            Color.Red, (float)Math.PI / 2, Vector2.Zero, new Vector2(rect.Height, 1), SpriteEffects.None, 0f);
    }

    public RenderBase() : base(Globals.game)
    {
        this.spriteBatch = Globals.spriteBatch;
    }
    
    
    protected override void LoadContent()
    {
        textureWhite = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        textureWhite.SetData(new[] {Color.White});
        textureRed = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        textureRed.SetData(new[] {Color.Red});

        base.LoadContent();
    }
}