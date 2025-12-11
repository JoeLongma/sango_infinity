using Sango.Game.Render.UI;
using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityTrainTroops : CityComandBase
    {
        public CityTrainTroops()
        {
            customTitleName = "训练";
            customTitleList = new List<ObjectSortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByStrength,
            };
            customMenuName = "军事/训练";
            customMenuOrder = 103;
            windowName = "window_city_command_base";
        }

        protected override bool MenuCanShow()
        {
            return true;
        }

        public override bool IsValid
        {
            get
            {
                return TargetCity.GetIntriorBuildingComplateNumber((int)BuildingKindType.Barracks) > 0 && TargetCity.CheckJobCost(CityJobType.TrainTroops) && TargetCity.morale < TargetCity.MaxMorale  ;
            }
        }

        protected override void OnUIInit()
        {
            base.OnUIInit();
            targetUI.title_value.gameObject.SetActive(true);
            targetUI.value_value.gameObject.SetActive(true);

            int destValue = TargetCity.morale + wonderNumber;
            if(destValue > TargetCity.MaxMorale)
                destValue = TargetCity.MaxMorale;

            targetUI.title_value.text = "士气";
            targetUI.value_value.text = $"{TargetCity.morale}→{destValue}";
        }

        public override int CalculateWonderNumber()
        {
            return TargetCity.JobTrainTroops(personList.ToArray(), true);
        }

        public override void InitPersonList()
        {
            personList.Clear();
            Person[] people = ForceAI.CounsellorRecommendTrainTroops(TargetCity.freePersons);
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
                TargetCity.JobTrainTroops(personList.ToArray());
                Done();
            }
        }
    }
}
