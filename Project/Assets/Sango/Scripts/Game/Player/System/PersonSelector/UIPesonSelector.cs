using Sango.Game.Player;
using Sango.Loader;
using Sango.Render;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Render.UI
{
    public class UIPesonSelector : UGUIWindow, LoopScrollPrefabSource, LoopScrollDataSource
    {
        Stack<Transform> pool = new Stack<Transform>();
        public GameObject personItemObject;
        public LoopScrollRect loopScrollRect;

        public List<SortTitle> sortItems;

        public Toggle[] toggleGroup;

        List<UIPersonSortButton> sortButtonPool = new List<UIPersonSortButton>();
        public UIPersonSortButton sortTitleItem;

        public RectTransform sorltTitleTransform;
        PersonSelectSystem personSelectSystem;

        void Awake()
        {
            //loopScrollRect.totalCount = totalCount;
            //loopScrollRect.RefillCells();
            for (int i = 0; i < toggleGroup.Length; i++)
            {
                toggleGroup[i].onValueChanged.RemoveAllListeners();
                toggleGroup[i].onValueChanged.AddListener((b) => OnSortGroupChanged(i, b));
            }
        }

        UIPersonSortButton CreateSortButtonItem()
        {
            GameObject btn = GameObject.Instantiate(sortTitleItem.gameObject, sortTitleItem.transform.parent);
            UIPersonSortButton sortBtn = btn.GetComponent<UIPersonSortButton>();
            sortButtonPool.Add(sortBtn);
            return sortBtn;
        }

        public override void OnShow()
        {
            loopScrollRect.prefabSource = this;
            loopScrollRect.dataSource = this;

            personSelectSystem = Sango.Singleton<PersonSelectSystem>.Instance;

            toggleGroup[0].GetComponentInChildren<Text>(true).text = personSelectSystem.customSortTitleName;
            sortItems = personSelectSystem.customSortItems;
            toggleGroup[0].isOn = true;
            for (int i = 1; i < toggleGroup.Length; i++)
            {
                toggleGroup[i].GetComponentInChildren<Text>(true).text = PersonSortFunction.Instance.GetSortTitleGroupName((PersonSortGroupType)i);
            }

            loopScrollRect.totalCount = personSelectSystem.People.Count;

            UpdateSortContent();
        }

        public void UpdateSortContent()
        {
            for (int i = 0; i < sortItems.Count; i++)
            {
                SortTitle sortTitle = sortItems[i];
                UIPersonSortButton uIPersonSortButton;
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
            personSelectSystem.OnSure();
        }

        public void OnCancel()
        {
            personSelectSystem.OnCancel();
        }

        public GameObject GetObject(int index)
        {
            if (pool.Count == 0)
            {
                GameObject obj = Instantiate(personItemObject);
                UIPersonListItem uiPersonListItem = obj.GetComponent<UIPersonListItem>();
                if (uiPersonListItem != null)
                {
                    uiPersonListItem.onSelected = OnPersonListSelected;
                    uiPersonListItem.onShow = OnPersonListShow;
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
                if (index == 0)
                {
                    sortItems = personSelectSystem.customSortItems;
                }
                else
                {
                    sortItems = new List<SortTitle>();
                    PersonSortFunction.Instance.GetSortTitleGroup((PersonSortGroupType)index, sortItems);
                }
                UpdateSortContent();
            }

        }

        public void OnPersonListSelected(UIPersonListItem item)
        {
            if (!item.IsSelected() && personSelectSystem.IsPersonLimit())
                return;

            item.SetSelected(!item.IsSelected());
            if (item.IsSelected())
            {
                personSelectSystem.Add(item.index);
            }
            else
            {
                personSelectSystem.Remove(item.index);
            }
        }

        public void OnPersonListShow(UIPersonListItem item)
        {
            RectTransform rectTransform = item.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            Person person = personSelectSystem.People[item.index];

            bool isSelected = personSelectSystem.selected.Contains(person);
            item.Clear();
            for (int i = 0; i < sortItems.Count; i++)
            {
                SortTitle sortTitle = sortItems[i];
                item.Add(sortTitle.valueGetCall.Invoke(person), sortTitle.width);
            }

            item.SetSelected(isSelected);
        }


    }
}
