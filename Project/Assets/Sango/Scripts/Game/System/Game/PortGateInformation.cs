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
        protected override bool CityMenuCanShow()
        {
            return !TargetCity.IsCity();
        }

        public override void Init()
        {
            base.Init();
            Name = "港关情报";
        }

        public override void OnScenarioInit(Scenario scenario)
        {
            default_citits.Clear();
            scenario.citySet.ForEach(city => {
                if (!city.IsCity())
                    default_citits.Add(city);
            });
        }
    }
}
