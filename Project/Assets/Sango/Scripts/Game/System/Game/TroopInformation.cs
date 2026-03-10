using Sango.Game.Player;
using Sango.Game.Render.UI;
using System.Collections.Generic;

namespace Sango.Game
{
    /// <summary>
    /// 城池治安系统逻辑
    /// </summary>
    [GameSystem(auto = true)]
    public class TroopInformation : GameSystem
    {
        public Troop Target;
        public List<SangoObject> default_objects = new List<SangoObject>();
        public List<SangoObject> all_objects = new List<SangoObject>();
        protected string windowName = "window_information_troop";

        public void Start(Troop target)
        {
            Target = target;
            all_objects = default_objects;
            Push();
        }

        public void Start(Troop target, List<SangoObject> troop_list)
        {
            Target = target;
            all_objects = troop_list;
            Push();
        }

        public override void Init()
        {
            Name = "部队情报";
            GameEvent.OnTroopRightMouseButtonContextMenuShow += OnTroopRightMouseButtonContextMenuShow;
#if UNITY_ANDROID
            GameEvent.OnCityContextMenuShow += OnCityContextMenuShow;
#endif
            GameEvent.OnScenarioInit += OnScenarioInit;
        }
        protected virtual bool CityMenuCanShow()
        {
            return true;
        }

        public override void Clear()
        {
#if UNITY_ANDROID
            GameEvent.OnCityContextMenuShow -= OnCityContextMenuShow;
#endif
            GameEvent.OnTroopRightMouseButtonContextMenuShow -= OnTroopRightMouseButtonContextMenuShow;
        }
#if UNITY_ANDROID


        protected virtual void OnCityContextMenuShow(IContextMenuData menuData, City city)
        {
            TargetCity = city;
            if(CityMenuCanShow())
                menuData.Add("情报", 1000, city, OnClickMenuItem, true);
        }
#endif

        protected virtual void OnTroopRightMouseButtonContextMenuShow(IContextMenuData menuData, Troop troop)
        {
            Target = troop;
            if (CityMenuCanShow())
                menuData.Add(Name, 20, null, OnClickMenuItem, true);
        }

        protected virtual void OnClickMenuItem(IContextMenuItem contextMenuItem)
        {
            ContextMenu.CloseAll();
            all_objects = default_objects;
            Push();
        }

        public virtual void OnScenarioInit(Scenario scenario)
        {
            default_objects.Clear();
            scenario.citySet.ForEach(city => {
                if (city.IsCity())
                    default_objects.Add(city);
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
