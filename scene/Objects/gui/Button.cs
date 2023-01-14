using System;
using GreenTrutle_crossplatform.interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GreenTrutle_crossplatform.scene.Objects;

public class Button:DrawableGameObject, IParticle
{
    public event EventHandler Click;
    public Rectangle hitbox;
    public Text text;
    public bool pressed=false;
    private MouseState oldStateM;
    public Rectangle aabb { get; set; }

    public Button(Rectangle rect)
    {
        text = new Text();
        hitbox = new Rectangle((int)position.X - rect.Width / 2, (int)position.Y - rect.Height / 2, rect.Width, rect.Height);
        aabb = hitbox;
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
        if (hitbox.Contains(Mstate.X, Mstate.Y))
        {
            if (Mstate.LeftButton == ButtonState.Pressed)
            {
                // Button was clicked
                OnClick();
            }
        }
    }
    protected virtual void OnClick()
    {
        Click?.Invoke(this, EventArgs.Empty);
    }

}