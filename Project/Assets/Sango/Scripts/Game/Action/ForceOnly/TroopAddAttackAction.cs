using Sango.Game.Tools;

namespace Sango.Game
{
    /// <summary>
    /// 某兵种攻击力增加  p1:增加值 p2:兵种kind
    /// </summary>
    public class TroopAddAttackAction : ForceActionBase
    {
        public override void Init(int[] p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            GameEvent.OnTroopCalculateAttribute += OnTroopCalculateAttribute;
        }

        public override void Clear()
        {
            GameEvent.OnTroopCalculateAttribute -= OnTroopCalculateAttribute;
        }

        void OnTroopCalculateAttribute(Troop troop, Scenario scenario)
        {
            if (Force == troop.BelongForce && Params.Length > 2)
            {
                int checkTroopTypeKind = Params[2];
                if (checkTroopTypeKind == 0)
                {
                    troop.landAttack += Params[1];
                    troop.waterAttack += Params[1];
                }
                else
                {
                    if (troop.LandTroopType.kind == checkTroopTypeKind)
                        troop.landAttack += Params[1];
                    if (troop.WaterTroopType.kind == checkTroopTypeKind)
                        troop.waterAttack += Params[1];
                }
            }
        }
    }
}
