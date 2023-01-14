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

namespace GreenTrutle_crossplatform.Graphics;

public class Renderer : RenderBase
{
    Sprite turtleSprite = new Sprite();
    AnimatedSprite turtleWalkingSprite = new AnimatedSprite();
    Sprite labirintSprite = new Sprite();
    Sprite lettuceSprite = new Sprite();
    Sprite rexSprite = new Sprite();
    AnimatedSprite rexWalkingSprite = new AnimatedSprite();

    RenderTarget2D renderTarget;
    const int ScreenWidth = 1920/2;
    const int ScreenHeight = 1080/2;
    static Rectangle CANVAS = new Rectangle(0, 0, 240 , 135 );

    Level level;

    public Renderer(Level level) : base()
    {
        this.DrawOrder = 0;
        this.level = level;
        Globals.graphics.PreferMultiSampling = false;
        Globals.graphics.SynchronizeWithVerticalRetrace = true;

        Globals.graphics.PreferredBackBufferWidth = ScreenWidth;
        Globals.graphics.PreferredBackBufferHeight = ScreenHeight;

        Globals.graphics.IsFullScreen = false;  //k zaklučeš game dj na true

        Globals.graphics.ApplyChanges();

        renderTarget = new RenderTarget2D(GraphicsDevice, CANVAS.Width, CANVAS.Height);
    }

    public override void Initialize()
    {

        base.Initialize();
    }

    protected override void LoadContent()
    {

        turtleSprite.texture = Globals.game.Content.Load<Texture2D>("Sprite-0002");
        turtleSprite.sourceRectangle = new Rectangle(0, 0, 16, 16);
        turtleSprite.origin = new Vector2(turtleSprite.sourceRectangle.Width/2, turtleSprite.sourceRectangle.Height/2);

        rexSprite.texture = Globals.game.Content.Load<Texture2D>("tiranozaver");
        rexSprite.sourceRectangle = new Rectangle(0, 0, 16, 16);
        rexSprite.origin = new Vector2(8, 9);

        labirintSprite.texture = Globals.game.Content.Load<Texture2D>("labirinth");
        labirintSprite.sourceRectangle = new Rectangle(0, 0, 240, 135);
        labirintSprite.origin = new Vector2(0, 0);

        lettuceSprite.texture = Globals.game.Content.Load<Texture2D>("solata");
        lettuceSprite.sourceRectangle = new Rectangle(1, 1, 14, 14);
        lettuceSprite.origin = new Vector2(6,6);

        turtleWalkingSprite.texture= Globals.game.Content.Load<Texture2D>("Sprite-0002");
        turtleWalkingSprite.Columns = 6;
        turtleWalkingSprite.Rows = 1;
        turtleWalkingSprite.origin = new Vector2(8, 9);

        rexWalkingSprite.texture = Globals.game.Content.Load<Texture2D>("tiranozaver");
        rexWalkingSprite.Columns = 7;
        rexWalkingSprite.Rows = 1;
        rexWalkingSprite.origin = new Vector2(8, 9);
        rexWalkingSprite.updateTimer = 20;
        base.LoadContent();
    }

    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(renderTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);
        //render igro v originalni velikosti
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.Draw(labirintSprite.texture, Vector2.Zero, labirintSprite.sourceRectangle, Color.White, 0f, labirintSprite.origin, Vector2.One, SpriteEffects.None, 0);
        foreach (DrawableGameObject o in level.scene)
        {
            Sprite sprite = null;
            Vector2 scale = Vector2.One;
            switch (o){
                case Turtle t:
                    //sprite = turtleSprite;
                    sprite = turtleWalkingSprite;
                    break;
                case Lettuce l:
                    sprite = lettuceSprite;
                    scale = new Vector2(0.5f, 0.5f);
                    break;
                case Trex tr:
                    sprite = rexWalkingSprite;
                    break;
            }
            if (o is IParticle && sprite != null)
            {
                AnimatedSprite animatesSprite = sprite is AnimatedSprite ? (AnimatedSprite)sprite : null;
                if (animatesSprite != null)
                {
                    sprite = animatesSprite.getFrame(gameTime);
                }
                Vector2 drawPosition = new Vector2((int)o.position.X, (int)o.position.Y);

                if (o is IRSE) { 
                    IRSE oRSE = (IRSE)o;
                    spriteBatch.Draw(sprite.texture, drawPosition, sprite.sourceRectangle, Color.White, oRSE.rotation, sprite.origin, oRSE.scale, oRSE.effect, 0);
                }
                else
                {
                    spriteBatch.Draw(sprite.texture, drawPosition, sprite.sourceRectangle, Color.White, 0f,
                        sprite.origin, scale, SpriteEffects.None, 0);
                }

                IParticle p = (IParticle)o;
                Rectangle rect = p.getRect();
                DrawRectangle(rect, spriteBatch);
            }
        }
        spriteBatch.End();
        GraphicsDevice.SetRenderTarget(null);

        //upscale
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.Draw(renderTarget, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
        spriteBatch.End();

        base.Draw(gameTime);
    }

}