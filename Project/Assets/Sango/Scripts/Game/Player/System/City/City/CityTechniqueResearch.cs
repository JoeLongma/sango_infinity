using Sango.Game.Render.UI;
using Sango.Render;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sango.Game.Player
{
    public class CityTechniqueResearch : CommandSystemBase
    {
        public City TargetCity { get; set; }
        public Cell TargetCell { get; set; }
        public MapObject SelectBuildingObject { get; set; }
        public List<Cell> buildRangeCell = new List<Cell>();
        public List<BuildingType> canBuildBuildingType = new List<BuildingType>();

        public List<Person> personList = new List<Person>();
        public int wonderBuildCounter = 0;

        public string customTitleName = "开发";
        public List<ObjectSortTitle> customTitleList = new List<ObjectSortTitle>()
        {
            PersonSortFunction.SortByName,
            PersonSortFunction.SortByPolitics,
        };

        public Technique TargetTechnique { get; set; }
        public int goldCost;
        public int tpCost;
        public int counter;

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
                return true;
                //return TargetCity.freePersons.Count > 0
                //    && TargetCity.gold > 200
                //    && TargetCity.BelongCorps.ActionPoint >= JobType.GetJobCostAP((int)CityJobType.Research);
            }
        }

        public void DoResearch()
        {
            if (TargetTechnique == null || !TargetTechnique.CanResearch(TargetCity.BelongForce))
                return;

            TargetCity.JobResearch(personList.ToArray(), TargetTechnique, false);
            Done();
        }

        void OnCityContextMenuShow(ContextMenuData menuData, City city)
        {
            TargetCity = city;
            if (city.IsCity() && city.BelongForce != null && city.BelongForce.IsPlayer && city.BelongForce == Scenario.Cur.CurRunForce)
                menuData.Add("都市/研究技巧", 2000, city, OnClickMenuItem, IsValid);
        }

        void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            TargetCity = contextMenuItem.customData as City;
            PlayerCommand.Instance.Push(this);
        }

        public override void OnEnter()
        {
            personList.Clear();
            Window.Instance.Open("window_technique");
        }

        public override void OnDestroy()
        {
            Window.Instance.Close("window_technique");
        }

        public void SelectTechnique(Technique tech)
        {
            TargetTechnique = tech;
            Person[] ps = ForceAI.CounsellorRecommendResearch(TargetCity.freePersons, TargetTechnique);
            if (ps != null)
            {
                personList.Clear();
                foreach (Person person in ps)
                {
                    personList.Add(person);
                }
            }
            UpdateJobValue();
        }

        public void UpdateJobValue()
        {
            if (personList.Count <= 0)
                return;

            int[] valus = TargetCity.JobResearch(personList.ToArray(), TargetTechnique, true);
            goldCost = valus[0];
            tpCost = valus[1];
            counter = valus[2];
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {

            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClickUp:
                    {
                        if (SelectBuildingObject == null)
                            PlayerCommand.Instance.Back();
                        break;
                    }
            }
        }

    }
}
