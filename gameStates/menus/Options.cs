using System;
using GreenTrutle_crossplatform.gameStates.gameplay;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.GameStates.menus;

public class Options:Menu
{
    public event EventHandler OnClickBack;
    private Gameplay gameplay;
    public Options(Gameplay gameplay)
    {
        this.gameplay = gameplay;
    }

    public override void Initialize()
    {
        base.Initialize();
        backB.Click += (sender, args) =>
        {
            OnClickBack?.Invoke(this, EventArgs.Empty);
            this.deactivate();

        };
    }

    public override void Update(GameTime gameTime)
    {
        
        base.Update(gameTime);
    }
}