using System.Collections.Generic;
using UnityEngine;

namespace Sango.Game.Render.UI
{
    public class UIContextMenu : UGUIWindow
    {
        public RectTransform[] menuRoot;
        public UIMenuItem[] menuItem;
        public GameObject menuLine;
        private List<UIMenuItem>[] nodePool = new List<UIMenuItem>[] { new List<UIMenuItem>(), new List<UIMenuItem>(), new List<UIMenuItem>() };
        private List<GameObject> linePool = new List<GameObject>();

        private UIMenuItem CreteNode(int index)
        {
            List<UIMenuItem> pool = nodePool[index];
            UIMenuItem rs;
            if (pool.Count > 0)
            {
                rs = pool[0];
                pool.RemoveAt(0);
            }
            else
            {
                rs = GameObject.Instantiate(menuItem[index].gameObject).GetComponent<UIMenuItem>();
            }
            rs.gameObject.SetActive(true);
            return rs;
        }

        private void Recycle(int index, UIMenuItem obj)
        {
            obj.gameObject.SetActive(false);
            nodePool[index].Add(obj);
        }

        private GameObject CreteLine()
        {
            List<GameObject> pool = linePool;
            GameObject rs;
            if (pool.Count > 0)
            {
                rs = pool[0];
                pool.RemoveAt(0);
            }
            else
            {
                rs = GameObject.Instantiate(menuLine);
            }
            rs.gameObject.SetActive(true);
            return rs;
        }

        private void Recycle(int index, GameObject obj)
        {
            obj.gameObject.SetActive(false);
            linePool.Add(obj);
        }

        public int showDepth = -1;
        public void Show(Vector2 screenPoint, int depth, List<ContextMenuItem> menuItems)
        {
            showDepth = depth;
            RectTransform root = menuRoot[showDepth];
            if (root != null)
            {
                root.gameObject.SetActive(true);
                UIMenuItem srcObj = menuItem[showDepth];
                RectTransform rectTransform = srcObj.GetComponent<RectTransform>();
                if (depth > 0)
                    screenPoint += new Vector2(rectTransform.sizeDelta.x + 2, 0);
                root.anchoredPosition = screenPoint;
                for (int i = 0; i < menuItems.Count; i++)
                {
                    ContextMenuItem contextMenuData = menuItems[i];
                    if (!string.IsNullOrEmpty(contextMenuData.title))
                    {
                        UIMenuItem obj = CreteNode(showDepth);
                        obj.SetParent(root.transform).SetValid(contextMenuData.valid).SetTitle(contextMenuData.title).SetListener(() =>
                        {
                            contextMenuData.OnClick(obj);
                        });
                    }
                    else
                    {
                        GameObject obj = CreteLine();
                        obj.transform.SetParent(root.transform, false);
                    }
                }
            }
        }

        public bool Close()
        {
            if (showDepth >= 0)
            {
                RectTransform rectTransform = menuRoot[showDepth];
                int childCount = rectTransform.childCount;
                for (int i = childCount - 1; i >= 0; i--)
                {
                    Transform trns = rectTransform.GetChild(i);
                    if (trns != null && trns.gameObject.activeSelf)
                    {
                        UIMenuItem uIMenuItem = trns.GetComponent<UIMenuItem>();
                        if (uIMenuItem != null)
                        {
                            Recycle(showDepth, uIMenuItem);
                        }
                        else
                        {
                            Recycle(showDepth, trns.gameObject);
                        }
                    }
                }
                menuRoot[showDepth].gameObject.SetActive(false);
                showDepth--;
            }
            return showDepth < 0;
        }

        public bool Close(int toDepth)
        {
            while (showDepth >= toDepth)
                Close();

            return showDepth < 0;
        }
    }
}
