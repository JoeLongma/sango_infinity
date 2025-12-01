using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    /// <summary>
    /// 剧本选择界面
    /// </summary>
    public class UIScenarioSelect : UGUIWindow
    {
        public int curSelectIndex;
        public UIScenarioItem uIScenarioItem;
        public Text scenarioDescText;
        public Text scenarioInfoText;
        public RawImage scenarioPosterImg;

        List<UIScenarioItem> selectedItems = new List<UIScenarioItem>();

        public GameObject cityObject;
        public RectTransform mapBounds;
        List<GameObject> cityList = new List<GameObject>();

        public override void OnShow()
        {
            curSelectIndex = 0;
            int count = Scenario.all_scenario_list.Count;
            for (int i = 0; i < count; i++)
            {
                Scenario scenario = Scenario.all_scenario_list[i];
                UIScenarioItem item;
                if (i < selectedItems.Count)
                {
                    item = selectedItems[i];
                }
                else
                {
                    item = GameObject.Instantiate(uIScenarioItem.gameObject, uIScenarioItem.transform.parent).GetComponent<UIScenarioItem>();
                    selectedItems.Add(item);
                }
                item.gameObject.SetActive(true);
                int selIndex = i;
                item.SetName(scenario.Name).SetSelected(i == curSelectIndex).BindCall(() => { OnSelectScenario(selIndex); });
            }

            for (int i = count; i < selectedItems.Count; i++)
            {
                selectedItems[i].gameObject.SetActive(false);
            }

            ShowScenario(curSelectIndex);
        }

        public void Clear()
        {

        }

        public void OnSelectScenario(int index)
        {
            if (curSelectIndex != index)
                selectedItems[curSelectIndex].SetSelected(false);
            curSelectIndex = index;
            selectedItems[curSelectIndex].SetSelected(true);
            ShowScenario(index);
        }

        public void ShowScenario(int index)
        {
            Scenario scenario = Scenario.all_scenario_list[curSelectIndex];
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

        public void OnReturn()
        {
            Clear();
            Window.Instance.Open("window_start");
            Window.Instance.Close("window_scenario_select");
        }

        public void OnNext()
        {
            Clear();
            Scenario scenario = Scenario.all_scenario_list[curSelectIndex];
            Scenario.CurSelected = scenario;
            Window.Instance.Open("window_scenario_force_select");
            Window.Instance.Close("window_scenario_select");
        }

    }
}
