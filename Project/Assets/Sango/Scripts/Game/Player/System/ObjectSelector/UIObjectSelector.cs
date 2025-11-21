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
    public class UIObjectSelector : UGUIWindow, LoopScrollPrefabSource, LoopScrollDataSource
    {
        Stack<Transform> pool = new Stack<Transform>();
        public GameObject objectItemObject;
        public LoopScrollRect loopScrollRect;

        public List<ObjectSortTitle> sortItems;

        public Toggle[] toggleGroup;

        List<UISortButton> sortButtonPool = new List<UISortButton>();
        public UISortButton sortTitleItem;

        public RectTransform sorltTitleTransform;

        ObjectSelectSystem objectSelectSystem;

        void Awake()
        {
            for (int i = 0; i < toggleGroup.Length; i++)
            {
                toggleGroup[i].onValueChanged.RemoveAllListeners();
                toggleGroup[i].onValueChanged.AddListener((b) => OnSortGroupChanged(i, b));
            }
        }

        UISortButton CreateSortButtonItem()
        {
            GameObject btn = GameObject.Instantiate(sortTitleItem.gameObject, sortTitleItem.transform.parent);
            UISortButton sortBtn = btn.GetComponent<UISortButton>();
            sortButtonPool.Add(sortBtn);
            return sortBtn;
        }

        public override void OnShow()
        {
            loopScrollRect.prefabSource = this;
            loopScrollRect.dataSource = this;
        }

        public virtual void Init(ObjectSelectSystem objectSelectSystem)
        {
            this.objectSelectSystem = objectSelectSystem;
            toggleGroup[0].GetComponentInChildren<Text>(true).text = objectSelectSystem.customSortTitleName;
            sortItems = objectSelectSystem.customSortItems;
            toggleGroup[0].isOn = true;
            for (int i = 1; i < toggleGroup.Length; i++)
                toggleGroup[i].GetComponentInChildren<Text>(true).text = objectSelectSystem.GetSortTitleGroupName(i);
            loopScrollRect.totalCount = objectSelectSystem.Objects.Count;
            UpdateSortContent();
        }

        public void UpdateSortContent()
        {
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
            }

            for (int i = sortItems.Count; i < sortButtonPool.Count; i++)
                sortButtonPool[i].gameObject.SetActive(false);

            loopScrollRect.RefillCells();
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

        public GameObject GetObject(int index)
        {
            if (pool.Count == 0)
            {
                GameObject obj = Instantiate(objectItemObject);
                UIObjectListItem uiObjectListItem = obj.GetComponent<UIObjectListItem>();
                if (uiObjectListItem != null)
                {
                    uiObjectListItem.onSelected = OnPersonListSelected;
                    uiObjectListItem.onShow = OnPersonListShow;
                }
                return obj;
            }
            Transform candidate = pool.Pop();
            candidate.gameObject.SetActive(true);
            return candidate.gameObject;
        }

        public void ReturnObject(Transform trans)
        {
            trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            pool.Push(trans);
        }

        public void ProvideData(Transform transform, int idx)
        {
            transform.SendMessage("ScrollCellIndex", idx);
        }

        public void OnSortGroupChanged(int index, bool b)
        {
            if (b)
            {
                sortItems = objectSelectSystem.GetSortTitleGroup(index);
                UpdateSortContent();
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

        public void OnPersonListShow(UIObjectListItem item)
        {
            RectTransform rectTransform = item.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            SangoObject obj = objectSelectSystem.Objects[item.index];

            bool isSelected = objectSelectSystem.selected.Contains(obj);
            item.Clear();
            for (int i = 0; i < sortItems.Count; i++)
            {
                ObjectSortTitle sortTitle = sortItems[i];
                item.Add(sortTitle.GetValueStr(obj), sortTitle.width);
            }

            item.SetSelected(isSelected);
        }


    }
}
