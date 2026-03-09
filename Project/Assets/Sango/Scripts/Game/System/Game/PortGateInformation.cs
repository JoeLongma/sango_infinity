using Sango.Game.Player;
using Sango.Game.Render.UI;
using System.Collections.Generic;

namespace Sango.Game
{
    /// <summary>
    /// 城池治安系统逻辑
    /// </summary>
    [GameSystem(auto = true)]
    public class PortGateInformation : CityInformation
    {
        public PortGateInformation()
        {
            windowName = "window_information_port_gate";
        }

        public override void Init()
        {
            Name = "港关情报";
        }

        public override void Clear()
        {
        }
    }
}
