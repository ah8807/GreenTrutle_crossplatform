using System;
using System.Collections;
using System.Collections.Generic;
using GreenTrutle_crossplatform.gameStates.gameplay;
using GreenTrutle_crossplatform.Graphics;
using GreenTrutle_crossplatform.scene;
using GreenTrutle_crossplatform.scene.Objects;
using GreenTrutle_crossplatform.tools;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.GameStates.menus;

public class MainMenu:Menu
{
    private Button startB;
    private Gameplay gameplay; 
    public MainMenu()
    {
        this.DrawOrder = 0;
        startB = new Button(new Rectangle(0,0,50,20));
        startB = new Button(new Rectangle(0,0,50,20));
        startB.position = new Vector2(Globals.ScreenWidth/2, 50);
        startB.text.text="STart";
        scene.addItem(startB);
        
        renderer.transparent = false;
        
        startB.Click += (sender, e) =>
        {
            gameplay = new Gameplay(new Level());
            gameplay.activate();
            /*gameplay.OnClose += (sender, args) =>
            {
                gameplay?.Close();
                gameplay = null;
                activate();
            };
            */
            gameplay.OnClose += (object? sender, EventArgs args) =>
            {
                gameplay.Close();
                activate();
                cleanUpComponents();
            };
            deactivate();
            startB.Unlock();
        };
        

    }
    public override void Initialize()
    {
        
        
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
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}