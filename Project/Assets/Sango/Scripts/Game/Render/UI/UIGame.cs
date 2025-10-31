using Sango.Loader;
using Sango.Render;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIGame : UGUIWindow, LoopScrollPrefabSource, LoopScrollDataSource
    {
        public Text forceText;
        public Text dateText;
        public Text fpsText;

        public Text frameBtnText;
        public Text speedBtnText;

        public bool gridShow = true;
        public bool troopListShow = false;
        public GameObject troopListObj;
        public Text troopListShowText;

        public LoopScrollRect loopScrollRect;
        public GameObject troopListItemObj;
        public Transform troopListContent;
        public int totalCount = -1;
        Stack<Transform> pool = new Stack<Transform>();
        //List<Troop> troops_list = new List<Troop>();
        List<SangoObject> item_list = new List<SangoObject>();
        public Type itemType;
        bool needUpdateItem = true;

        public GameObject pauseObj;
        public GameObject resumeObj;

        public GameObject[] fpaObj;

        int destSaveTurn = -1;
        bool needSave = false;
        private float deltaTime = 0.0f;
        bool cityInfoShow = true;

        float gameSpeed = 1;

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
            GameEvent.OnTroopCreated += OnTroopChange;
            GameEvent.OnTroopDestroyed += OnTroopChange;
            GameEvent.OnForceStart += OnForceStart;
            GameEvent.OnDayUpdate += OnDayUpdate;
            GameEvent.OnCityFall += OnCityFall;

            loopScrollRect.prefabSource = this;
            loopScrollRect.dataSource = this;

            itemType = typeof(Troop);
            needUpdateItem = true;

            InvokeRepeating("UpdateFPS", 1f, 1f);
            //loopScrollRect.totalCount = totalCount;
            //loopScrollRect.RefillCells();
        }

        public void OnCityFall(City city, Troop atker)
        {
            if (itemType == typeof(City))
            {
                loopScrollRect.RefreshCells();
            }

        }

        public void OnTroopChange(Troop troop, Scenario scenario)
        {
            if (itemType == typeof(Troop))
            {
                needUpdateItem = true;
            }

        }

        public void OnForceStart(Force force, Scenario scenario)
        {
            forceText.text = force.Name;
        }

        string[] seasonText = new string[] { "(秋)", "(春)", "(夏)", "(冬)" };
        public void OnDayUpdate(Scenario scenario)
        {
            dateText.text = scenario.GetDateStr() + seasonText[(int)scenario.CurSeason];
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
            if (index < 0 || index >= item_list.Count)
                return;

            SangoObject obj = item_list[index];
            if (obj is Troop)
            {
                Troop troop = (Troop)obj;
                Vector3 position = troop.cell.Position;
                MapRender.Instance.MoveCameraTo(position);
            }
            else if (obj is City)
            {
                City troop = (City)obj;
                Vector3 position = troop.CenterCell.Position;
                MapRender.Instance.MoveCameraTo(position);
            }
        }

        public void OnTroopListShow(UITroopListItem item)
        {
            if (item.index < 0 || item.index >= item_list.Count)
            {
                item.name.text = "无效";
                return;
            }
            SangoObject obj = item_list[item.index];
            if (obj is Troop)
            {
                Troop troop = (Troop)obj;

                if (troop.BelongForce == null)
                {
                    int dd = 33;
                    dd++;
                }
                if (troop.TroopType.isFight)
                    item.name.text = $"[{troop.BelongForce.Name}]<{troop.TroopType.Name}>{troop.Name}队,{troop.Member1?.Name}{troop.Member2?.Name}";
                else
                    item.name.text = $"**[{troop.BelongForce.Name}]<{troop.TroopType.Name}>{troop.Name}运输队,{troop.Member1?.Name}{troop.Member2?.Name}";

                item.name.color = troop.BelongForce.Flag.color;
            }
            else if (obj is City)
            {
                City city = (City)obj;
                if (city.BelongForce != null)
                {
                    item.name.text = $"[{city.BelongForce.Name}]{city.Name}";
                    item.name.color = city.BelongForce.Flag.color;

                }
                else
                {
                    item.name.text = $"{city.Name}";
                    item.name.color = Color.white;
                }

            }
        }

        public void OnTroopTab(Toggle b)
        {
            if (b.isOn)
            {
                itemType = typeof(Troop);
                needUpdateItem = true;
            }
        }

        public void OnCityTab(Toggle b)
        {
            if (b.isOn)
            {
                itemType = typeof(City);
                needUpdateItem = true;
            }
        }

        void Save()
        {
            int count = Scenario.all_scenario_list.Count;
            string savePath = Path.ContentRootPath + $"/Scenario/scenario_save_{count}.json";
            Scenario.Cur.Save(savePath);

            WindowEvent windowEvent = new WindowEvent()
            {
                windowName = "window_dialog",
                arg1 = "保存成功!!!"
            };
            RenderEvent.Instance.Add(windowEvent);
        }

        public void OnSave()
        {
            if (Sango.Game.Scenario.Cur.PauseTrunCount == Sango.Game.Scenario.Cur.Info.turnCount)
            {
                Save();
            }
            else
            {
                needSave = true;
                OnBtnNextTurn();
            }
        }

        public void OnLoad()
        {

        }
        public void OnSwitchCityInfoShow()
        {
            UICityHeadbar.showIndo = !UICityHeadbar.showIndo;
            GameEvent.OnCityHeadbarShowInfoChange?.Invoke();
        }

        public void OnSpeedChange()
        {
            gameSpeed = gameSpeed * 2;
            if (gameSpeed > 8)
                gameSpeed = 1;

            Time.timeScale = gameSpeed;
            speedBtnText.text = $"游戏速度:{(int)gameSpeed}倍";
        }

        public void OnLowFPS()
        {
#if UNITY_STANDALONE_WIN
            if (Application.targetFrameRate == 60)
                Application.targetFrameRate = 120;
            else
                Application.targetFrameRate = 60;

#else
            if (Application.targetFrameRate == 30)
                Application.targetFrameRate = 60;
            else
                Application.targetFrameRate = 30;
#endif
            frameBtnText.text = $"切换帧率:{Application.targetFrameRate}";
        }

        void UpdateFPS()
        {
            float FPS = 1f / deltaTime;
            fpsText.text = $"FPS:{Math.Floor(FPS)}";
        }

        public void Update()
        {
            if (needSave)
            {
                if (Sango.Game.Scenario.Cur.PauseTrunCount == Sango.Game.Scenario.Cur.Info.turnCount)
                {
                    Save();
                    needSave = false;
                }
            }

            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

            if (!troopListShow) return;

            if (needUpdateItem)
            {
                needUpdateItem = false;
                if (itemType == typeof(Troop))
                {

                    item_list.Clear();
                    Scenario.Cur.troopsSet.ForEach(t =>
                    {
                        if (t.IsAlive)
                        {
                            item_list.Add(t);
                        }
                    });

                    loopScrollRect.totalCount = item_list.Count;
                    loopScrollRect.RefillCells(loopScrollRect.GetFirstItem(out _));
                }
                else if (itemType == typeof(City))
                {
                    item_list.Clear();
                    Scenario.Cur.citySet.ForEach(t =>
                    {
                        if (t.IsAlive)
                        {
                            item_list.Add(t);
                        }
                    });

                    loopScrollRect.totalCount = item_list.Count;
                    loopScrollRect.RefillCells(loopScrollRect.GetFirstItem(out _));
                }
            }
        }
    }
}
