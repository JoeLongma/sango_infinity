using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sango.Game.Player
{
    public class Player : Singleton<Player>
    {
        public void Init()
        {
            Singleton<CityRecruitTroops>.Instance.Init();
            Singleton<CityBuildBuilding>.Instance.Init();
        }
    }
}
