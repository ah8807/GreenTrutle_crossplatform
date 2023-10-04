using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using GreenTrutle_crossplatform.interfaces;

namespace GreenTrutle_crossplatform.scene;
[Serializable]
[XmlInclude(typeof(Turtle))]
public class Scene : DrawableGameComponent, IEnumerable<object>
{
    static int trololo = 0;
    public int id;
    public String name;
    
    [XmlArray("Items"), XmlArrayItem(typeof(object), ElementName = "item")]
    public List<object> items { get; set; }
    List<DrawableGameObject> removeList = new List<DrawableGameObject>();
    List<DrawableGameObject> addList = new List<DrawableGameObject>();

    public Scene():base(Globals.game)
    {
        id = trololo;
        trololo += 1;
        items = new List<object>();
    }

    public void Add(object obj)
    {
        addItem((DrawableGameObject)obj);
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
            if (!items.Contains(item))
                continue;
            this.items.Remove(item);
            item.scene = null;
        }

        foreach (DrawableGameObject item in addList)
        {
            if (items.Contains(item))
                continue;
            this.items.Add(item);
            item.scene = this;
        }

        if (addList.OfType<IStatic>().Any() || removeList.OfType<IStatic>().Any())
        {
            Globals.eventManager.Trigger("sceneIStaticUpdate",this,null);
        }
        removeList.Clear();
        addList.Clear();
        base.Update(gameTime);
    }

    public XDocument getXML()
    {
        XDocument xml = new XDocument(new XElement("Root",
            new XElement("GameObjects","")));
        foreach (DrawableGameObject obj in items)
        {
            xml.Element("Root").Element("GameObjects").Add(obj.GetXML());
        }

        return xml;
    }
    
    public List<object> getItemsOFClass(Type classType)
    {
        return items.Where(x => classType.IsAssignableFrom(x.GetType())).ToList();
    }
}