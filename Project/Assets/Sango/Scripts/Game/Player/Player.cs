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
            Singleton<CityBuildBuilding>.Instance.Init();
            Singleton<CityRecruitTroops>.Instance.Init();   // 征兵
            Singleton<CityCreateBoat>.Instance.Init();
            Singleton<CityCreateItems>.Instance.Init();
            Singleton<CityCreateMachine>.Instance.Init();
            Singleton<CityDevelop>.Instance.Init();
            Singleton<CityFarming>.Instance.Init();
            Singleton<CityInspection>.Instance.Init();
            Singleton<CityTrade>.Instance.Init();

            //军事
            Singleton<CityExpedition>.Instance.Init();      // 出征
            Singleton<CityTrainTroops>.Instance.Init();     // 训练

            //人事
            Singleton<CityCallPerson>.Instance.Init();
            Singleton<CityTransformPerson>.Instance.Init();

        }
    }
}
