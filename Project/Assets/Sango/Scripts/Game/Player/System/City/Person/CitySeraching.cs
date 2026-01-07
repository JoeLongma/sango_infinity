using Sango.Game.Render.UI;
using System.Collections.Generic;

namespace Sango.Game.Player
{
    public class CitySeraching : CommandSystemBase
    {
        public City TargetCity { get; set; }
        public List<Person> personList = new List<Person>();
        public List<Person> counsellorRecommendList = new List<Person>();

        public string customTitleName;
        public List<ObjectSortTitle> customTitleList;
        public string customMenuName;
        public int customMenuOrder;
        public string windowName;

        public CitySeraching()
        {
            customTitleName = "探索人才";

            customMenuName = "人事/探索人才";
            customMenuOrder = 221;
            windowName = "window_city_searching";

        }

        public override void Init()
        {
            GameEvent.OnCityContextMenuShow += OnCityContextMenuShow;
        }

        public override void Clear()
        {
            GameEvent.OnCityContextMenuShow -= OnCityContextMenuShow;
        }

        public override bool IsValid
        {
            get
            {
                return TargetCity.freePersons.Count > 0 && TargetCity.BelongCorps.ActionPoint >= JobType.GetJobCostAP((int)CityJobType.Searching);
            }
        }

        protected virtual void OnCityContextMenuShow(ContextMenuData menuData, City city)
        {
            TargetCity = city;
            if (TargetCity.IsCity() && city.BelongForce != null && city.BelongForce.IsPlayer && city.BelongForce == Scenario.Cur.CurRunForce)
            {
                menuData.Add(customMenuName, customMenuOrder, city, OnClickMenuItem, IsValid);
            }
        }

        protected virtual void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            TargetCity = contextMenuItem.customData as City;
            PlayerCommand.Instance.Push(this);
        }

        public override void OnEnter()
        {
            counsellorRecommendList.Clear();
            int[] recommandFeatrues = new int[] { 86 };
            Person[] recommandList = ForceAI.CounsellorRecommendSearching(TargetCity.freePersons, TargetCity, recommandFeatrues);
            if (recommandList != null)
            {
                for (int i = 0; i < recommandList.Length; i++)
                {
                    counsellorRecommendList.Add(recommandList[i]);
                }
            }

            counsellorRecommendList.Sort(
                (a, b) =>
                        {
                            bool ahas = a.HasFeatrue(recommandFeatrues);
                            bool bhas = b.HasFeatrue(recommandFeatrues);
                            if (ahas == bhas)
                            {
                                return -a.Glamour.CompareTo(b.Glamour);
                            }
                            else
                            {
                                return -ahas.CompareTo(bhas);
                            }
                        }
                );

            personList.Clear();
            if(counsellorRecommendList.Count > 0)
                personList.Add(counsellorRecommendList[0]);

            if (customTitleList == null)
            {
                customTitleList = new List<ObjectSortTitle>()
                {
                    PersonSortFunction.SortByName,
                    PersonSortFunction.GetSortByContainsInList("军师推荐", counsellorRecommendList),
                    PersonSortFunction.SortByPolitics,
                    PersonSortFunction.GetSortByFeatrueId(86),
                };
            }

            Window.Instance.Open(windowName);
        }
        public override void OnDestroy()
        {
            UIDialog.Close();
            Window.Instance.Close(windowName);
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClickUp:
                    {
                        PlayerCommand.Instance.Back();
                        break;
                    }
            }
        }

        public void DoJob()
        {
            if(personList.Count <= 0)
                return;

            for(int i = 0; i < personList.Count; i++)
            {
                TargetCity.JobSearching(personList.ToArray());
            }

            Done();
        }
    }
}
