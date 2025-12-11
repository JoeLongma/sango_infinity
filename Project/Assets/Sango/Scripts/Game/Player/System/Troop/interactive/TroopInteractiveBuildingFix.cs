using Sango.Game.Render;
using Sango.Game.Render.UI;
using System.Collections.Generic;

namespace Sango.Game.Player
{
    public class TroopInteractiveBuildingFix : TroopInteractiveBase
    {
        List<Cell> MovePath { get; set; }
        protected bool isMoving = false;

        public TroopInteractiveBuildingFix()
        {
            customMenuName = "修理";
            customMenuOrder = 0;
        }

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }

        protected override void OnTroopInteractiveContextMenuShow(ContextMenuData menuData, Troop troop, Cell actionCell)
        {
            if (troop.BelongForce != null && troop.BelongForce.IsPlayer && troop.BelongForce == Scenario.Cur.CurRunForce)
            {
                TargetTroop = troop;
                ActionCell = actionCell;
                if (actionCell.building != null && actionCell.building.IsSameForce(troop) && !actionCell.building.IsCityBase())
                {
                    if(!actionCell.building.isComplte || actionCell.building.durability < actionCell.building.DurabilityLimit)
                        menuData.Add(customMenuName, customMenuOrder, actionCell, OnClickMenuItem, IsValid);
                }
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            ContextMenu.CloseAll();
            Singleton<TroopActionMenu>.Instance.ShowSpellRange();
            Singleton<TroopActionMenu>.Instance.troopRender.Clear();
            MovePath = Singleton<TroopSystem>.Instance.movePath;
            isMoving = true;
            if (MovePath.Count <= 2)
            {
                OnMoveDone();
                return;
            }

            Cell start = TargetTroop.cell;
            for (int i = 1; i < MovePath.Count-1; i++)
            {
                bool isLast = i == MovePath.Count - 2;
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

        public override void Update()
        {
            if (!isMoving)
            {
                if (TargetTroop.BuildBuilding(ActionCell, null))
                {
                    TargetTroop.ActionOver = true;
                    TargetTroop.Render?.UpdateRender();
                    Done();
                }
            }
        }

        public void OnMoveDone()
        {
            isMoving = false;
        }
    }
}
