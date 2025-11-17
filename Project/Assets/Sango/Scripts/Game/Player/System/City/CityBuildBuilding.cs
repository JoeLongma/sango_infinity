using Sango.Game.Render.UI;
using System;
using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityBuildBuilding : CommandSystemBase<CityBuildBuilding>
    {
        public City TargetCity { get; set; }
        public List<Person> personList = new List<Person>();
        public int wonderBuildCounter = 0;

        public string customTitleName = "城建";
        public List<SortTitle> customTitleList = new List<SortTitle>()
        {
            PersonSortFunction.SortByName,
            PersonSortFunction.SortByPolitics,
        };

        public override void Init()
        {
            GameEvent.OnCityContextMenuShow += OnCityContextMenuShow;
        }

        public override void Clear()
        {
            GameEvent.OnCityContextMenuShow -= OnCityContextMenuShow;
        }

        void OnCityContextMenuShow(ContextMenuData menuData, City city)
        {
            if (city.BelongForce != null && city.BelongForce.IsPlayer && city.BelongForce == Scenario.Cur.CurRunForce)
                menuData.Add("都市/城建", 1, city, OnClickMenuItem);
        }

        void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            TargetCity = contextMenuItem.customData as City;
            PlayerCommand.Instance.Push(this);
        }

        public void UpdateJobValue()
        {

        }

        public override void OnEnter()
        {
            personList.Clear();
            Person[] people = ForceAI.CounsellorRecommendRecuritTroop(TargetCity.freePersons);
            if (people != null)
            {
                for (int i = 0; i < people.Length; ++i)
                {
                    Person p = people[i];
                    if (p != null)
                        personList.Add(p);
                }
            }
            UpdateJobValue();
            Window.Instance.ShowWindow("window_city_recurit_troops");
        }

        public override void OnDestroy()
        {
            Window.Instance.HideWindow("window_city_recurit_troops");
        }

        public override void OnDone()
        {
            Window.Instance.HideWindow("window_city_recurit_troops");
        }

        public void DoJob()
        {

        }
    }
}
