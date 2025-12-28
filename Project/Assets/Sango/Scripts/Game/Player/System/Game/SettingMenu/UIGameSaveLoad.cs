using Sango.Game.Player;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    /// <summary>
    /// 存档读档界面
    /// </summary>
    public class UIGameSaveLoad : UGUIWindow
    {
        bool isSave = false;
        public int curShowPage;
        public int CountInPage = 10;
        public int curSelectIndex = 0;

        public Text titleLabel;

        public UITextField id;
        public UITextField name;
        public RawImage head;
        public UITextField forceName;
        public UITextField playYear;
        public UITextField playNum;
        public UITextField time;
        public UITextField playTime;
        public UITextField desc;

        public UIScenarioSaveItem[] selectedItems;

        public GameObject cityObject;
        public RectTransform mapBounds;
        List<GameObject> cityList = new List<GameObject>();
        ShortScenario newestData;
        UIScenarioSaveItem curSelectedItem;
        public override void OnShow(params object[] objects)
        {
            isSave = (bool)objects[0];
            titleLabel.text = isSave ? "存档" : "读档";

            long t = 0;
            for (int k = 0; k < Player.Player.Instance.all_saved_scenario_list.Length; k++)
            {
                ShortScenario scenario = Player.Player.Instance.all_saved_scenario_list[k];
                if (scenario != null)
                {
                    if (scenario.Info.dateTime > t)
                    {
                        newestData = scenario;
                        t = scenario.Info.dateTime;
                    }
                }
            }
            ShowPage(0);
        }

        public void ShowPage(int curShowPage)
        {
            curSelectedItem = null;
            curSelectIndex = -1;
            id.SetText("");
            name.SetText("");
            forceName.SetText("");
            playYear.SetText("");
            playNum.SetText("");
            time.SetText("");
            playTime.SetText("");
            desc.SetText("");
            head.texture = null;
            this.curShowPage = curShowPage;
            for (int i = 0; i < CountInPage; i++)
            {
                int index = curShowPage * CountInPage + i;
                ShortScenario scenario = Player.Player.Instance.all_saved_scenario_list[index];
                UIScenarioSaveItem item = selectedItems[i];
                item.SetIsLoad(!isSave);
                item.SetSelected(false);
                if (scenario == null)
                {
                    item.SetId(-1).SetInactive(true).SetName("").SetTime(-1).SetNew(false);
                }
                else
                {
                    item.SetId(index + 1).SetInactive(false).SetName(scenario.forceSet[scenario.Info.curForceId].Name).SetTime(scenario.Info.dateTime).SetNew(newestData == scenario);
                }
                item.BindCall(() =>
                {
                    if (curSelectedItem != null)
                        curSelectedItem.SetSelected(false);
                    curSelectedItem = null;
                    ShortScenario scenario1 = Player.Player.Instance.all_saved_scenario_list[index];
                    if (scenario1 != null)
                    {
                        curSelectedItem = item;
                        //if(!isSave)
                        //    curSelectedItem.SetSelected(true);
                    }
                    ShowScenario(index);
                });
                //item.BindSureCall(() =>
                //{
                //    Load(index);
                //});
            }
        }

        public void Clear()
        {

        }

        public void Save(int index)
        {
            ShortScenario scenario = Player.Player.Instance.all_saved_scenario_list[index];
            string content = scenario != null ? $"是否覆盖{index + 1}号存档" : $"是否保存至{index + 1}号存档";
            UIDialog.Open(content, () =>
            {
                Player.Player.Instance.Save(index);
                newestData = Player.Player.Instance.all_saved_scenario_list[index];
                UIDialog.Close();
                ShowPage(curShowPage);

            });
        }

        public void Load(int index)
        {
            ShortScenario scenario = Player.Player.Instance.all_saved_scenario_list[index];
            string content = $"是否加载{index + 1}号存档";
            UIDialog.Open(content, () =>
            {
                PlayerCommand.Instance.Done();
                Player.Player.Instance.Load(index);
                UIDialog.Close();
            });
        }

        public void ShowScenario(int index)
        {

            if (isSave)
            {
                Save(index);
            }
           
            curSelectIndex = index;
            ShortScenario scenario = Player.Player.Instance.all_saved_scenario_list[index];
            if (scenario == null)
            {
                id.SetText("");
                name.SetText("");
                forceName.SetText("");
                playYear.SetText("");
                playNum.SetText("");
                time.SetText("");
                playTime.SetText("");
                desc.SetText("");
                head.texture = null;
                return;
            }

            if (!isSave)
            {
                Load(index);
            }

            ScenarioInfo scenarioInfo = scenario.Info;
            id.SetText((index + 1).ToString());
            name.SetText($"{scenarioInfo.year} 年 {scenarioInfo.month}月 {scenarioInfo.day}日   {scenarioInfo.name}");

            ShortForce force = scenario.forceSet[scenarioInfo.playerForceList[0]];
            head.texture = GameRenderHelper.LoadHeadIcon(scenario.personSet[force.Governor].headIconID);
            forceName.SetText(force.Name);
            playYear.SetText($"{scenarioInfo.year} 年 {scenarioInfo.month}月 {scenarioInfo.day}日");
            playNum.SetText($"{scenarioInfo.playerForceList.Length.ToString()}人游玩");
            DateTime date = DateTime.FromFileTime(scenarioInfo.dateTime);
            time.SetText(date.ToString("yyyy-MM-dd HH:mm:ss"));
            playTime.SetText("");
            desc.SetText(scenarioInfo.description);
            int i = 0;
            foreach (var city in scenario.citySet.Values)
            {
                if (city.BuildingType > 1) return;
                if (city.Id == 0) continue;

                GameObject cityObj;
                if (i >= cityList.Count)
                {
                    cityObj = GameObject.Instantiate(cityObject, cityObject.transform.parent);
                    cityList.Add(cityObj);
                    cityObj.name = city.Id.ToString();
                }
                else
                {
                    cityObj = cityList[i];
                    cityObj.name = city.Id.ToString();
                }

                i++;
                RectTransform rectTransform = cityObj.GetComponent<RectTransform>();
                float x = city.x * mapBounds.sizeDelta.x / scenario.Map.Width - mapBounds.sizeDelta.x / 2;
                float y = mapBounds.sizeDelta.y / 2 - city.y * mapBounds.sizeDelta.y / scenario.Map.Height;
                rectTransform.anchoredPosition = new Vector2(x, y);

                Image bgImg = cityObj.transform.GetChild(0).GetComponent<Image>();
                if (city.BelongForce > 0)
                {
                    ShortForce shortForce = scenario.forceSet[city.BelongForce];
                    Flag flag = scenario.CommonData.Flags[shortForce.Flag];
                    bgImg.color = flag.color;
                }
                else
                {
                    bgImg.color = Color.white;
                }

                cityObj.SetActive(true);
            }

            for (int j = i; j < cityList.Count; j++)
            {
                cityList[j].SetActive(false);
            }

        }

        public void OnSure()
        {
            if (isSave)
            {
                Save(curSelectIndex);
            }
            else
            {
                Load(curSelectIndex);
            }
        }

        public void OnClose()
        {
            Clear();
            if (isSave)
            {
                Singleton<GameSave>.Instance.Done();
            }
            else
            {
                Singleton<GameLoad>.Instance.Done();
            }
        }

        public void OnPage1(bool select)
        {
            if (select)
            {
                ShowPage(0);
            }
        }

        public void OnPage2(bool select)
        {
            if (select)
            {
                ShowPage(1);
            }
        }

        public void OnPage3(bool select)
        {
            if (select)
            {
                ShowPage(2);
            }
        }

        public void OnPage4(bool select)
        {
            if (select)
            {
                ShowPage(3);
            }
        }

        public void OnPage5(bool select)
        {
            if (select)
            {
                ShowPage(4);
            }
        }
    }
}
