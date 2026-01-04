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
        public List<ObjectSortTitle> sortItems;
        public Toggle[] toggleGroup;
        public UIObjectListItem[] uIObjectListItems;
        public Scrollbar scrollbar;
        protected List<UISortButton> sortButtonPool = new List<UISortButton>();
        public UISortButton sortTitleItem;
        public GameObject selectSortBtn;
        public RectTransform sorltTitleTransform;
        ObjectsDisplaySystem objectSelectSystem;
        protected int startIndex = 0;
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
            GameObject btn = GameObject.Instantiate(sortTitleItem.gameObject, sortTitleItem.transform.parent);
            UISortButton sortBtn = btn.GetComponent<UISortButton>();
            sortButtonPool.Add(sortBtn);
            return sortBtn;
        }

        public virtual void Init(ObjectsDisplaySystem objectSelectSystem)
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

        public virtual void UpdateItemStartIndex(int startIndex)
        {
            for (int i = 0; i < uIObjectListItems.Length; i++)
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
