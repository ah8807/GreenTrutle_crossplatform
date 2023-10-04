using System;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.Physics;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.scene.Objects;
[Serializable] 
public class Brick: DrawableGameObject, IParticle, ICustomCollider,IStatic
{
    
    public Rectangle aabb { get; set; }
    public Brick()
    {
        aabb = new Rectangle(0, 0, 2, 2);

    }
    public bool collidingWithItem(object item)
    {
        return false;
    }

    public void collidedWithItem(object item)
    {

    }
}