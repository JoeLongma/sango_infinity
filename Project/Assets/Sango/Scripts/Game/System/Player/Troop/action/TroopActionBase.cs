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

        protected virtual void OnTroopActionContextMenuShow(IContextMenuData menuData, Troop troop, Cell actionCell)
        {
            if (troop.BelongForce != null && troop.BelongForce.IsPlayer && troop.BelongForce == Scenario.Cur.CurRunForce)
            {
                TargetTroop = troop;
                ActionCell = actionCell;
                menuData.Add(customMenuName, customMenuOrder, actionCell, OnClickMenuItem, IsValid);
            }
        }

        protected virtual void OnClickMenuItem(IContextMenuItem contextMenuItem)
        {
            Start(TargetTroop, ActionCell);
        }

        public virtual void Start(Troop troop, Cell actionCell)
        {
            TargetTroop = troop;
            ActionCell = actionCell;
            GameSystemManager.Instance.Push(this);
        }
    }
}
