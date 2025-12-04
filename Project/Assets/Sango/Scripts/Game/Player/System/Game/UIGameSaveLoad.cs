using Sango.Game.Player;
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

        public Text titleLabel;

        public Text scenarioDescText;
        public Text scenarioInfoText;
        public RawImage scenarioPosterImg;

        public UIScenarioItem[] selectedItems;

        public GameObject cityObject;
        public RectTransform mapBounds;
        List<GameObject> cityList = new List<GameObject>();

        public override void OnShow(params object[] objects)
        {
            isSave = (bool)objects[0];
            titleLabel.text = isSave ? "存档" : "读档";
            ShowPage(0);
        }

        public void ShowPage(int curShowPage)
        {
            this.curShowPage = curShowPage;
            for (int i = 0; i < CountInPage; i++)
            {
                int index = curShowPage * CountInPage + i;

                Scenario scenario = Player.Player.Instance.all_saved_scenario_list[index];
                UIScenarioItem item = selectedItems[i];

                if (scenario == null)
                {
                    item.SetName("空");
                }
                else
                {
                    item.SetName(scenario.Name);
                }
                item.SetColor(isSave ? Color.green : Color.red);
                item.BindCall(() =>
                {
                    if (isSave)
                    {
                        Save(index);
                    }
                    else
                    {
                        Load(index);
                    }
                });

            }
        }

        public void Clear()
        {

        }

        public void Save(int index)
        {
            Scenario scenario = Player.Player.Instance.all_saved_scenario_list[index];
            string content = scenario != null ? $"是否覆盖{index + 1}号存档" : $"是否保存至{index + 1}号存档";
            UIDialog.Open(content, () =>
            {
                Player.Player.Instance.Save(index);
                UIDialog.Close();
                ShowPage(curShowPage);
            });
        }

        public void Load(int index)
        {
            Scenario scenario = Player.Player.Instance.all_saved_scenario_list[index];
            if (scenario == null) return;
            PlayerCommand.Instance.Done();
            Player.Player.Instance.Load(index);
        }

        public void ShowScenario(int index)
        {
            Scenario scenario = Player.Player.Instance.all_saved_scenario_list[index];
            if (scenario == null)
            {
                scenarioInfoText.text = "";
                scenarioDescText.text = "";
                return;
            }

            ScenarioInfo scenarioInfo = scenario.Info;
            scenarioInfoText.text = $"{scenarioInfo.year} 年 {scenarioInfo.month}月 {scenarioInfo.day}日  {scenarioInfo.name}";
            scenarioDescText.text = scenarioInfo.description;
            scenario.LoadBaseContent();
            int i = 0;
            scenario.citySet.ForEach(city =>
            {
                if (!city.IsCity()) return;

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
                if (city.BelongForce != null)
                {
                    bgImg.color = city.BelongForce.Flag.color;
                }
                else
                {
                    bgImg.color = Color.white;
                }

                cityObj.SetActive(true);
            });

            for (int j = i; j < cityList.Count; j++)
            {
                cityList[j].SetActive(false);
            }

        }

        public void OnClose()
        {
            Clear();
            Hide();
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
