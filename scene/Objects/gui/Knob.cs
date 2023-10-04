using GreenTrutle_crossplatform.interfaces;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.scene.Objects;

public class Knob: DrawableGameObject, IParticle
{
    public Rectangle aabb { get; set; }
}