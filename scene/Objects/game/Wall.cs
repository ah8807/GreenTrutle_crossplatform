using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.Physics;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.scene.Objects;

public class Wall: DrawableGameObject, IParticle, ICustomCollider
{
    public Rectangle aabb { get; set; }
    public Wall()
    {
        aabb = new Rectangle(0, 0, 13, 13);

    }
    public bool collidingWithItem(object item)
    {
        return false;
    }

    public void collidedWithItem(object item)
    {

    }
}