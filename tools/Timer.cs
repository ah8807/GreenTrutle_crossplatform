using System;
using System.Data;
using System.Xml;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.tools;

public class Timer: GameComponent
{
    private TimeSpan prevTime;
    private TimeSpan curTime;
    public int updateTimer=100;
    public event EventHandler repeat;
    public event EventHandler oneTime;
    private bool _elapsed;
    private bool initialized = false;
    public bool elapsed
    {
        set
        {
            prevTime = curTime.Duration();
            _elapsed = value;
        }
        get { return _elapsed; }
    }

    public Timer(int updateTimer):base(Globals.game)
    {
        this.updateTimer = updateTimer;
        Globals.game.Components.Add(this);
        this.elapsed = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (!initialized)
        {
            initialized = true;
            prevTime = gameTime.TotalGameTime;
            return;
        }
        if (gameTime.TotalGameTime.TotalMilliseconds - prevTime.TotalMilliseconds > updateTimer)
        {
            repeat?.Invoke(this,EventArgs.Empty);
            prevTime = gameTime.TotalGameTime;
            if (!_elapsed)
            {
                oneTime?.Invoke(this, EventArgs.Empty);
                _elapsed = true;
            }
        }

        if (_elapsed)
        {
            curTime=gameTime.TotalGameTime;
        }
    }

    public void Close()
    {
        Globals.game.Components.Remove(this);
    }
}