using System.Collections.Generic;
using System.Security.AccessControl;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene;
using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GreenTrutle_crossplatform.Graphics;

public class DebugRenderer: RenderBase
{
    private Vector2 gameScale = new Vector2(ScreenWidth, ScreenHeight) / new Vector2(240, 135);
    private List<object> objects = new List<object>();
    public DebugRenderer()
    {
        DrawOrder = 2;
    }

    public void addScene(object obj)
    {   
        objects.Add(obj);
    }
    public void removeScene(object obj)
    {   
        if(objects.Contains(obj))
            objects.Remove(obj);
    }
    public override void Draw(GameTime gameTime)
    {
        foreach (DrawableGameComponent o in objects)
        {
            if(!o.Enabled)
                continue;
            IScene scene = o is IScene ? (IScene)o : null;
            if(scene == null)
                continue;
            foreach (DrawableGameObject gameObject in scene.scene)
            {
                Texture2D texture= textureRed;
                IParticle particle = gameObject is IParticle ? (IParticle)gameObject : null;
                if(particle == null)
                    continue;
                if (scene is Level)
                {
                    spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    if (particle is Trex&&((Trex)particle).hunting)
                        texture = textureGreen;
                    DrawRectangle(particle.getRect(),Globals.spriteBatch,texture,gameScale);
                    spriteBatch.End();
                }
                else
                {
                    spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    DrawRectangle(particle.getRect(),Globals.spriteBatch,texture,Vector2.One);
                    spriteBatch.End();
                }
            }
        }
    }

}