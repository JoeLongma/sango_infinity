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
            Singleton<CityCreateBoat>.Instance.Init();
            Singleton<CityCreateItems>.Instance.Init();
            Singleton<CityCreateMachine>.Instance.Init();
            Singleton<CityDevelop>.Instance.Init();
            Singleton<CityFarming>.Instance.Init();
            Singleton<CityInspection>.Instance.Init();
            //Singleton<CityBuildBuilding>.Instance.Init();

            //军事
            Singleton<CityRecruitTroops>.Instance.Init();
            Singleton<CityTrainTroops>.Instance.Init();

            //人事
            Singleton<CityCallPerson>.Instance.Init();
            Singleton<CityTransformPerson>.Instance.Init();

        }
    }
}
