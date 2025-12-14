using Sango.Game.Render.UI;

namespace Sango.Game.Player
{
    public class GameSettingMenuBase : CommandSystemBase
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

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            switch (eventType)
            {
                case CommandEventType.RClickUp:
                    PlayerCommand.Instance.Back(); break;
            }

            base.HandleEvent(eventType, cell, clickPosition, isOverUI);
        }
    }
}
