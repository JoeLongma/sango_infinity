using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sango.Game.Player
{
    public class Player : Singletion<Player>
    {
        public void Init()
        {
            CityRecruitTroops.Instance.Init();
            CityBuildBuilding.Instance.Init();
        }
    }
}
