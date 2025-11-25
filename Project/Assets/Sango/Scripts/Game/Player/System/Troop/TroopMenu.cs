using Sango.Game.Render.UI;
using Sango.Render;
using System.Collections.Generic;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;
namespace Sango.Game.Player
{
    public class TroopMenu : CommandSystemBase
    {
        public Troop TargetTroop { get; set; }
        public void Start(Troop troop, Vector3 startPoint)
        {
            if (!troop.IsAlive) return;
            TargetTroop = troop;
            ContextMenuData.MenuData.Clear();
            GameEvent.OnTroopContextMenuShow?.Invoke(ContextMenuData.MenuData, troop);
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

        public override void OnExit()
        {
            OnDestroy();
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClick:
                    {
                        ContextMenu.CloseAll();
                        break;
                    }

                case CommandEventType.Click:
                    {
                        break;
                    }
            }
        }
    }
}
