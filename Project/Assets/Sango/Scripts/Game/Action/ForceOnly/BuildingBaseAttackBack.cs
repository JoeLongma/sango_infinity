using Sango.Game.Tools;

namespace Sango.Game
{
    /// <summary>
    /// 建筑的反击力修改, p1:增加值, p2:建筑类型
    /// </summary>
    public class BuildingBaseAttackBack : ForceActionBase
    {
        public override void Init(int[] p, params SangoObject[] sangoObjects)
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
            if (Force == buildingBase.BelongForce && Params.Length > 2)
            {
                int checkBuildingKindType = Params[2];
                if (checkBuildingKindType == 0 || (checkBuildingKindType > 0 && buildingBase.BuildingType.kind == checkBuildingKindType))
                    overrideData.Value += Params[1];
            }
        }

    }
}
