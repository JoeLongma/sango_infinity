using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityRecruitTroops : CityComandBase
    {
        public CityRecruitTroops()
        {
            customTitleName = "征兵";
            customTitleList = new List<SortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByGlamour,
            };
            customMenuName = "军事/征兵";
            customMenuOrder = 100;
            windowName = "window_city_command_base";
        }

        public override bool IsValid
        {
            get
            {
                return TargetCity.GetIntriorBuildingComplateNumber((int)BuildingKindType.Barracks) > 0 && 
                    TargetCity.CheckJobCost(CityJobType.RecuritTroops);
            }
        }

        public override int CalculateWonderNumber()
        {
            return TargetCity.JobRecuritTroop(personList.ToArray(), true);
        }

        public override void InitPersonList()
        {
            personList.Clear();
            Person[] people = ForceAI.CounsellorRecommendRecuritTroop(TargetCity.freePersons);
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
                TargetCity.JobRecuritTroop(personList.ToArray());
                Done();
            }
        }
    }
}
