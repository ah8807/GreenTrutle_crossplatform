using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    public Menu(GameState prevState) : base(prevState)
    {
        if (prevState == null)
            DrawOrder = 0;
        else
            DrawOrder = prevState.DrawOrder + 1;
        scene = new Scene();
        renderer = new HudRenderer(scene);
        backB = new Button(new Rectangle(0, 0, 50, 50));
        backB.position = new Vector2(Globals.ScreenWidth/2, Globals.ScreenHeight*0.9f);
        backB.text.text="Back";
        scene.addItem(backB);
        
        addComp(scene);
        addComp(renderer);
        Globals.debugRenderer.addScene(this);

        
        backB.Click += goBack;
    }

    protected void goBack(object o,EventArgs args)
    {
        base.goBack(o,args);
        backB.Unlock();
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
            Slider slider = o is Slider ? (Slider)o : null;
            if (slider != null)
            {
                slider.Update();
            }
        }
    }
    
    public void cleanUpComponents()
    {
        List<GameComponent> removeList = new List<GameComponent>();
        IEnumerator e = Globals.game.Components.GetEnumerator();
        while (e.MoveNext())
        {
            if ((e.Current is DrawableGameComponent&&comps.Contains((DrawableGameComponent)e.Current))||e.Current is DebugRenderer)
            {
                continue;
            }
            removeList.Add((GameComponent)e.Current);
        }

        foreach (Object comp in removeList)
        {
            GameComponent gcomp = (GameComponent)comp;
            Globals.game.Components.Remove(gcomp);
        }
    }
    public override void activate()
    {
        if(!Globals.debugRenderer.scenes.Contains(scene))
            Globals.debugRenderer.addScene(scene);
        base.activate();
    }
    
}
