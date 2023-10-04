using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.scene.Levels;

public class CollTestLevel:Level
{
    public Lettuce lettuce { get; set; }
    
    public CollTestLevel()
    {

        turtle= new Turtle();
        turtle.position = new Vector2(8,8);

        lettuce = new Lettuce();
        lettuce.position = new Vector2(25, 7);
        
        Lettuce lettuce1 = new Lettuce();
        lettuce1.position = new Vector2(25, 40);

        Lettuce lettuce2 = new Lettuce();
        lettuce2.position = new Vector2(25, 50);
        Lettuce lettuce3 = new Lettuce();
        lettuce3.position = new Vector2(25, 60);
        Lettuce lettuce4 = new Lettuce();
        lettuce4.position = new Vector2(25, 70);
        trex = new Trex();
        trex.position = new Vector2(25, 20);
        
        scene.addItem(turtle);
        scene.addItem(lettuce);
        scene.addItem(trex);
        scene.addItem(lettuce1);
        scene.addItem(lettuce2);
        scene.addItem(lettuce3);
        scene.addItem(lettuce4);
        
        scene.Update(Globals.gameTime);
    }
}