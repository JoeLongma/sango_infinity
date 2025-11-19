using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityCreateItems : CityComandBase
    {
        public List<ItemType> ItemTypes = new List<ItemType>();
        public int CurSelectedItemTypeIndex { get; set; }
        public ItemType CurSelectedItemType { get; set; }
        public int TotalBuildingCount { get; set; }
        public int[] TurnAndDestNumber { get; set; }
        public CityCreateItems()
        {
            customTitleName = "生产兵装";
            customTitleList = new List<SortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByIntelligence,
                PersonSortFunction.SortByBaseCreativeAbility,
            };
            customMenuName = "都市/生产兵装";
            customMenuOrder = 1;
            windowName = "window_city_create_items";
            

        }

        public override void OnEnter()
        {
            ItemTypes.Clear();
            Scenario scenario = Scenario.Cur;
            for (int i = 2; i <= 5; i++)
            {
                ItemTypes.Add(scenario.GetObject<ItemType>(i));
            }
            CurSelectedItemTypeIndex = 0;
            CurSelectedItemType = ItemTypes[CurSelectedItemTypeIndex];
            TotalBuildingCount = TargetCity.GetIntriorBuildingComplateNumber((int)BuildingKindType.BlacksmithShop);
            base.OnEnter();
        }

        public override bool IsValid
        {
            get
            {
                return TargetCity.itemStore.TotalNumber < TargetCity.StoreLimit && TargetCity.CheckJobCost(CityJobType.CreateItems)
                    && TargetCity.GetIntriorBuildingComplateNumber((int)BuildingKindType.BlacksmithShop) > 0;
            }
        }

        public override int CalculateWonderNumber()
        {
            TurnAndDestNumber = new int[2]
            {
                0,
                TargetCity.JobCreateItems(personList.ToArray(), CurSelectedItemType, TotalBuildingCount, true)
            };
            return TurnAndDestNumber[1];
        }

        public override void InitPersonList()
        {
            personList.Clear();
            Person[] people = ForceAI.CounsellorRecommendCreateItems(TargetCity.freePersons);
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
                TargetCity.JobCreateItems(personList.ToArray(), CurSelectedItemType, TotalBuildingCount);
                Done();
            }
        }
    }
}
