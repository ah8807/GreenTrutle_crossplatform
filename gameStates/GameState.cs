using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Framework.Utilities.Deflate;

namespace GreenTrutle_crossplatform.GameStates;

public abstract class GameState : DrawableGameComponent
{
    public List<DrawableGameComponent> comps = new List<DrawableGameComponent>();
    public GameState() : base(Globals.game)
    {
        addComp(this);
    }
    public void addComp(DrawableGameComponent comp)
    {
        if(!Globals.game.Components.Contains(comp))
        {
            Globals.game.Components.Add(comp);
            comps.Add(comp);
            comp.Enabled = false;
            comp.Visible = false;
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

    public void activate()
    {
        foreach (DrawableGameComponent comp in comps)
        {
            comp.Enabled = true;
            comp.Visible = true;
        }   
    }

    public  void deactivate()
    {
        foreach (DrawableGameComponent comp in comps)
        {
            comp.Enabled = false;
            comp.Visible = false;
        }
    }

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