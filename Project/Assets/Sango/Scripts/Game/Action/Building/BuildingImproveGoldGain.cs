using Newtonsoft.Json.Linq;
using Sango.Game.Tools;
using System.Collections.Generic;

namespace Sango.Game.Action
{
    /// <summary>
    /// 计算建筑的资金产量，并提升系数  
    /// value:提升系数（百分比） kinds：有效kind类型合集 bound: 生效范围 0：该城市全局 1：周围一圈
    /// </summary>
    public class BuildingImproveGoldGain : BuildingActionBase
    {
        int value;
        List<int> kinds;
        int bound;

        public override void Init(JObject p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            if (Building == null)
                return;
            GameEvent.OnBuildingCalculateGoldGain += OnBuildingCalculateGoldGain;
            value = p.Value<int>("value");
            bound = p.Value<int>("bound");
            JArray kindsArray = p.Value<JArray>("kinds");
            if (kindsArray != null)
            {
                kinds = new List<int>(kindsArray.Count);
                for (int i = 0; i < kindsArray.Count; i++)
                    kinds.Add(kindsArray[i].ToObject<int>());
            }
        }

        public override void Clear()
        {
            GameEvent.OnBuildingCalculateGoldGain -= OnBuildingCalculateGoldGain;
        }

        void OnBuildingCalculateGoldGain(BuildingBase buildingBase, OverrideData<int> overrideData)
        {
            if (Force != null && buildingBase.BelongForce != Force) return;
            if (Building != null && Building != buildingBase) return;

            if (bound == 0)
            {
                int gold = 0;
                Building.BelongCity.allBuildings.ForEach(b =>
                {
                    if (!b.IsIntorBuilding()) return;

                    // 只对农田有效
                    if (kinds != null && !kinds.Contains(b.BuildingType.kind))
                        return;

                    gold += b.BuildingType.goldGain * value;

                });
                overrideData.Value += gold / 100;
            }
            else
            {

                int gold = 0;
                for (int i = 0; i < Building.CenterCell.Neighbors.Length; ++i)
                {
                    Cell neighbor = Building.CenterCell.Neighbors[i];
                    if (neighbor == null || !neighbor.IsInterior || neighbor.building == null) continue;

                    BuildingBase target = neighbor.building;
                    // 只对农田有效
                    if (kinds != null && !kinds.Contains(target.BuildingType.kind))
                        continue;

                    gold += target.BuildingType.goldGain * value;
                }
                overrideData.Value += gold / 100;
            }
        }
    }
}
