using Sango.Game.Render.UI;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;

namespace Sango.Game.Player
{
    /// <summary>
    /// 建筑系统菜单
    /// </summary>
    public class BuildingSystem : GameSystem
    {
        public override void Init()
        {
            GameEvent.OnClick += OnClick;
        }

        void OnClick(Cell clickCell, Vector3 clickPosition, bool isOverUI)
        {
            if (clickCell.building == null) return;

            BuildingBase building = clickCell.building;
            if (building.IsCityBase()) return;
            if (building.BelongForce == Scenario.Cur.CurRunForce && building.BelongForce.IsPlayer)
            {
                ContextMenuData.MenuData.Clear();
                GameEvent.OnBuildingContextMenuShow?.Invoke(ContextMenuData.MenuData, building);
                if (!ContextMenuData.MenuData.IsEmpty())
                {
                    TargetBuilding = building;
                    ContextMenu.Show(ContextMenuData.MenuData, clickPosition);
                    Enter();
                }
            }
        }

        public BuildingBase TargetBuilding { get; set; }

        public override void OnEnter()
        {
            base.OnEnter();
            TargetBuilding.Render?.SetFlash(true);
        }

        public override void OnDestroy()
        {
            TargetBuilding.Render?.SetFlash(false);
            ContextMenu.CloseAll();
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClickDown:
                    {
                        if (ContextMenu.Close())
                            Exit();
                        break;
                    }
                case CommandEventType.ClickDown:
                    {
                        if (isOverUI) return;

                        Done();
                        break;
                    }
            }
        }
    }
}
