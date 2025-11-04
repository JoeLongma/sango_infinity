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
                                if (GameRandom.Chance(enemysenemy_relation, 10000))
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
        public delegate bool Recommend3PersonValue(ref int[] checkValue, Person check1, Person check2, Person check3);
        public delegate bool Recommend2PersonValue(ref int[] checkValue, Person check1, Person check2);
        public delegate bool Recommend1PersonValue(ref int[] checkValue, Person check1);

        /// <summary>
        /// 1-3人遍历最优推荐基础函数(效率慢,最优解,尽量不用在人数>20的城市)
        /// </summary>
        /// <param name="personList"></param>
        /// <param name="recommend3PersonValueFunc"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static Person[] CounsellorRecommend3Person(List<Person> personList, Recommend3PersonValue recommend3PersonValueFunc)
        {
            int count = personList.Count;
            if (count <= 0)
                return null;

            if (count >= 20)
                return CounsellorFastRecommend3Person(personList, recommend3PersonValueFunc);

            Person[] checkPersons = new Person[3];
            int[] maxValue = new int[] { -99999, 99999 };
            bool hasValue = false;
            for (int i = 0; i < count; i++)
            {
                Person person1 = personList[i];
                if (recommend3PersonValueFunc(ref maxValue, person1, null, null))
                {
                    checkPersons[0] = person1;
                    checkPersons[1] = null;
                    checkPersons[2] = null;
                    hasValue = true;
                }
                for (int j = i + 1; j < count; j++)
                {
                    Person person2 = personList[j];
                    if (recommend3PersonValueFunc(ref maxValue, person1, person2, null))
                    {
                        checkPersons[0] = person1;
                        checkPersons[1] = person2;
                        checkPersons[2] = null;
                        hasValue = true;
                    }
                    for (int k = j + 1; k < count; k++)
                    {
                        Person person3 = personList[k];
                        if (recommend3PersonValueFunc(ref maxValue, person1, person2, person3))
                        {
                            checkPersons[0] = person1;
                            checkPersons[1] = person2;
                            checkPersons[2] = person3;
                            hasValue = true;
                        }
                    }
                }
            }

            if (!hasValue)
                return null;

            return checkPersons;
        }

        static Person[] CounsellorRecommend2Person(List<Person> personList, Recommend2PersonValue recommend2PersonValueFunc)
        {
            int count = personList.Count;
            if (count <= 0)
                return null;

            if (count >= 20)
                return CounsellorFastRecommend2Person(personList, recommend2PersonValueFunc);

            Person[] checkPersons = new Person[3];
            int[] maxValue = new int[] { -99999, 99999 };
            bool hasValue = false;
            for (int i = 0; i < count; i++)
            {
                Person person1 = personList[i];
                if (recommend2PersonValueFunc(ref maxValue, person1, null))
                {
                    checkPersons[0] = person1;
                    checkPersons[1] = null;
                    hasValue = true;
                }
                for (int j = i + 1; j < count; j++)
                {
                    Person person2 = personList[j];
                    if (recommend2PersonValueFunc(ref maxValue, person1, person2))
                    {
                        checkPersons[0] = person1;
                        checkPersons[1] = person2;
                        hasValue = true;
                    }
                }
            }

            if (!hasValue)
                return null;

            return checkPersons;
        }

        static Person[] CounsellorRecommend1Person(List<Person> personList, Recommend1PersonValue recommend1PersonValueFunc)
        {
            int count = personList.Count;
            if (count <= 0)
                return null;

            if (count >= 20)
                return CounsellorFastRecommend1Person(personList, recommend1PersonValueFunc);

            Person[] checkPersons = new Person[3];
            int[] maxValue = new int[] { -99999, 99999 };
            bool hasValue = false;
            for (int i = 0; i < count; i++)
            {
                Person person1 = personList[i];
                if (recommend1PersonValueFunc(ref maxValue, person1))
                {
                    checkPersons[0] = person1;
                    hasValue = true;
                }
            }

            if (!hasValue)
                return null;

            return checkPersons;
        }

        /// <summary>
        /// 3人快速推荐, 不考虑溢出, 先找到第一个最高武将, 第N个武将第N高或者能改变执行值
        /// </summary>
        /// <param name="personList"></param>
        /// <param name="recommend3PersonValueFunc"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static Person[] CounsellorFastRecommend3Person(List<Person> personList, Recommend3PersonValue fastRecommend3PersonValueFunc)
        {
            int count = personList.Count;
            if (count <= 0)
                return null;

            if (personList.Count <= 3)
                return personList.ToArray();

            Person[] checkPersons = new Person[3];
            int[] maxValues = new int[] { -99999, 99999 };
            Person person1 = null, person2 = null;
            // 第一位找目标属性最大
            for (int i = 0; i < count; i++)
            {
                Person person = personList[i];
                if (fastRecommend3PersonValueFunc(ref maxValues, person, null, null))
                {
                    person1 = person;
                    checkPersons[0] = person;
                }
            }

            maxValues[0] = -99999;
            maxValues[1] = 99999;
            for (int i = 0; i < count; i++)
            {
                Person person = personList[i];
                if (person != person1 && fastRecommend3PersonValueFunc(ref maxValues, person1, person, null))
                {
                    person2 = person;
                    checkPersons[1] = person;
                }
            }

            maxValues[0] = -99999;
            maxValues[1] = 99999;
            for (int i = 0; i < count; i++)
            {
                Person person = personList[i];
                if (person != person1 && person != person2 && fastRecommend3PersonValueFunc(ref maxValues, person1, person2, person))
                {
                    checkPersons[2] = person;
                }
            }

            return checkPersons;
        }
        static Person[] CounsellorFastRecommend2Person(List<Person> personList, Recommend2PersonValue fastRecommend2PersonValueFunc)
        {
            int count = personList.Count;
            if (count <= 0)
                return null;

            if (personList.Count <= 2)
                return personList.ToArray();

            Person[] checkPersons = new Person[3];
            int[] maxValues = new int[] { -99999, 99999 };
            Person person1 = null;
            // 第一位找目标属性最大
            for (int i = 0; i < count; i++)
            {
                Person person = personList[i];
                if (fastRecommend2PersonValueFunc(ref maxValues, person, null))
                {
                    person1 = person;
                    checkPersons[0] = person;
                }
            }

            maxValues[0] = -99999;
            maxValues[1] = 99999;
            for (int i = 0; i < count; i++)
            {
                Person person = personList[i];
                if (person != person1 && fastRecommend2PersonValueFunc(ref maxValues, person1, person))
                {
                    checkPersons[1] = person;
                }
            }
            return checkPersons;
        }
        static Person[] CounsellorFastRecommend1Person(List<Person> personList, Recommend1PersonValue fastRecommend1PersonValueFunc)
        {
            int count = personList.Count;
            if (count <= 0)
                return null;

            if (personList.Count <= 1)
                return personList.ToArray();

            Person[] checkPersons = new Person[3];
            int[] maxValues = new int[] { -99999, 99999 };
            // 第一位找目标属性最大
            for (int i = 0; i < count; i++)
            {
                Person person = personList[i];
                if (fastRecommend1PersonValueFunc(ref maxValues, person))
                {
                    checkPersons[0] = person;
                }
            }
            return checkPersons;
        }


        /// <summary>
        /// 军师建造推荐
        /// </summary>
        public static Person[] CounsellorRecommendBuild(List<Person> personList, BuildingType buildingType)
        {
            return CounsellorRecommend3Person(personList, (ref int[] maxBuildTurn, Person check1, Person check2, Person check3) =>
            {
                int buildAbility = GameUtility.Method_PersonBuildAbility(check1, check2, check3);
                int turnCount = buildingType.durabilityLimit % buildAbility == 0 ? 0 : 1;
                int buildCount = Math.Min(Scenario.Cur.Variables.BuildMaxTurn, buildingType.durabilityLimit / buildAbility + turnCount);

                if (buildCount < maxBuildTurn[1])
                {
                    maxBuildTurn[1] = buildCount;
                    return true;
                }

                return false;

            });
        }

        /// <summary>
        /// 军师巡查推荐
        /// </summary>
        public static Person[] CounsellorRecommendInspection(List<Person> personList)
        {
            return CounsellorRecommend3Person(personList, (ref int[] maxValue, Person check1, Person check2, Person check3) =>
            {
                int buildAbility = 0;
                if (check1 != null) buildAbility += check1.BaseSecurityAbility;
                if (check2 != null) buildAbility += check2.BaseSecurityAbility;
                if (check3 != null) buildAbility += check3.BaseSecurityAbility;

                buildAbility = GameUtility.Method_SecurityAbility(buildAbility, 1);

                if (buildAbility > maxValue[0])
                {
                    maxValue[0] = buildAbility;
                    return true;
                }

                return false;
            });
        }

        /// <summary>
        /// 军师商业推荐
        /// </summary>
        public static Person[] CounsellorRecommendDevelop(List<Person> personList)
        {
            return CounsellorRecommend3Person(personList, (ref int[] maxValue, Person check1, Person check2, Person check3) =>
            {
                int buildAbility = 0;
                if (check1 != null) buildAbility += check1.BaseCommerceAbility;
                if (check2 != null) buildAbility += check2.BaseCommerceAbility;
                if (check3 != null) buildAbility += check3.BaseCommerceAbility;

                buildAbility = GameUtility.Method_DevelopAbility(buildAbility);

                if (buildAbility > maxValue[0])
                {
                    maxValue[0] = buildAbility;
                    return true;
                }
                return false;

            });
        }


        /// <summary>
        /// 军师农业推荐
        /// </summary>
        public static Person[] CounsellorRecommendFarming(List<Person> personList)
        {
            return CounsellorRecommend3Person(personList, (ref int[] maxValue, Person check1, Person check2, Person check3) =>
            {
                int buildAbility = 0;
                if (check1 != null) buildAbility += check1.BaseAgricultureAbility;
                if (check2 != null) buildAbility += check2.BaseAgricultureAbility;
                if (check3 != null) buildAbility += check3.BaseAgricultureAbility;

                buildAbility = GameUtility.Method_FarmingAbility(buildAbility);

                if (buildAbility > maxValue[0])
                {
                    maxValue[0] = buildAbility;
                    return true;
                }
                return false;

            });
        }

        /// <summary>
        /// 军师训练推荐
        /// </summary>
        public static Person[] CounsellorRecommendTrainTroop(List<Person> personList)
        {
            return CounsellorRecommend3Person(personList, (ref int[] maxValue, Person check1, Person check2, Person check3) =>
            {
                int buildAbility = 0;
                if (check1 != null) buildAbility += check1.BaseTrainTroopAbility;
                if (check2 != null) buildAbility += check2.BaseTrainTroopAbility;
                if (check3 != null) buildAbility += check3.BaseTrainTroopAbility;

                buildAbility = GameUtility.Method_TrainTroop(buildAbility, 1);

                if (buildAbility > maxValue[0])
                {
                    maxValue[0] = buildAbility;
                    return true;
                }
                return false;

            });
        }

        /// <summary>
        /// 军师招募士兵推荐
        /// </summary>
        public static Person[] CounsellorRecommendRecuritTroop(List<Person> personList)
        {
            return CounsellorRecommend3Person(personList, (ref int[] maxValue, Person check1, Person check2, Person check3) =>
            {
                int buildAbility = 0;
                if (check1 != null) buildAbility += check1.BaseRecruitmentAbility;
                if (check2 != null) buildAbility += check2.BaseRecruitmentAbility;
                if (check3 != null) buildAbility += check3.BaseRecruitmentAbility;

                if (buildAbility > maxValue[0])
                {
                    maxValue[0] = buildAbility;
                    return true;
                }
                return false;

            });
        }

        /// <summary>
        /// 军师招募兵装生产推荐
        /// </summary>
        public static Person[] CounsellorRecommendCreateItems(List<Person> personList)
        {
            return CounsellorRecommend3Person(personList, (ref int[] maxValue, Person check1, Person check2, Person check3) =>
            {
                int buildAbility = 0;
                if (check1 != null) buildAbility += check1.BaseCreativeAbility;
                if (check2 != null) buildAbility += check2.BaseCreativeAbility;
                if (check3 != null) buildAbility += check3.BaseCreativeAbility;

                if (buildAbility > maxValue[0])
                {
                    maxValue[0] = buildAbility;
                    return true;
                }
                return false;

            });
        }

        /// <summary>
        /// 军师部队推荐指定部队
        /// </summary>
        public static Person[] CounsellorRecommendMakeTroop(List<Person> personList, TroopType troopType, int maxPersonLimit = 3)
        {
            int count = personList.Count;
            if (count <= 0)
                return null;

            if (personList.Count <= 1)
                return personList.ToArray();

            Person[] checkPersons = new Person[3];
            Person person1 = null;

            //TODO: 优先内置推荐队伍
            ///
            int level = 3;
            int checkValue = 0;
            if (troopType.validItemId > 0)
            {
                // 优先兵符携带者为主将
                for (int i = 0; i < count; i++)
                {
                    Person person = personList[i];
                    if (person.HasItem(troopType.validItemId))
                    {
                        checkPersons[0] = person;
                        person1 = person;
                        break;
                    }
                }
            }
            else
            {
                while(person1 == null)
                {
                    // 优先战斗力最高为主将
                    for (int i = 0; i < count; i++)
                    {
                        Person person = personList[i];
                        int militaryAbility = person.MilitaryAbility;
                        int pLevel = Troop.CheckTroopTypeLevel(troopType, person);
                        if (pLevel == level && militaryAbility > checkValue)
                        {
                            checkValue = militaryAbility;
                            checkPersons[0] = person;
                            person1 = person;
                        }
                    }
                    level--;
                }
            }

            if (maxPersonLimit == 1)
                return checkPersons;

            // 补充适应,且战斗力最低
            int v_int = person1.Intelligence;
            int v_stength = person1.Strength; ;
            int v_command = person1.Command; ;
            int destSlot = 1;
            if (level < 3)
            {
                for (int i = 0; i < count; i++)
                {
                    Person person = personList[i];
                    if (person != person1)
                    {
                        int checkLvl = Troop.CheckTroopTypeLevel(troopType, person);
                        if (checkLvl > level)
                        {
                            if (person.MilitaryAbility < checkValue)
                            {
                                checkValue = person.MilitaryAbility;
                                level = checkLvl;
                                checkPersons[destSlot] = person;
                                v_int = Math.Max(person1.Intelligence, person.Intelligence);
                                v_stength = Math.Max(person1.Strength, person.Strength);
                                v_command = Math.Max(person1.Command, person.Command);
                                destSlot++;
                            }
                        }
                    }
                }
            }

            for (int k = 0; k < 2; k++)
            {
                // 不需要补充适应,尝试补充战斗力
                if (destSlot == k + 1)
                {
                    if (v_stength < 60)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Person person = personList[i];
                            if (person != person1)
                            {
                                int checkLvl = Troop.CheckTroopTypeLevel(troopType, person);
                                if (person.Strength >= 70 && (checkLvl < level && level >= 3 || checkLvl <= level && level < 3))
                                {
                                    checkPersons[destSlot] = person;
                                    destSlot++;
                                    v_int = Math.Max(v_int, person.Intelligence);
                                    v_stength = Math.Max(v_stength, person.Strength);
                                    v_command = Math.Max(v_command, person.Command);
                                    break;
                                }
                            }
                        }
                    }
                    else if (v_command < 60)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Person person = personList[i];
                            if (person != person1)
                            {
                                int checkLvl = Troop.CheckTroopTypeLevel(troopType, person);
                                if (person.Command >= 70 && (checkLvl < level && level >= 3 || checkLvl <= level && level < 3))
                                {
                                    checkPersons[destSlot] = person;
                                    destSlot++;
                                    v_int = Math.Max(v_int, person.Intelligence);
                                    v_stength = Math.Max(v_stength, person.Strength);
                                    v_command = Math.Max(v_command, person.Command);
                                    break;
                                }
                            }
                        }
                    }
                }

                if (destSlot == k + 1)
                {
                    if (person1.Intelligence < 70)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Person person = personList[i];
                            if (person != person1)
                            {
                                int checkLvl = Troop.CheckTroopTypeLevel(troopType, person);
                                if (person.Intelligence >= 80 && (checkLvl < level && level >= 3 || checkLvl <= level && level < 3))
                                {
                                    checkPersons[destSlot] = person;
                                    v_int = Math.Max(v_int, person.Intelligence);
                                    v_stength = Math.Max(v_stength, person.Strength);
                                    v_command = Math.Max(v_command, person.Command);
                                    destSlot++;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (destSlot == k + 2 && maxPersonLimit == k + 2)
                    return checkPersons;

            }

            return checkPersons;
        }

        /// <summary>
        /// 军师运输队伍推荐
        /// </summary>
        public static Person[] CounsellorRecommendTransportTroop(List<Person> personList)
        {
            return CounsellorRecommend1Person(personList, (ref int[] maxValue, Person check1) =>
            {
                int buildAbility = check1.MilitaryAbility;
                if (buildAbility < maxValue[1])
                {
                    maxValue[1] = buildAbility;
                    return true;
                }
                return false;
            });
        }

        /// <summary>
        /// 军师交易推荐
        /// </summary>
        public static Person[] CounsellorRecommendTrade(List<Person> personList)
        {
            return CounsellorRecommend1Person(personList, (ref int[] maxValue, Person check1) =>
            {
                int buildAbility = check1.Politics;
                if (buildAbility > maxValue[0])
                {
                    maxValue[0] = buildAbility;
                    return true;
                }
                return false;
            });
        }
    }
}
