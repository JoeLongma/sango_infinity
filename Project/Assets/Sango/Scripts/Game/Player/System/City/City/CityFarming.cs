using Sango.Game.Render.UI;
using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityFarming : CityComandBase
    {
        public CityFarming()
        {
            customTitleName = "农业";
            customTitleList = new List<ObjectSortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByStrength,
            };
            customMenuName = "都市/农业";
            customMenuOrder = 10;
            windowName = "window_city_command_base";
        }

        public override bool IsValid
        {
            get
            {
                return TargetCity.agriculture < TargetCity.agricultureLimit && TargetCity.CheckJobCost(CityJobType.Farming);
            }
        }

        protected override void OnUIInit()
        {
            base.OnUIInit();
            targetUI.title_value.gameObject.SetActive(true);
            targetUI.value_value.gameObject.SetActive(true);
            targetUI.title_gold.gameObject.SetActive(true);
            targetUI.value_gold.gameObject.SetActive(true);

            targetUI.title_value.text = "农业";
            targetUI.title_gold.text = "资金";
            targetUI.value_value.text = $"{TargetCity.agriculture}→{TargetCity.agriculture + wonderNumber}";
            targetUI.value_gold.text = $"{TargetCity.GetJobCost(CityJobType.Farming)}/{TargetCity.gold}";
        }

        public override int CalculateWonderNumber()
        {
            return TargetCity.JobFarming(personList.ToArray(), true);
        }

        public override void InitPersonList()
        {
            personList.Clear();
            Person[] people = ForceAI.CounsellorRecommendFarming(TargetCity.freePersons);
            if (people != null)
            {
                for (int i = 0; i < people.Length; ++i)
                {
                    Person p = people[i];
                    if (p != null)
                        personList.Add(p);
                }
            }
        }

        public override void DoJob()
        {
            if (personList.Count > 0)
            {
                TargetCity.JobFarming(personList.ToArray());
                Done();
            }
        }
    }
}
