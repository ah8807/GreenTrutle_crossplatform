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
    EmptyLevel level;

    public Renderer(EmptyLevel level) : base()
    {
        DrawOrder = 0;
        this.level = level;
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override void Dispose(bool disposing)
    {
        turtleWalkingSprite.Close();
        rexWalkingSprite.Close();
        
        base.Dispose(disposing);
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
                    DrawSprite(o,sprite,scale*t.scale);
                    break;
                case Lettuce l:
                    sprite = lettuceSprite;
                    scale = new Vector2(0.5f, 0.5f)*gameScale;
                    DrawSprite(o,sprite,scale);
                    break;
                case Trex tr:
                    sprite = rexWalkingSprite;
                    DrawSprite(o,sprite,scale);
                    break;
                case Brick brick:
                    sprite = lettuceSprite;
                    scale = new Vector2(2f/16f, 2f / 16f)*gameScale;
                    DrawSprite(o,sprite,scale);
                    break;
                case Wall wall:
                    sprite = lettuceSprite;
                    scale = new Vector2(2f/16f, 2f / 16f)*gameScale;
                    drawArrayOfSprites(o,sprite,scale);
                    break;
            }
        }
        spriteBatch.End();

        base.Draw(gameTime);
    }



    public void DrawSprite(DrawableGameObject o, Sprite sprite, Vector2 scale)
    {
        if (o is IParticle && sprite != null)
        {
            AnimatedSprite animatesSprite = sprite is AnimatedSprite ? (AnimatedSprite)sprite : null;
            if (animatesSprite != null)
            {
                sprite = animatesSprite.getFrame();
            }

            IParticle p = (IParticle)o;
            // Vector2 drawPosition = new Vector2((int)o.position.X-p.aabb.Width/2, (int)o.position.Y-p.aabb.Height/2);
            Vector2 drawPosition = new Vector2((int)o.position.X, (int)o.position.Y);
            // Vector2 drawPosition = new Vector2(o.position.X, o.position.Y);

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

    public void drawArrayOfSprites(DrawableGameObject obj, Sprite sprite, Vector2 scale)
    {
        foreach (DrawableGameObject o in (obj as ICombinedGameObject).game_objects)
        {
            if (o is IParticle && sprite != null)
            {
                AnimatedSprite animatesSprite = sprite is AnimatedSprite ? (AnimatedSprite)sprite : null;
                if (animatesSprite != null)
                {
                    sprite = animatesSprite.getFrame();
                }

                IParticle p = (IParticle)o;
                // Vector2 drawPosition = new Vector2((int)o.position.X-p.aabb.Width/2, (int)o.position.Y-p.aabb.Height/2);
                Vector2 drawPosition = new Vector2((int)o.position.X, (int)o.position.Y);
                // Vector2 drawPosition = new Vector2(o.position.X, o.position.Y);

                if (o is IRSE)
                {
                    IRSE oRSE = (IRSE)o;
                    spriteBatch.Draw(sprite.texture, drawPosition * gameScale, sprite.sourceRectangle, Color.White,
                        oRSE.rotation, sprite.origin, scale, oRSE.effect, 0);
                }
                else
                {
                    spriteBatch.Draw(sprite.texture, drawPosition * gameScale, sprite.sourceRectangle, Color.White, 0f,
                        sprite.origin, scale, SpriteEffects.None, 0);
                }
            }
        }
    }

}