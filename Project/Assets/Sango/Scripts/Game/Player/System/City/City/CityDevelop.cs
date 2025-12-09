using Sango.Game.Render.UI;
using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityDevelop : CityComandBase
    {
        public CityDevelop()
        {
            customTitleName = "商业";
            customTitleList = new List<ObjectSortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByIntelligence,
            };
            customMenuName = "都市/商业";
            customMenuOrder = 5;
            windowName = "window_city_command_base";
        }

        public override bool IsValid
        {
            get
            {
                return TargetCity.commerce < TargetCity.commerceLimit && TargetCity.CheckJobCost(CityJobType.Develop);
            }
        }

        protected override void OnUIInit()
        {
            base.OnUIInit();
            targetUI.title_value.gameObject.SetActive(true);
            targetUI.value_value.gameObject.SetActive(true);
            targetUI.title_gold.gameObject.SetActive(true);
            targetUI.value_gold.gameObject.SetActive(true);

            targetUI.title_value.text = "商业";
            targetUI.title_gold.text = "资金";

            int destValue = TargetCity.commerce + wonderNumber;
            if (destValue > TargetCity.CommerceLimit)
                destValue = TargetCity.CommerceLimit;

            targetUI.value_value.text = $"{TargetCity.commerce}→{destValue}";
            targetUI.value_gold.text = $"{TargetCity.GetJobCost(CityJobType.Develop)}/{TargetCity.gold}";
        }

        public override int CalculateWonderNumber()
        {
            return TargetCity.JobDevelop(personList.ToArray(), true);
        }

        public override void InitPersonList()
        {
            personList.Clear();
            Person[] people = ForceAI.CounsellorRecommendDevelop(TargetCity.freePersons);
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
                TargetCity.JobDevelop(personList.ToArray());
                Done();
            }
        }
    }
}
