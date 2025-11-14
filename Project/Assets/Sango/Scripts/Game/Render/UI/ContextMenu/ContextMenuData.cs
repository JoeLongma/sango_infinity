using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sango.Game.Render.UI
{
    public class ContextMenuData
    {
        public static ContextMenuData MenuData = new ContextMenuData();

        public List<ContextMenuItem> headList = new List<ContextMenuItem>();
        public void Add(string title, int order, object custom, Action<ContextMenuItem> action)
        {
            string[] menuPath = title.Split('/');
            List<ContextMenuItem> checkList = headList;
            ContextMenuItem checkItem;
            for (int i = 0; i < menuPath.Length; ++i)
            {
                string depthTitle = menuPath[i];
                checkItem = checkList.Find(x => x.title == depthTitle);
                if (checkItem == null)
                {
                    ContextMenuItem contextMenuItem = new()
                    {
                        title = depthTitle,
                        depth = i,
                    };
                    if (i == menuPath.Length - 1)
                    {
                        contextMenuItem.order = order;
                        contextMenuItem.customData = custom;
                        contextMenuItem.action = action;
                    }
                    checkList.Add(contextMenuItem);
                    checkItem = contextMenuItem;
                    checkList = checkItem.children;
                }
                else
                {
                    checkList = checkItem.children;
                }
            }
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
        public void Clear()
        {
            headList.Clear();
        }

        public bool IsEmpty()
        {
            return headList.Count == 0;
        }
    }
}
