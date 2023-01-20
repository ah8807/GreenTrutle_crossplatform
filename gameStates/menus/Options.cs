using System;
using System.Xml.Linq;
using GreenTrutle_crossplatform.gameStates.gameplay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GreenTrutle_crossplatform.GameStates.menus;

public class Options:Menu
{
    private GameState prevState;
    public event EventHandler OnClickBack;
    private Gameplay gameplay;
    public Options(Gameplay gameplay)
    {
        prevState = gameplay;
        this.DrawOrder = 1;
        this.gameplay = gameplay;
        base.Initialize();
        backB.Click += (sender, args) =>
        {
            OnClickBack?.Invoke(this, EventArgs.Empty);
            SaveOptions();
        };
    }

    private void SaveOptions()
    {
        XDocument xml = new XDocument(new XElement("Root",
                                                    new XElement("Options","")));
        xml.Element("Root").Element("Options").Add(new XElement("Option",
                                                                    new XElement("Name","Score"),
                                                                    new XElement("Value",gameplay.gameHud.score.points)));
        Globals.save.saveFile(xml,"options.xml");
    }
    public override void Initialize()
    {
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}