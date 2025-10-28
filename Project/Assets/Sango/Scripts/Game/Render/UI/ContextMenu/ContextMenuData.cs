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
                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, rect.position);
                ContextMenu.Show(this, screenPos);
                return;
            }
            action.Invoke(this);
        }

        public void Add(string title, int order, object custom, Action<ContextMenuData> action)
        {
            ContextMenuData contextMenuData = new()
            {
                title = title,
                order = order,
                depth = depth + 1,
                customData = custom,
                action = action
            };

            ContextMenu.Add(contextMenuData);
        }

        public void Add(string title, int order, object custom)
        {
            Add(title, order, custom, null);
        }
        public void Add(string title, int order)
        {
            Add(title, order, null, null);
        }
        public void Add(string title)
        {
            Add(title, -1, null, null);
        }
        public void AddLine()
        {
            Add(null, -1, null, null);
        }
    }
}
