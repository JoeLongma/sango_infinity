using System.Collections.Generic;

namespace Sango.Game.Player
{
    public class CityCreateMachine : CityCreateBoat
    {
        public CityCreateMachine()
        {
            customTitleName = "生产兵器";
            customTitleList = new List<ObjectSortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByIntelligence,
                PersonSortFunction.SortByBaseCreativeAbility,
            };
            customMenuName = "都市/生产兵器";
            customMenuOrder = 35;
            windowName = "window_city_create_items";
           
        }

        public override void OnEnter()
        {
            ItemTypes.Clear();
            Scenario scenario = Scenario.Cur;
            for (int i = 6; i <= 9; i++)
            {
                ItemTypes.Add(scenario.GetObject<ItemType>(i));
            }
            CurSelectedItemTypeIndex = 0;
            CurSelectedItemType = ItemTypes[CurSelectedItemTypeIndex];
            TotalBuildingCount = TargetCity.GetIntriorBuildingComplateNumber((int)BuildingKindType.MechineFactory);
            base.OnEnter();
        }

        public override bool IsValid
        {
            get
            {
                return TargetCity.itemStore.TotalNumber < TargetCity.StoreLimit && TargetCity.CheckJobCost(CityJobType.CreateMachine)
                    && TargetCity.GetIntriorBuildingComplateNumber((int)BuildingKindType.MechineFactory) > 0;
            }
        }

        public override int CalculateWonderNumber()
        {
            TurnAndDestNumber = TargetCity.JobCreateMachine(personList.ToArray(), CurSelectedItemType, TotalBuildingCount, true);
            return TurnAndDestNumber[1];
        }

        public override void DoJob()
        {
            if (personList.Count > 0)
            {
                TargetCity.JobCreateMachine(personList.ToArray(), CurSelectedItemType, TotalBuildingCount);
                Done();
            }
        }
    }
}
