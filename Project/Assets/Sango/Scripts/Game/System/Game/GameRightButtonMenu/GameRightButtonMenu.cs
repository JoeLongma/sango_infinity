using Sango.Game.Render.UI;
using UnityEngine;

namespace Sango.Game.Player
{
    /// <summary>
    /// 游戏右键菜单
    /// </summary>
    [GameSystem(auto = true)]
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
                    // 特殊处理<进行>
                    UIContextMenu contextMenu = Render.UI.ContextMenu.Show(ContextMenuData.MenuData, clickPosition, ContextMenuType.Command);
                    PlayerEndTurn playerEndTurn = GameSystem.GetSystem<PlayerEndTurn>();
                    if (playerEndTurn != null)
                    {
                        if (contextMenu != null && contextMenu.menuItem.Length > 1)
                        {
                            UIMenuItem item = contextMenu.menuItem[1];
                            if (item != null)
                            {
                                item.SetTitle(playerEndTurn.customTitleName).SetListener(() =>
                                {
                                    Render.UI.ContextMenu.CloseAll();
                                    playerEndTurn.Push();
                                });
                            }
                        }
                    }
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
