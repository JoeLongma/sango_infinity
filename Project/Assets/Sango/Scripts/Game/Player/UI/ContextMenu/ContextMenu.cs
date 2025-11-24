using System.Collections.Generic;
using UnityEngine;
using static Sango.Window;

namespace Sango.Game.Render.UI
{
    public class ContextMenu
    {
        public static void Show(ContextMenuData itemData, Vector2 position)
        {
            Show(itemData.headList, position);
        }

        internal static void Show(List<ContextMenuItem> menuItems, Vector2 position)
        {
            if (menuItems.Count == 0)
                return;

            WindowInterface windowInterface = Window.Instance.Open("window_contextMenu");
            if (windowInterface != null)
            {
                UIContextMenu uIContextMenu = windowInterface.ugui_instance as UIContextMenu;
                if (uIContextMenu != null)
                {
                    Vector2 anchorPos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(uIContextMenu.GetComponent<RectTransform>(),
                        position, Game.Instance.UICamera, out anchorPos);

                    ContextMenuItem item = menuItems[0];
                    uIContextMenu.Close(item.depth);
                    menuItems.Sort((a, b) => a.order.CompareTo(b.order));
                    uIContextMenu.Show(anchorPos, item.depth, menuItems);
                }
            }
        }

        public static void SetVisible(bool b)
        {
            WindowInterface windowInterface = Window.Instance.GetWindow("window_contextMenu");
            if (windowInterface != null)
            {
                windowInterface.SetVisible(b);
            }
        }

        public static bool IsVisible()
        {
            WindowInterface windowInterface = Window.Instance.GetWindow("window_contextMenu");
            if (windowInterface == null) return false;
            return windowInterface.IsVisible();
        }


        public static void Add(ContextMenuData itemData)
        {
            //ContenDatas[itemData.depth].Add(itemData);
        }

        public static bool Close()
        {
            WindowInterface windowInterface = Window.Instance.GetWindow("window_contextMenu");
            if (windowInterface != null)
            {
                UIContextMenu uIContextMenu = windowInterface.ugui_instance as UIContextMenu;
                if (uIContextMenu.Close())
                {
                    Window.Instance.Close("window_contextMenu");
                    return true;
                }
            }
            return false;
        }

        public static void CloseAll()
        {
            WindowInterface windowInterface = Window.Instance.GetWindow("window_contextMenu");
            if (windowInterface != null)
            {
                UIContextMenu uIContextMenu = windowInterface.ugui_instance as UIContextMenu;
                uIContextMenu.Close(0);
                Window.Instance.Close("window_contextMenu");
            }
        }

    }
}
