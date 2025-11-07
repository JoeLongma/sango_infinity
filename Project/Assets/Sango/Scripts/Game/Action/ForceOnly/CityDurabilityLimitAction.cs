using Sango.Game.Tools;

namespace Sango.Game
{
    /// <summary>
    /// 某类型城池的最大耐久增加	p1:增加值, p2:建筑类型
    /// </summary>
    public class CityDurabilityLimitAction : ForceActionBase
    {
        public override void Init(int[] p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            GameEvent.OnCityCalculateMaxDurability += OnCityCalculateMaxDurability;
        }

        public override void Clear()
        {
            GameEvent.OnCityCalculateMaxDurability -= OnCityCalculateMaxDurability;
        }

        void OnCityCalculateMaxDurability(City city, OverrideData<int> overrideData)
        {
            if (Force == city.BelongForce && Params.Length > 2)
            {
                int checkBuildingKindType =  Params[2];
                if (checkBuildingKindType == 0 || (checkBuildingKindType > 0 && city.BuildingType.kind == checkBuildingKindType))
                    overrideData.Value += Params[1];
            }
        }
    }
}
