using Sango.Game.Render.UI;
using System.Collections.Generic;

namespace Sango.Game.Player
{
    public class CityRecruit : CommandSystemBase
    {
        public City TargetCity { get; set; }
        public List<Person> personList = new List<Person>();
        public string customTargetTitleName;
        public string customActionTitleName;
        public List<ObjectSortTitle> customTargetTitleList;
        public List<ObjectSortTitle> customActionTitleList;
        public string customMenuName;
        public int customMenuOrder;
        public string windowName;
        public List<Person> targetList = new List<Person>();
        public List<Person> target = new List<Person>();

        public CityRecruit()
        {
            customTargetTitleName = "登庸武将";
            customActionTitleName = "执行武将";

            customMenuName = "人事/登庸武将";
            customMenuOrder = 231;
            windowName = "window_city_recruit";

            customActionTitleList = new List<ObjectSortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByIntelligence,
                PersonSortFunction.SortByPolitics,
                PersonSortFunction.SortByGlamour,
                PersonSortFunction.SortByFeatureList,
            };

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
                return TargetCity.freePersons.Count > 0 &&
                    TargetCity.BelongCorps.ActionPoint >= JobType.GetJobCostAP((int)CityJobType.RecuritPerson);
            }
        }

        protected virtual void OnCityContextMenuShow(IContextMenuData menuData, City city)
        {
            TargetCity = city;
            if (TargetCity.IsCity() && city.BelongForce != null && city.BelongForce.IsPlayer && city.BelongForce == Scenario.Cur.CurRunForce)
            {
                menuData.Add(customMenuName, customMenuOrder, city, OnClickMenuItem, IsValid);
            }
        }

        protected virtual void OnClickMenuItem(IContextMenuItem contextMenuItem)
        {
            TargetCity = contextMenuItem.CustomData as City;
            GameSystemManager.Instance.Push(this);
        }

        public override void OnEnter()
        {
            personList.Clear();
            target.Clear();
            customTargetTitleList = new List<ObjectSortTitle>()
            {
                PersonSortFunction.SortByName,
                PersonSortFunction.SortByBelongCity,
                PersonSortFunction.SortByBelongCorps,
                PersonSortFunction.GetSortByDistanceDay(TargetCity),
                PersonSortFunction.SortByLoyalty,
                PersonSortFunction.SortByTroopsLimit,
                PersonSortFunction.SortByCommand,
                PersonSortFunction.SortByStrength,
                PersonSortFunction.SortByIntelligence,
                PersonSortFunction.SortByPolitics,
                PersonSortFunction.SortByGlamour,
                PersonSortFunction.SortBySpearLv,
                PersonSortFunction.SortByHalberdLv,
                PersonSortFunction.SortByCrossbowLv,
                PersonSortFunction.SortByRideLv,
                PersonSortFunction.SortByWaterLv,
                PersonSortFunction.SortByMachineLv,
                PersonSortFunction.SortByFeatureList,
            };
            targetList.Clear();
            Scenario.Cur.citySet.ForEach(x =>
            {
                if (!x.IsSameForce(TargetCity))
                {
                    int dis = x.Distance(TargetCity);
                    if (dis < 10)
                    {
                        x.allPersons.ForEach(y => { targetList.Add(y); });
                        x.wildPersons.ForEach(y => { targetList.Add(y); });
                        x.captiveList.ForEach(y => { targetList.Add(y); });
                    }
                }
                else
                {
                    int dis = x.Distance(TargetCity);
                    if (dis < 10)
                    {
                        x.wildPersons.ForEach(y => { targetList.Add(y); });
                    }
                }
            });
            targetList.Sort((a, b) => -PersonSortFunction.SortByLoyalty.personSortFunc.Invoke(a, b));

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
                        GameSystemManager.Instance.Back();
                        break;
                    }
            }
        }
        public void SetTarget(List<Person> target)
        {
            this.target = target;
            if (target.Count > 0)
            {
                Person recommandPerson = ForceAI.CounsellorRecommendRecuritPerson(TargetCity.freePersons, target[0], null);
                if (recommandPerson != null)
                {
                    personList.Clear();
                    personList.Add(recommandPerson);
                }
            }
        }

        public void DoJob()
        {
            if (personList.Count <= 0 || target.Count <= 0)
                return;

            if (!TargetCity.JobRecuritPerson(personList[0], target[0]))
            {
                UIDialog dialog1 = UIDialog.Open(UIDialog.DialogStyle.ClickPersonSay, $"交给我吧", () =>
                {
                    // TODO:展示武将
                    // 暂时直接招募
                    UIDialog.Close();
                    Done();

                });
                dialog1.SetPerson(personList[0]);
            }
            else
            {
                Done();
            }
        }

    }
}
