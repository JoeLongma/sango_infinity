using Sango.Game.Render.UI;
using System.Collections.Generic;

namespace Sango.Game.Player
{
    public class CityComandBase : CommandSystemBase
    {
        public City TargetCity { get; set; }
        public List<Person> personList = new List<Person>();
        public int wonderNumber = 0;

        public string customTitleName;
        public List<ObjectSortTitle> customTitleList;
        public string customMenuName;
        public int customMenuOrder;
        public string windowName;

        protected UICityComandBase targetUI;

        public override void Init()
        {
            GameEvent.OnCityContextMenuShow += OnCityContextMenuShow;
        }

        public override void Clear()
        {
            GameEvent.OnCityContextMenuShow -= OnCityContextMenuShow;
        }

        protected virtual void OnUIInit()
        {
            targetUI.windiwTitle.text = customTitleName;
        }

        protected virtual bool CityOnly()
        {
            return TargetCity.IsCity();
        }

        protected virtual void OnCityContextMenuShow(ContextMenuData menuData, City city)
        {
            TargetCity = city;
            if (CityOnly() && city.BelongForce != null && city.BelongForce.IsPlayer && city.BelongForce == Scenario.Cur.CurRunForce)
            {
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
            Window.WindowInterface windowInterface = Window.Instance.Open(windowName);
            if (windowInterface != null)
            {
                UICityComandBase uICityComandBase = windowInterface.ugui_instance as UICityComandBase;
                if (uICityComandBase != null)
                {
                    targetUI = uICityComandBase;
                    uICityComandBase.Init(this, OnUIInit);
                }
            }
        }

        public override void OnDestroy()
        {
            Window.Instance.Close(windowName);
        }

        public virtual void DoJob()
        {

        }

        public virtual void OnSelectPerson()
        {
            Singleton<PersonSelectSystem>.Instance.Start(TargetCity.freePersons,
               personList, 3, OnPersonChange, customTitleList, customTitleName);

        }

        public virtual void OnSelectCity()
        {
            
        }

        public virtual void OnPersonChange(List<Person> personList)
        {
            this.personList = personList;
            UpdateJobValue();
            targetUI?.OnInit();
        }
    }
}
