using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sango.Game.Render.UI
{
    public class ContextMenuItem : IContextMenuItem
    {
        public string title;
        public int order;
        public int depth;
        public Action<ContextMenuItem> action;
        public object CustomData { get; set; }
        public List<ContextMenuItem> children = new List<ContextMenuItem>();
        public bool valid;

        public void OnClick(UIMenuItem item)
        {
            if (action == null)
            {
                RectTransform rect = item.GetComponent<RectTransform>();
                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Game.Instance.UICamera, rect.position);
                ContextMenu.Show(children, screenPos);
                return;
            }
            action.Invoke(this);
        }
    }
}
