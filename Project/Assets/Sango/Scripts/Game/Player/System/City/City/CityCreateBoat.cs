using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityCreateBoat : CityCreateItems
    {
        public CityCreateBoat()
        {
            customTitleName = "生产舰船";
            customTitleList = new List<ObjectSortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByIntelligence,
                PersonSortFunction.SortByBaseCreativeAbility,
            };
            customMenuName = "都市/生产舰船";
            customMenuOrder = 40;
            windowName = "window_city_create_items";
        }

        public override void OnEnter()
        {
            ItemTypes.Clear();
            Scenario scenario = Scenario.Cur;
            for (int i = 10; i <= 12; i++)
            {
                ItemTypes.Add(scenario.GetObject<ItemType>(i));
            }
            CurSelectedItemTypeIndex = 0;
            CurSelectedItemType = ItemTypes[CurSelectedItemTypeIndex];
            TotalBuildingCount = TargetCity.GetIntriorBuildingComplateNumber((int)BuildingKindType.BoatFactory);
            base.OnEnter();
        }

        public override bool IsValid
        {
            get
            {
                return TargetCity.itemStore.TotalNumber < TargetCity.StoreLimit && TargetCity.CheckJobCost(CityJobType.CreateItems)
                    && TargetCity.GetIntriorBuildingComplateNumber((int)BuildingKindType.BoatFactory) > 0;
            }
        }

        public override int CalculateWonderNumber()
        {
            TurnAndDestNumber = TargetCity.JobCreateBoat(personList.ToArray(), CurSelectedItemType, TotalBuildingCount, true);
            return TurnAndDestNumber[1];
        }

        public override void DoJob()
        {
            if (personList.Count > 0)
            {
                TargetCity.JobCreateBoat(personList.ToArray(), CurSelectedItemType, TotalBuildingCount);
                Done();
            }
        }
    }
}
