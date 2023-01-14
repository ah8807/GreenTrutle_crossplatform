using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GreenTrutle_crossplatform.scene;

public class Scene : DrawableGameComponent, IEnumerable<object>
{
    static int trololo = 0;
    public int id;
    public String name;
    public List<object> items { get; set; }
    List<DrawableGameObject> removeList = new List<DrawableGameObject>();
    List<DrawableGameObject> addList = new List<DrawableGameObject>();

    public Scene():base(Globals.game)
    {
        id = trololo;
        trololo += 1;
        items = new List<object>();
    }

    public IEnumerator<object> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return items.GetEnumerator();
    }

    internal void removeItem(DrawableGameObject item)
    {
        removeList.Add(item);
    }

    internal void addItem(DrawableGameObject item)
    {
        addList.Add(item);
    }

    public override void Update(GameTime gameTime)
    {
        foreach (DrawableGameObject item in removeList)
        {
            this.items.Remove(item);
            item.scene = null;
        }

        foreach (DrawableGameObject item in addList)
        {
            this.items.Add(item);
            item.scene = this;
        }

        removeList.Clear();
        addList.Clear();
        base.Update(gameTime);
    }


}