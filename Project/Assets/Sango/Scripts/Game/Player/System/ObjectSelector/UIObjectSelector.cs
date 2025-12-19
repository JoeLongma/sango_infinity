using Sango.Game.Player;
using Sango.Loader;
using Sango.Render;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIObjectSelector : UGUIWindow
    {
        public List<ObjectSortTitle> sortItems;
        public Toggle[] toggleGroup;

        public UIObjectListItem[] uIObjectListItems;
        public RectTransform[] uIObjectListItemsRect;
        public Scrollbar scrollbar;

        List<UISortButton> sortButtonPool = new List<UISortButton>();
        public UISortButton sortTitleItem;

        public RectTransform sorltTitleTransform;

        ObjectSelectSystem objectSelectSystem;
        int startIndex = 0;
        bool dragFlag = false;
        void Awake()
        {
            for (int i = 0; i < toggleGroup.Length; i++)
            {
                toggleGroup[i].onValueChanged.RemoveAllListeners();
                int btIndex = i;
                toggleGroup[i].onValueChanged.AddListener((b) => OnSortGroupChanged(btIndex, b));
            }
        }

        UISortButton CreateSortButtonItem()
        {
            GameObject btn = GameObject.Instantiate(sortTitleItem.gameObject, sortTitleItem.transform.parent);
            UISortButton sortBtn = btn.GetComponent<UISortButton>();
            sortButtonPool.Add(sortBtn);
            return sortBtn;
        }

        public virtual void Init(ObjectSelectSystem objectSelectSystem)
        {
            this.objectSelectSystem = objectSelectSystem;
            toggleGroup[0].GetComponentInChildren<Text>(true).text = objectSelectSystem.customSortTitleName;
            sortItems = objectSelectSystem.customSortItems;
            toggleGroup[0].isOn = true;
            for (int i = 1; i < toggleGroup.Length; i++)
                toggleGroup[i].GetComponentInChildren<Text>(true).text = objectSelectSystem.GetSortTitleGroupName(i);

            startIndex = 0;
            int dataCount = objectSelectSystem.Objects.Count;
            if (dataCount < uIObjectListItems.Length)
            {
                scrollbar.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                scrollbar.transform.parent.gameObject.SetActive(true);
                scrollbar.size = (float)uIObjectListItems.Length / (float)dataCount;
                scrollbar.SetValueWithoutNotify(0);
            }


            UpdateSortContent();
            OnScrollBarValueChange(0);
        }

        public void UpdateSortContent()
        {
            for (int j = 0; j < uIObjectListItems.Length; j++)
            {
                UIObjectListItem listItem = uIObjectListItems[j];
                listItem.Clear();
            }

            for (int i = 0; i < sortItems.Count; i++)
            {
                ObjectSortTitle sortTitle = sortItems[i];
                UISortButton uIPersonSortButton;
                if (i < sortButtonPool.Count)
                    uIPersonSortButton = sortButtonPool[i];
                else
                    uIPersonSortButton = CreateSortButtonItem();

                uIPersonSortButton.gameObject.SetActive(true);
                uIPersonSortButton.Clear().SetWidth(sortTitle.width).SetName(sortTitle.name);

                uIPersonSortButton.onClick = (up) =>
                {
                    objectSelectSystem.Objects.Sort(sortTitle.Sort);
                    if (!up) objectSelectSystem.Objects.Reverse();
                    scrollbar.SetValueWithoutNotify(0);
                    OnScrollBarValueChange(0);
                };

                for (int j = 0; j < uIObjectListItems.Length; j++)
                {
                    UIObjectListItem listItem = uIObjectListItems[j];
                    listItem.Add("", sortTitle.width);
                }
            }

            for (int i = sortItems.Count; i < sortButtonPool.Count; i++)
                sortButtonPool[i].gameObject.SetActive(false);
        }

        public void UpShow()
        {
            if (startIndex > 0)
                startIndex--;
            UpdateItemStartIndex(startIndex);
            scrollbar.SetValueWithoutNotify((float)startIndex / (objectSelectSystem.Objects.Count - uIObjectListItems.Length));
        }

        public void DownShow()
        {
            if (startIndex < objectSelectSystem.Objects.Count - uIObjectListItems.Length)
                startIndex++;

            UpdateItemStartIndex(startIndex);
            scrollbar.SetValueWithoutNotify((float)startIndex / (objectSelectSystem.Objects.Count - uIObjectListItems.Length));
        }

        public void OnScrollBarValueChange(float value)
        {
            startIndex = (int)UnityEngine.Mathf.Lerp(0, objectSelectSystem.Objects.Count - uIObjectListItems.Length, value);
            UpdateItemStartIndex(startIndex);
        }

        public void UpdateItemStartIndex(int startIndex)
        {
            for (int i = 0; i < uIObjectListItems.Length; i++)
            {
                UIObjectListItem listItem = uIObjectListItems[i];
                SangoObject sango = objectSelectSystem.Objects[i + startIndex];
                bool isSelected = objectSelectSystem.selected.Contains(sango);
                for (int j = 0; j < sortItems.Count; j++)
                {
                    ObjectSortTitle sortTitle = sortItems[j];
                    listItem.Set(j, sortTitle.GetValueStr(sango));
                }
                listItem.index = i + startIndex;
                listItem.SetSelected(isSelected);
            }
        }


        public override void OnHide()
        {
            base.OnHide();
            for (int i = 0; i < sortButtonPool.Count; i++)
                sortButtonPool[i].gameObject.SetActive(false);
        }

        public void OnSure()
        {
            objectSelectSystem.OnSure();
        }

        public void OnCancel()
        {
            objectSelectSystem.OnCancel();
        }

        public void OnSortGroupChanged(int index, bool b)
        {
            if (b)
            {
                sortItems = objectSelectSystem.GetSortTitleGroup(index);
                UpdateSortContent();
                OnScrollBarValueChange(0);
            }

        }

        public void OnPersonListSelected(UIObjectListItem item)
        {
            if (!item.IsSelected() && objectSelectSystem.IsPersonLimit())
                return;

            item.SetSelected(!item.IsSelected());
            if (item.IsSelected())
            {
                objectSelectSystem.Add(item.index);
            }
            else
            {
                objectSelectSystem.Remove(item.index);
            }
        }

        public void OnPointDownPersonListSelected(UIObjectListItem item)
        {
            dragFlag = !item.IsSelected();
        }

        public void OnDragPersonListSelected(UIObjectListItem item)
        {
            if (dragFlag && objectSelectSystem.IsPersonLimit())
                return;

            if (!dragFlag && objectSelectSystem.IsPersonEmpty())
                return;

            if (item.IsSelected() && !dragFlag)
            {
                item.SetSelected(false);
                objectSelectSystem.Remove(item.index);
            }
            else if (!item.IsSelected() && dragFlag)
            {
                item.SetSelected(true);
                objectSelectSystem.Add(item.index);
            }

            for (int i = 0; i < uIObjectListItems.Length; i++)
            {
                RectTransform itemRect = uIObjectListItemsRect[i];
                UIObjectListItem listItem = uIObjectListItems[i];
                if (listItem != item && RectTransformUtility.RectangleContainsScreenPoint(itemRect, Input.mousePosition, Sango.Game.Game.Instance.UICamera))
                {
                    if (listItem.IsSelected() && !dragFlag)
                    {
                        listItem.SetSelected(false);
                        objectSelectSystem.Remove(listItem.index);
                    }
                    else if (!listItem.IsSelected() && dragFlag)
                    {
                        listItem.SetSelected(true);
                        objectSelectSystem.Add(listItem.index);
                    }
                }
            }
        }
    }
}
