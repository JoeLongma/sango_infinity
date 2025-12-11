using Sango.Game.Render;
using Sango.Render;
using System.Collections.Generic;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;

namespace Sango.Game.Player
{
    public class BuildingActionDestroy : BuildingActionBase
    {
        public BuildingActionDestroy()
        {
            customMenuName = "拆除";
            customMenuOrder = 0;
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
