using Sango.Game.Render.UI;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;

namespace Sango.Game.Player
{
    public class BuildingSystem : CommandSystemBase
    {
        public BuildingBase TargetBuilding { get; set; }

        public void Start(BuildingBase building, Vector3 startPoint)
        {
            if (building.IsCity())
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
    }
}
