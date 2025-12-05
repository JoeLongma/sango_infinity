using Sango.Game.Render;
using Sango.Game.Render.UI;
using System.Collections.Generic;

namespace Sango.Game.Player
{
    public class TroopCommandStay : TroopComandBase
    {
        List<Cell> MovePath { get; set; }

        public TroopCommandStay()
        {
            customMenuName = "待命";
            customMenuOrder = 0;
        }

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }

        protected override void OnTroopActionContextMenuShow(ContextMenuData menuData, Troop troop, Cell actionCell)
        {
            if (troop.BelongForce != null && troop.BelongForce.IsPlayer && troop.BelongForce == Scenario.Cur.CurRunForce)
            {
                TargetTroop = troop;
                ActionCell = actionCell;
                if (actionCell.building != null && actionCell.building.IsSameForce(troop) && actionCell.building.IsCityBase())
                    menuData.Add("进入", customMenuOrder, actionCell, OnClickMenuItem, IsValid);
                else
                    menuData.Add("待命", customMenuOrder, actionCell, OnClickMenuItem, IsValid);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            ContextMenu.CloseAll();
            Singleton<TroopActionMenu>.Instance.ShowSpellRange();
            Singleton<TroopActionMenu>.Instance.troopRender.Clear();
            MovePath = Singleton<TroopSystem>.Instance.movePath;
            Cell start = TargetTroop.cell;
            for (int i = 1; i < MovePath.Count; i++)
            {
                bool isLast = i == MovePath.Count - 1;
                Cell dest = MovePath[i];
                TroopMoveEvent @event = new TroopMoveEvent()
                {
                    troop = TargetTroop,
                    dest = dest,
                    start = start,
                    isLastMove = isLast
                };

                if (isLast)
                {
                    @event.doneAction = OnMoveDone;
                }

                RenderEvent.Instance.Add(@event);
                start = dest;
            }

        }

        public override void OnDestroy()
        {
            ContextMenu.CloseAll();
        }

        public void OnMoveDone()
        {
            TargetTroop.ActionOver = true;
            TargetTroop.Render?.UpdateRender();

            if (ActionCell.building != null && ActionCell.building.IsSameForce(TargetTroop) && ActionCell.building.IsCityBase())
                TargetTroop.EnterCity(ActionCell.building as City);

            Done();
        }
    }
}
