using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GreenTrutle_crossplatform.Graphics;

public class RenderBase: DrawableGameComponent
{
    protected Texture2D textureWhite;
    protected Texture2D textureRed;
    protected Texture2D textureGreen;
    protected SpriteBatch spriteBatch;
    protected RenderTarget2D gameSizeRT;
    protected static readonly int ScreenWidth = Globals.ScreenWidth;
    protected static readonly int ScreenHeight = Globals.ScreenHeight; 
    protected static Rectangle CANVAS = new Rectangle(0, 0, 240 , 135 );
    
    public RenderBase() : base(Globals.game)
    {
        this.spriteBatch = Globals.spriteBatch;
        Globals.graphics.PreferMultiSampling = false;
        Globals.graphics.SynchronizeWithVerticalRetrace = true;

        Globals.graphics.PreferredBackBufferWidth = ScreenWidth;
        Globals.graphics.PreferredBackBufferHeight = ScreenHeight;

        Globals.graphics.IsFullScreen = false;  //k zaklučeš game dj na true

        Globals.graphics.ApplyChanges();

        gameSizeRT = new RenderTarget2D(GraphicsDevice, CANVAS.Width, CANVAS.Height);
    }
    protected void DrawRectangle(Rectangle rect, SpriteBatch spriteBatch,Texture2D texture,Vector2 scale)
    {
        spriteBatch.Draw(texture, new Vector2(rect.X, rect.Y)*scale, null,
            Color.Red, 0f, Vector2.Zero, new Vector2(rect.Width*scale.X, 1), SpriteEffects.None, 0f);
        
        spriteBatch.Draw(texture, new Vector2(rect.X, rect.Y + rect.Height)*scale, null, 
            Color.Red, 0f, Vector2.Zero,  new Vector2(rect.Width*scale.X, 1), SpriteEffects.None, 0f);

        spriteBatch.Draw(texture, new Vector2(rect.X, rect.Y)*scale, null, 
            Color.Red, (float)Math.PI / 2, Vector2.Zero, new Vector2(rect.Height*scale.Y, 1), SpriteEffects.None, 0f);
        
        spriteBatch.Draw(texture, new Vector2(rect.X + rect.Width, rect.Y)*scale, null, 
            Color.Red, (float)Math.PI / 2, Vector2.Zero,   new Vector2(rect.Height*scale.Y, 1), SpriteEffects.None, 0f);
    }

    
    
    protected override void LoadContent()
    {
        textureWhite = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        textureWhite.SetData(new[] {Color.White});
        textureRed = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        textureRed.SetData(new[] {Color.Red});
        textureGreen = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        textureGreen.SetData(new[] {Color.Green});

        base.LoadContent();
    }
}