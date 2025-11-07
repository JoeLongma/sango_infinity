using Sango.Game.Tools;

namespace Sango.Game
{
    /// <summary>
    /// 增加势力全队伍的兵力上限  p1:增加值 p2:兵种类型
    /// </summary>
    public class ForceTroopMaxTroopAction : ForceActionBase
    {
        public override void Init(int[] p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            GameEvent.OnTroopCalculateMaxTroops += OnTroopCalculateMaxTroops;
        }

        public override void Clear()
        {
            GameEvent.OnTroopCalculateMaxTroops -= OnTroopCalculateMaxTroops;
        }

        void OnTroopCalculateMaxTroops(City city, Troop troop, OverrideData<int> overrideData)
        {
            if (this.Force == troop.BelongForce && Params.Length > 2)
            {
                int checkTroopTypeKind = Params[2];
                if (checkTroopTypeKind == 0 || (checkTroopTypeKind > 0 && troop.TroopType.kind == checkTroopTypeKind))
                    overrideData.Value += Params[1];
            }
        }

    }
}
