using Sango.Game.Render.UI;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;

namespace Sango.Game.Player
{
    public class CitySystem : CommandSystemBase
    {
        public override void Init()
        {
            base.Init();
            Singleton<CityBuildBuilding>.Instance.Init();
            Singleton<CityRecruitTroops>.Instance.Init();   // 征兵
            //Singleton<CityCreateBoat>.Instance.Init();
            Singleton<CityCreateItems>.Instance.Init();
            //Singleton<CityCreateMachine>.Instance.Init();
            Singleton<CityDevelop>.Instance.Init();
            Singleton<CityFarming>.Instance.Init();
            Singleton<CityInspection>.Instance.Init();
            Singleton<CityTrade>.Instance.Init();
            Singleton<CityTechniqueResearch>.Instance.Init();

            //军事
            Singleton<CityExpedition>.Instance.Init();      // 出征
            Singleton<CityTrainTroops>.Instance.Init();     // 训练
            Singleton<CityTransport>.Instance.Init();     // 训练

            //人事
            Singleton<CityCallPerson>.Instance.Init();
            Singleton<CityTransformPerson>.Instance.Init();
            Singleton<CitySeraching>.Instance.Init();
            Singleton<CityRecruit>.Instance.Init();
            Singleton<CityReward>.Instance.Init();
        }

        public City TargetCity { get; set; }

        public void Start(City city, Vector3 startPoint)
        {
            ContextMenuData.MenuData.Clear();
            GameEvent.OnCityContextMenuShow?.Invoke(ContextMenuData.MenuData, city);
            if (!ContextMenuData.MenuData.IsEmpty())
            {
                TargetCity = city;
                ContextMenu.Show(ContextMenuData.MenuData, startPoint);
                PlayerCommand.Instance.Push(this);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Window.Instance.Open("window_city_info_panel", TargetCity);
            TargetCity.Render?.SetFlash(true);
        }

        /// <summary>
        /// 离开当前命令的时候触发
        /// </summary>
        public override void OnDestroy()
        {
            TargetCity.Render?.SetFlash(false);
            ContextMenu.CloseAll();
            Window.Instance.Close("window_city_info_panel");
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClickDown:
                    {
                        if (ContextMenu.Close())
                            PlayerCommand.Instance.Back();

                        break;
                    }

                case CommandEventType.ClickDown:
                    {
                        if (isOverUI) return;

                        Done();
                        break;
                    }
            }
        }
    }
}
