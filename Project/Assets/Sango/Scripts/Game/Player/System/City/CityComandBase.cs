using Sango.Game.Render.UI;
using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityComandBase : CommandSystemBase
    {
        public City TargetCity { get; set; }
        public List<Person> personList = new List<Person>();
        public int wonderNumber = 0;

        public string customTitleName;
        public List<SortTitle> customTitleList;
        public string customMenuName;
        public int customMenuOrder;
        public string windowName;

        public override void Init()
        {
            GameEvent.OnCityContextMenuShow += OnCityContextMenuShow;
        }

        public override void Clear()
        {
            GameEvent.OnCityContextMenuShow -= OnCityContextMenuShow;
        }

        protected virtual void OnCityContextMenuShow(ContextMenuData menuData, City city)
        {
            if (city.BelongForce != null && city.BelongForce.IsPlayer && city.BelongForce == Scenario.Cur.CurRunForce)
            {
                TargetCity = city;
                menuData.Add(customMenuName, customMenuOrder, city, OnClickMenuItem, IsValid);
            }
        }

        protected virtual void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            TargetCity = contextMenuItem.customData as City;
            PlayerCommand.Instance.Push(this);
        }

        public virtual void UpdateJobValue()
        {
            wonderNumber = CalculateWonderNumber();
        }

        public virtual int CalculateWonderNumber()
        {
            return 0;
        }

        public virtual void InitPersonList()
        {
        }

        public override void OnEnter()
        {
            InitPersonList();
            UpdateJobValue();
            Window.WindowInterface windowInterface = Window.Instance.ShowWindow(windowName);
            if (windowInterface != null)
            {
                UICityComandBase uICityComandBase = windowInterface.ugui_instance as UICityComandBase;
                if (uICityComandBase != null)
                    uICityComandBase.Init(this);
            }
        }

        public override void OnDestroy()
        {
            Window.Instance.HideWindow(windowName);
        }

        public virtual void DoJob()
        {

        }
    }
}
