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
        /// 3人推荐代理
        /// </summary>
        /// <param name="checkValue"></param>
        /// <param name="check1"></param>
        /// <param name="check2"></param>
        /// <param name="check3"></param>
        /// <returns></returns>
        public delegate bool Recommend3PersonValue(ref int checkValue, Person check1, Person check2, Person check3);

        /// <summary>
        /// 3人推荐基础函数
        /// </summary>
        /// <param name="personList"></param>
        /// <param name="recommend3PersonValueFunc"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Person[] CounsellorRecommend3Person(List<Person> personList, Recommend3PersonValue recommend3PersonValueFunc, out int value)
        {
            value = 0;
            int count = personList.Count;
            if (count <= 0)
                return null;

            Person[] checkPersons = new Person[3];
            int maxValue = 99999;
            for (int i = 0; i < count; i++)
            {
                Person person1 = personList[i];
                if (recommend3PersonValueFunc(ref maxValue, person1, null, null))
                {
                    checkPersons[0] = person1;
                    checkPersons[1] = null;
                    checkPersons[2] = null;
                }
                for (int j = i + 1; j < count; j++)
                {
                    Person person2 = personList[j];
                    if (recommend3PersonValueFunc(ref maxValue, person1, person2, null))
                    {
                        checkPersons[0] = person1;
                        checkPersons[1] = person2;
                        checkPersons[2] = null;
                    }
                    for (int k = j + 1; k < count; k++)
                    {
                        Person person3 = personList[k];
                        if (recommend3PersonValueFunc(ref maxValue, person1, person2, person3))
                        {
                            checkPersons[0] = person1;
                            checkPersons[1] = person2;
                            checkPersons[2] = person3;
                        }
                    }
                }
            }
            value = maxValue;
            return checkPersons;
        }

        /// <summary>
        /// 军师建造推荐
        /// </summary>
        public static Person[] CounsellorRecommendBuild(List<Person> sortedFreePersons, BuildingType buildingType, out int maxBuildTurn)
        {
            return CounsellorRecommend3Person(sortedFreePersons, (ref int maxBuildTurn, Person check1, Person check2, Person check3) =>
            {
                int buildAbility = 0;
                if (check1 != null) buildAbility += check1.BaseBuildAbility;
                if (check2 != null) buildAbility += check2.BaseBuildAbility;
                if (check3 != null) buildAbility += check3.BaseBuildAbility;

                buildAbility = GameUtility.Method_BuildAbility(buildAbility);

                int turnCount = buildingType.durabilityLimit % buildAbility == 0 ? 0 : 1;
                int buildCount = Math.Min(Scenario.Cur.Variables.BuildMaxTurn, buildingType.durabilityLimit / buildAbility + turnCount);

                if( buildCount >= maxBuildTurn ) return true;

                maxBuildTurn = buildCount;
                return true;

            }, out maxBuildTurn);
        }

        /// <summary>
        /// 军师巡查推荐
        /// </summary>
        public static Person[] CounsellorRecommendInspection(List<Person> sortedFreePersons, out int value)
        {
            return CounsellorRecommend3Person(sortedFreePersons, (ref int maxValue, Person check1, Person check2, Person check3) =>
            {
                int buildAbility = 0;
                if (check1 != null) buildAbility += check1.BaseSecurityAbility;
                if (check2 != null) buildAbility += check2.BaseSecurityAbility;
                if (check3 != null) buildAbility += check3.BaseSecurityAbility;

                buildAbility = GameUtility.Method_SecurityAbility(buildAbility);

                if (buildAbility >= maxValue) return true;

                maxValue = buildAbility;
                return true;

            }, out value);
        }

        /// <summary>
        /// 军师商业推荐
        /// </summary>
        public static Person[] CounsellorRecommendDevelop(List<Person> sortedFreePersons, out int value)
        {
            return CounsellorRecommend3Person(sortedFreePersons, (ref int maxValue, Person check1, Person check2, Person check3) =>
            {
                int buildAbility = 0;
                if (check1 != null) buildAbility += check1.BaseCommerceAbility;
                if (check2 != null) buildAbility += check2.BaseCommerceAbility;
                if (check3 != null) buildAbility += check3.BaseCommerceAbility;

                buildAbility = GameUtility.Method_DevelopAbility(buildAbility);

                if (buildAbility >= maxValue) return true;

                maxValue = buildAbility;
                return true;

            }, out value);
        }


        /// <summary>
        /// 军师农业推荐
        /// </summary>
        public static Person[] CounsellorRecommendFarming(List<Person> sortedFreePersons, out int value)
        {
            return CounsellorRecommend3Person(sortedFreePersons, (ref int maxValue, Person check1, Person check2, Person check3) =>
            {
                int buildAbility = 0;
                if (check1 != null) buildAbility += check1.BaseAgricultureAbility;
                if (check2 != null) buildAbility += check2.BaseAgricultureAbility;
                if (check3 != null) buildAbility += check3.BaseAgricultureAbility;

                buildAbility = GameUtility.Method_FarmingAbility(buildAbility);

                if (buildAbility >= maxValue) return true;

                maxValue = buildAbility;
                return true;

            }, out value);
        }

        /// <summary>
        /// 军师训练推荐
        /// </summary>
        public static Person[] CounsellorRecommendTrainTroop(List<Person> sortedFreePersons, out int value)
        {
            return CounsellorRecommend3Person(sortedFreePersons, (ref int maxValue, Person check1, Person check2, Person check3) =>
            {
                int buildAbility = 0;
                if (check1 != null) buildAbility += check1.BaseTrainTroopAbility;
                if (check2 != null) buildAbility += check2.BaseTrainTroopAbility;
                if (check3 != null) buildAbility += check3.BaseTrainTroopAbility;

                buildAbility = GameUtility.Method_TrainTroop(buildAbility);

                if (buildAbility >= maxValue) return true;

                maxValue = buildAbility;
                return true;

            }, out value);
        }

        /// <summary>
        /// 军师招募士兵推荐
        /// </summary>
        public static Person[] CounsellorRecommendRecuritTroop(List<Person> sortedFreePersons, out int value)
        {
            return CounsellorRecommend3Person(sortedFreePersons, (ref int maxValue, Person check1, Person check2, Person check3) =>
            {
                int buildAbility = 0;
                if (check1 != null) buildAbility += check1.BaseRecruitmentAbility;
                if (check2 != null) buildAbility += check2.BaseRecruitmentAbility;
                if (check3 != null) buildAbility += check3.BaseRecruitmentAbility;

                if (buildAbility >= maxValue) return true;

                maxValue = buildAbility;
                return true;

            }, out value);
        }

        /// <summary>
        /// 军师招募兵装生产推荐
        /// </summary>
        public static Person[] CounsellorRecommendCreateItems(List<Person> sortedFreePersons, out int value)
        {
            return CounsellorRecommend3Person(sortedFreePersons, (ref int maxValue, Person check1, Person check2, Person check3) =>
            {
                int buildAbility = 0;
                if (check1 != null) buildAbility += check1.BaseCreativeAbility;
                if (check2 != null) buildAbility += check2.BaseCreativeAbility;
                if (check3 != null) buildAbility += check3.BaseCreativeAbility;

                if (buildAbility >= maxValue) return true;

                maxValue = buildAbility;
                return true;

            }, out value);
        }
    }
}
