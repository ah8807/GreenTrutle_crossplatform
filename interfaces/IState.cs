using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenTrutle_crossplatform.interfaces
{
    internal interface IState
    {
        SpriteEffects effect { get; set; }
        float rotation { get; set; }
    }
}
