using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    /// <summary>
    /// 游戏开始界面
    /// </summary>
    public class UIMiniMap : MonoBehaviour
    {
        abstract class MapNodeData
        {
            public Image image;
            protected RectTransform rectTransform;
            public MapNodeData(Image image)
            {
                this.image = image;
                rectTransform = image.rectTransform;
            }
        }

        class MapCityNodeData : MapNodeData
        {
            public City city;
            public MapCityNodeData(City city, Image image) : base(image)
            {
                this.city = city;
                Color c = city.BelongForce == null ? Color.white : city.BelongForce.Flag.color;
                image.color = c;
            }

            public MapCityNodeData UpdateCell(RectTransform mapBounds)
            {
                float x = city.x * mapBounds.sizeDelta.x / Scenario.Cur.Map.Width - mapBounds.sizeDelta.x / 2;
                float y = mapBounds.sizeDelta.y / 2 - city.y * mapBounds.sizeDelta.y / Scenario.Cur.Map.Height;
                rectTransform.anchoredPosition = new Vector2(x, y);
                return this;
            }

            public void UpdateImage()
            {
                Color c = city.BelongForce == null ? Color.white : city.BelongForce.Flag.color;
                image.color = c;
            }
        }
        class MapTroopNodeData : MapNodeData
        {
            public Troop troop;
            public MapTroopNodeData(Image image) : base(image)
            {
            }

            public void Init(Troop troop)
            {
                this.troop = troop;
                image.color = troop.BelongForce.Flag.color;
                image.enabled = true;
            }

            public MapTroopNodeData UpdateCell(Cell dest, RectTransform mapBounds)
            {
                float x = dest.x * mapBounds.sizeDelta.x / Scenario.Cur.Map.Width - mapBounds.sizeDelta.x / 2;
                float y = mapBounds.sizeDelta.y / 2 - dest.y * mapBounds.sizeDelta.y / Scenario.Cur.Map.Height;
                rectTransform.anchoredPosition = new Vector2(x, y);
                return this;
            }

            public void Clear()
            {
                image.enabled = false;
            }
        }

        public GameObject troopObj;
        public GameObject cityObj;
        public GameObject miniCityObj;
        public RectTransform mapBounds;

        List<MapCityNodeData> mapCityNodes = new List<MapCityNodeData>();
        List<MapTroopNodeData> mapTroopNodes = new List<MapTroopNodeData>();
        Queue<MapTroopNodeData> mapTroopNodesPool = new Queue<MapTroopNodeData>();

        public void Start()
        {
            GameEvent.OnTroopCreated += OnTroopCreated;
            GameEvent.OnTroopDestroyed += OnTroopDestroyed;
            GameEvent.OnCityFall += OnCityFall;
            InitCities();
        }

        public void OnDestroy()
        {
            mapCityNodes.Clear();
            mapTroopNodes.Clear();
            mapTroopNodesPool.Clear();
            GameEvent.OnTroopCreated -= OnTroopCreated;
            GameEvent.OnTroopDestroyed -= OnTroopDestroyed;
            GameEvent.OnTroopEnterCell -= OnTroopEnterCell;
            GameEvent.OnCityFall -= OnCityFall;
        }

        void OnCityFall(City city, Troop troop)
        {
            for (int i = 0; i < mapCityNodes.Count; i++)
            {
                MapCityNodeData data = mapCityNodes[i];
                if (data.city == city)
                {
                    data.UpdateImage();
                    return;
                }
            }
        }
        void OnTroopCreated(Troop troop, Scenario scenario)
        {
            MapTroopNodeData data;
            if (mapTroopNodesPool.Count > 0)
                data = mapTroopNodesPool.Dequeue();
            else
            {
                GameObject go = GameObject.Instantiate(troopObj, troopObj.transform.parent);
                go.SetActive(true);
                Image image = go.GetComponentInChildren<Image>(true);
                data = new MapTroopNodeData(image);
            }

            data.Init(troop);
            mapTroopNodes.Add(data);
        }

        void OnTroopDestroyed(Troop troop, Scenario scenario)
        {
            for (int i = 0; i < mapTroopNodes.Count; i++)
            {
                MapTroopNodeData data = mapTroopNodes[i];
                if (data.troop == troop)
                {
                    mapTroopNodes.RemoveAt(i);
                    data.Clear();
                    mapTroopNodesPool.Enqueue(data);
                    return;
                }
            }
        }

        void OnTroopEnterCell(Troop troop, Cell destCell, Cell lastCell)
        {
            for (int i = 0; i < mapTroopNodes.Count; i++)
            {
                MapTroopNodeData data = mapTroopNodes[i];
                if (data.troop == troop)
                {
                    data.UpdateCell(destCell, mapBounds);
                    return;
                }
            }
        }

        void InitCities()
        {
            Scenario.Cur.citySet.ForEach(city =>
            {
                if (city.IsCity())
                {
                    GameObject go = GameObject.Instantiate(cityObj, cityObj.transform.parent);
                    go.SetActive(true);
                    Image image = go.GetComponentInChildren<Image>(true);
                    mapCityNodes.Add(new MapCityNodeData(city, image).UpdateCell(mapBounds));
                }
                else
                {
                    GameObject go = GameObject.Instantiate(miniCityObj, miniCityObj.transform.parent);
                    go.SetActive(true);
                    Image image = go.GetComponentInChildren<Image>(true);
                    mapCityNodes.Add(new MapCityNodeData(city, image).UpdateCell(mapBounds));
                }
            });
        }

    }
}
