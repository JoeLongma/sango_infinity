using Sango.Game.Render.UI;
using System;
using System.Collections.Generic;

namespace Sango.Game.Player
{
    public class CityRecruitTroops : CommandSystemBase<CityRecruitTroops>
    {
        public City TargetCity { get; set; }
        public List<Person> personList = new List<Person>();
        public int wonderTroopsAddNumber = 0;
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
            TargetCity = contextMenuItem.customData as City;
            PlayerCommand.Instance.Push(this);
        }

        public void UpdateJobValue()
        {
            int barracksNum = TargetCity.GetIntriorBuildingComplateTotalLevel((int)BuildingKindType.Barracks);
            int jobId = (int)CityJobType.RecuritTroop;
            Scenario scenario = Scenario.Cur;
            ScenarioVariables variables = scenario.Variables;

            int totalValue = 0;
            int maxValue = 0;
            Person maxPerson = null;
            for (int i = 0; i < personList.Count; i++)
            {
                Person person = personList[i];
                if (person == null) continue;
                if (person.BaseRecruitmentAbility > maxValue)
                {
                    maxPerson = person;
                    maxValue = person.BaseRecruitmentAbility;
                }
            }

            // 最高属性武将获得100%加成,其余两个获取50%加成
            for (int i = 0; i < personList.Count; i++)
            {
                Person person = personList[i];
                if (person == null) continue;
                if (person != maxPerson)
                {
                    totalValue += maxPerson.BaseRecruitmentAbility * 5;
                }
                else
                {
                    totalValue += maxPerson.BaseRecruitmentAbility * 10;
                }
            }

            totalValue = GameUtility.Method_RecuritTroop(totalValue, barracksNum);

            Tools.OverrideData<int> overrideData = GameUtility.IntOverrideData.Set(totalValue);
            GameEvent.OnCityJobResult?.Invoke(TargetCity, jobId, personList.ToArray(), overrideData);
            totalValue = overrideData.Value;

            // 治安对征兵的影响
            wonderTroopsAddNumber = (int)(totalValue * (1f - Math.Max(0, (100 - TargetCity.security)) * variables.securityInfluenceRecruitTroops));
        }

        public override void OnEnter()
        {
            personList.Clear();
            Person[] people = ForceAI.CounsellorRecommendRecuritTroop(TargetCity.freePersons);
            if (people != null)
            {
                for (int i = 0; i < people.Length; ++i)
                {
                    Person p = people[i];
                    if (p != null)
                        personList.Add(p);
                }
            }
            UpdateJobValue();
            Window.Instance.ShowWindow("window_city_recurit_troops");
        }

        public override void OnDestroy()
        {
            Window.Instance.HideWindow("window_city_recurit_troops");
        }

        public override void OnDone()
        {
            Window.Instance.HideWindow("window_city_recurit_troops");
        }

        public void DoJob()
        {

        }
    }
}
