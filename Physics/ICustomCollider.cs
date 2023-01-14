using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenTrutle_crossplatform.Physics
{
    internal interface ICustomCollider
    {
        // pove  ali naj se zgodi odboj
        public Boolean collidingWithItem(Object item);
        //pove kaj naj se zgodi ob detekciji trka
        public void collidedWithItem(object item);
    }
}
