using System;
using System.Collections.Generic;

namespace Sango.Game.Player
{
    public class CityTransformPerson : CityComandBase
    {
        List<City> TransformTo = new List<City>();
        List<ObjectSortTitle> citySortTitleList;
        public CityTransformPerson()
        {
            customTitleName = "移动";
            customTitleList = new List<ObjectSortTitle>()
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
                PersonSortFunction.SortByRideLv,
                PersonSortFunction.SortByWaterLv,
                PersonSortFunction.SortByMachineLv,
                PersonSortFunction.SortByFeatureList,
            };
            customMenuName = "人事/移动";
            customMenuOrder = 201;
            windowName = "window_city_command_base";

            citySortTitleList = new List<ObjectSortTitle>()
            {
                CitySortFunction.SortByName,
                CitySortFunction.SortByPersonCount,
                CitySortFunction.SortByBelongCity,
                CitySortFunction.SortByTroops,
                CitySortFunction.SortByGold,
                CitySortFunction.SortByFood,
                CitySortFunction.SortByLevel,
            };
        }

        public override bool IsValid
        {
            get
            {
                // 需要至少两座城
                return TargetCity.BelongForce.CityBaseCount > 1 &&
                    TargetCity.BelongCorps.ActionPoint >= JobType.GetJobCostAP((int)CityJobType.TransformPerson);
            }
        }
        protected override bool MenuCanShow()
        {
            return true;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            TransformTo.Clear();
            personList.Clear();
        }

        protected override void OnUIInit()
        {
            base.OnUIInit();

            targetUI.person_extra_value.gameObject.SetActive(true);
            targetUI.person_extra_value.text = $"{personList.Count}人";

            targetUI.button_extra.gameObject.SetActive(true);
            targetUI.button_extra_value.gameObject.SetActive(true);

            if (TransformTo.Count == 0)
            {
                targetUI.button_extra_value.text = "";
                targetUI.value_1.text = "";
            }
            else
            {
                City dest = TransformTo[0];
                targetUI.button_extra_value.text = dest.Name;
                UpdateJobValue();
                targetUI.value_1.text = $"{Math.Max(1, TargetCity.Distance(dest)) * 10}日";
            }

            targetUI.title1.gameObject.SetActive(true);
            targetUI.title1.text = "抵达日数";

            targetUI.value_1.gameObject.SetActive(true);

            targetUI.action_value.text = $"{JobType.GetJobCostAP((int)CityJobType.TransformPerson)}/{TargetCity.BelongCorps.ActionPoint}";

        }

        public override int CalculateWonderNumber()
        {
            return personList.Count;
        }

        public override void InitPersonList()
        {
            personList.Clear();
        }

        public override void OnSelectCity()
        {
            List<City> cities = new List<City>();
            TargetCity.BelongForce.ForEachCityBase(city =>
            {
                if (city != TargetCity)
                {
                    cities.Add(city);
                }
            });

            Singleton<CitySelectSystem>.Instance.Start(cities,
              TransformTo, 1, OnCityChange, citySortTitleList, "目的城池选择");
        }

        public void OnCityChange(List<City> cities)
        {
            TransformTo = cities;
            City dest = TransformTo[0];
            targetUI.button_extra_value.text = dest.Name;
            targetUI.value_1.text = $"{Math.Max(1, TargetCity.Distance(dest)) * 10}日";
        }

        public override void DoJob()
        {
            if (personList.Count == 0) return;
            if (TransformTo.Count == 0) return;

            for (int i = 0; i < personList.Count; i++)
            {
                personList[i].TransformToCity(TransformTo[0]);
            }
            Done();
        }

        public override void OnSelectPerson()
        {
            Singleton<PersonSelectSystem>.Instance.Start(TargetCity.freePersons,
              personList, TargetCity.freePersons.Count, OnPersonChange, customTitleList, customTitleName);
        }
    }
}
