using Newtonsoft.Json;
using Sango.Game.Render;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Gate : City
    {
        public override void OnPrepareRender()
        {
            Render = new GateRender(this);
        }

        public override void AIPrepare(Scenario scenario)
        {
            // 准备敌人信息
            PrepareEnemiesInfo(scenario);

            UpdateActiveTroopTypes();
            UpdateFightPower();

            AICommandList.Add(CityAI.AIAttack);
            // 物资输送
            AICommandList.Add(CityAI.AITransfromToBelongCity);
            GameEvent.OnCityAIPrepare?.Invoke(this, scenario);
        }

    }
}
