using GreenTrutle_crossplatform.Graphics;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene;
using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.GameStates.menus;

public class Menu:GameState, IScene
{
    public Scene scene { get; set; }
    protected HudRenderer renderer;
    protected Button backB;
    
    public Menu()
    {
        scene = new Scene();
        renderer = new HudRenderer(scene);
        backB = new Button(new Rectangle(0, 0, 50, 50));
        backB.position = new Vector2(Globals.ScreenWidth/2, Globals.ScreenHeight-50);
        backB.text.text="Back";
        scene.addItem(backB);
        
        addComp(scene);
        addComp(renderer);
        Globals.debugRenderer.addScene(this);
    }

    public override void Initialize()
    {
        
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
    
}
