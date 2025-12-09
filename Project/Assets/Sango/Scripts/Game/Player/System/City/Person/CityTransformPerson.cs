using Sango.Game.Render.UI;
using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityTransformPerson : CityComandBase
    {
        List<City> TransformTo = new List<City>();
       
        public CityTransformPerson()
        {
            customTitleName = "移动";
            customTitleList = new List<ObjectSortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByStrength,
            };
            customMenuName = "人事/移动";
            customMenuOrder = 202;
            windowName = "window_city_command_base";
        }

        public override bool IsValid
        {
            get
            {
                // 需要至少两座城
                return TargetCity.BelongForce.CityCount > 1;
            }
        }
        protected override bool CityOnly()
        {
            return false;
        }

        protected override void OnUIInit()
        {
            base.OnUIInit();

            targetUI.person_extra_value.gameObject.SetActive(true);
            targetUI.person_extra_value.text = $"{personList.Count}人";
             
            targetUI.button_extra.gameObject.SetActive(true);
            targetUI.button_extra_value.text = "";

            targetUI.title_extra.text = "抵达日数";
            targetUI.value_extra.text = "";
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
            TargetCity.BelongForce.ForEachCity(city =>
            {
                if (city != TargetCity)
                {
                    cities.Add(city);
                }
            });

            Singleton<CitySelectSystem>.Instance.Start(cities,
              TransformTo, 1, OnCityChange, customTitleList, customTitleName);
        }

        public void OnCityChange(List<City> cities)
        {
            TransformTo = cities;
            City dest = TransformTo[0];
            targetUI.button_extra_value.text = dest.Name;
            targetUI.value_extra.text = $"{TargetCity.Distance(dest) * 10}日";
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
