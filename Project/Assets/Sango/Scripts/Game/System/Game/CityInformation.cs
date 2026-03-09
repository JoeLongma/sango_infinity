using Sango.Game.Player;
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
        public List<SangoObject> default_citits = new List<SangoObject>();
        public List<SangoObject> all_citits = new List<SangoObject>();
        protected string windowName = "window_information_city";

        public void Start(City target)
        {
            TargetCity = target;
            all_citits = default_citits;
            Push();
        }

        public void Start(City target, List<SangoObject> city_list)
        {
            TargetCity = target;
            all_citits = city_list;
            Push();
        }

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
            all_citits = default_citits;
            Push();
        }

        public void OnScenarioInit(Scenario scenario)
        {
            default_citits.Clear();
            scenario.citySet.ForEach(city => {
                if (city.IsCity())
                    default_citits.Add(city);
            });
        }

        public override void OnEnter()
        {
            Window.Instance.Open(windowName, this);
        }

        public override void OnBack(ICommandEvent whoGone)
        {
            Window.Instance.SetVisible(windowName, true);
        }

        public override void OnExit()
        {
            Window.Instance.SetVisible(windowName, false);
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
