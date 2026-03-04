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
    public class UIObjectDisplay : UGUIWindow
    {
        public Text title;
        public List<ObjectSortTitle> sortItems;
        public Toggle[] toggleGroup;
        public UIObjectListItem[] uIObjectListItems;
        public UIObjectListItem creatItemObj;
        public Scrollbar scrollbar;
        public Scrollbar scrollbar_h;
        protected List<UISortButton> sortButtonPool = new List<UISortButton>();
        public UISortButton sortTitleItem;
        public GameObject selectSortBtn;
        public RectTransform sorltTitleTransform;
        public RectTransform maskRect;
        public RectTransform[] contentRect;
        ObjectsDisplaySystem objectSelectSystem;
        protected int startIndex = 0;
        protected int itemWidth = 0;
        protected int itemCount = 0;
        protected override void Awake()
        {
            for (int i = 0; i < toggleGroup.Length; i++)
            {
                toggleGroup[i].onValueChanged.RemoveAllListeners();
                int btIndex = i;
                toggleGroup[i].onValueChanged.AddListener((b) => OnSortGroupChanged(btIndex, b));
            }
        }

        protected UISortButton CreateSortButtonItem()
        {
            GameObject btn = GameObject.Instantiate(sortTitleItem.gameObject, sorltTitleTransform);
            UISortButton sortBtn = btn.GetComponent<UISortButton>();
            sortButtonPool.Add(sortBtn);
            return sortBtn;
        }

        public virtual void Init(ObjectsDisplaySystem objectSelectSystem)
        {
            this.objectSelectSystem = objectSelectSystem;
            title.text = objectSelectSystem.customSortTitleName;
            sortItems = objectSelectSystem.customSortItems;

            itemWidth = GetContentWidth();
            itemCount = uIObjectListItems.Length;
            bool show_scrollbar_h = maskRect.rect.width < itemWidth;
            scrollbar_h.gameObject.SetActive(show_scrollbar_h);
            if (show_scrollbar_h)
            {
                itemCount--;
                scrollbar_h.size = (float)maskRect.rect.width / (float)itemWidth;
                scrollbar_h.SetValueWithoutNotify(0);
                uIObjectListItems[uIObjectListItems.Length - 1].gameObject.SetActive(false);
            }
            else
            {
                uIObjectListItems[uIObjectListItems.Length - 1].gameObject.SetActive(true);
            }

            toggleGroup[0].GetComponentInChildren<Text>(true).text = objectSelectSystem.customSortTitleName;
            toggleGroup[0].isOn = true;
            for (int i = 1; i < toggleGroup.Length; i++)
                toggleGroup[i].GetComponentInChildren<Text>(true).text = objectSelectSystem.GetSortTitleGroupName(i);

            startIndex = 0;
            int dataCount = objectSelectSystem.Objects.Count;
            if (dataCount < itemCount)
            {
                scrollbar.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                scrollbar.transform.parent.gameObject.SetActive(true);
                scrollbar.size = (float)itemCount / (float)dataCount;
                scrollbar.SetValueWithoutNotify(0);
            }

            for (int j = 0; j < uIObjectListItems.Length; j++)
            {
                UIObjectListItem listItem = uIObjectListItems[j];
                Vector2 p = listItem.contentRect.anchoredPosition;
                p.x = 0;
                listItem.contentRect.anchoredPosition = p;
            }

            UpdateSortContent();
            OnScrollBarValueChange(0);

            foreach (RectTransform r in contentRect)
            {
                Vector2 p = r.anchoredPosition;
                p.x = 0;
                r.anchoredPosition = p;
            }

        }

        public int GetContentWidth()
        {
            int w = 0;
            for (int i = 0; i < sortItems.Count; i++)
            {
                ObjectSortTitle sortTitle = sortItems[i];
                w += sortTitle.width;
            }
            return w + 24;
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

                if (i == 0)
                {
                    uIPersonSortButton = sortTitleItem;
                }
                else
                {
                    if (i - 1 < sortButtonPool.Count)
                        uIPersonSortButton = sortButtonPool[i - 1];
                    else
                        uIPersonSortButton = CreateSortButtonItem();
                }



                uIPersonSortButton.gameObject.SetActive(true);
                uIPersonSortButton.Clear().SetWidth(sortTitle.width).SetName(sortTitle.name);

                uIPersonSortButton.onClick = (up) =>
                {
                    objectSelectSystem.Objects.Sort(sortTitle.Sort);
                    if (!up) objectSelectSystem.Objects.Reverse();
                    scrollbar.SetValueWithoutNotify(0);
                    OnScrollBarValueChange(0);
                };

                if (i > 0)
                {
                    for (int j = 0; j < itemCount; j++)
                    {
                        UIObjectListItem listItem = uIObjectListItems[j];
                        listItem.Add("", sortTitle.width);
                    }
                }
                else
                {
                    for (int j = 0; j < itemCount; j++)
                    {
                        UIObjectListItem listItem = uIObjectListItems[j];
                        listItem.textItem.SetWidth(sortTitle.width);
                    }
                }
            }

            for (int i = sortItems.Count - 1; i < sortButtonPool.Count; i++)
                sortButtonPool[i].gameObject.SetActive(false);
        }

        public void UpShow()
        {
            if (startIndex > 0)
                startIndex--;
            UpdateItemStartIndex(startIndex);
            scrollbar.SetValueWithoutNotify((float)startIndex / (objectSelectSystem.Objects.Count - itemCount));
        }

        public void DownShow()
        {
            if (startIndex < objectSelectSystem.Objects.Count - itemCount)
                startIndex++;

            UpdateItemStartIndex(startIndex);
            scrollbar.SetValueWithoutNotify((float)startIndex / (objectSelectSystem.Objects.Count - itemCount));
        }

        public void OnScrollBarValueChange(float value)
        {
            startIndex = (int)UnityEngine.Mathf.Lerp(0, objectSelectSystem.Objects.Count - itemCount, value);
            UpdateItemStartIndex(startIndex);
        }

        public void OnScrollBar_H_ValueChange(float value)
        {
            float dis = (float)itemWidth - (float)maskRect.rect.width;
            float pos = -(int)UnityEngine.Mathf.Lerp(0, dis, value);
            foreach (RectTransform r in contentRect)
            {
                Vector2 p = r.anchoredPosition;
                p.x = pos;
                r.anchoredPosition = p;
            }

            for (int j = 0; j < uIObjectListItems.Length; j++)
            {
                UIObjectListItem listItem = uIObjectListItems[j];
                Vector2 p = listItem.contentRect.anchoredPosition;
                p.x = pos;
                listItem.contentRect.anchoredPosition = p;
            }
        }


        public virtual void UpdateItemStartIndex(int startIndex)
        {
            for (int i = 0; i < itemCount; i++)
            {
                UIObjectListItem listItem = uIObjectListItems[i];
                SangoObject sango = objectSelectSystem.Objects[i + startIndex];
                for (int j = 0; j < sortItems.Count; j++)
                {
                    ObjectSortTitle sortTitle = sortItems[j];
                    listItem.Set(j, sortTitle.GetValueStr(sango));
                }
                listItem.index = i + startIndex;
            }
        }


        public override void OnHide()
        {
            base.OnHide();
            for (int i = 0; i < sortButtonPool.Count; i++)
                sortButtonPool[i].gameObject.SetActive(false);
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

        public void Update()
        {
            Vector2 scrollWheel = Input.mouseScrollDelta;
            if (scrollWheel.y > 0)
            {
                UpShow();
            }
            else if (scrollWheel.y < 0)
            {
                DownShow();
            }
        }
    }
}
