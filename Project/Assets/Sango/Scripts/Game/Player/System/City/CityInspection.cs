using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityInspection : CityComandBase
    {
        public CityInspection()
        {
            customTitleName = "巡视";
            customTitleList = new List<SortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByStrength,
            };
            customMenuName = "都市/巡视";
            customMenuOrder = 1;
            windowName = "window_city_command_base";
        }

        public override bool IsValid
        {
            get
            {
                return TargetCity.security < 100 &&
                    TargetCity.CheckJobCost(CityJobType.Inspection) &&
                    TargetCity.GetIntriorBuildingComplateNumber((int)BuildingKindType.PatrolBureau) > 0;
            }
        }

        public override int CalculateWonderNumber()
        {
            return TargetCity.JobInspection(personList.ToArray(), true);
        }

        public override void InitPersonList()
        {
            personList.Clear();
            Person[] people = ForceAI.CounsellorRecommendInspection(TargetCity.freePersons);
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
                TargetCity.JobInspection(personList.ToArray());
                Done();
            }
        }
    }
}
