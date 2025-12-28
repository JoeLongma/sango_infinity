using Sango.Game.Render.UI;
using UnityEngine;

namespace Sango.Game.Player
{
    public class GameRightButtonMenu : CommandSystemBase
    {
        public GameRightButtonMenu()
        {

        }

        public void Start(Vector3 startPoint)
        {
            if (Scenario.Cur.CurRunForce.IsPlayer)
            {
                ContextMenuData.MenuData.Clear();
                GameEvent.OnRightMouseButtonContextMenuShow?.Invoke(ContextMenuData.MenuData);
                if (!ContextMenuData.MenuData.IsEmpty())
                {
                    Render.UI.ContextMenu.Show(ContextMenuData.MenuData, startPoint);
                    PlayerCommand.Instance.Push(this);
                }
            }
        }

        public override void OnEnter()
        {

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
                    PlayerCommand.Instance.Back(); break;

                case CommandEventType.ClickDown:
                    {
                        if (!isOverUI)
                            PlayerCommand.Instance.Back();
                        break;
                    }
            }

        }
    }
}
