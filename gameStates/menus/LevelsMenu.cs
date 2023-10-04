using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using GreenTrutle_crossplatform.gameStates.gameplay;
using GreenTrutle_crossplatform.scene;
using GreenTrutle_crossplatform.scene.Levels;
using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.GameStates.menus;

public class LevelsMenu:Menu
{

    private List<Button> buttons;
    private Button level_one;
    private Button level_two;
    private Button level_three;
    public LevelsMenu(GameState prevState) : base(prevState)
    {
        level_one = new Button(new Rectangle(0,0,50,50));
        level_one.position = new Vector2(Globals.ScreenWidth*0.3f, Globals.ScreenHeight/2);
        level_one.text.text = "1";
        
        level_two = new Button(new Rectangle(0,0,50,50));
        level_two.position = new Vector2(Globals.ScreenWidth/2, Globals.ScreenHeight/2);
        level_two.text.text = "2";
        
        level_three = new Button(new Rectangle(0,0,50,50));
        level_three.position = new Vector2(Globals.ScreenWidth*.7f, Globals.ScreenHeight/2);
        level_three.text.text = "3";
        
        
        
        scene.addItem(level_one);
        scene.addItem(level_two);
        scene.addItem(level_three);

        level_one.Click += StartLevel;
        level_two.Click += StartLevel;
        level_three.Click += StartLevel;
    }

    public void StartLevel(object o, EventArgs args)
    {
        switch (((Button)o).text.text)
        {
            case "1":
                StartGameplay(null,"scene1.xml");
                // StartGameplay(new LevelOne(),null);
                break;
            
            case "2":
                StartGameplay(null,"scene2.xml");
                break;
            
            case "3":
                StartGameplay(null,"scene3.xml");
                break;
        }
        
    }
    
    private void StartGameplay(EmptyLevel? level,String? scenePath)
    {
        // gameplay = new Gameplay(new Level());
        if(scenePath==null)
            gameplay = new Gameplay(this,new CollTestLevel());
        if (level == null)
            gameplay = new Gameplay(this,scenePath);
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
    }

    public Gameplay gameplay { get; set; }
}