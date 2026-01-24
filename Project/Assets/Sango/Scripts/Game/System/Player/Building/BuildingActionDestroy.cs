using ContextMenu = Sango.Game.Render.UI.ContextMenu;

namespace Sango.Game.Player
{
    /// <summary>
    /// 建筑拆除
    /// </summary>
    public class BuildingActionDestroy : BuildingActionBase
    {
        public BuildingActionDestroy()
        {
            customMenuName = "拆除";
            customMenuOrder = 10000;
        }

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }
       
        public override void OnEnter()
        {
            ContextMenu.CloseAll();
            base.OnEnter();
            TargetBuilding.OnFall(null);
            Done();
        }
    }
}
