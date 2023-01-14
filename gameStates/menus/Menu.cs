using GreenTrutle_crossplatform.Graphics;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene;
using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.GameStates.menus;

public class Menu:GameState
{
    protected Scene scene;
    protected HudRenderer renderer;
    protected Button backB;
    
    public Menu()
    {
        scene = new Scene();
        renderer = new HudRenderer(scene);
    }

    public override void Initialize()
    {
        backB = new Button(new Rectangle(0, 0, 50, 50));
        backB.position = new Vector2(Globals.ScreenWidth/2, Globals.ScreenHeight-50);
        backB.text.text="Back";
        scene.addItem(backB);
        
        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        
        foreach (DrawableGameObject o in scene)
        {
            Button button = o is Button ? (Button)o : null;
            if (button != null)
            {
                button.Update();
            }
        }
    }

    public override void activate()
    {
        addComp(this);
        addComp(renderer);
        addComp(scene);
    }

    public override void deactivate()
    {
        removeComp(this);
        removeComp(renderer);
        removeComp(scene);
    }
    
}
