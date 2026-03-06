using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIScenarioCityMap : MonoBehaviour
    {
        public UICityMapItem cityMapItem;
        public RectTransform mapBounds;
        CreatePool<UICityMapItem> cityPool;

        private void Awake()
        {
            cityPool = new CreatePool<UICityMapItem>(cityMapItem);
        }

        public void Show(Scenario scenario)
        {
            Show(scenario, (City)null);
        }

        public void Show(ShortScenario scenario)
        {
            Show(scenario, null);
        }

        public void Show(Scenario scenario, Force current)
        {
            cityPool.Reset();
            scenario.citySet.ForEach(c =>
            {
                if (c.BuildingType.Id == 1 && c.Id > 0)
                {
                    City city = c;
                    UICityMapItem item = cityPool.Create();
                    item.name = city.Id.ToString();
                    RectTransform rectTransform = item.root;
                    float x = city.x * mapBounds.sizeDelta.x / scenario.Map.Width - mapBounds.sizeDelta.x / 2;
                    float y = mapBounds.sizeDelta.y / 2 - city.y * mapBounds.sizeDelta.y / scenario.Map.Height;
                    rectTransform.anchoredPosition = new Vector2((int)(x + 0.5f), (int)(y + 0.5f));
                    item.icon.color = city.BelongForce != null ? city.BelongForce.Color : Color.white;
                    item.effect?.SetActive(city.BelongForce == current);
                }
            });
        }

        public void Show(Scenario scenario, City current)
        {
            cityPool.Reset();
            scenario.citySet.ForEach(c =>
            {
                if (c.BuildingType.Id == 1 && c.Id > 0)
                {
                    City city = c;
                    UICityMapItem item = cityPool.Create();
                    item.name = city.Id.ToString();
                    RectTransform rectTransform = item.root;
                    float x = city.x * mapBounds.sizeDelta.x / scenario.Map.Width - mapBounds.sizeDelta.x / 2;
                    float y = mapBounds.sizeDelta.y / 2 - city.y * mapBounds.sizeDelta.y / scenario.Map.Height;
                    rectTransform.anchoredPosition = new Vector2((int)(x + 0.5f), (int)(y + 0.5f));
                    item.icon.color = city.BelongForce != null ? city.BelongForce.Color : Color.white;
                    item.effect?.SetActive(city.Id == current.Id);
                }
            });
        }

        public void Show(ShortScenario scenario, Force current)
        {
            cityPool.Reset();
            foreach (var city in scenario.citySet.Values)
            {

                if (city.BuildingType > 1) continue;
                if (city.Id == 0) continue;

                UICityMapItem item = cityPool.Create();
                item.name = city.Id.ToString();
                RectTransform rectTransform = item.root;
                float x = city.x * mapBounds.sizeDelta.x / scenario.Map.Width - mapBounds.sizeDelta.x / 2;
                float y = mapBounds.sizeDelta.y / 2 - city.y * mapBounds.sizeDelta.y / scenario.Map.Height;
                rectTransform.anchoredPosition = new Vector2((int)(x + 0.5f), (int)(y + 0.5f));
                if (city.BelongForce > 0)
                {
                    ShortForce shortForce = scenario.forceSet[city.BelongForce];
                    Flag flag = scenario.CommonData.Flags[shortForce.Flag];
                    item.icon.color = flag.color;
                }
                else
                {
                    item.icon.color = Color.white;
                }

                item.effect?.SetActive(city.BelongForce == current.Id);
            }
        }

    }
}