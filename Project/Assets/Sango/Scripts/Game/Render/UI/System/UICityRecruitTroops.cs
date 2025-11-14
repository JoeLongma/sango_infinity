using Sango.Game.Player;
using Sango.Loader;
using Sango.Render;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UICityRecruitTroops : UGUIWindow
    {


        public override void OnShow()
        {
            Person[] people = ForceAI.CounsellorRecommendRecuritTroop(CityRecruitTroops.Instance.TargetCity.freePersons);
                
        }


    }
}
