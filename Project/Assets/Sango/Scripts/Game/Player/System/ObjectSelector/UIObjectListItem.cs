using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIObjectListItem : MonoBehaviour
    {
        public UITextItem textItem;
        public UISelectItem selectItem;
        List<UITextItem> pool = new List<UITextItem>();
        List<UITextItem> usedItems = new List<UITextItem>();
        public int index;
        public delegate void OnSelect(UIObjectListItem item);
        public delegate void OnShow(UIObjectListItem item);
        public OnSelect onSelected;
        public OnShow onShow;

        void ScrollCellIndex(int idx)
        {
            index = idx;
            onShow?.Invoke(this);
        }
        public void OnClick()
        {
            onSelected?.Invoke(this);
        }

        public void Clear()
        {
            for (int i = 0; i < usedItems.Count; i++)
            {
                usedItems[i].gameObject.SetActive(false);
                pool.Add(usedItems[i]);
            }
            usedItems.Clear();
        }

        public void Add(string content, int width)
        {
            UITextItem item;
            if (pool.Count == 0)
            {
                GameObject obj = GameObject.Instantiate(textItem.gameObject, textItem.transform.parent);
                item = obj.GetComponent<UITextItem>();
            }
            else
            {
                item = pool[0];
                pool.RemoveAt(0);
            }
            usedItems.Add(item);
            item.gameObject.SetActive(true);
            item.SetWidth(width).SetText(content);
            item.transform.SetAsLastSibling();
        }

        public void Set(int index, string content)
        {
            usedItems[index].SetText(content);
        }

        public void SetSelected(bool b)
        {
            selectItem.SetSelected(b);
            for (int i = 0; i < usedItems.Count; i++)
            {
                usedItems[i].SetSelected(b);
            }
        }
        public bool IsSelected()
        {
            return selectItem.IsSelected();
        }
    }
}