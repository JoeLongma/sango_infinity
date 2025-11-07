using Sango.Game.Tools;

namespace Sango.Game
{
    /// <summary>
    /// 某兵种类型战法的暴击率增加	p1:兵种类型(0全兵种全地形 -1陆地 -2水上) p2:增加值(百分比) p3: 是否一般攻击 p4:其他条件
    /// </summary>
    public class TroopSkillCalculateSuccessAction : TroopActionBase
    {
        public override void Init(int[] p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            GameEvent.OnTroopCalculateSkillSuccess += OnTroopCalculateSkillSuccess;
        }

        public override void Clear()
        {
            GameEvent.OnTroopCalculateSkillSuccess -= OnTroopCalculateSkillSuccess;
        }

        void OnTroopCalculateSkillSuccess(Troop troop, Skill skill, Cell cell, OverrideData<int> overrideData)
        {
            if (Force != null && troop.BelongForce != Force) return;
            if (Troop != null && Troop != troop) return;

            if (Params.Length > 4)
            {
                int checkTroopTypeKind = Params[1];
                int conditionId = Params[4];
                bool isNormalSkill = Params[3] > 0;
                if (isNormalSkill && skill.costEnergy > 0)
                    return;

                Condition condition = null;
                if (conditionId > 0)
                    condition = Condition.Get(conditionId);

                if (checkTroopTypeKind == 0)
                {
                    if (condition == null || condition.Check(troop, cell.troop, skill))
                    {
                        overrideData.Value += Params[2];
                    }
                }
                else if (checkTroopTypeKind == -1)
                {
                    if (!troop.IsInWater)
                    {
                        if (condition == null || condition.Check(troop, cell.troop, skill))
                        {
                            overrideData.Value += Params[2];
                        }
                    }
                }
                else if (checkTroopTypeKind == -2)
                {
                    if (troop.IsInWater)
                    {
                        if (condition == null || condition.Check(troop, cell.troop, skill))
                        {
                            overrideData.Value += Params[2];
                        }
                    }
                }
                else
                {
                    if (troop.LandTroopType.kind == checkTroopTypeKind && !troop.IsInWater)
                    {
                        if (condition == null || condition.Check(troop, cell.troop, skill))
                        {
                            overrideData.Value += Params[2];
                        }
                    }
                    else if (troop.WaterTroopType.kind == checkTroopTypeKind && troop.IsInWater)
                    {
                        if (condition == null || condition.Check(troop, cell.troop, skill))
                        {
                            overrideData.Value += Params[2];
                        }
                    }
                }

            }
        }
    }
}
