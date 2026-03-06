using Sango.Game.Render.UI;
using System.Collections.Generic;

namespace Sango.Game
{
    /// <summary>
    /// 城池治安系统逻辑
    /// </summary>
    [GameSystem(auto = true)]
    public class CityInformation : GameSystem
    {
        public City TargetCity;
        public List<SangoObject> all_citits = new List<SangoObject>();
        string windowName = "window_city_information";

        public override void Init()
        {
            Name = "都市情报";
            GameEvent.OnCityRightMouseButtonContextMenuShow += OnCityRightMouseButtonContextMenuShow;
            GameEvent.OnScenarioInit += OnScenarioInit;
        }

        public override void Clear()
        {
            GameEvent.OnCityRightMouseButtonContextMenuShow -= OnCityRightMouseButtonContextMenuShow;
        }

        protected virtual void OnCityRightMouseButtonContextMenuShow(IContextMenuData menuData, City city)
        {
            TargetCity = city;
            menuData.Add("详细情报", 20, null, OnClickMenuItem, true);
        }

        protected virtual void OnClickMenuItem(IContextMenuItem contextMenuItem)
        {
            ContextMenu.CloseAll();
            GameSystemManager.Instance.Push(this);
        }

        public void OnScenarioInit(Scenario scenario)
        {
            all_citits.Clear();
            scenario.citySet.ForEach(city => {
                if (city.IsCity())
                    all_citits.Add(city);

            });
        }

        public override void OnEnter()
        {
            Window.Instance.Open(windowName);
        }

        public override void OnDestroy()
        {
            Window.Instance.Close(windowName);
        }
        
        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClickUp:
                    {
                        GameSystemManager.Instance.Back();
                        break;
                    }
            }
        }
    }
}
