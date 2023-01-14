using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Audio;

namespace GreenTrutle_crossplatform.scene;

public class GameHud : DrawableGameComponent, IDisposable
{
    private static GameHud _instance = new GameHud();
    
    public static GameHud GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameHud();
        }
        _instance.Reset();
        return _instance;
    }

    private void Reset()
    {
        score.Reset();
    }

    Random r = new Random();
    public int id;
    public Scene scene { get; set; }
    private Score score;
    SoundEffect soundEffect;
    private GameHud() : base(Globals.game)
    {
        id = r.Next(0, 1000);
        score = new Score();
        score.position = new Vector2(900, 30);
        ///Lettuce.OnPickUp += (sender, args) => { score.incScore();
          //  soundEffect.Play();
        //};

        Globals.eventManager.Subscribe("lettucePickup", func);
        scene = new Scene();
        Globals.game.Components.Add(scene);

        scene.addItem(score);


    }

    public void func(Object o, Dictionary<string, object> args)
    {
        score.incScore(); soundEffect.Play();
    }
    
    
    protected override void LoadContent()
    {
        soundEffect = Globals.game.Content.Load<SoundEffect>("pickupCoin");
        base.LoadContent();
    }

    public void close()
    {
        Globals.eventManager.clearAll("lettucePickup");
    }public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
