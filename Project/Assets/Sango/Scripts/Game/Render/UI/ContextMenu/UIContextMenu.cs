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
        public void Show(Vector2 pos, int depth, List<ContextMenuData> contextMenuDatas)
        {
            showDepth = depth;
            RectTransform root = menuRoot[showDepth];
            if (root != null)
            {
                root.anchoredPosition = pos;
                for (int i = 0; i < contextMenuDatas.Count; i++)
                {
                    ContextMenuData contextMenuData = contextMenuDatas[i];
                    if (string.IsNullOrEmpty(contextMenuData.title))
                    {
                        UIMenuItem obj = CreteNode(showDepth);
                        obj.transform.SetParent(root.transform, false);
                        obj.title.text = contextMenuData.title;
                        obj.button.onClick.RemoveAllListeners();
                        obj.button.onClick.AddListener(() =>
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
                for (int i = 0; i <= childCount; i++)
                {
                    Transform trns = rectTransform.GetChild(i);
                    if (trns != null && trns.gameObject.activeSelf)
                    {
                        UIMenuItem  uIMenuItem = trns.GetComponent<UIMenuItem>();
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
            while (showDepth > toDepth)
                Close();

            return showDepth < 0;
        }
    }
}
