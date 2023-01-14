using GreenTrutle_crossplatform.gameStates.gameplay;
using GreenTrutle_crossplatform.Graphics;
using GreenTrutle_crossplatform.scene;
using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.GameStates.menus;

public class MainMenu:Menu
{
    private Button startB;
    private Gameplay gameplay; 
    public MainMenu()
    {
        startB = new Button(new Rectangle(0,0,50,20));
        
    }
    public override void Initialize()
    {
        base.Initialize();
        startB = new Button(new Rectangle(0,0,50,20));
        startB.position = new Vector2(Globals.ScreenWidth/2, 50);
        startB.text.text="STart";
        scene.addItem(startB);
        
        renderer.transparent = false;
        
        startB.Click += (sender, e) =>
        {
            gameplay = new Gameplay(new Level());
            gameplay.activate();
            gameplay.OnClose += (sender, args) =>
            {
                gameplay?.deactivate();
                gameplay = null;
                activate();
            };
            this.deactivate();
        };
        
        
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}