using Sango.Game.Tools;

namespace Sango.Game
{
    /// <summary>
    /// 某兵种类型战法的增减伤害  p1:0攻击方 1受击方 p2:兵种类型(0全兵种全地形 -1陆地 -2水上) p3:增加值(百分比) p4: 是否一般攻击 -1都可以 p5: 是否是远程 -1都可以 p6:其他条件(troop,troop,skill)
    /// </summary>
    public class TroopChangeTroopsAction : TroopActionBase
    {
        public override void Init(int[] p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            GameEvent.OnTroopChangeTroops += OnTroopChangeTroops;
        }

        public override void Clear()
        {
            GameEvent.OnTroopChangeTroops -= OnTroopChangeTroops;
        }

        void OnTroopChangeTroops(Troop defencer, SangoObject atker, Skill skill, int atkBack, OverrideData<int> overrideData)
        {
            if (Params.Length > 6)
            {
                Troop troop = defencer;
                if (Params[1] == 0)
                {
                    if (atker.ObjectType != SangoObjectType.Troops) return;
                    troop = (Troop)atker;
                }

                if (Force != null && troop.BelongForce != Force) return;
                if (Troop != null && Troop != troop) return;

                if (!CheckIsNormalSkill(skill, Params[4]))
                    return;

                if (!CheckIsRangeSkill(skill, Params[5]))
                    return;

                if (!CheckTroopTypeKind(troop, Params[2]))
                    return;

                int conditionId = Params[6];
                Condition condition = null;
                if (conditionId > 0)
                    condition = Condition.Get(conditionId);

                if (condition == null || condition.Check(troop, defencer, skill))
                {
                    int value = Params[3];
                    overrideData.Value = overrideData.Value * value / 100;
                }
            }
        }
    }
}
