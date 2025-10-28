using System.Collections.Generic;
using UnityEngine;
using static Sango.Window;

namespace Sango.Game.Render.UI
{
    public class ContextMenu
    {
        static List<ContextMenuData>[] ContenDatas = new List<ContextMenuData>[] { new List<ContextMenuData>(), new List<ContextMenuData>(), new List<ContextMenuData>() };

        public static void Show(Vector2 position)
        {
            Show(null, position);
        }

        public static void Show(ContextMenuData itemData, Vector2 position)
        {
            int depth = 0;
            if (itemData != null)
                depth = itemData.depth + 1;
            if (depth >= 3)
                return;

            WindowInterface windowInterface = Window.Instance.ShowWindow("window_contextMenu");
            if (windowInterface != null)
            {
                UIContextMenu uIContextMenu = windowInterface.ugui_instance as UIContextMenu;
                if (uIContextMenu != null)
                {
                    uIContextMenu.Close(depth);
                    List<ContextMenuData> contextMenuDatas = ContenDatas[depth];
                    contextMenuDatas.Clear();
                    EventBase.OnContextMenuShow?.Invoke(itemData);
                    contextMenuDatas.Sort((a, b) => a.order.CompareTo(b.order));
                    uIContextMenu.Show(position, depth, ContenDatas[depth]);
                }
            }
        }
    }
}
