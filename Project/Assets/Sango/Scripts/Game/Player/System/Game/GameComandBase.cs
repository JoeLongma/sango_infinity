using Sango.Game.Render.UI;
using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class GameComandBase : CommandSystemBase
    {
        public string customMenuName;
        public int customMenuOrder;
        public string windowName;

        public override bool IsValid => true;

        public override void Init()
        {
            GameEvent.OnGameSettingContextMenuShow += OnGameSettingContextMenuShow;
        }

        public override void Clear()
        {
            GameEvent.OnGameSettingContextMenuShow -= OnGameSettingContextMenuShow;
        }

        protected virtual void OnGameSettingContextMenuShow(ContextMenuData menuData)
        {
             menuData.Add(customMenuName, customMenuOrder, null, OnClickMenuItem, IsValid);
        }

        protected virtual void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            ContextMenu.CloseAll();
            PlayerCommand.Instance.Push(this);
        }
    }
}
