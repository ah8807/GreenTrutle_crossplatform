using System;
using System.Collections.Generic;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;
namespace GreenTrutle_crossplatform.scene;

public class Level : EmptyLevel
{
    public Trex trex { get; set; }
    public Turtle turtle { get; set; }

    public Level():base()
    {

    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

}