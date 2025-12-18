using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityCreateItems : CityComandBase
    {
        public enum CreateType
        {
            Weapon,
            Horse,
            Machine,
            Boat
        }

        public List<ItemType> ItemTypes = new List<ItemType>();
        public int CurSelectedItemTypeIndex { get; set; }
        public ItemType CurSelectedItemType { get; set; }
        public Building TargetBuilding { get; set; }
        public int[] TurnAndDestNumber { get; set; }

        public CreateType CurCreateType = CreateType.Weapon;

        public CityCreateItems()
        {
            customTitleName = "生产兵装";
            customTitleList = new List<ObjectSortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByIntelligence,
                PersonSortFunction.SortByBaseCreativeAbility,
            };
            customMenuName = "都市/生产兵装";
            customMenuOrder = 30;
            windowName = "window_city_create_items";
        }

        public override void OnEnter()
        {
            InitItem();
            base.OnEnter();
        }

        protected virtual void InitItem()
        {
            ItemTypes.Clear();
            Scenario scenario = Scenario.Cur;
            for (int i = 2; i < 5; i++)
            {
                ItemTypes.Add(scenario.GetObject<ItemType>(i));
            }

            TargetBuilding = TargetCity.GetFreeBuilding((int)BuildingKindType.BlacksmithShop);
            CurSelectedItemTypeIndex = 0;
            CurSelectedItemType = ItemTypes[CurSelectedItemTypeIndex];
        }


        public override bool IsValid
        {
            get
            {
                return TargetCity.FreePersonCount > 0 && TargetCity.itemStore.TotalNumber < TargetCity.StoreLimit &&
                    TargetCity.CheckJobCost(CityJobType.CreateItems)
                    && TargetCity.GetFreeBuilding((int)BuildingKindType.BlacksmithShop) != null &&
                    TargetCity.BelongCorps.ActionPoint >= JobType.GetJobCostAP((int)CityJobType.CreateItems);

            }
        }

        public override int CalculateWonderNumber()
        {
            TurnAndDestNumber = new int[2]
            {
                0,
                TargetCity.JobCreateItems(personList.ToArray(), CurSelectedItemType, TargetBuilding, true)
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
                TargetCity.JobCreateItems(personList.ToArray(), CurSelectedItemType, TargetBuilding);
                Done();
            }
        }
    }
}
