using Sango.Game.Tools;
using Newtonsoft.Json.Linq;

namespace Sango.Game
{
    /// <summary>
    /// 某类型城池的最大仓库存量增加
    /// value: 值
    /// kinds: 城市类型
    /// </summary>
    public class CityStoreLimitAction : ForceBuildingActionBase
    {
        public override void Init(JObject p, params SangoObject[] sangoObjects)
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
            if (!CheckForceBuilding(city)) return;
            overrideData.Value += value;
        }
    }
}
