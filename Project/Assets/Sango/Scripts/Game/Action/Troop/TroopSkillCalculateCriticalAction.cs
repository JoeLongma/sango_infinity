using Sango.Game.Tools;

namespace Sango.Game
{
    /// <summary>
    /// 某兵种类型战法的暴击率增加	p1:兵种类型(0全兵种全地形 -1陆地 -2水上) p2:增加值(百分比) p3: 是否一般攻击, -1都可以 p4:其他条件
    /// </summary>
    public class TroopSkillCalculateCriticalAction : TroopActionBase
    {
        public override void Init(int[] p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            GameEvent.OnTroopCalculateSkillCritical += OnTroopCalculateSkillCritical;
        }

        public override void Clear()
        {
            GameEvent.OnTroopCalculateSkillCritical -= OnTroopCalculateSkillCritical;
        }

        void OnTroopCalculateSkillCritical(Troop troop, Skill skill, Cell cell, OverrideData<int> overrideData)
        {
            if (Force != null && troop.BelongForce != Force) return;
            if (Troop != null && Troop != troop) return;

            if (Params.Length > 4)
            {

                if (!CheckIsNormalSkill(skill, Params[3]))
                    return;

                //if (CheckIsRangeSkill(skill, Params[5]))
                //    return;

                if (!CheckTroopTypeKind(troop, Params[1]))
                    return;

                int conditionId = Params[4];
                Condition condition = null;
                if (conditionId > 0)
                    condition = Condition.Get(conditionId);

                if (condition == null || condition.Check(troop, cell.troop, skill))
                {
                    overrideData.Value += Params[2];
                }

            }
        }
    }
}
