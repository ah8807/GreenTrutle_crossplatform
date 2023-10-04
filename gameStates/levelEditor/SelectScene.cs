using System;
using System.Collections.Generic;
using GreenTrutle_crossplatform.gameStates.gameplay;
using GreenTrutle_crossplatform.GameStates.menus;
using GreenTrutle_crossplatform.scene.Levels;
using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.GameStates.levelEditor;

public class SelectScene:Menu
{
    private List<Button> buttons;
    private Button level_one;
    private Button level_two;
    private Button level_three;
    public SelectScene(GameState prevState) : base(prevState)
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
                OpenLevelEditor("scene1.xml");
                // StartGameplay(new LevelOne(),null);
                break;
            
            case "2":
                OpenLevelEditor("scene2.xml");
                break;
            
            case "3":
                OpenLevelEditor("scene3.xml");
                break;
        }
        
    }
    
    private void OpenLevelEditor(string? scenePath)
    {
        // gameplay = new Gameplay(new Level());

        levelEditor = new LevelEditor(this,scenePath);
        levelEditor.activate();
        /*gameplay.OnClose += (sender, args) =>
        {
            gameplay?.Close();
            gameplay = null;
            activate();
        };
        */
        deactivate();
    }

    public LevelEditor levelEditor { get; set; }
}