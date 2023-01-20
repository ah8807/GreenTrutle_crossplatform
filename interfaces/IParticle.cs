using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenTrutle_crossplatform.scene.Objects;
using tainicom.Aether.Physics2D.Fluids;

namespace GreenTrutle_crossplatform.interfaces
{
    internal interface IParticle : IMovable, IMass, IParticleCollider
    {
        
        public Rectangle getRect()
        {
            int x0 = (int)this.position.X - (int)this.aabb.Width / 2;
            int y0 = (int)this.position.Y - (int)this.aabb.Height / 2;
            int x1 = x0 + (int)this.aabb.Width;
            int y1 = y0 + (int)this.aabb.Height;
            return new Rectangle((int)position.X - aabb.Width / 2, (int)position.Y - aabb.Height / 2, aabb.Width, aabb.Height);
        }
        public void rotate()
        {
            aabb = new Rectangle(aabb.X, aabb.Y, aabb.Height, aabb.Width);
        }
        
    }
    
}
