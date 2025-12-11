using Sango.Game.Render.UI;

namespace Sango.Game.Player
{
    public class BuildingActionBase : CommandSystemBase
    {
        public BuildingBase TargetBuilding { get; set; }

        public string customMenuName;
        public int customMenuOrder;

        public override void Init()
        {
            GameEvent.OnBuildingContextMenuShow += OnBuildingContextMenuShow;
        }

        public override void Clear()
        {
            GameEvent.OnBuildingContextMenuShow -= OnBuildingContextMenuShow;
        }

        protected virtual void OnBuildingContextMenuShow(ContextMenuData menuData, BuildingBase building)
        {
            if (building.BelongForce != null && building.BelongForce.IsPlayer && building.BelongForce == Scenario.Cur.CurRunForce)
            {
                TargetBuilding = building;
                menuData.Add(customMenuName, customMenuOrder, null, OnClickMenuItem, IsValid);
            }
        }

        protected virtual void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            Start(TargetBuilding);
        }

        public virtual void Start(BuildingBase building)
        {
            TargetBuilding = building;
            PlayerCommand.Instance.Push(this);
        }
    }
}
