using Sango.Game.Render.UI;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;
namespace Sango.Game.Player
{
    public class TroopSystem : CommandSystemBase
    {
        public Troop TargetTroop { get; set; }
        public void Start(Troop troop, Vector3 startPoint)
        {
            ContextMenuData.MenuData.Clear();
            GameEvent.OnTroopContextMenuShow?.Invoke(ContextMenuData.MenuData, troop);
            if (!ContextMenuData.MenuData.IsEmpty())
            {
                TargetTroop = troop;
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

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClick:
                    {
                        if (ContextMenu.Close())
                            PlayerCommand.Instance.Back();
                        break;
                    }

                case CommandEventType.ClickDown:
                    {
                        Done();
                        break;
                    }
            }
        }
    }
}
