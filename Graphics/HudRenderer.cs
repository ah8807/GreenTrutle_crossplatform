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


public class HudRenderer : RenderBase
{
    public bool transparent = false;
    RenderTarget2D renderTarget;
    static Rectangle CANVAS = new Rectangle(0, 0, 240, 135);
    SpriteFont font;
    private Sprite turtleSprite=new Sprite();
    Scene scene;

    public HudRenderer(Scene scene) : base()
    {
        DrawOrder = 1;
        this.spriteBatch = Globals.spriteBatch;
        this.scene = scene;
    }

    public override void Initialize()
    {

        base.Initialize();
    }

    protected override void LoadContent()
    {
        Globals.font = Globals.game.Content.Load<SpriteFont>("GUI/cupHead_font");
        
        turtleSprite.texture = Globals.game.Content.Load<Texture2D>("Sprite-0002");
        turtleSprite.sourceRectangle = new Rectangle(0, 1, 16, 15);
        turtleSprite.origin = new Vector2((turtleSprite.texture.Width / 6)/2, turtleSprite.texture.Height/2);
        

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
                    t.draw(hudScale);
                    break;
                case Button b:
                    Button button = (Button)o;
                    text = button.text;
                    textMiddlePoint = Globals.font.MeasureString(text.text) / 2;
                    Vector2 buttonScale = new Vector2((float)button.aabb.Width / textureWhite.Width, (float)button.aabb.Height / textureWhite.Height);
                    spriteBatch.Draw(textureWhite, button.position,textureWhite.Bounds , Color.White, 0,new Vector2(textureWhite.Bounds.Width/2f, textureWhite.Bounds.Height/2f), buttonScale*hudScale, SpriteEffects.None, 0);
                    spriteBatch.DrawString(Globals.font, text.text, button.position, Color.Black, 0, textMiddlePoint, 1.0f*hudScale,
                        SpriteEffects.None, 1);
                    
                    Rectangle rect = new Rectangle((int)button.position.X - button.aabb.Width / 2,
                        (int)button.position.Y - button.aabb.Height / 2, button.aabb.Width, button.aabb.Height);
                    break;
                case Lives l:
                    Lives lives = (Lives)l;
                    lives.sprite = turtleSprite;
                    lives.Update(hudScale);
                    foreach (Vector2 position in lives.positions)
                    {
                        spriteBatch.Draw(turtleSprite.texture, position,turtleSprite.sourceRectangle , Color.White, 0,turtleSprite.origin, lives.textScale, SpriteEffects.None, 0);
                    }
                    break;
                case Slider s:
                    Slider slider = (Slider)s;
                    Vector2 sliderScale = new Vector2((float)slider.aabb.Width / textureWhite.Width, (float)slider.aabb.Height / textureWhite.Height);
                    Vector2 knobScale = new Vector2((float)slider.knob.aabb.Width / textureWhite.Width, (float)slider.knob.aabb.Height / textureWhite.Height);
                    spriteBatch.Draw(textureWhite, slider.position,textureWhite.Bounds , Color.White, 0,new Vector2(textureWhite.Bounds.Width/2f, textureWhite.Bounds.Height/2f), sliderScale*hudScale, SpriteEffects.None, 0);  
                    spriteBatch.Draw(textureRed, slider.knob.position, textureRed.Bounds, Color.White, 0, new Vector2(textureWhite.Bounds.Width/2f, textureWhite.Bounds.Height/2f), knobScale*hudScale,
                        SpriteEffects.None, 1);
                    slider.nameT.draw(hudScale);
                    slider.valueT.draw(hudScale);
                    break;
                    
            }
        }
        spriteBatch.End();

        base.Draw(gameTime);
    }
    

}

