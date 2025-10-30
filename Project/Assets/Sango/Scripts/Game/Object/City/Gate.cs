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
            enemies.Clear();
            for (int i = 0; i < enemiesRound.Length; i++)
                enemiesRound[i] = false;

            scenario.troopsSet.ForEach(x =>
            {
                if (x.IsEnemy(this))
                {
                    int round = scenario.Map.Distance(CenterCell, x.cell);
                    if (round < SAVE_ROUND)
                    {
                        enemies.Add(new EnemyInfo { troop = x, distance = round });
                        for (int j = round; j < enemiesRound.Length; j++)
                            enemiesRound[j] = true;
                    }
                }
            });

            if (enemies.Count > 1)
            {
                enemies.Sort((a, b) =>
                {
                    return a.distance.CompareTo(b.distance);
                });
            }


            UpdateFightPower();

            AICommandList.Add(CityAI.AIAttack);
            // 物资输送
            AICommandList.Add(CityAI.AITransfromToBelongCity);
            GameEvent.OnCityAIPrepare?.Invoke(this, scenario);
        }

    }
}
