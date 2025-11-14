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
    public class UIScenarioForceSelect : UGUIWindow
    {
        public Image forceHead;
        public Text forceInfo;
        public Text forceDesc;
        public RectTransform mapBounds;

        public GameObject cityObject;

        List<GameObject> cityList = new List<GameObject>();
        List<Force> playerList = new List<Force>();

        public override void OnShow()
        {
            Scenario scenario = Scenario.CurSelected;
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
                Toggle toggle = cityObj.GetComponent<Toggle>();
                if (toggle != null)
                {
                    toggle.isOn = false;
                    toggle.onValueChanged.RemoveAllListeners();
                    toggle.onValueChanged.AddListener((b) =>
                    {
                        SetPlayer(city, b);
                    });
                }

                RectTransform rectTransform = cityObj.GetComponent<RectTransform>();
                float x = city.x * mapBounds.sizeDelta.x / scenario.Map.Width - mapBounds.sizeDelta.x / 2;
                float y = mapBounds.sizeDelta.y / 2 - city.y * mapBounds.sizeDelta.y / scenario.Map.Height;
                rectTransform.anchoredPosition = new Vector2(x, y);

                Image bgImg = cityObj.transform.GetChild(1).GetComponent<Image>();
                if (city.BelongForce != null)
                {
                    bgImg.color = city.BelongForce.Flag.color;
                    toggle.interactable = true;
                }
                else
                {
                    toggle.interactable = false;
                    bgImg.color = Color.white;
                }

                cityObj.SetActive(true);
            });
        }

        public void SetPlayer(City city, bool b)
        {
            Force force = city.BelongForce;
            if (force == null) return;

            if (b)
            {
                playerList.Add(force);
            }
            else
            {
                playerList.Remove(force);
            }

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
            Scenario.CurSelected.personSet.ForEach(x =>
            {
                if (x.BelongForce == force)
                {
                    personCount++;
                }
            });

            int cityCount = 0;
            int foodCount = 0;
            int troopsCount = 0;
            int goldCount = 0;
            Scenario.CurSelected.citySet.ForEach(x =>
            {
                if (x.BelongForce == force && x.IsCity())
                {
                    cityCount++;
                    foodCount += x.food;
                    troopsCount += x.troops;
                    goldCount += x.gold;
                }
            });

            forceHead.sprite = GameRenderHelper.LoadHeadIcon(force.Governor.headIconID);
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
            Window.Instance.ShowWindow("window_scenario_select");
            Window.Instance.HideWindow("window_scenario_force_select");
        }

        public void OnNext()
        {
            List<int> forceIds = new List<int>();
            for (int i = 0; i < playerList.Count; i++)
                forceIds.Add(playerList[i].Id);
            Scenario.CurSelected.Info.playerForceList = forceIds.ToArray();
            Window.Instance.ShowWindow("window_loading");
            Window.Instance.HideWindow("window_scenario_force_select");
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
