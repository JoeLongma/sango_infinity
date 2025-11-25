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
            //都市
            Singleton<CitySystem>.Instance.Init();

            // 部队
            Singleton<TroopSystem>.Instance.Init();
            
        }
    }
}
