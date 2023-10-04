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
    public List<object> scenes = new List<object>();
    public List<Vector2> wayPoints = new List<Vector2>();
    public DebugRenderer()
    {
        DrawOrder = 2;
    }

    public void addScene(object obj)
    {   
        scenes.Add(obj);
    }
    public void removeScene(object obj)
    {   
        if(scenes.Contains(obj))
            scenes.Remove(obj);
    }
    public override void Draw(GameTime gameTime)
    {
        foreach (DrawableGameComponent o in scenes)
        {
            if(!o.Enabled)
                continue;
            IScene scene = o is IScene ? (IScene)o : null;
            if(scene == null)
                continue;
            foreach (DrawableGameObject gameObject in scene.scene)
            {
                Texture2D texture= textureRed;
                IParticle particle = gameObject is IParticle ? (IParticle)gameObject.Clone() : null;
                if(particle == null)
                    continue;
                if (scene is EmptyLevel)
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
                    particle.aabb = new Rectangle(0, 0, (int)(particle.aabb.Width * hudScale.X),
                        (int)(particle.aabb.Height * hudScale.Y));
                    DrawRectangleHudScale(particle.getRect(),Globals.spriteBatch,texture,hudScale);
                    spriteBatch.End();
                }
            }
        }

        foreach (Vector2 point in wayPoints)
        {
            Vector2 cord = point * gameScale;
            spriteBatch.Begin();
            spriteBatch.Draw(textureRed, new Rectangle((int)cord.X-2, (int)cord.Y-2, 4, 4), Color.White);
            spriteBatch.End();
        }
        wayPoints.Clear();
        base.Draw(gameTime);
    }

}