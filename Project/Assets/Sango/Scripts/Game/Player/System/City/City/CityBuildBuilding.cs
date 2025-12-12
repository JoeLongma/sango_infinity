using Sango.Game.Render;
using Sango.Game.Render.UI;
using Sango.Render;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityBuildBuilding : CommandSystemBase
    {
        public City TargetCity { get; set; }
        public Cell TargetCell { get; set; }
        public BuildingRender SelectBuildingRender { get; set; }
        public List<Cell> buildRangeCell = new List<Cell>();
        public List<BuildingType> canBuildBuildingType = new List<BuildingType>();

        public List<Person> personList = new List<Person>();
        public int wonderBuildCounter = 0;

        public string customTitleName = "内城";
        public List<ObjectSortTitle> customTitleList = new List<ObjectSortTitle>()
        {
            PersonSortFunction.SortByName,
            PersonSortFunction.SortByPolitics,
        };

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
                InitCanBuildingTypes();

                return TargetCity.freePersons.Count > 0 && TargetCity.gold > 200 && !TargetCity.IsInteriorBuildFull();
            }
        }



        void OnCityContextMenuShow(ContextMenuData menuData, City city)
        {
            TargetCity = city;
            if (city.IsCity() && city.BelongForce != null && city.BelongForce.IsPlayer && city.BelongForce == Scenario.Cur.CurRunForce)
                menuData.Add("都市/开发", 0, city, OnClickMenuItem, IsValid);
        }

        void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            TargetCity = contextMenuItem.customData as City;
            PlayerCommand.Instance.Push(this);
        }

        void InitCanBuildingTypes()
        {
            canBuildBuildingType.Clear();
            Scenario.Cur.CommonData.BuildingTypes.ForEach(x =>
            {
                if (x.IsIntrior && x.IsValid(TargetCity.BelongForce))
                {
                    canBuildBuildingType.Add(x);
                }
            });
        }

        public void UpdateJobValue()
        {
            if (personList.Count <= 0)
                return;

            int buildAbility = GameUtility.Method_PersonBuildAbility(personList.ToArray());
            int turnCount = TargetBuildingType.durabilityLimit % buildAbility == 0 ? 0 : 1;
            wonderBuildCounter = Math.Min(Scenario.Cur.Variables.BuildMaxTurn, TargetBuildingType.durabilityLimit / buildAbility + turnCount);
        }

        public override void OnEnter()
        {
            Window.Instance.Open("window_city_building");
        }

        public void SelectBuildingType(BuildingType buildingType)
        {
            TargetBuildingType = buildingType;
            Person[] ps = ForceAI.CounsellorRecommendBuild(TargetCity.freePersons, TargetBuildingType);
            if(ps != null)
            {
                personList.Clear();
                foreach (Person person in ps)
                {
                    personList.Add(person);
                }
            }
            UpdateJobValue();
        }

        public override void OnDestroy()
        {
            Window.Instance.Close("window_city_building");
            ClearShowBuildRange();
            buildRangeCell.Clear();
        }

        public void OnSelectCell()
        {

        }

        public void DoBuildBuilding()
        {
            if (personList.Count <= 0)
                return;
            TargetCity.JobBuildBuilding(TargetCell, personList.ToArray(), TargetBuildingType, wonderBuildCounter);
        }

        protected void ShowBuildRange()
        {
            MapRender mapRender = MapRender.Instance;
            mapRender.SetDarkMask(true);
            if (buildRangeCell.Count == 0) return;
            for (int i = 0, count = buildRangeCell.Count; i < count; ++i)
            {
                Cell c = buildRangeCell[i];
                mapRender.SetGridMaskColor(c.x, c.y, Color.red);
                mapRender.SetDarkMaskColor(c.x, c.y, Color.black);
            }
            mapRender.EndSetGridMask();
            mapRender.EndSetDarkMask();
        }

        protected void ClearShowBuildRange()
        {
            MapRender mapRender = MapRender.Instance;
            mapRender.SetDarkMask(false);
            if (buildRangeCell.Count == 0) return;
            for (int i = 0, count = buildRangeCell.Count; i < count; ++i)
            {
                Cell c = buildRangeCell[i];
                mapRender.SetGridMaskColor(c.x, c.y, Color.black);
                mapRender.SetDarkMaskColor(c.x, c.y, Color.clear);

            }
            mapRender.EndSetGridMask();
            mapRender.EndSetDarkMask();
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            if (SelectBuildingRender == null) return;

            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClick:
                    {
                        PlayerCommand.Instance.Back();
                        break;
                    }

                case CommandEventType.Click:
                    {
                        if (isOverUI) return;

                        if (buildRangeCell.Contains(cell) && cell.building == null)
                        {
                            TargetCell = cell;
                            DoBuildBuilding();
                            Done();
                        }
                        break;
                    }
            }
        }

    }
}
