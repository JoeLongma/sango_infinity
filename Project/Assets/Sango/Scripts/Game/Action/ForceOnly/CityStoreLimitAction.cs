using Sango.Game.Tools;

namespace Sango.Game
{
    /// <summary>
    /// 某类型城池的最大仓库存量增加	p1:增加值, p2:建筑类型
    /// </summary>
    public class CityStoreLimitAction : ForceActionBase
    {
        public override void Init(int[] p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            GameEvent.OnCityCalculateMaxItemStoreSize += OnCityCalculateMaxItemStoreSize;
        }

        public override void Clear()
        {
            GameEvent.OnCityCalculateMaxItemStoreSize -= OnCityCalculateMaxItemStoreSize;
        }

        void OnCityCalculateMaxItemStoreSize(City city, OverrideData<int> overrideData)
        {
            if (Force == city.BelongForce && Params.Length > 2)
            {
                int checkBuildingKindType = Params[2];

                if (checkBuildingKindType == 0 || (checkBuildingKindType > 0 && city.BuildingType.kind == checkBuildingKindType))
                    overrideData.Value += Params[1];
            }
        }
    }
}
