using System;
using System.Collections.Generic;
using System.Xml.Linq;
using GreenTrutle_crossplatform.scene;
using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.GameStates.menus;

public class SettingsMenu:Menu
{
    private List<DrawableGameObject> settings;
    private Slider SFXVolumeS;
    private Slider musicVolumeS;
    private Button applyB;
    public SettingsMenu(GameState prevState) : base(prevState)
    {
        settings = new List<DrawableGameObject>();
        
        musicVolumeS = new Slider();
        musicVolumeS.position = new Vector2(Globals.ScreenWidth/2, Globals.ScreenHeight/2);
        musicVolumeS.aabb = new Rectangle(0, 0, 200, 10);
        musicVolumeS.name = "Music volume";
        musicVolumeS.directValue = (int)(Globals.soundControl.musicV * 100f);
        
        SFXVolumeS = new Slider();
        SFXVolumeS.position = new Vector2(Globals.ScreenWidth/2, musicVolumeS.position.Y+musicVolumeS.knob.aabb.Height*2);
        SFXVolumeS.aabb = new Rectangle(0, 0, 200, 10);
        SFXVolumeS.name = "SFX volume";
        SFXVolumeS.directValue = (int)(Globals.soundControl.SFXV * 100f);
        
        applyB = new Button(new Rectangle(0,0,50,20));
        applyB = new Button(new Rectangle(0,0,50,20));
        applyB.position = new Vector2(Globals.ScreenWidth/2, backB.position.Y-backB.aabb.Height);
        applyB.text.text="Apply";
        
        
        settings.Add(musicVolumeS);
        settings.Add(SFXVolumeS);
        scene.addItem(musicVolumeS);
        scene.addItem(applyB);
        scene.addItem(SFXVolumeS);

        musicVolumeS.Click += ChangeMusicVolume;
        SFXVolumeS.Click += ChangeSFXVolume;
        applyB.Click += applySettings;
        backB.Click += restoreValues;
    }

    
    public void restoreValues(object o , EventArgs args)
    {
        Globals.soundControl.initialize();
    }

    public void ChangeMusicVolume(object o, EventArgs args)
    {
        Globals.soundControl.adjustMusicVolume(((Slider)o).value/100f);
    }
    public void ChangeSFXVolume(object o, EventArgs args)
    {
        Globals.soundControl.adjustSFXVolume(((Slider)o).value/100f);
    }
    public void applySettings(object o, EventArgs args)
    {
        XDocument xml = new XDocument(new XElement("Root",
            new XElement("Settings","")));
        foreach (DrawableGameObject obj in settings)
        {
            xml.Element("Root").Element("Settings").Add(obj.GetXML());
        }
        Globals.save.saveFile(xml,"settings.xml");
        goBack(o,args);
    }
    
    
}