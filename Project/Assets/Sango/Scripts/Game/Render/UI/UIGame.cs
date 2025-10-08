using Sango.Loader;
using Sango.Render;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace Sango.Game.Render.UI
{
    public class UIGame : UGUIWindow, LoopScrollPrefabSource, LoopScrollDataSource
    {
        public Text forceText;
        public Text dateText;

        public bool gridShow = true;
        public bool troopListShow = false;
        public GameObject troopListObj;
        public Text troopListShowText;

        public LoopScrollRect loopScrollRect;
        public GameObject troopListItemObj;
        public Transform troopListContent;
        public int totalCount = -1;
        Stack<Transform> pool = new Stack<Transform>();
        List<Troop> troops_list = new List<Troop>();

        public GameObject pauseObj;
        public GameObject resumeObj;


        public GameObject GetObject(int index)
        {
            if (pool.Count == 0)
            {
                GameObject obj = Instantiate(troopListItemObj);
                UITroopListItem uITroopListItem = obj.GetComponent<UITroopListItem>();
                if (uITroopListItem != null)
                {
                    uITroopListItem.onSelected = OnTroopListSelected;
                    uITroopListItem.onShow = OnTroopListShow;
                }
                return obj;
            }
            Transform candidate = pool.Pop();
            candidate.gameObject.SetActive(true);
            return candidate.gameObject;
        }

        public void ReturnObject(Transform trans)
        {
            // Use `DestroyImmediate` here if you don't need Pool
            trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            pool.Push(trans);
        }

        public void ProvideData(Transform transform, int idx)
        {
            transform.SendMessage("ScrollCellIndex", idx);
        }

        void Start()
        {
            Scenario.Cur.Event.OnTroopCreated += OnTroopChange;
            Scenario.Cur.Event.OnTroopDestroyed += OnTroopChange;
            Scenario.Cur.Event.OnForceStart += OnForceStart;
            Scenario.Cur.Event.OnDayUpdate += OnDayUpdate;

            loopScrollRect.prefabSource = this;
            loopScrollRect.dataSource = this;
            loopScrollRect.totalCount = totalCount;
            loopScrollRect.RefillCells();
        }

        public void OnTroopChange(Troop troop, Scenario scenario)
        {
            troops_list.Clear();
            scenario.troopsSet.ForEach(t =>
            {
                if (t.IsAlive)
                {
                    troops_list.Add(t);
                }
            });

            loopScrollRect.totalCount = troops_list.Count;
            loopScrollRect.RefillCells();
        }

        public void OnForceStart(Force force, Scenario scenario)
        {
            forceText.text = force.Name;
        }

        public void OnDayUpdate(Scenario scenario)
        {
            ScenarioInfo info = scenario.Info;
            dateText.text = $"{info.year}年{info.month}月{info.day}日";
        }

        public void OnBtnPause()
        {
            pauseObj.SetActive(false);
            resumeObj.SetActive(true);
            Sango.Game.Scenario.Pause();
        }

        public void OnBtnResume()
        {
            pauseObj.SetActive(true);
            resumeObj.SetActive(false);
            Sango.Game.Scenario.Resume();
        }


        public void OnBtnNextForce()
        {
            pauseObj.SetActive(false);
            resumeObj.SetActive(true);
            Sango.Game.Scenario.NextForce();
        }

        public void OnBtnNextTurn()
        {
            pauseObj.SetActive(false);
            resumeObj.SetActive(true);
            Sango.Game.Scenario.NextTurn();
        }

        public void OnBtnDebugAI()
        {

        }

        public void OnBtnGirdShow()
        {
            gridShow = !gridShow;
            MapRender.Instance.ShowGrid(gridShow);
        }

        public void OnTroopListShow()
        {
            troopListShow = !troopListShow;
            troopListObj.SetActive(troopListShow);
            troopListShowText.text = troopListShow ? "隐藏" : "显示";
        }

        public void OnTroopListSelected(int index)
        {
            if (index < 0 || index >= troops_list.Count)
                return;

            Troop troop = troops_list[index];
            Vector3 position = troop.cell.Position;
            MapRender.Instance.MoveCameraTo(position);
        }

        public void OnTroopListShow(UITroopListItem item)
        {
            Troop troop = troops_list[item.index];
            item.name.text = $"[{troop.BelongForce.Name}]<{troop.BelongCity.Name}>{troop.Name}队";
        }
    }
}
