using Sango.Game.Render.UI;
using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class TroopComandBase : CommandSystemBase
    {
        public Troop TargetTroop { get; set; }

        public string customMenuName;
        public int customMenuOrder;

        public override void Init()
        {
            GameEvent.OnTroopContextMenuShow += OnTroopContextMenuShow;
        }

        public override void Clear()
        {
            GameEvent.OnTroopContextMenuShow -= OnTroopContextMenuShow;
        }

        protected virtual void OnTroopContextMenuShow(ContextMenuData menuData, Troop troop)
        {
            if (troop.BelongForce != null && troop.BelongForce.IsPlayer && troop.BelongForce == Scenario.Cur.CurRunForce)
            {
                TargetTroop = troop;
                menuData.Add(customMenuName, customMenuOrder, troop, OnClickMenuItem, IsValid);
            }
        }

        protected virtual void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            Start(contextMenuItem.customData as Troop);
        }

        public virtual void Start(Troop troop)
        {
            TargetTroop = troop;
            PlayerCommand.Instance.Push(this);
        }
    }
}
