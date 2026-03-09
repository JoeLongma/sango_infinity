using System;
using System.Collections.Generic;

namespace Sango.Game.Player
{
    [GameSystem(auto = true)]
    public class PortGateSelectSystem : CitySelectSystem
    {
        public PortGateSelectSystem()
        {
            defualtTitleName = "港关";
        }
    }
}
