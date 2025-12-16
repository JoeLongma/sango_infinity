using Sango.Game.Tools;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Sango.Game
{
    /// <summary>
    /// 建筑的反击力修改,
    /// value: 值, kinds: 建筑类型
    /// </summary>
    public class BuildingBaseAttackBack : ForceBuildingActionBase
    {
        public override void Init(JObject p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            GameEvent.OnBuildCalculateAttackBack += OnBuildCalculateAttackBack;
        }

        public override void Clear()
        {
            GameEvent.OnBuildCalculateAttackBack -= OnBuildCalculateAttackBack;
        }

        void OnBuildCalculateAttackBack(Troop troop, Cell spellCell, BuildingBase buildingBase, Skill skill, OverrideData<int> overrideData)
        {
            if (!CheckForceBuilding(buildingBase)) return;
            overrideData.Value += value;
        }

    }
}
