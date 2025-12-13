using Sango.Game.Render.UI;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;

namespace Sango.Game.Player
{
    public class BuildingSystem : CommandSystemBase
    {
        public override void Init()
        {
            base.Init();
            Singleton<BuildingActionDestroy>.Instance.Init(); // 拆除
            Singleton<BuildingActionUpgrade>.Instance.Init(); // 升级
        }

        public BuildingBase TargetBuilding { get; set; }

        public void Start(BuildingBase building, Vector3 startPoint)
        {
            if (building.IsCityBase())
            {
                Singleton<CitySystem>.Instance.Start((City)building, startPoint);
                return;
            }

            if (building.BelongForce == Scenario.Cur.CurRunForce && building.BelongForce.IsPlayer)
            {
                ContextMenuData.MenuData.Clear();
                GameEvent.OnBuildingContextMenuShow?.Invoke(ContextMenuData.MenuData, building);
                if (!ContextMenuData.MenuData.IsEmpty())
                {
                    TargetBuilding = building;
                    ContextMenu.Show(ContextMenuData.MenuData, startPoint);
                    PlayerCommand.Instance.Push(this);
                }
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            //Window.Instance.Open("window_city_info_panel", TargetCity);
            TargetBuilding.Render?.SetFlash(true);
        }

        public override void OnDestroy()
        {
            TargetBuilding.Render?.SetFlash(false);
            //Window.Instance.Close("window_city_info_panel");
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
                            PlayerCommand.Instance.Back();

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
