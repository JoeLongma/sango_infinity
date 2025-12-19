using Sango.Game.Render.UI;
using System;
using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityTransport : CommandSystemBase
    {
        public City TargetCity { get; set; }
        public List<Person> personList = new List<Person>();

        string windowName = "window_city_create_transport";
        public string customTitleName = "运输";
        public List<ObjectSortTitle> customTitleList = new List<ObjectSortTitle>()
        {
            PersonSortFunction.SortByName,
            PersonSortFunction.SortByLevel,
            PersonSortFunction.SortByTroopsLimit,
            PersonSortFunction.SortByCommand,
            PersonSortFunction.SortByStrength,
            PersonSortFunction.SortByIntelligence,
            PersonSortFunction.SortByPolitics,
            PersonSortFunction.SortByGlamour,
            PersonSortFunction.SortBySpearLv,
            PersonSortFunction.SortByHalberdLv,
            PersonSortFunction.SortByCrossbowLv,
            PersonSortFunction.SortByHorseLv,
            PersonSortFunction.SortByWaterLv,
            PersonSortFunction.SortByMachineLv,
            PersonSortFunction.SortByFeatureList,
        };

        public Troop TargetTroop { get; set; }

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
            TargetCity = city;
            if (city.BelongForce != null && city.BelongForce.IsPlayer && city.BelongForce == Scenario.Cur.CurRunForce)
                menuData.Add("军事/运输", 110, city, OnClickMenuItem, IsValid);
        }

        void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            TargetCity = contextMenuItem.customData as City;
            PlayerCommand.Instance.Push(this);
        }

        public override bool IsValid
        {
            get
            {
                return TargetCity.troops > 0 && TargetCity.food > 0 && TargetCity.freePersons.Count > 0 &&
                    TargetCity.BelongCorps.ActionPoint >= JobType.GetJobCostAP((int)CityJobType.MakeTansport);
            }
        }
        public void UpdateJobValue()
        {
            if (personList.Count == 0) return;

            TargetTroop.Leader = personList[0];

            if (personList.Count > 1) TargetTroop.Member1 = personList[1];
            if (personList.Count > 2) TargetTroop.Member2 = personList[2];

            TargetTroop.CalculateAttribute(Scenario.Cur);
        }

        public override void OnEnter()
        {
            personList.Clear();
            TargetTroop = new Troop();
            Scenario scenario = Scenario.Cur;
            TroopType troopType = scenario.GetObject<TroopType>(6);
            Person[] persons = ForceAI.CounsellorRecommendTransportTroop(TargetCity.freePersons);
            Person leader = persons[0];

            if (leader != null)
                personList.Add(leader);
            
            TargetTroop.morale = TargetCity.morale;
            TargetTroop.MaxMorale = TargetCity.MaxMorale;
            TargetTroop.energy = TargetCity.energy;
            TargetTroop.Leader = leader;
            TargetTroop.Member1 = null;
            TargetTroop.Member2 = null;
            TargetTroop.TroopType = troopType;
            if (TargetTroop.troops == 0)
            {
                TargetTroop.troops = 1;
                TargetTroop.food = Math.Min(20, TargetCity.food);
            }
            TargetTroop.missionType = (int)MissionType.TroopTransformGoodsToCity;
            Window.Instance.Open(windowName);
        }

        public override void OnDestroy()
        {
            Window.Instance.Close(windowName);
        }

        public void MakeTroop()
        {
            if (TargetTroop.troops <= 0) return;
            if (TargetTroop.food <= 0) return;
            if (personList.Count == 0) return;

            ContextMenu.CloseAll();
            TargetTroop.ActionOver = false;
            TargetTroop.IsAlive = true;

            TargetCity.troops -= TargetTroop.troops;
            TargetCity.food -= TargetTroop.food;
            TargetCity.gold -= TargetTroop.gold;
            TargetCity.itemStore.Remove(TargetTroop.itemStore);
            TargetTroop.ForEachPerson(person =>
            {
                TargetCity.freePersons.Remove(person);
            });
            TargetCity.Render?.UpdateRender();
            TargetCity.EnsureTroop(TargetTroop, Scenario.Cur);
            Window.Instance.SetVisible(windowName, false);
            Singleton<TroopSystem>.Instance.Start(TargetTroop);
        }

        public override void OnBack(ICommandEvent whoGone)
        {
            base.OnBack(whoGone);
            if (whoGone is ObjectSelectSystem) return;
            Window.Instance.SetVisible(windowName, true);
            TargetTroop.EnterCity(TargetCity);
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClickUp:
                    PlayerCommand.Instance.Back(); break;
            }

        }
    }
}
