using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.scene.Levels;

public class LevelOne:Level
{

    public Lettuce lettuce { get; set; }
    public Lettuce lettuce1 { get; set; }
    public Lettuce lettuce2 { get; set; }
    public Lettuce lettuce3 { get; set; }

    
    public LevelOne()
    {
        turtle= new Turtle();
        turtle.position = new Vector2(8,7);

        lettuce = new Lettuce();
        lettuce.position = new Vector2(25, 7);

        lettuce1 = new Lettuce();
        lettuce1.position = new Vector2(40, 7);

        lettuce2 = new Lettuce();
        lettuce2.position = new Vector2(40, 21);

        lettuce3 = new Lettuce();
        lettuce3.position = new Vector2(40, 35);

        trex = new Trex();
        trex.position = new Vector2(25, 7);
        
        scene.addItem(turtle);
        scene.addItem(lettuce);
        scene.addItem(trex);
        scene.addItem(lettuce1);
        scene.addItem(lettuce2);
        scene.addItem(lettuce3);

    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}