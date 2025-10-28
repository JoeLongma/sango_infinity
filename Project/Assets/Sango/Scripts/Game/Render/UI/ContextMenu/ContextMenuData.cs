using System;
using UnityEngine;

namespace Sango.Game.Render.UI
{
    public class ContextMenuData
    {
        public string title;
        public int order;
        public int depth;
        public Action<ContextMenuData> action;
        public object customData;

        public void OnClick(UIMenuItem item)
        {
            if (action == null)
            {
                RectTransform rect = item.GetComponent<RectTransform>();
                ContextMenu.Show(this, rect.anchoredPosition + new Vector2(rect.sizeDelta.x, rect.sizeDelta.y));
                return;
            }
            action.Invoke(this);
        }
    }
}
