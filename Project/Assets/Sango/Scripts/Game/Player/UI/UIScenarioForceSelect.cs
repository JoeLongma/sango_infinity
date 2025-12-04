using Sango.Loader;
using Sango.Render;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    /// <summary>
    /// 剧本势力选择界面
    /// </summary>
    public class UIScenarioForceSelect : UGUIWindow
    {
        public Image forceHead;
        public Text forceInfo;
        public Text forceDesc;
        public RectTransform mapBounds;

        public Button nextBtn;
        public GameObject cityObject;

        List<GameObject> cityList = new List<GameObject>();
        List<ShortForce> playerList = new List<ShortForce>();

        public override void OnShow()
        {
            ShortScenario scenario = ShortScenario.CurSelected;
            nextBtn.interactable = false;
            int i = 0;

            foreach (var city in scenario.citySet.Values)
            {
                if (city.BuildingType > 1) return;

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
                UIMapCitySelectItem toggle = cityObj.GetComponent<UIMapCitySelectItem>();
                if (toggle != null)
                {
                    toggle.shortCity = city;
                    if (city.BelongForce == 0)
                    {
                        toggle.SetSelected(false).SetInavtive(true);
                    }
                    else
                    {
                        ShortForce shortForce = scenario.forceSet[city.BelongForce];
                        Flag flag = scenario.CommonData.Flags[shortForce.Flag];
                        toggle.SetSelected(false).SetInavtive(false).SetColor(flag.color).onSelectShortAction = SetPlayer;
                    }
                }

                RectTransform rectTransform = cityObj.GetComponent<RectTransform>();
                float x = city.x * mapBounds.sizeDelta.x / scenario.Map.Width - mapBounds.sizeDelta.x / 2;
                float y = mapBounds.sizeDelta.y / 2 - city.y * mapBounds.sizeDelta.y / scenario.Map.Height;
                rectTransform.anchoredPosition = new Vector2(x, y);

                cityObj.SetActive(true);
            }
        }

        public void SetPlayer(ShortCity city, bool b)
        {
            ShortScenario scenario = ShortScenario.CurSelected;
            ShortForce force = scenario.forceSet[city.BelongForce];
            if (force == null) return;

            if (b)
            {
                playerList.Add(force);
            }
            else
            {
                playerList.Remove(force);
            }

            nextBtn.interactable = playerList.Count > 0;

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < playerList.Count; i++)
            {
                stringBuilder.Append(playerList[i].Name);
                if (i < playerList.Count - 1)
                    stringBuilder.Append(",");
            }

            forceDesc.text = $"已选择: {stringBuilder}";

            if (!b)
            {
                if (playerList.Count == 0)
                {
                    forceHead.sprite = null;
                    forceInfo.text = "";
                    return;
                }
                else
                {
                    force = playerList[playerList.Count - 1];
                }
            }

            int personCount = 0;

            foreach (var x in ShortScenario.CurSelected.personSet.Values)
            {
                if (x.BelongForce == force.Id)
                {
                    personCount++;
                }
            }

            int cityCount = 0;
            int foodCount = 0;
            int troopsCount = 0;
            int goldCount = 0;

            foreach (var x in ShortScenario.CurSelected.citySet.Values)
            {
                if (x.BelongForce == force.Id && x.BuildingType == 1)
                {
                    cityCount++;
                    foodCount += x.food;
                    troopsCount += x.troops;
                    goldCount += x.gold;
                }
            }

            forceHead.sprite = GameRenderHelper.LoadHeadIcon(ShortScenario.CurSelected.personSet[force.Governor].headIconID, 1);
            forceInfo.text = $"{force.Name}\n城池:{cityCount} 武将:{personCount} \n士兵: {troopsCount} 粮食:{foodCount} 金钱:{goldCount}";
        }

        public override void OnHide()
        {
            for (int i = 0; i < cityList.Count; i++)
            {
                cityList[i].SetActive(false);
            }
        }


        public void OnReturn()
        {
            Scenario.CurSelected.Clear();
            Window.Instance.Open("window_scenario_select");
            Window.Instance.Close("window_scenario_force_select");
        }

        public void OnNext()
        {
            List<int> forceIds = new List<int>();
            for (int i = 0; i < playerList.Count; i++)
                forceIds.Add(playerList[i].Id);



            // 确定第一个视角
            foreach (var x in ShortScenario.CurSelected.forceSet.Values)
            {
                bool isFind = false;
                for (int j = 0; j < playerList.Count; j++)
                {
                    if (playerList[j].Id == x.Id)
                    {
                        ShortPerson person = ShortScenario.CurSelected.personSet[playerList[j].Governor];
                        ShortCity city = ShortScenario.CurSelected.citySet[person.BelongCity];
                        Vector3 position = ShortScenario.CurSelected.Map.Coords2Position(city.x, city.y);
                        Scenario.CurSelected.Info.cameraPosition = position;
                        Scenario.CurSelected.Info.cameraRotation = new Vector3(40f, -50f, 0f);
                        Scenario.CurSelected.Info.cameraDistance = 400f;
                        isFind = true;
                        break;
                    }
                }
                if (isFind) break;
            }

            if (playerList.Count == 0)
            {
                foreach (var x in ShortScenario.CurSelected.forceSet.Values)
                {
                    ShortForce force = x;
                    if (force != null)
                    {
                        ShortPerson person = ShortScenario.CurSelected.personSet[force.Governor];
                        ShortCity city = ShortScenario.CurSelected.citySet[person.BelongCity];
                        Vector3 position = ShortScenario.CurSelected.Map.Coords2Position(city.x, city.y);
                        Scenario.CurSelected.Info.cameraPosition = position;
                        Scenario.CurSelected.Info.cameraRotation = new Vector3(40f, -50f, 0f);
                        Scenario.CurSelected.Info.cameraDistance = 400f;
                        break;
                    }
                }
            }

            Scenario.CurSelected.Info.playerForceList = forceIds.ToArray();
            Window.Instance.Open("window_loading");
            Window.Instance.Close("window_scenario_force_select");
            Scenario.StartScenario(Scenario.CurSelected);
        }

        public void Update()
        {
            //GameObject gameObject = EventSystem.current.currentSelectedGameObject;
            //if (gameObject != null)
            //{
            //    Debug.LogError(gameObject.name);
            //}
        }
    }
}
