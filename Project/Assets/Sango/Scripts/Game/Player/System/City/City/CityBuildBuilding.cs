using Sango.Game.Render.UI;
using System;
using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityBuildBuilding : CommandSystemBase
    {
        public City TargetCity { get; set; }
        public List<Person> personList = new List<Person>();
        public int wonderBuildCounter = 0;

        public string customTitleName = "内城";
        public List<ObjectSortTitle> customTitleList = new List<ObjectSortTitle>()
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

        public override bool IsValid
        {
            get
            {

                //return TargetCity.freePersons.Count > 0 && TargetCity.gold > 500;
                return true;
            }
        }

        void OnCityContextMenuShow(ContextMenuData menuData, City city)
        {
            TargetCity = city;
            if (city.IsCity() && city.BelongForce != null && city.BelongForce.IsPlayer && city.BelongForce == Scenario.Cur.CurRunForce)
                menuData.Add("都市/内城", 0, city, OnClickMenuItem, IsValid);
        }

        void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            TargetCity = contextMenuItem.customData as City;
            PlayerCommand.Instance.Push(this);
        }

        public void UpdateJobValue()
        {
            if (personList.Count <= 0)
                return;
            TargetBuildingType = canSelectBuildingTypes[CurSelectBuildingTypeIndex];
            int buildAbility = GameUtility.Method_PersonBuildAbility(personList.ToArray());
            int turnCount = TargetBuildingType.durabilityLimit % buildAbility == 0 ? 0 : 1;
            wonderBuildCounter = Math.Min(Scenario.Cur.Variables.BuildMaxTurn, TargetBuildingType.durabilityLimit / buildAbility + turnCount);
        }

        public override void OnEnter()
        {
            CurSelectBuildingTypeIndex = -1;
            CurSelectSlotIndex = -1;
            Window.Instance.Open("window_city_building");
        }

        public override void OnDestroy()
        {
            Window.Instance.Close("window_city_building");
        }

        public void DoBuildBuilding()
        {
            if (personList.Count <= 0)
                return;
            TargetCity.JobBuildBuilding(CurSelectSlotIndex, personList.ToArray(), TargetBuildingType, wonderBuildCounter);
        }

        public void DoUpgradeBuilding()
        {
            int buildingId = TargetCity.innerSlot[CurSelectSlotIndex];
            if (buildingId > 0)
            {
                Building building = Scenario.Cur.GetObject<Building>(buildingId);
                TargetCity.JobUpgradeBuilding(building, personList.ToArray(), TargetBuildingType, wonderBuildCounter);
            }
        }

        public void DoDestroyBuilding()
        {
            int buildingId = TargetCity.innerSlot[CurSelectSlotIndex];
            if (buildingId > 0)
            {
                Building building = Scenario.Cur.GetObject<Building>(buildingId);
                building.OnFall(null);
            }
        }
    }
}
