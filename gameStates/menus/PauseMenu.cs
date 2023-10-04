using System;
using System.Xml.Linq;
using GreenTrutle_crossplatform.gameStates.gameplay;
using GreenTrutle_crossplatform.Graphics;
using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GreenTrutle_crossplatform.GameStates.menus;

public class PauseMenu:Menu
{
    private GameState prevState;
    public event EventHandler OnClickBack;
    private GameState gameplay;
    private Button exitB;
    public PauseMenu(GameState gameplay) : base(gameplay)
    {
        prevState = gameplay;
        this.gameplay = gameplay;
        base.Initialize();
        
        renderer.transparent = true;
        
        backB.position = new Vector2(Globals.ScreenWidth/2, Globals.ScreenHeight*0.1f);
        backB.text.text = "Resume";

        exitB = new Button(new Rectangle(0, 0, 50, 50));
        exitB.position = new Vector2(Globals.ScreenWidth/2, Globals.ScreenHeight*0.9f);
        
        scene.addItem(exitB);
        
        exitB.Click += (sender, args) =>
        {
            OnClickBack?.Invoke(this, EventArgs.Empty);
            SaveOptions();
        };
        
        
    }

    private void SaveOptions()
    {
        // XDocument xml = new XDocument(new XElement("Root",
        //                                             new XElement("Options","")));
        // xml.Element("Root").Element("Options").Add(new XElement("Option",
        //                                                             new XElement("Name","Score"),
        //                                                             new XElement("Value",gameplay.gameHud.score.points)));
        // Globals.save.saveFile(xml,"options.xml");
    }
    public override void Initialize()
    {
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}