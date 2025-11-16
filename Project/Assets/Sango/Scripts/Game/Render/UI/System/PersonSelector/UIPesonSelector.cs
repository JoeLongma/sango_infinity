using Sango.Game.Player;
using Sango.Loader;
using Sango.Render;
using System;
using System.Collections.Generic;
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


        public Toggle[] toggleGroup;

        List<UIPersonSortButton> sortButtonPool = new List<UIPersonSortButton>();
        public UIPersonSortButton sortTitleItem;

        public RectTransform sorltTitleTransform;
        PersonSelectSystem personSelectSystem;



        void Start()
        {
            loopScrollRect.prefabSource = this;
            loopScrollRect.dataSource = this;
            //loopScrollRect.totalCount = totalCount;
            //loopScrollRect.RefillCells();
        }

        UIPersonSortButton CreateSortButtonItem()
        {
            return new UIPersonSortButton();
        }

        public override void OnShow()
        {
            personSelectSystem = PersonSelectSystem.Instance;
            for (int i = 0; i < toggleGroup.Length; i++)
            {
                toggleGroup[i].gameObject.SetActive(i < (int)PersonSortGroupType.Max);
            }

            for (int i = 0; i < personSelectSystem.sorltItems.Count; i++)
            {
                SortTitle sortTitle = personSelectSystem.sorltItems[i];
                UIPersonSortButton uIPersonSortButton;
                if (i < sortButtonPool.Count)
                    uIPersonSortButton = sortButtonPool[i];
                else
                    uIPersonSortButton = CreateSortButtonItem();

                uIPersonSortButton.Clear().SetWidth(sortTitle.width).SetName(sortTitle.name);
            }

            loopScrollRect.totalCount = personSelectSystem.People.Count;
            loopScrollRect.RefillCells();
        }

        public void OnSure()
        {
            PersonSelectSystem.Instance.OnSure();
        }

        public void OnCancel()
        {
            PersonSelectSystem.Instance.OnCancel();
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

        public void OnPersonListSelected(int index)
        {

        }

        public void OnPersonListShow(UIPersonListItem item)
        {

        }


    }
}
