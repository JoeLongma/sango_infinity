using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sango.Game.Player
{
    public class Player
    {
        public Scenario scenario { get; private set; }
        public Player(Scenario scenario)
        { 
            this.scenario = scenario;
        }

        public void OnMonthUpdate(Scenario scenario)
        {
            ScenarioInfo info = scenario.Info;
        }

        public void OnSeasonUpdate(Scenario scenario)
        {
            ScenarioInfo info = scenario.Info;
            SeasonType cur_season = GameDefine.SeasonInMonth[info.month];
            if(cur_season == SeasonType.Autumn || cur_season == SeasonType.Spring)
            {
            }
        }

        public void OnYearUpdate(Scenario scenario)
        {
        }
    }
}
