using System;
using GreenTrutle_crossplatform.interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GreenTrutle_crossplatform.scene.Objects;

public class Button : DrawableGameObject, IParticle
{
    public event EventHandler Click;
    public Text text;
    public bool pressed = false;
    private MouseState oldStateM;
    private bool isLocked = false;
    public Vector2 origin
    {
        get { return new Vector2(this.aabb.Width/2, this.aabb.Height/2);}
    }

    public Rectangle aabb { get; set; }

    public Button(Rectangle rect)
    {
        text = new Text();
        aabb = rect;
        text.text="Turtle";
    }

    public void Update()
    {
        MouseState Mstate = Mouse.GetState();
/*
        // Update our sprites position to the current cursor location
        Vector2 position = new Vector2();
        position.X = Mstate.X;
        position.Y = Mstate.Y;
        IParticle p = (IParticle)this;
        Rectangle r = p.getRect();
        // Check if Right Mouse Button pressed, if so, exit
        if (Mstate.LeftButton == ButtonState.Released && oldStateM.LeftButton == ButtonState.Pressed)
        {
            if (position.X > r.Left && position.X < r.Right && position.Y > r.Top && position.Y < r.Bottom )
            {
                text.text = "pressed";
                OnClick?.Invoke(this,EventArgs.Empty);
            }
        }
        oldStateM = Mstate;
        */
        IParticle p = (IParticle)this;
        Rectangle r = p.getRect();
        if (r.Contains(Mstate.X, Mstate.Y))
        {
            if (Mstate.LeftButton == ButtonState.Pressed && oldStateM.LeftButton == ButtonState.Released)
            {
                // Button was clicked
                OnClick();
            }
        }

        oldStateM = Mstate;
    }
    protected virtual void OnClick()
    {
        if (!isLocked)
        {
            // isLocked = true;
            Click?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Unlock()
    {
        isLocked=false;
    }

}