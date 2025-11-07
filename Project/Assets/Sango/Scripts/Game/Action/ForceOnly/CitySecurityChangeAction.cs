using Sango.Game.Tools;

namespace Sango.Game
{
    /// <summary>
    /// 某类型城池的治安下降值比例	p1:城市类型 p2:修改值(百分比)
    /// </summary>
    public class CitySecurityChangeAction : ForceActionBase
    {
        public override void Init(int[] p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            GameEvent.OnCitySecurityChangeOnSeasonStart += OnCitySecurityChangeOnSeasonStart;
        }

        public override void Clear()
        {
            GameEvent.OnCitySecurityChangeOnSeasonStart -= OnCitySecurityChangeOnSeasonStart;
        }

        void OnCitySecurityChangeOnSeasonStart(City city, OverrideData<int> overrideData)
        {
            if (Force == city.BelongForce && Params.Length > 2)
            {
                int checkBuildingKindType = Params[1];
                if (checkBuildingKindType == 0 || (checkBuildingKindType > 0 && city.BuildingType.kind == checkBuildingKindType))
                {
                    overrideData.Value = (int)System.Math.Ceiling(overrideData.Value * Params[2] / 100f);
                }
            }
        }
    }
}
