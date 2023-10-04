using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.tools;
using Microsoft.Xna.Framework.Audio;

namespace GreenTrutle_crossplatform.scene;

public class GameHud : DrawableGameComponent, IDisposable, IScene
{
    /*
    private static GameHud _instance; 
    
    public static GameHud GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameHud();
            XDocument xml = Globals.save.GetFile("options.xml");
            _instance.LoadData(xml);
            Globals.debugRenderer.addScene(_instance);
        }else
            _instance.Reset();
        return _instance;
    }
*/
    private void LoadData(XDocument xml)
    {
        if (xml == null)
            return;
        List<XElement>? optionList = (from t in xml.Element("Root").Element("Options").Descendants("Option")
                where t.Element("Name").Value=="Score"
                select t ).ToList<XElement>();
        score.points=Convert.ToInt32(optionList[0].Element("Value").Value);
    }

    private void Reset()
    {
        score.Reset();
    }

    Random r = new Random();
    public int id;
    public Scene scene { get; set; }
    public Score score;
    public Lives lives;
    private Timer respawnTimer=new Timer(5000);
    public GameHud() : base(Globals.game)
    {
        id = r.Next(0, 1000);
        score = new Score();
        score.position = new Vector2(Globals.ScreenWidth*0.95f, Globals.ScreenHeight*0.03f);
        
        lives = new Lives();
        lives.position = new Vector2(Globals.ScreenWidth*0.95f, Globals.ScreenHeight*0.07f);
        lives.aabb = new Rectangle(0, 0, 100, 50);


        Globals.eventManager.Subscribe("lettucePickup", incScore);
        Globals.eventManager.Subscribe("killTurtle", killTurtle);
        scene = new Scene();
        Globals.game.Components.Add(scene);

        scene.addItem(score);
        scene.addItem(lives);
        
        
        XDocument xml = Globals.save.GetFile("options.xml");
        //LoadData(xml);
        Globals.debugRenderer.addScene(this);


    }

    public void killTurtle(Object o, Dictionary<string, object> args)
    {
        if (!respawnTimer.elapsed)
        {
            return;
        }
        Globals.soundControl.playSound("die");
        lives.lives--;
        args["lives"] = lives.lives;
        Globals.eventManager.Trigger("respawnTurtle",this,args);
        respawnTimer.elapsed = false;
    }
    public void incScore(Object o, Dictionary<string, object> args)
    {
        score.incScore();
    }

    public void close()
    {
        Globals.eventManager.ClearListeners("lettucePickup");
        Globals.game.Components.Remove(scene);
        Globals.debugRenderer.removeScene(this);
        respawnTimer.Close();
    }public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
