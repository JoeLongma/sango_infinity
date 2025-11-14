using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIPersonListItem : MonoBehaviour
    {
        public UITextItem textItem;
        public UISelectItem selectItem;
        List<UITextItem> items = new List<UITextItem>();

        public int index;
        public delegate void OnSelect(int idx);
        public delegate void OnShow(UIPersonListItem item);
        public OnSelect onSelected;
        public OnShow onShow;

        void ScrollCellIndex(int idx)
        {
            index = idx;
            onShow?.Invoke(this);
        }
        public void OnClick()
        {
            onSelected?.Invoke(index);
        }
    }
}