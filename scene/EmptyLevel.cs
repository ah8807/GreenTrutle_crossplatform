using GreenTrutle_crossplatform.interfaces;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.scene.Objects;

public class EmptyLevel: DrawableGameComponent,IScene
{
    public Scene scene { get; set; }

    public EmptyLevel():base(Globals.game)
    {
        scene = new Scene();
        Globals.debugRenderer.addScene(this);
    }
}