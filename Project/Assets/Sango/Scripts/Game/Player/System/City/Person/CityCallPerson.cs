using System.Collections.Generic;

namespace Sango.Game.Player
{
    public class CityCallPerson : CityComandBase
    {
        List<ObjectSortTitle> citySortTitleList;
        public CityCallPerson()
        {
            customTitleName = "召唤";
            customTitleList = new List<ObjectSortTitle>()
            {
                PersonSortFunction.SortByName,
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
            customMenuName = "人事/召唤";
            customMenuOrder = 201;
            windowName = "window_city_command_base";

        }
        protected override bool MenuCanShow()
        {
            return true;
        }
        public override bool IsValid
        {
            get
            {
                // 需要至少两座城
                return TargetCity.BelongForce.CityBaseCount > 1 &&
                    TargetCity.BelongCorps.ActionPoint >= JobType.GetJobCostAP((int)CityJobType.CallPerson);
            }
        }

        protected override void OnUIInit()
        {
            base.OnUIInit();
            targetUI.person_extra_value.gameObject.SetActive(true);
            targetUI.person_extra_value.text = $"{personList.Count}人";

            targetUI.action_value.text = $"{JobType.GetJobCostAP((int)CityJobType.CallPerson)}/{TargetCity.BelongCorps.ActionPoint}";

        }

        public override int CalculateWonderNumber()
        {
            return personList.Count;
        }

        public override void InitPersonList()
        {
            personList.Clear();
        }

        public override void DoJob()
        {
            if (personList.Count == 0) return;

            for (int i = 0; i < personList.Count; i++)
            {
                personList[i].TransformToCity(TargetCity);
            }
            Done();
        }

        public override void OnSelectPerson()
        {
            List<Person> list = new List<Person>();
            TargetCity.BelongForce.ForEachCityBase(city =>
            {
                if (city != TargetCity)
                {
                    list.AddRange(city.freePersons);
                }
            });

            Singleton<PersonSelectSystem>.Instance.Start(list,
               personList, list.Count, OnPersonChange, customTitleList, customTitleName);

        }


    }
}
