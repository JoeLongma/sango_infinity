using System.Collections.Generic;

namespace Sango.Game.Player
{
    [GameSystem(auto = true)]
    public class CityRecruitTroops : CityBaseSystem
    {
        public CityRecruitTroops()
        {
            customTitleName = "征兵";
            customTitleList = new List<ObjectSortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByGlamour,
            };
            customMenuName = "都市/征兵";
            customMenuOrder = 15;
            windowName = "window_city_recruit_troops";
        }

        public override bool IsValid
        {
            get
            {
                return TargetCity.FreePersonCount > 0 && 
                    TargetCity.GetFreeBuilding((int)BuildingKindType.Barracks) != null &&
                    TargetCity.CheckJobCost(CityJobType.RecuritTroops) &&
                    TargetCity.BelongCorps.ActionPoint >= JobType.GetJobCostAP((int)CityJobType.RecuritTroops);
            }
        }
        
        public override int CalculateWonderNumber()
        {
            return TargetCity.JobRecuritTroop(personList.ToArray(), true);
        }

        public override void RecommandPersonList()
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
