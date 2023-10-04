using System;
using System.Net.Mime;
using System.Xml.Linq;
using GreenTrutle_crossplatform.interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GreenTrutle_crossplatform.scene.Objects;

public class Slider : DrawableGameObject, IParticle
{
    public event EventHandler Click;
    public int value
    {
        get { return _value;}
        set
        {
            _value = (int)(value * (100f / (aabb.Width - 1)));
            valueT.text = _value + "";
            knob.position = new Vector2(value+ ((IParticle)this).getRect().X,this.position.Y);
        }
    }

    public int directValue
    {
        get { return _value;}
        set
        {
            _value = value;
            valueT.text = _value + "";
            knob.position = new Vector2(value/(100f / (aabb.Width - 1))+ ((IParticle)this).getRect().X,this.position.Y);
        }
    }
    public Knob knob;
    private Rectangle _aabb;
    private Vector2 _position;
    public Text nameT;
    public Text valueT;
    public int _value;

    public String name
    {
        set { nameT.text = value; }
        get { return nameT.text; }
    }

    public Rectangle aabb
    {
        get { return _aabb; }
        set
        {
            _aabb = value;
            knob.aabb = new Rectangle(0, 0, aabb.Height / 3, (int)(aabb.Height * 2));
            if (position == null)
                position = Vector2.One;
            if (aabb == null)
                aabb = new Rectangle(0, 0, 1, 1);
            nameT.position = new Vector2(position.X-aabb.Width/2+nameT.textMiddlePoint.X,position.Y-aabb.Height/2-nameT.textMiddlePoint.Y);
            valueT.position = new Vector2(position.X+aabb.Right-aabb.Width/2-valueT.textMiddlePoint.X,position.Y-aabb.Height/2-valueT.textMiddlePoint.Y);
        }
    }

    public Vector2 position { get { return _position; }
        set
        {
            _position = value;
            knob.position = _position;
        }
    }

    public Slider()
    {
        knob = new Knob();
        nameT = new Text();
        valueT = new Text();
    }
    
    



    public void Update()
    {
        MouseState Mstate = Mouse.GetState();
        IParticle p = (IParticle)this;
        Rectangle r = p.getRect();
        if (r.Contains(Mstate.X, Mstate.Y))
        {
            if (Mstate.LeftButton == ButtonState.Pressed)
            {
                value = Mstate.X-r.X;
                // knob.position = new Vector2(Mstate.X,this.position.Y);
                OnClick();
            }
        }
    }

    protected virtual void OnClick()
    {
        Click?.Invoke(this, EventArgs.Empty);
    }

    public override XElement GetXML()
    {
        XElement xml = new XElement(new XElement("Setting",
            new XElement("Type",this.GetType().Name),new XElement("Name",this.name),
            new XElement("Value",value)));
        return xml;
    }
}