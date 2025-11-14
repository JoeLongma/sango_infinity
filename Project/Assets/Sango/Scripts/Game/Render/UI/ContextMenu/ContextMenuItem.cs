using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sango.Game.Render.UI
{
    public class ContextMenuItem
    {
        public string title;
        public int order;
        public int depth;
        public Action<ContextMenuItem> action;
        public object customData;
        public List<ContextMenuItem> children = new List<ContextMenuItem>();

        public void OnClick(UIMenuItem item)
        {
            if (action == null)
            {
                RectTransform rect = item.GetComponent<RectTransform>();
                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, rect.position);
                ContextMenu.Show(children, screenPos);
                return;
            }
            action.Invoke(this);
        }
    }
}
