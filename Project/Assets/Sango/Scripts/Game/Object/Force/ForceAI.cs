using System;
using System.Collections.Generic;

namespace Sango.Game
{
    public class ForceAI
    {
        /// <summary>
        /// AI外交
        /// </summary>
        public static bool AIDiplomacy(Force force, Scenario scenario)
        {
            if (force.Governor == null) return true;
            if (force.Governor.BelongCity == null) return true;

            City centerCity = force.Governor.BelongCity;
            if (centerCity.freePersons.Count == 0)
                return true;

            if (centerCity.gold < 3000)
                return true;

            // 找到
            foreach (Force neighbor in force.NeighborForceList)
            {
                if (neighbor.IsAlliance(force)) continue;

                int neighborRelation = scenario.GetRelation(neighbor, force);
                if (neighborRelation > 0) continue;
                // 敌人的敌人 就是朋友
                foreach (Force enemysenemy in neighbor.NeighborForceList)
                {
                    if (enemysenemy != force && !enemysenemy.IsAlliance(neighbor) && !force.NeighborForceList.Contains(enemysenemy) && !enemysenemy.IsAlliance(force))
                    {
                        int enemysenemy_relation = scenario.GetRelation(enemysenemy, force);
                        if (enemysenemy_relation > 2000)
                        {
                            if (centerCity.gold > 3000)
                            {
                                // 派遣结盟
                                if (GameRandom.Changce(enemysenemy_relation, 10000))
                                {
                                    centerCity.gold -= 1000;
                                    Alliance alliance = new Alliance()
                                    {
                                        ForceList = new SangoObjectList<Force>(),
                                        leftCount = 36,
                                        allianceType = 1,
                                        IsAlive = true,
                                    };
                                    alliance.ForceList.Add(force);
                                    alliance.ForceList.Add(enemysenemy);

                                    scenario.Add(alliance);

                                    force.AllianceList.Add(alliance);
                                    enemysenemy.AllianceList.Add(alliance);
#if SANGO_DEBUG
                                    Sango.Log.Print($"@外交@{force.Name} 于{scenario.GetDateStr()} 与 {enemysenemy.Name} 达成了12个月的结盟 Id={alliance.Id}!!");
#endif
                                }
                            }
                        }
                        else
                        {
                            if (centerCity.gold > 3000)
                            { // 结交
                                scenario.AddRelation(enemysenemy, force, 1000);
                                centerCity.gold -= 1000;
#if SANGO_DEBUG
                                Sango.Log.Print($"@外交@{force.Name} 与 {enemysenemy.Name} 亲密接触,关系到达了{scenario.GetRelation(enemysenemy, force)}!!");
#endif
                                return true;
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// AI-俘虏
        /// </summary>
        public static bool AICaptives(Force force, Scenario scenario)
        {
            return true;
            // 释放或者招降俘虏
            // 赎回俘虏
        }
        /// <summary>
        /// AI-科技研发
        /// </summary>

        public static bool AITechniques(Force force, Scenario scenario)
        {
            return true;
            //if ((this.ArchitectureCount != 0) && (this.UpgradingTechnique < 0))
            //{
            //    if (this.PlanTechnique == null)
            //    {
            //        if (this.PreferredTechniqueKinds.Count > 0)
            //        {
            //            Dictionary<Technique, float> list = new Dictionary<Technique, float>();
            //            float preferredTechniqueComplition = this.GetPreferredTechniqueComplition();
            //            foreach (Technique technique in Session.Current.Scenario.GameCommonData.AllTechniques.Techniques.Values)
            //            {
            //                if (!this.IsTechniqueUpgradable(technique))
            //                {
            //                    continue;
            //                }
            //                if (this.GetTechniqueUsefulness(technique) <= 0) continue;

            //                float weight = 1;
            //                foreach (KeyValuePair<Condition, float> c in technique.AIConditionWeight)
            //                {
            //                    if (c.Key.CheckCondition(this))
            //                    {
            //                        weight *= c.Value;
            //                    }
            //                }

            //                if (preferredTechniqueComplition < 0.5f)
            //                {
            //                    if (this.PreferredTechniqueKinds.IndexOf(technique.Kind) >= 0)
            //                    {
            //                        list.Add(technique, weight);
            //                    }
            //                }
            //                else if (preferredTechniqueComplition < 0.75f)
            //                {
            //                    if ((this.PreferredTechniqueKinds.IndexOf(technique.Kind) >= 0) || GameObject.Chance(0x19))
            //                    {
            //                        list.Add(technique, weight);
            //                    }
            //                }
            //                else if (preferredTechniqueComplition < 1f)
            //                {
            //                    if ((this.PreferredTechniqueKinds.IndexOf(technique.Kind) >= 0) || GameObject.Chance(50))
            //                    {
            //                        list.Add(technique, weight);
            //                    }
            //                }
            //                else if ((this.PreferredTechniqueKinds.IndexOf(technique.Kind) >= 0) || GameObject.Chance(0x4b))
            //                {
            //                    list.Add(technique, weight);
            //                }
            //            }
            //            if (list.Count > 0)
            //            {
            //                this.PlanTechnique = GameObject.WeightedRandom(list);
            //            }
            //            else
            //            {
            //                this.PlanTechnique = this.GetRandomTechnique();
            //            }
            //        }
            //        else
            //        {
            //            this.PlanTechnique = this.GetRandomTechnique();
            //        }
            //    }
            //    if (this.PlanTechnique != null)
            //    {
            //        if (((this.TechniquePoint + this.TechniquePointForTechnique) >= this.getTechniqueActualPointCost(this.PlanTechnique)) && (this.Reputation >= this.getTechniqueActualReputation(this.PlanTechnique)))
            //        {
            //            if (this.ArchitectureCount > 1)
            //            {
            //                this.Architectures.PropertyName = "Fund";
            //                this.Architectures.IsNumber = true;
            //                this.Architectures.ReSort();
            //            }
            //            Architecture a = this.Architectures[0] as Architecture;
            //            if (a.IsFundEnough)
            //            {
            //                this.PlanTechniqueArchitecture = this.Architectures[0] as Architecture;
            //                if (this.PlanTechniqueArchitecture.Fund >= this.getTechniqueActualFundCost(this.PlanTechnique))
            //                {
            //                    this.DepositTechniquePointForTechnique(this.TechniquePointForTechnique);
            //                    this.UpgradeTechnique(this.PlanTechnique, this.PlanTechniqueArchitecture);
            //                    this.PlanTechniqueArchitecture = null;
            //                    this.PlanTechnique = null;
            //                }
            //            }
            //            else
            //            {
            //                this.PlanTechniqueArchitecture = null;
            //                this.PlanTechnique = null;
            //            }
            //        }
            //        else if ((this.Reputation >= this.getTechniqueActualReputation(this.PlanTechnique)) && GameObject.Chance(0x21))
            //        {
            //            this.SaveTechniquePointForTechnique(this.getTechniqueActualPointCost(this.PlanTechnique) / this.PlanTechnique.Days);
            //        }
            //        else if (GameObject.Chance(10))
            //        {
            //            this.PlanTechniqueArchitecture = null;
            //            this.PlanTechnique = null;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 军师建造推荐
        /// </summary>
        public static Person[] CounsellorRecommendBuild(List<Person> sortedFreePersons, BuildingType buildingType, out int maxBuildTurn)
        {
            maxBuildTurn = 0;
            sortedFreePersons.RemoveAll(x => x.ActionOver);
            int count = sortedFreePersons.Count;
            if (count <= 0)
                return null;

            ScenarioVariables scenarioVariables = Scenario.Cur.Variables;

            int maxPersonCount = scenarioVariables.jobMaxPersonCount[(int)CityJobType.Build];
            Person[] checkPersons = new Person[3];
            int buildAbility = 99999;
            int buildCount = 999;
            for (int i = 0; i < count; i++)
            {
                Person person1 = sortedFreePersons[i];
                int buildAbility1 = GameUtility.Method_BuildAbility(person1.BaseBuildAbility);
                int _count1 = buildingType.durabilityLimit % buildAbility1 == 0 ? 0 : 1;
                int buildCount1 = Math.Min(scenarioVariables.BuildMaxTurn, buildingType.durabilityLimit / buildAbility1 + _count1);

                if (buildCount1 < buildCount)
                {
                    checkPersons[0] = person1;
                    checkPersons[1] = null;
                    checkPersons[2] = null;
                    buildCount = buildCount1;
                }
                else if (buildCount1 == buildCount)
                {
                    if (buildAbility1 < buildAbility)
                    {
                        checkPersons[0] = person1;
                        checkPersons[1] = null;
                        checkPersons[2] = null;
                        buildAbility = buildAbility1;
                    }
                }

                for (int j = i + 1; j < count; j++)
                {
                    Person person2 = sortedFreePersons[j];
                    int buildAbility2 = GameUtility.Method_BuildAbility(person1.BaseBuildAbility + person2.BaseBuildAbility);
                    int _count2 = buildingType.durabilityLimit % buildAbility2 == 0 ? 0 : 1;
                    int buildCount2 = Math.Min(scenarioVariables.BuildMaxTurn, buildingType.durabilityLimit / buildAbility2 + _count2);
                    if (buildCount2 < buildCount)
                    {
                        checkPersons[0] = person1;
                        checkPersons[1] = person2;
                        checkPersons[2] = null;
                        buildCount = buildCount2;
                    }
                    else if (buildCount2 == buildCount)
                    {
                        if (buildAbility2 < buildAbility)
                        {
                            checkPersons[0] = person1;
                            checkPersons[1] = person2;
                            checkPersons[2] = null;
                            buildAbility = buildAbility2;
                        }
                    }
                    for (int k = j + 1; k < count; k++)
                    {
                        Person person3 = sortedFreePersons[k];
                        int buildAbility3 = GameUtility.Method_BuildAbility(person1.BaseBuildAbility + person2.BaseBuildAbility + person3.BaseBuildAbility);
                        int _count3 = buildingType.durabilityLimit % buildAbility3 == 0 ? 0 : 1;
                        int buildCount3 = Math.Min(scenarioVariables.BuildMaxTurn, buildingType.durabilityLimit / buildAbility3 + _count3);
                        if (buildCount3 < buildCount)
                        {
                            checkPersons[0] = person1;
                            checkPersons[1] = person2;
                            checkPersons[2] = person3;
                            buildCount = buildCount3;
                        }
                        else if (buildCount3 == buildCount)
                        {
                            if (buildAbility3 < buildAbility)
                            {
                                checkPersons[0] = person1;
                                checkPersons[1] = person2;
                                checkPersons[2] = person3;
                                buildAbility = buildAbility3;
                            }
                        }
                    }
                }
            }
            maxBuildTurn = buildCount;
            return checkPersons;
        }

        /// <summary>
        /// 军师巡查推荐
        /// </summary>
        public static Person[] CounsellorRecommendInspection(List<Person> sortedFreePersons, out int value)
        {
            value = 0;
            sortedFreePersons.RemoveAll(x => x.ActionOver);
            int count = sortedFreePersons.Count;
            if (count <= 0)
                return null;

            ScenarioVariables scenarioVariables = Scenario.Cur.Variables;

            int maxPersonCount = scenarioVariables.jobMaxPersonCount[(int)CityJobType.Inspection];
            Person[] checkPersons = new Person[3];
            int buildAbility = 99999;
            for (int i = 0; i < count; i++)
            {
                Person person1 = sortedFreePersons[i];
                int buildAbility1 = GameUtility.Method_SecurityAbility(person1.BaseSecurityAbility);
                if (buildAbility1 < buildAbility)
                {
                    checkPersons[0] = person1;
                    checkPersons[1] = null;
                    checkPersons[2] = null;
                    buildAbility = buildAbility1;
                }
                for (int j = i + 1; j < count; j++)
                {
                    Person person2 = sortedFreePersons[j];
                    int buildAbility2 = GameUtility.Method_SecurityAbility(person1.BaseSecurityAbility + person2.BaseSecurityAbility);
                    if (buildAbility2 < buildAbility)
                    {
                        checkPersons[0] = person1;
                        checkPersons[1] = person2;
                        checkPersons[2] = null;
                        buildAbility = buildAbility2;
                    }
                    for (int k = j + 1; k < count; k++)
                    {
                        Person person3 = sortedFreePersons[k];
                        int buildAbility3 = GameUtility.Method_SecurityAbility(person1.BaseSecurityAbility + person2.BaseSecurityAbility + person3.BaseSecurityAbility);
                        if (buildAbility3 < buildAbility)
                        {
                            checkPersons[0] = person1;
                            checkPersons[1] = person2;
                            checkPersons[2] = person3;
                            buildAbility = buildAbility3;
                        }
                    }
                }
            }
            value = buildAbility;
            return checkPersons;
        }

        /// <summary>
        /// 军师商业推荐
        /// </summary>
        public static Person[] CounsellorRecommendDevelop(List<Person> sortedFreePersons, out int value)
        {
            value = 0;
            sortedFreePersons.RemoveAll(x => x.ActionOver);
            int count = sortedFreePersons.Count;
            if (count <= 0)
                return null;

            ScenarioVariables scenarioVariables = Scenario.Cur.Variables;

            int maxPersonCount = scenarioVariables.jobMaxPersonCount[(int)CityJobType.Develop];
            Person[] checkPersons = new Person[3];
            int buildAbility = 99999;
            for (int i = 0; i < count; i++)
            {
                Person person1 = sortedFreePersons[i];
                int buildAbility1 = GameUtility.Method_DevelopAbility(person1.BaseCommerceAbility);
                if (buildAbility1 < buildAbility)
                {
                    checkPersons[0] = person1;
                    checkPersons[1] = null;
                    checkPersons[2] = null;
                    buildAbility = buildAbility1;
                }
                for (int j = i + 1; j < count; j++)
                {
                    Person person2 = sortedFreePersons[j];
                    int buildAbility2 = GameUtility.Method_DevelopAbility(person1.BaseCommerceAbility + person2.BaseCommerceAbility);
                    if (buildAbility2 < buildAbility)
                    {
                        checkPersons[0] = person1;
                        checkPersons[1] = person2;
                        checkPersons[2] = null;
                        buildAbility = buildAbility2;
                    }
                    for (int k = j + 1; k < count; k++)
                    {
                        Person person3 = sortedFreePersons[k];
                        int buildAbility3 = GameUtility.Method_DevelopAbility(person1.BaseCommerceAbility + person2.BaseCommerceAbility + person3.BaseCommerceAbility);
                        if (buildAbility3 < buildAbility)
                        {
                            checkPersons[0] = person1;
                            checkPersons[1] = person2;
                            checkPersons[2] = person3;
                            buildAbility = buildAbility3;
                        }
                    }
                }
            }

            value = buildAbility;
            return checkPersons;
        }


        /// <summary>
        /// 军师农业推荐
        /// </summary>
        public static Person[] CounsellorRecommendFarming(List<Person> sortedFreePersons, out int value)
        {
            value = 0;
            sortedFreePersons.RemoveAll(x => x.ActionOver);
            int count = sortedFreePersons.Count;
            if (count <= 0)
                return null;

            ScenarioVariables scenarioVariables = Scenario.Cur.Variables;

            int maxPersonCount = scenarioVariables.jobMaxPersonCount[(int)CityJobType.Farming];
            Person[] checkPersons = new Person[3];
            int buildAbility = 99999;
            for (int i = 0; i < count; i++)
            {
                Person person1 = sortedFreePersons[i];
                int buildAbility1 = GameUtility.Method_FarmingAbility(person1.BaseAgricultureAbility);
                if (buildAbility1 < buildAbility)
                {
                    checkPersons[0] = person1;
                    checkPersons[1] = null;
                    checkPersons[2] = null;
                    buildAbility = buildAbility1;
                }
                for (int j = i + 1; j < count; j++)
                {
                    Person person2 = sortedFreePersons[j];
                    int buildAbility2 = GameUtility.Method_FarmingAbility(person1.BaseAgricultureAbility + person2.BaseAgricultureAbility);
                    if (buildAbility2 < buildAbility)
                    {
                        checkPersons[0] = person1;
                        checkPersons[1] = person2;
                        checkPersons[2] = null;
                        buildAbility = buildAbility2;
                    }
                    for (int k = j + 1; k < count; k++)
                    {
                        Person person3 = sortedFreePersons[k];
                        int buildAbility3 = GameUtility.Method_FarmingAbility(person1.BaseAgricultureAbility + person2.BaseAgricultureAbility + person3.BaseAgricultureAbility);
                        if (buildAbility3 < buildAbility)
                        {
                            checkPersons[0] = person1;
                            checkPersons[1] = person2;
                            checkPersons[2] = person3;
                            buildAbility = buildAbility3;
                        }
                    }
                }
            }
            value = buildAbility;
            return checkPersons;
        }

        /// <summary>
        /// 军师训练推荐
        /// </summary>
        public static Person[] CounsellorRecommendTrainTroop(List<Person> sortedFreePersons, out int value)
        {
            value = 0;
            sortedFreePersons.RemoveAll(x => x.ActionOver);
            int count = sortedFreePersons.Count;
            if (count <= 0)
                return null;

            ScenarioVariables scenarioVariables = Scenario.Cur.Variables;
            Person[] checkPersons = new Person[3];
            int buildAbility = 99999;
            for (int i = 0; i < count; i++)
            {
                Person person1 = sortedFreePersons[i];
                int buildAbility1 = (person1.BaseTrainTroopAbility);
                if (buildAbility1 < buildAbility)
                {
                    checkPersons[0] = person1;
                    checkPersons[1] = null;
                    checkPersons[2] = null;
                    buildAbility = buildAbility1;
                }
                for (int j = i + 1; j < count; j++)
                {
                    Person person2 = sortedFreePersons[j];
                    int buildAbility2 = (person1.BaseTrainTroopAbility + person2.BaseTrainTroopAbility);
                    if (buildAbility2 < buildAbility)
                    {
                        checkPersons[0] = person1;
                        checkPersons[1] = person2;
                        checkPersons[2] = null;
                        buildAbility = buildAbility2;
                    }
                    for (int k = j + 1; k < count; k++)
                    {
                        Person person3 = sortedFreePersons[k];
                        int buildAbility3 = (person1.BaseTrainTroopAbility + person2.BaseTrainTroopAbility + person3.BaseTrainTroopAbility);
                        if (buildAbility3 < buildAbility)
                        {
                            checkPersons[0] = person1;
                            checkPersons[1] = person2;
                            checkPersons[2] = person3;
                            buildAbility = buildAbility3;
                        }
                    }
                }
            }
            value = buildAbility;
            return checkPersons;
        }

        /// <summary>
        /// 军师招募士兵推荐
        /// </summary>
        public static Person[] CounsellorRecommendRecuritTroop(List<Person> sortedFreePersons, out int value)
        {
            value = 0;
            sortedFreePersons.RemoveAll(x => x.ActionOver);
            int count = sortedFreePersons.Count;
            if (count <= 0)
                return null;

            ScenarioVariables scenarioVariables = Scenario.Cur.Variables;
            Person[] checkPersons = new Person[3];
            int buildAbility = 99999;
            for (int i = 0; i < count; i++)
            {
                Person person1 = sortedFreePersons[i];
                int buildAbility1 = (person1.BaseRecruitmentAbility);
                if (buildAbility1 < buildAbility)
                {
                    checkPersons[0] = person1;
                    checkPersons[1] = null;
                    checkPersons[2] = null;
                    buildAbility = buildAbility1;
                }
                for (int j = i + 1; j < count; j++)
                {
                    Person person2 = sortedFreePersons[j];
                    int buildAbility2 = (person1.BaseRecruitmentAbility + person2.BaseRecruitmentAbility);
                    if (buildAbility2 < buildAbility)
                    {
                        checkPersons[0] = person1;
                        checkPersons[1] = person2;
                        checkPersons[2] = null;
                        buildAbility = buildAbility2;
                    }
                    for (int k = j + 1; k < count; k++)
                    {
                        Person person3 = sortedFreePersons[k];
                        int buildAbility3 = (person1.BaseRecruitmentAbility + person2.BaseRecruitmentAbility + person3.BaseRecruitmentAbility);
                        if (buildAbility3 < buildAbility)
                        {
                            checkPersons[0] = person1;
                            checkPersons[1] = person2;
                            checkPersons[2] = person3;
                            buildAbility = buildAbility3;
                        }
                    }
                }
            }
            value = buildAbility;
            return checkPersons;
        }

    }
}
