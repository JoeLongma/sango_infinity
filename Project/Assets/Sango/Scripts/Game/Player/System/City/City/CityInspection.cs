using Sango.Game.Render.UI;
using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityInspection : CityComandBase
    {
        public CityInspection()
        {
            customTitleName = "巡视";
            customTitleList = new List<ObjectSortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByStrength,
            };
            customMenuName = "都市/巡视";
            customMenuOrder = 20;
            windowName = "window_city_command_base";
        }

        public override bool IsValid
        {
            get
            {
                return TargetCity.security < 100 &&
                    TargetCity.CheckJobCost(CityJobType.Inspection) 
                    //&&TargetCity.GetBuildingComplateNumber((int)BuildingKindType.PatrolBureau) > 0
                    ;
            }
        }

        protected override void OnUIInit()
        {
            base.OnUIInit();
            targetUI.title_value.gameObject.SetActive(true);
            targetUI.value_value.gameObject.SetActive(true);
            targetUI.title_gold.gameObject.SetActive(true);
            targetUI.value_gold.gameObject.SetActive(true);

            targetUI.title_value.text = "治安";
            targetUI.title_gold.text = "资金";

            int destValue = TargetCity.security + wonderNumber;
            if (destValue > 100)
                destValue = 100;

            targetUI.value_value.text = $"{TargetCity.security}→{destValue}";
            targetUI.value_gold.text = $"{TargetCity.GetJobCost(CityJobType.Inspection)}/{TargetCity.gold}";
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
