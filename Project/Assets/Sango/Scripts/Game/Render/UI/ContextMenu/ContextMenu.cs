using System.Collections.Generic;
using UnityEngine;
using static Sango.Window;

namespace Sango.Game.Render.UI
{
    public class ContextMenu
    {
        static ContextMenuData RootContextMenuData = new ContextMenuData() { depth = -1 };
        static List<ContextMenuData>[] ContenDatas = new List<ContextMenuData>[] { new List<ContextMenuData>(), new List<ContextMenuData>(), new List<ContextMenuData>() };

        public static void Show(Vector2 position)
        {
            Show(RootContextMenuData, position);
        }

        internal static void Show(ContextMenuData itemData, Vector2 screenPoint)
        {
            int depth = itemData.depth + 1;
            if (depth >= 3)
                return;

            List<ContextMenuData> contextMenuDatas = ContenDatas[depth];
            contextMenuDatas.Clear();
            GameEvent.OnContextMenuShow?.Invoke(itemData);
            if (contextMenuDatas.Count == 0)
                return;

            WindowInterface windowInterface = Window.Instance.ShowWindow("window_contextMenu");
            if (windowInterface != null)
            {
                UIContextMenu uIContextMenu = windowInterface.ugui_instance as UIContextMenu;
                if (uIContextMenu != null)
                {
                    Vector2 anchorPos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(uIContextMenu.GetComponent<RectTransform>(),
                        screenPoint, null, out anchorPos);

                    uIContextMenu.Close(depth);
                    contextMenuDatas.Sort((a, b) => a.order.CompareTo(b.order));
                    uIContextMenu.Show(anchorPos, depth, ContenDatas[depth]);
                }
            }
        }

        public static void Add(ContextMenuData itemData)
        {
            ContenDatas[itemData.depth].Add(itemData);
        }

        public static bool Close()
        {
            WindowInterface windowInterface = Window.Instance.GetWindow("window_contextMenu");
            if (windowInterface != null)
            {
                UIContextMenu uIContextMenu = windowInterface.ugui_instance as UIContextMenu;
                if (uIContextMenu.Close())
                {
                    Window.Instance.HideWindow("window_contextMenu");
                    return true;
                }
            }
            return false;
        }

        public static bool Close(int depth)
        {
            WindowInterface windowInterface = Window.Instance.GetWindow("window_contextMenu");
            if (windowInterface != null)
            {
                UIContextMenu uIContextMenu = windowInterface.ugui_instance as UIContextMenu;
                if (uIContextMenu.Close(depth))
                {
                    Window.Instance.HideWindow("window_contextMenu");
                    return true;
                }
            }
            return false;
        }

    }
}
