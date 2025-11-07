using Sango.Game.Tools;

namespace Sango.Game
{
    /// <summary>
    /// 增加势力城池的士气上限  p1:增加值
    /// </summary>
    public class ForceCityMaxMoraleAction : ForceActionBase
    {
        public override void Init(int[] p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            GameEvent.OnCityCalculateMaxMorale += OnCityCalculateMaxMorale;
        }

        public override void Clear()
        {
            GameEvent.OnCityCalculateMaxMorale -= OnCityCalculateMaxMorale;
        }

        void OnCityCalculateMaxMorale(City city, OverrideData<int> overrideData)
        {
            if (this.Force == city.BelongForce && Params.Length > 1)
            {
                overrideData.Value += Params[1];
            }
        }

    }
}
