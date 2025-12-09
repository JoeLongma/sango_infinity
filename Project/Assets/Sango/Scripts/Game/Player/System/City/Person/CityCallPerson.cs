using Sango.Game.Render.UI;
using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityCallPerson : CityComandBase
    {
        public CityCallPerson()
        {
            customTitleName = "召唤";
            customTitleList = new List<ObjectSortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByStrength,
            };
            customMenuName = "人事/召唤";
            customMenuOrder = 201;
            windowName = "window_city_command_base";
        }
        protected override bool CityOnly()
        {
            return false;
        }
        public override bool IsValid
        {
            get
            {
                // 需要至少两座城
                return TargetCity.BelongForce.CityCount > 1;
            }
        }

        protected override void OnUIInit()
        {
            base.OnUIInit();
            targetUI.person_extra_value.gameObject.SetActive(true);
            targetUI.person_extra_value.text = $"{personList.Count}人";
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
            TargetCity.BelongForce.ForEachCity(city =>
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
