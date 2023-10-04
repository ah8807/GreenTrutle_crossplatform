using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using GreenTrutle_crossplatform.gameStates.gameplay;
using GreenTrutle_crossplatform.GameStates.levelEditor;
using GreenTrutle_crossplatform.Graphics;
using GreenTrutle_crossplatform.scene;
using GreenTrutle_crossplatform.scene.Levels;
using GreenTrutle_crossplatform.scene.Objects;
using GreenTrutle_crossplatform.tools;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.GameStates.menus;

public class MainMenu:Menu
{
    private Button startB;
    private Button setingsB;
    private Gameplay gameplay;
    private Slider slider;
    private SettingsMenu settingsMenu;
    private Button levelEditB;
    public MainMenu(GameState prevState) : base(prevState)
    {
        startB = new Button(new Rectangle(0,0,50,20));
        startB = new Button(new Rectangle(0,0,50,20));
        startB.position = new Vector2(Globals.ScreenWidth/2, Globals.ScreenHeight*0.1f);
        startB.text.text = "Start";
        
        levelEditB = new Button(new Rectangle(0,0,50,20));
        levelEditB = new Button(new Rectangle(0,0,50,20));
        levelEditB.position = new Vector2(Globals.ScreenWidth/2, startB.position.Y+startB.aabb.Height*3);
        levelEditB.text.text = "Level editor";

        setingsB = new Button(new Rectangle(0,0,50,20));
        setingsB = new Button(new Rectangle(0,0,50,20));
        setingsB.position = new Vector2(Globals.ScreenWidth/2, Globals.ScreenHeight/2);
        setingsB.text.text="Settings";

        // slider = new Slider();
        // slider.position = new Vector2(Globals.ScreenWidth/2, Globals.ScreenHeight/2);
        // slider.aabb = new Rectangle(0, 0, 200, 50);
        
        scene.addItem(startB);
        scene.addItem(setingsB);
        scene.addItem(levelEditB);
        // scene.addItem(slider);
        
        renderer.transparent = false;
        
        startB.Click += OpenLevelsMenu;
        
        setingsB.Click += OpenSettings;

        levelEditB.Click += OpenLevelEditor;
    }

    private SelectScene _selectScene;

    private void OpenLevelEditor(object o, EventArgs args)
    {
        _selectScene = new SelectScene(this);
        _selectScene.activate();
        deactivate();
        startB.Unlock();
    }
    public LevelsMenu levelsMenu { get; set; }
    private void OpenLevelsMenu(object o, EventArgs args)
    {
        levelsMenu = new LevelsMenu(this);
        levelsMenu.activate();
        deactivate();
        startB.Unlock();
    }


    private void StartGameplay(object sender, EventArgs args)
    {
        // gameplay = new Gameplay(new Level());
        gameplay = new Gameplay(this,new CollTestLevel());
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
    }

    private void OpenSettings(object o, EventArgs args)
    {
        settingsMenu = new SettingsMenu(this);
        settingsMenu.activate();
        deactivate();
        startB.Unlock();
    }

    public override void Initialize()
    {
        
        
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}