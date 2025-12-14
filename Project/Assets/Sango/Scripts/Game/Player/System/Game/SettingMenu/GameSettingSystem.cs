using Sango.Game.Render.UI;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;

namespace Sango.Game.Player
{
    public class GameSettingSystem : CommandSystemBase
    {
        public override void Init()
        {
            base.Init();
            Singleton<GameSave>.Instance.Init();
            Singleton<GameLoad>.Instance.Init();
            Singleton<GameBackToMain>.Instance.Init();
        }

        public void Start(Vector3 startPoint)
        {
            ContextMenuData.MenuData.Clear();
            GameEvent.OnGameSettingContextMenuShow?.Invoke(ContextMenuData.MenuData);
            if (!ContextMenuData.MenuData.IsEmpty())
            {
                ContextMenu.Show(ContextMenuData.MenuData, startPoint);
                PlayerCommand.Instance.Push(this);
            }
        }

        /// <summary>
        /// 离开当前命令的时候触发
        /// </summary>
        public override void OnDestroy()
        {

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
