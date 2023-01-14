namespace GreenTrutle_crossplatform.Graphics;

using System.Net.Security;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using GreenTrutle_crossplatform.Sprites;
using GreenTrutle_crossplatform.scene;
using GreenTrutle_crossplatform.scene.Objects;
using tainicom.Aether.Physics2D.Fluids;
using GreenTrutle_crossplatform.interfaces;
using global::GreenTrutle_crossplatform.interfaces;
using global::GreenTrutle_crossplatform.scene.Objects;
using global::GreenTrutle_crossplatform.scene;
using global::GreenTrutle_crossplatform.Sprites;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Emit;
using GreenTrutle_crossplatform.scene.Objects;


public class HudRenderer : DrawableGameComponent
{
    public bool transparent = true;
    SpriteBatch spriteBatch;
    RenderTarget2D renderTarget;
    const int ScreenWidth = 1920 / 2;
    const int ScreenHeight = 1080 / 2;
    static Rectangle CANVAS = new Rectangle(0, 0, 240, 135);
    private Texture2D buttonTexture;
    SpriteFont font;

    Scene scene;

    public HudRenderer(Scene scene) : base(Globals.game)
    {
        this.spriteBatch = Globals.spriteBatch;
        this.scene = scene;
    }

    public override void Initialize()
    {
        Globals.graphics.PreferMultiSampling = false;
        Globals.graphics.SynchronizeWithVerticalRetrace = true;

        Globals.graphics.PreferredBackBufferWidth = ScreenWidth;
        Globals.graphics.PreferredBackBufferHeight = ScreenHeight;

        Globals.graphics.IsFullScreen = false;  //k zaklučeš game dj na true

        Globals.graphics.ApplyChanges();

        renderTarget = new RenderTarget2D(GraphicsDevice, CANVAS.Width, CANVAS.Height);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        font = Globals.game.Content.Load<SpriteFont>("GUI/cupHead_font");
        
        buttonTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        buttonTexture.SetData(new[] {Color.White});

        base.LoadContent();
    }

    public override void Draw(GameTime gameTime)
    {
        //render igro v originalni velikosti




        //upscale
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        if(!transparent)
            GraphicsDevice.Clear(Color.CornflowerBlue);
        foreach (DrawableGameObject o in scene)
        {
            scene.Objects.Text text;
            Vector2 textMiddlePoint;
            switch (o)
            {
                case scene.Objects.Text t:
                    text = (scene.Objects.Text)o;
                    textMiddlePoint = font.MeasureString(text.text) / 2;
                    spriteBatch.DrawString(font, text.text, text.position, Color.White, 0, textMiddlePoint, 1.0f,
                        SpriteEffects.None, 0.5f);

                    break;
                case Button b:
                    Button button = (Button)o;
                    text = button.text;
                    textMiddlePoint = font.MeasureString(text.text) / 2;
                    spriteBatch.Draw(buttonTexture, button.position, new Rectangle(0,0,2,2), Color.White, 0,Vector2.One, new Vector2(button.hitbox.Width,button.hitbox.Height), SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, text.text, button.position, Color.Black, 0, textMiddlePoint, 1.0f,
                        SpriteEffects.None, 1);
                    break;
                    
            }
        }
        spriteBatch.End();

        base.Draw(gameTime);
    }

}