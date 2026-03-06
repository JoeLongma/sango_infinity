using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIObjectList : MonoBehaviour
    {
        public Scrollbar scrollbar;
        public ObjectSortTitle targetObjectSortTitle;
        public List<SangoObject> objectDatas = new List<SangoObject>();
        public UIObjectListItem[] uIObjectListItems;

        public delegate void OnObjectSelected(int index);
        protected OnObjectSelected onObjectSelected;
        protected int startIndex = 0;
        protected int itemCount = 0;
        protected int totalCount = 0;
        protected int selectedIndex = 0;

        private void Awake()
        {
            itemCount = uIObjectListItems.Length;
        }

        public void Init(List<SangoObject> objectDatas, ObjectSortTitle objectSortTitle, OnObjectSelected onObjectSelected)
        {
            this.objectDatas = objectDatas;
            targetObjectSortTitle = objectSortTitle;
            totalCount = objectDatas.Count;
            if (totalCount < itemCount)
            {
                scrollbar.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                scrollbar.transform.parent.gameObject.SetActive(true);
                scrollbar.size = (float)itemCount / (float)totalCount;
                scrollbar.SetValueWithoutNotify(0);
            }
            startIndex = 0;
            selectedIndex = 0;
            this.onObjectSelected = onObjectSelected;
            OnScrollBarValueChange(0);
        }

        public void SelectDefaultObject(SangoObject obj)
        {
            for(int i = 0; i < totalCount; i++)
            {
                if (objectDatas[i] == obj)
                {
                    selectedIndex = i;
                    break;
                }
            }

            if(startIndex + itemCount <= selectedIndex)
            {
                startIndex = selectedIndex - itemCount + 1;
                scrollbar.SetValueWithoutNotify((float)startIndex / (totalCount - itemCount));
            }

            UpdateItemStartIndex(startIndex);
        }

        public void OnScrollBarValueChange(float value)
        {
            startIndex = (int)UnityEngine.Mathf.Lerp(0, totalCount - itemCount, value);
            UpdateItemStartIndex(startIndex);
        }

        public void UpdateItemStartIndex(int startIndex)
        {
            for (int i = 0; i < itemCount; i++)
            {
                UIObjectListItem listItem = uIObjectListItems[i];
                int destIndex = i + startIndex;
                listItem.index = destIndex;
                if (destIndex < totalCount)
                {
                    listItem.SetSelected(selectedIndex == destIndex);
                    listItem.textItem.label.text = targetObjectSortTitle.GetValueStr(objectDatas[destIndex]);
                }
                else
                {
                    listItem.SetSelected(false);
                    listItem.textItem.label.text = "";
                }
            }
        }

        public void UpShow()
        {
            if (startIndex > 0)
                startIndex--;
            UpdateItemStartIndex(startIndex);
            scrollbar.SetValueWithoutNotify((float)startIndex / (totalCount - itemCount));
        }

        public void DownShow()
        {
            if (startIndex < totalCount - itemCount)
                startIndex++;

            UpdateItemStartIndex(startIndex);
            scrollbar.SetValueWithoutNotify((float)startIndex / (totalCount - itemCount));
        }

        public virtual void OnItemSelected(UIObjectListItem item)
        {
            if (item.index >= totalCount)
                return;

            int destIndex = item.index;
            if (destIndex == selectedIndex)
                return;

            for (int i = 0; i < itemCount; i++)
            {
                UIObjectListItem listItem = uIObjectListItems[i];
                if (listItem.index == selectedIndex)
                {
                    listItem.SetSelected(false);
                    break;
                }
            }
            selectedIndex = destIndex;
            item.SetSelected(true);
            onObjectSelected?.Invoke(selectedIndex);
        }
    }
}