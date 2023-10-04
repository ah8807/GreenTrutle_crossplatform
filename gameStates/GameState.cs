using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene;
using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;
using MonoGame.Framework.Utilities.Deflate;

namespace GreenTrutle_crossplatform.GameStates;

public abstract class GameState : DrawableGameComponent
{
    public List<DrawableGameComponent> comps = new List<DrawableGameComponent>();
    private GameState? prevState;
    public event EventHandler onBack;
    public GameState(GameState prevState) : base(Globals.game)
    {  
        this.prevState = prevState;
        addComp(this);
        onBack += goToPrevState;
    }

    public void goToPrevState(Object o, EventArgs args)
    {
        ((GameState)o).Close();
        activate();
    }

    public void goBack(object o,EventArgs args)
    {
        if (prevState == null)
        {
            Globals.game.Exit();
            return;
        }
        prevState.onBack?.Invoke(this, EventArgs.Empty);
    }
    public void addComp(DrawableGameComponent comp)
    {
        if(!Globals.game.Components.Contains(comp))
        {
            Globals.game.Components.Add(comp);
        }
        comps.Add(comp);
        comp.Enabled = false;
        comp.Visible = false;
    }
    public void removeComp(DrawableGameComponent comp)
    {
        if(comps.Contains(comp))
        {
            comp.Dispose();
            Globals.game.Components.Remove(comp);
            comps.Remove(comp);
        }
    }

    public virtual void activate()
    {
        foreach (DrawableGameComponent comp in comps)
        {
            if (Globals.game.Components.Contains(comp))
            {
                comp.Enabled = true;
                comp.Visible = true;
            }
            else
            {
                Globals.game.Components.Add(comp);
            }
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

    public virtual void Close()
    {
        foreach (DrawableGameComponent comp in comps)
        {
            comp.Dispose();
            Globals.game.Components.Remove(comp);
        }
        Globals.debugRenderer.removeScene(this);
    }
    protected static List<object> getXElemetsOfType(XDocument xml, String type)
    {
        List<XElement> objList = (from t in xml.Element("Root").Element("GameObjects").Descendants("GameObject")
            where t.Element("Type").Value == type
            select t).ToList<XElement>();
        List<object> gameObjects = new List<object>();
        foreach (XElement obj in objList)
        {
            Vector2 position = new Vector2(Convert.ToSingle(obj.Element("Position").Element("X").Value),
                Convert.ToSingle(obj.Element("Position").Element("Y").Value));
            object gameObject = new DrawableGameObject();
            switch (type)
            {
                case "Brick":
                    gameObject = new Brick();
                    break;
                case "Lettuce":
                    gameObject = new Lettuce();
                    break;
                case "Trex":
                    gameObject = new Trex();
                    break;
                case "Turtle":
                    gameObject = new Turtle();
                    break;
                case "Wall":
                    gameObject = new Wall();
                    break;
            }
                
            if(gameObject is IPosition)
                ((IPosition)gameObject).position = position;
            gameObjects.Add(gameObject);
        }

        return gameObjects;
    }

    protected virtual void loadScene(Scene scene ,String scenePath)
    {
        XDocument xml = Globals.save.GetFile(scenePath);
        if (xml == null)
        {
            scene = null;
            return;
        }

        var bricks = getXElemetsOfType(xml,"Brick");
        while (bricks.OfType<Brick>().Any())
        {
            Wall wall = new Wall();
            scene.addItem((DrawableGameObject)wall.CreateWall(bricks));
        }
            
        var lettuces = getXElemetsOfType(xml,"Lettuce");
        foreach (Lettuce lettuce in lettuces)
        {
            scene.addItem(lettuce);
        }
            
        var trexs = getXElemetsOfType(xml,"Trex");
        foreach (Trex trex in trexs)
        {
            scene.addItem(trex);
        }
            
        var turtles = getXElemetsOfType(xml,"Turtle");
        foreach (Turtle turtle in turtles)
        {
            scene.addItem(turtle);
        }
        var walls = getXElemetsOfType(xml,"Wall");
        // foreach (Wall wall in walls)
        // {
        //     level.scene.addItem(wall);
        // }
        scene.Update(Globals.gameTime);
        return;
    }

}