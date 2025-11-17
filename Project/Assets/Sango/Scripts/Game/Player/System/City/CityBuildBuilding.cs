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

        public List<BuildingType> canSelectBuildingTypes = new List<BuildingType>();
        public int CurSelectBuildingTypeIndex { get; set; }
        public int CurSelectSlotIndex { get; set; }
        public BuildingType TargetBuildingType { get; set; }

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
                menuData.Add("都市/城建", 0, city, OnClickMenuItem);
        }

        void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            TargetCity = contextMenuItem.customData as City;
            PlayerCommand.Instance.Push(this);
        }

        public void UpdateJobValue()
        {
            TargetBuildingType = canSelectBuildingTypes[CurSelectBuildingTypeIndex];
            int buildAbility = GameUtility.Method_PersonBuildAbility(personList.ToArray());
            int turnCount = TargetBuildingType.durabilityLimit % buildAbility == 0 ? 0 : 1;
            wonderBuildCounter = Math.Min(Scenario.Cur.Variables.BuildMaxTurn, TargetBuildingType.durabilityLimit / buildAbility + turnCount);
        }

        public override void OnEnter()
        {
            Window.Instance.ShowWindow("window_city_building");
        }

        public override void OnDestroy()
        {
            Window.Instance.HideWindow("window_city_building");
        }

        public override void OnDone()
        {
            Window.Instance.HideWindow("window_city_building");
        }

        public void DoJob()
        {

        }
    }
}
