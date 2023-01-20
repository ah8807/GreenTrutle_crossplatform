using System.Collections.Generic;
using System.Net.NetworkInformation;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GreenTrutle_crossplatform.scene.Objects;

public class Lives:DrawableGameObject, IParticle
{
    public Rectangle aabb { get; set; }
    public int lives;
    public Sprite sprite;
    public Texture2D texture
    {
        get { return sprite.texture; }
    }
    public Vector2 textScale;
    private int maxLives=3;
    public List<Vector2> positions;
    public Lives()
    {
        positions = new List<Vector2>();
        lives = maxLives;
    }

    public void Update()
    {
        positions.Clear();
        //textScale = new Vector2(((float)aabb.Width/maxLives/aabb.Width),((float)aabb.Width/maxLives)/aabb.Width);
        textScale = new Vector2((aabb.Width / sprite.sourceRectangle.Width)/maxLives, (aabb.Width / sprite.sourceRectangle.Width)/maxLives);
        for (int i = 0; i < maxLives; i++)
        {
            if(positions.Count<lives)
                positions.Add(new Vector2(((aabb.Width / maxLives)/2+(aabb.Width / maxLives)*i)+position.X-aabb.Width/2, position.Y));
        }
        
    }
}