using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityFarming : CityComandBase
    {
        public CityFarming()
        {
            customTitleName = "农业";
            customTitleList = new List<SortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByStrength,
            };
            customMenuName = "都市/农业";
            customMenuOrder = 1;
            windowName = "window_city_command_base";
        }

        public override bool IsValid
        {
            get
            {
                return TargetCity.agriculture < TargetCity.agricultureLimit && TargetCity.CheckJobCost(CityJobType.Farming);
            }
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
