using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.GameStates;

public abstract class GameState : DrawableGameComponent
{
    public List<DrawableGameComponent> comps = new List<DrawableGameComponent>();
    public GameState() : base(Globals.game)
    {

    }
    public void addComp(DrawableGameComponent comp)
    {
        if(!Globals.game.Components.Contains(comp))
        {
            Globals.game.Components.Add(comp);
            comps.Add(comp);
        }
    }
    public void removeComp(DrawableGameComponent comp)
    {
        if(comps.Contains(comp))
        {
            Globals.game.Components.Remove(comp);
            comps.Remove(comp);
        }
    }
    public abstract void activate();
    public abstract void deactivate();

    public void activateAll()
    {
        foreach (DrawableGameComponent comp in comps)
        {
            Globals.game.Components.Add(comp);
        }
    }

    public void Close()
    {
        foreach (DrawableGameComponent comp in comps)
        {
            Globals.game.Components.Remove(comp);
        }
    }

}