using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GreenTrutle_crossplatform.interfaces
{
    internal interface IRSE
    {
        float rotation { get; set; }
        Vector2 scale { get; set; }
        SpriteEffects effect { get; set; }
    }
}
