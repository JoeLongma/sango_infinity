using Sango.Game.Render.UI;
using UnityEngine;

namespace Sango.Game.Player
{
    /// <summary>
    /// 游戏右键菜单
    /// </summary>
    public class GameRightButtonMenu : GameSystem
    {
        public override void Init()
        {
            GameEvent.OnRClick += OnRClick;
        }

        void OnRClick(Cell clickCell, Vector3 clickPosition, bool isOverUI)
        {
            if (Scenario.Cur.CurRunForce.IsPlayer)
            {
                ContextMenuData.MenuData.Clear();
                GameEvent.OnRightMouseButtonContextMenuShow?.Invoke(ContextMenuData.MenuData);
                if (!ContextMenuData.MenuData.IsEmpty())
                {
                    Render.UI.ContextMenu.Show(ContextMenuData.MenuData, clickPosition);
                    Enter();
                }
            }
        }

        public override void OnDestroy()
        {
            Render.UI.ContextMenu.CloseAll();
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClickDown:
                    Exit(); break;

                case CommandEventType.ClickDown:
                    {
                        if (!isOverUI)
                            Exit();
                        break;
                    }
            }
        }
    }
}
