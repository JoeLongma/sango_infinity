using Sango.Game.Render.UI;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;

namespace Sango.Game.Player
{
    public class CityRecruitTroops : CommandSystemBase<CityRecruitTroops>
    {
        public City TargetCity { get; set; }

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
                menuData.Add("军事/募兵", 1, city, OnClickMenuItem);
        }

        void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            PlayerCommand.Instance.Push(this);
            TargetCity = contextMenuItem.customData as City;
        }

        public override void OnEnter()
        {
            Window.Instance.ShowWindow("window_city_recruit");
        }

        public override void OnDestroy()
        {
            Window.Instance.HideWindow("window_city_recruit");
        }

        public override void OnDone()
        {
            Window.Instance.HideWindow("window_city_recruit");
        }

        public void DoJob(Person[] people)
        {

        }
    }
}
