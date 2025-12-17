using System.Collections.Generic;

namespace Sango.Game.Player
{
    public class CityCreateHorse : CityCreateItems
    {
        public CityCreateHorse()
        {
            customTitleName = "繁殖军马";
            customTitleList = new List<ObjectSortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByIntelligence,
                PersonSortFunction.SortByBaseCreativeAbility,
            };
            customMenuName = "都市/繁殖军马";
            customMenuOrder = 38;
            windowName = "window_city_create_items";
        }

        protected override void InitItem()
        {
            ItemTypes.Clear();
            Scenario scenario = Scenario.Cur;
            ItemTypes.Add(scenario.GetObject<ItemType>(5));
            CurSelectedItemTypeIndex = 0;
            CurSelectedItemType = ItemTypes[CurSelectedItemTypeIndex];
            TargetBuilding = TargetCity.GetFreeBuilding((int)BuildingKindType.Stable);
        }

        public override bool IsValid
        {
            get
            {
                return TargetCity.FreePersonCount > 0 && 
                    TargetCity.itemStore.TotalNumber < TargetCity.StoreLimit && 
                    TargetCity.CheckJobCost(CityJobType.CreateItems)
                    && TargetCity.GetFreeBuilding((int)BuildingKindType.Stable) != null;
            }
        }
    }
}
