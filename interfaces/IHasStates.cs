using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenTrutle_crossplatform.interfaces
{
    internal interface IHasStates
    {
        List<IState> states { get; set; }
        IState currState { get; set; }

    }
}
