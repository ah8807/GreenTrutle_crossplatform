using System;
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
        };
    }

    public override void Initialize()
    {
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}