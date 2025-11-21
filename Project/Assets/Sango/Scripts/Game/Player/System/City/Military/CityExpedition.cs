using Sango.Game.Render.UI;
using System;
using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class CityExpedition : CommandSystemBase
    {
        public City TargetCity { get; set; }
        public List<Person> personList = new List<Person>();

        public string customTitleName = "出征";
        public List<ObjectSortTitle> customTitleList = new List<ObjectSortTitle>()
        {
            PersonSortFunction.SortByName,
            PersonSortFunction.SortByPolitics,
        };

        public List<TroopType> ActivedLandTroopTypes = new List<TroopType>();
        public List<TroopType> ActivedWaterTroopTypes = new List<TroopType>();

        public int CurSelectLandTrropTypeIndex { get; set; }
        public int CurSelectWaterTrropTypeIndex { get; set; }

        public Troop TargetTroop { get; set; }

        public override void Init()
        {
            GameEvent.OnCityContextMenuShow += OnCityContextMenuShow;
        }

        public override void Clear()
        {
            GameEvent.OnCityContextMenuShow -= OnCityContextMenuShow;
        }

        void OnCityContextMenuShow(ContextMenuData menuData, City city)
        {
            if (city.BelongForce != null && city.BelongForce.IsPlayer && city.BelongForce == Scenario.Cur.CurRunForce)
                menuData.Add("军事/出征", 100, city, OnClickMenuItem);
        }

        void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            TargetCity = contextMenuItem.customData as City;
            PlayerCommand.Instance.Push(this);
        }

        public void UpdateJobValue()
        {

        }

        public override void OnEnter()
        {
            CurSelectLandTrropTypeIndex = 0;
            CurSelectWaterTrropTypeIndex = 0;
            Window.Instance.ShowWindow("window_city_create_troop");
        }

        public override void OnDestroy()
        {
            Window.Instance.HideWindow("window_city_create_troop");
        }

    }
}
