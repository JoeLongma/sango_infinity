using Sango.Game.Render.UI;

namespace Sango.Game.Player
{
    public class TroopActionBase : CommandSystemBase
    {
        public Troop TargetTroop { get; set; }
        public Cell ActionCell { get; set; }

        public string customMenuName;
        public int customMenuOrder;

        public override void Init()
        {
            GameEvent.OnTroopActionContextMenuShow += OnTroopActionContextMenuShow;
        }

        public override void Clear()
        {
            GameEvent.OnTroopActionContextMenuShow -= OnTroopActionContextMenuShow;
        }

        protected virtual void OnTroopActionContextMenuShow(ContextMenuData menuData, Troop troop, Cell actionCell)
        {
            if (troop.BelongForce != null && troop.BelongForce.IsPlayer && troop.BelongForce == Scenario.Cur.CurRunForce)
            {
                TargetTroop = troop;
                ActionCell = actionCell;
                menuData.Add(customMenuName, customMenuOrder, actionCell, OnClickMenuItem, IsValid);
            }
        }

        protected virtual void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            Start(TargetTroop, ActionCell);
        }

        public virtual void Start(Troop troop, Cell actionCell)
        {
            TargetTroop = troop;
            ActionCell = actionCell;
            PlayerCommand.Instance.Push(this);
        }
    }
}
