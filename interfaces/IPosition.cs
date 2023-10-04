using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.interfaces
{
    public interface IPosition
    {
        Vector2 position { get; set; }
        void setX(float x)
        {
            position = new Vector2(x,position.Y);
        }
        void setY(float y)
        {
            position = new Vector2(position.X, y);
        }
    }
}