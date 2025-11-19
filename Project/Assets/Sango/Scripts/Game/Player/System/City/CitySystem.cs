using Sango.Game.Render.UI;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;

namespace Sango.Game.Player
{
    public class CitySystem : CommandSystemBase
    {
        public City TargetCity { get; set; }

        public void Start(City city, Vector3 startPoint)
        {
            ContextMenuData.MenuData.Clear();
            GameEvent.OnCityContextMenuShow?.Invoke(ContextMenuData.MenuData, city);
            if (!ContextMenuData.MenuData.IsEmpty())
            {
                TargetCity = city;
                ContextMenu.Show(ContextMenuData.MenuData, startPoint);
                PlayerCommand.Instance.Push(this);
            }
        }

        /// <summary>
        /// 离开当前命令的时候触发
        /// </summary>
        public override void OnDestroy() {

            ContextMenu.CloseAll();
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell)
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

            }
        }
    }
}
