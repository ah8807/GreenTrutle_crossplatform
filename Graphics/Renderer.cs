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
    private AnimatedSprite rexWalkingSprite = new AnimatedSprite();
    Level level;
    private Vector2 gameScale;

    public Renderer(Level level) : base()
    {
        DrawOrder = 0;
        this.level = level;
        gameScale = new Vector2(ScreenWidth, ScreenHeight) / new Vector2(240, 135);
    }

    public override void Initialize()
    {

        base.Initialize();
    }

    protected override void LoadContent()
    {

        turtleSprite.texture = Globals.game.Content.Load<Texture2D>("Sprite-0002");
        turtleSprite.sourceRectangle = new Rectangle(0, 1, 16, 15);
        turtleSprite.origin = new Vector2((turtleSprite.texture.Width / 6)/2, turtleSprite.texture.Height/2);

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
        rexWalkingSprite.speed = 20;
        base.LoadContent();
    }

    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        //render igro v originalni velikosti
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.Draw(labirintSprite.texture, Vector2.Zero, labirintSprite.sourceRectangle, Color.White, 0f, labirintSprite.origin, gameScale, SpriteEffects.None, 0);
        foreach (DrawableGameObject o in level.scene)
        {
            Sprite sprite = null;
            Vector2 scale = gameScale;
            switch (o){
                case Turtle t:
                    if(o.velocity==Vector2.Zero)
                        sprite = turtleSprite;
                    else
                        sprite = turtleWalkingSprite;
                    break;
                case Lettuce l:
                    sprite = lettuceSprite;
                    scale = new Vector2(0.5f, 0.5f)*gameScale;
                    break;
                case Trex tr:
                    sprite = rexWalkingSprite;
                    break;
                case Wall wall:
                    sprite = lettuceSprite;
                    scale = new Vector2(0.5f, 0.5f)*gameScale;
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
                    spriteBatch.Draw(sprite.texture, drawPosition*gameScale, sprite.sourceRectangle, Color.White, oRSE.rotation, sprite.origin, scale, oRSE.effect, 0);
                }
                else
                {
                    spriteBatch.Draw(sprite.texture, drawPosition*gameScale, sprite.sourceRectangle, Color.White, 0f,
                        sprite.origin, scale, SpriteEffects.None, 0);
                }
            }
        }
        spriteBatch.End();

        base.Draw(gameTime);
    }

}