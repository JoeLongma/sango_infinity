using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityDevelop : CityComandBase
    {
        public CityDevelop()
        {
            customTitleName = "商业";
            customTitleList = new List<SortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByStrength,
            };
            customMenuName = "都市/商业";
            customMenuOrder = 1;
            windowName = "window_city_command_base";
        }

        public override bool IsValid
        {
            get
            {
                return TargetCity.commerce < TargetCity.commerceLimit && TargetCity.CheckJobCost(CityJobType.Develop);
            }
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
