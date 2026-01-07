using Sango.Game.Render.UI;
using System.Collections.Generic;

namespace Sango.Game.Player
{
    public class CityReward : CommandSystemBase
    {
        public City TargetCity { get; set; }
        public List<Person> personList = new List<Person>();
        public List<Person> targetList = new List<Person>();

        public string customTitleName;
        public List<ObjectSortTitle> customTitleList;
        public string customMenuName;
        public int customMenuOrder;
        public string windowName;

        public CityReward()
        {
            customTitleName = "褒赏";
            customTitleList = new List<ObjectSortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByLoyalty,
                PersonSortFunction.SortByBelongCity,
                PersonSortFunction.SortByBelongCorps,
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
                PersonSortFunction.SortByRideLv,
                PersonSortFunction.SortByWaterLv,
                PersonSortFunction.SortByMachineLv,
                PersonSortFunction.SortByFeatureList,
            };
            customMenuName = "人事/褒赏";
            customMenuOrder = 241;
            windowName = "window_city_reward";

        }


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
                return TargetCity.gold > 100 && TargetCity.BelongCorps.ActionPoint >= JobType.GetJobCostAP((int)CityJobType.Reward);
            }
        }

        protected virtual void OnCityContextMenuShow(ContextMenuData menuData, City city)
        {
            TargetCity = city;
            if (TargetCity.IsCity() && city.BelongForce != null && city.BelongForce.IsPlayer && city.BelongForce == Scenario.Cur.CurRunForce)
            {
                menuData.Add(customMenuName, customMenuOrder, city, OnClickMenuItem, IsValid);
            }
        }

        protected virtual void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            TargetCity = contextMenuItem.customData as City;
            PlayerCommand.Instance.Push(this);
        }

        public override void OnEnter()
        {
            targetList.Clear();
            TargetCity.BelongForce.ForEachPerson(x => {
                if(x != TargetCity.BelongForce.Governor && x.IsFree)
                {
                    targetList.Add(x);
                }
            });
            targetList.Sort((a, b) => -PersonSortFunction.SortByLoyalty.personSortFunc.Invoke(a, b));
            Window.Instance.Open(windowName);
        }
        public override void OnDestroy()
        {
            UIDialog.Close();
            Window.Instance.Close(windowName);
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClickUp:
                    {
                        PlayerCommand.Instance.Back();
                        break;
                    }
            }
        }

        public void DoJob()
        {
            if (personList.Count <= 0)
                return;

            for (int i = 0; i < personList.Count; i++)
            {
                TargetCity.JobRewardPersons(personList.ToArray());
            }

            Done();
        }

    }
}
