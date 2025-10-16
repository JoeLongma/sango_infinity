using Sango.Tools;
using System;
using System.Collections.Generic;

namespace Sango.Game
{
    public class CityAI
    {
        static internal PriorityQueue<City> priorityQueue = new PriorityQueue<City>();

        public static bool AIAttack(City city, Scenario scenario)
        {
            if (city.BelongForce == null)
                return true;

            if (city.CurActiveTroop != null)
            {
                if (!city.CurActiveTroop.DoAI(scenario))
                    return false;

                city.CurActiveTroop = null;
                return false;
            }

            if (city.TroopMissionType != MissionType.None)
            {
                if (city.TroopMissionType == MissionType.OccupyCity)
                {
                    Troop troop = AIMakeTroop(city, 20, true, scenario);
                    if (troop != null)
                    {
                        troop = city.EnsureTroop(troop, scenario);
                        city.CurActiveTroop = troop;
#if SANGO_DEBUG
                        City targetCity = scenario.citySet.Get(troop.missionTarget);
                        Sango.Log.Print($"{scenario.GetDateStr()}{city.BelongForce.Name}3势力在{city.Name}由{troop.Leader.Name}率领军队出城 进攻{targetCity.BelongForce?.Name}的{targetCity.Name}!");
#endif
                    }
                }
                else if (city.TroopMissionType == MissionType.ProtectCity)
                {
                    if (city.CheckEnemiesIfAlive())
                    {
                        if (city.TroopsCount < Math.Max(3, city.EnemyCount + 1))
                        {
                            Troop troop = AIMakeTroop(city, 15, false, scenario);
                            if (troop != null)
                            {
                                troop = city.EnsureTroop(troop, scenario);
                                city.CurActiveTroop = troop;
#if SANGO_DEBUG
                                Sango.Log.Print($"{city.BelongForce.Name}势力在{city.Name}由{troop.Leader.Name}率领军队出城防守!");
#endif
                            }
                        }
                    }
                }

                if (city.CurActiveTroop == null)
                {
                    city.TroopMissionType = MissionType.None;
                    return true;
                }
                else
                {
                    //city.EnsureTroop(city.CurActiveTroop, scenario);
                    city.Render?.UpdateRender();
                }
                return false;
            }

            if (AICanDefense(city, scenario))
            {
                city.TroopMissionType = MissionType.ProtectCity;
                city.TroopMissionTargetId = city.Id;
                return false;
            }
            else if (AICanAttack(city, scenario))
            {
                City lastTargetCity = null;
                Troop activedTroop = scenario.troopsSet.Find(x => x.IsAlive && x.BelongCity == city);
                if (activedTroop != null)
                {
                    if (activedTroop.missionType == (int)MissionType.OccupyCity)
                    {
                        lastTargetCity = scenario.citySet.Get(activedTroop.missionTarget);
                    }
                }

                if (lastTargetCity != null)
                {
                    city.TroopMissionType = MissionType.OccupyCity;
                    city.TroopMissionTargetId = lastTargetCity.Id;
                    return false;
                }

                // 计算进攻概率
                priorityQueue.Clear();
                city.ForeachNeighborCities(x =>
                {
                    if (x.IsEnemy(city))
                    {
                        if (x.BelongForce == null)
                        {
                            priorityQueue.Push(x, 9999);
                        }
                        else
                        {
                            // 需要兵力充足
                            if (city.troops > x.troops - 5000)
                            {
                                // 范围大约在
                                int weight = (int)(500 * (float)city.virtualFightPower / (float)x.virtualFightPower);
                                int relation = scenario.GetRelation(city.BelongForce, x.BelongForce);
                                // 8000亲密 6000友好 4000普通 2000中立 0冷漠 -2000敌对 -4000厌恶 -6000仇视 -8000不死不休
                                // 5 4 3 2 1 0 -1 -2 -3 -4 -5
                                // 0 1 2 3 4 5 6 7 8 9 10
                                weight = UnityEngine.Mathf.FloorToInt((float)weight * (1f - (float)relation / 10000f));
                                priorityQueue.Push(x, weight);
                            }
                        }
                    }
                });

                int count = GameRandom.Range(0, UnityEngine.Mathf.Max(0, priorityQueue.Count) + 1);
                for (int i = 0; i < count; i++)
                {
                    int priority = 0;
                    City targetCity = priorityQueue.Higher(out priority);
                    if (targetCity != null)
                    {
                        if (GameRandom.Changce(priority, 10000))
                        {
                            city.TroopMissionType = MissionType.OccupyCity;
                            city.TroopMissionTargetId = targetCity.Id;
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 内政
        /// </summary>
        /// <param name="scenario"></param>
        public static bool AIIntrior(City city, Scenario scenario)
        {
            // 兵临城下
            if (city.IsEnemiesRound(9))
                return true;

            AIBuilding(city, scenario);
            AIIntriorBalance(city, scenario);

            if (city.wildPersons.Count > 0 && city.freePersons.Count > 0)
            {
                foreach (Person wild in city.wildPersons)
                {
                    if (wild.beFinded)
                    {
                        city.JobRecuritPerson(city.freePersons[0], wild);
                        break;
                    }
                }
            }

            AISecurity(city, scenario);
            AITrainTroop(city, scenario);

            if (city.freePersons.Count > 3)
            {
                city.JobSearching(city.freePersons.GetRange(0, GameRandom.Range(city.freePersons.Count / 2)).ToArray());
            }
            return true;
        }

        public static bool AITransfrom(City city, Scenario scenario)
        {
            return true;
        }

        public static bool AIBuilding(City city, Scenario scenario)
        {
            AIBuldIntriore(city, scenario);
            return true;
        }

        static int[][] CityBuildingTemplate = new int[][] {
            // 后方城市
            new int[] {
                (int)BuildingKindType.Farm,
                (int)BuildingKindType.Market,
                (int)BuildingKindType.Barracks,
                (int)BuildingKindType.BlacksmithShop,
                (int)BuildingKindType.Farm,
                (int)BuildingKindType.Market,
                (int)BuildingKindType.Farm,
                (int)BuildingKindType.Market,
                (int)BuildingKindType.PatrolBureau,
                (int)BuildingKindType.Farm,
                (int)BuildingKindType.Stable,
                (int)BuildingKindType.MechineFactory,// 12小城

                (int)BuildingKindType.Farm,
                (int)BuildingKindType.Market,
                (int)BuildingKindType.BoatFactory,
                (int)BuildingKindType.CustomKind,// 16中城


                (int)BuildingKindType.Farm,
                (int)BuildingKindType.Market,
                (int)BuildingKindType.CustomKind,
                (int)BuildingKindType.Market,// 20大城

                (int)BuildingKindType.Farm,
                (int)BuildingKindType.Market,
                (int)BuildingKindType.Farm,
                (int)BuildingKindType.Market,// 24巨城
            },

            // 边境城市
            new int[] {
                (int)BuildingKindType.Barracks,
                (int)BuildingKindType.BlacksmithShop,
                (int)BuildingKindType.Market,
                (int)BuildingKindType.Market,
                (int)BuildingKindType.Farm,
                (int)BuildingKindType.Market,
                (int)BuildingKindType.Farm,
                (int)BuildingKindType.Farm,
                (int)BuildingKindType.PatrolBureau,
                (int)BuildingKindType.Stable,
                (int)BuildingKindType.MechineFactory,
                (int)BuildingKindType.BoatFactory,// 12小城

                (int)BuildingKindType.Farm,
                (int)BuildingKindType.Market,
                (int)BuildingKindType.Barracks,
                (int)BuildingKindType.BlacksmithShop,// 16中城


                (int)BuildingKindType.Farm,
                (int)BuildingKindType.Market,
                (int)BuildingKindType.Farm,
                (int)BuildingKindType.Market,// 20大城

                (int)BuildingKindType.Market,
                (int)BuildingKindType.Farm,
                (int)BuildingKindType.Market,
                (int)BuildingKindType.Stable,// 24巨城
            },
        };

        public static bool AIBuldIntriore(City city, Scenario scenario)
        {
            if (city.allIntriorBuildings.Count >= city.InsideSlot)
                return true;

            if (city.freePersons.Count <= 0)
                return true;

            if (city.IsBorderCity)
            {
                // 优先建造军事
                return AIBuildingTemplate(1, city, scenario);
            }
            else
            {
                return AIBuildingTemplate(0, city, scenario);

            }
        }

        // 进是确定有空槽位和空闲武将才能执行
        public static bool AIBuildingTemplate(int templateId, City city, Scenario scenario)
        {
            Dictionary<int, int> buildingCountMap = new Dictionary<int, int>();
            foreach (int buildingTypeId in city.buildingCountMap.Keys)
                buildingCountMap[buildingTypeId] = city.buildingCountMap[buildingTypeId];

            int[] building_list = CityBuildingTemplate[templateId];
            int[] buildingFlag = new int[building_list.Length];

            // 先排除确定的建筑
            for (int i = 0; i < city.InsideSlot; i++)
            {
                int buildTypeId = building_list[i];
                if (buildTypeId > 0)
                {
                    if (buildingCountMap.TryGetValue(buildTypeId, out int num))
                    {
                        if (num > 0)
                        {
                            buildingCountMap[buildTypeId] = num - 1;
                            buildingFlag[i] = buildTypeId;
                        }
                    }
                }
            }

            // 再排除不确定的建筑
            for (int i = 0; i < city.InsideSlot; i++)
            {
                int buildTypeId = building_list[i];
                if (buildTypeId == 0)
                {
                    bool findAny = false;
                    foreach (int buildingTypeId in buildingCountMap.Keys)
                    {
                        int num = buildingCountMap[buildingTypeId];
                        if (num > 0)
                        {
                            buildingCountMap[buildingTypeId] = num - 1;
                            buildingFlag[i] = buildingTypeId;
                            findAny = true;
                            break;
                        }
                    }
                    if (findAny)
                        continue;
                }
            }

            // 修建未入坑的
            for (int i = 0; i < city.InsideSlot; i++)
            {
                int exsistId = buildingFlag[i];
                if (exsistId > 0)
                    continue;

                int buildTypeId = building_list[i];
                BuildingType buildingType = scenario.GetObject<BuildingType>(buildTypeId);
                if (buildingType == null || buildingType.Id == 0)
                {
                    buildingType = scenario.GetObject<BuildingType>(GameRandom.Range((int)BuildingKindType.Market, (int)BuildingKindType.PatrolBureau));
                }

                if (city.gold < buildingType.cost + 200)
                    return true;

                int index = city.GetEmptySlot();
                if (index < 0)
                    return true;

                Person[] people = ForceAI.CounsellorRecommendBuild(city.freePersons, buildingType);
                if (people != null)
                {
                    int buildAbility = 0;
                    for (int k = 0; k < people.Length; k++)
                    {
                        Person person = people[k];
                        if (person == null) continue;
                        buildAbility += person.BaseBuildAbility;
                    }

                    buildAbility = GameUtility.Method_BuildAbility(buildAbility);
                    int turnCount = buildingType.durabilityLimit % buildAbility == 0 ? 0 : 1;
                    int buildCount = Math.Min(Scenario.Cur.Variables.BuildMaxTurn, buildingType.durabilityLimit / buildAbility + turnCount);

                    if (buildCount <= 6)
                    {
                        city.BuildBuilding(index, people, buildingType);
                    }
                    return true;
                }
            }

            return true;
        }

        public static bool AIRecuritTroop(City city, Scenario scenario)
        {
            if (city.freePersons.Count <= 2) return true;

            int expectationTroops = (city.totalGainFood * 3);
            if (city.troops >= expectationTroops)
                return true;

            int barracksNum = city.GetIntriorBuildingComplateNumber((int)BuildingKindType.Barracks);
            if (barracksNum <= 0) return true;

            Person[] people = ForceAI.CounsellorRecommendRecuritTroop(city.freePersons);
            if (people == null) return true;

            if (city.JobRecuritTroop(people, barracksNum))
            {

            }
            return true;
        }

        //public void AITroopLevelUp(Scenario scenario)
        //{

        //}

        //public void AITroopMerge(Scenario scenario)
        //{

        //}
        //public void AITroopTrain(Scenario scenario)
        //{

        //}

        public static bool AICanDefense(City city, Scenario scenario)
        {
            if (city.IsRoadBlocked())
                return false;

            City.EnemyInfo enemyInfo;
            // 兵临城下且敌军存活
            if (city.IsEnemiesRound(6) && city.CheckEnemiesIfAlive(out enemyInfo))
                return true;

            return false;
        }

        public static bool AICanAttack(City city, Scenario scenario)
        {
            if (city.troops < 20000)
                return false;

            if (city.morale < 80 || city.security < 80)
                return false;

            if (city.freePersons.Count < 6)
                return false;

            if (city.durability < city.DurabilityLimit)
                return false;

            if (!city.IsBorderCity)
                return false;

            if (city.IsEnemiesRound())
                return false;

            // 兵装检查
            if (city.itemStore.TotalNumber < Math.Min(60000, city.troops * 3 / 2))
                return false;

            int needFood = (int)(120000 * scenario.Variables.baseFoodCostInCity + 20 * (city.troops - 20000) * scenario.Variables.baseFoodCostInTroop);
            // 粮食检查
            if (city.food <= needFood)
                return false;

            List<City> enemiesCities = new List<City>();
            city.ForeachNeighborCities(x =>
            {
                if (x.IsEnemy(city))
                    enemiesCities.Add(x);
            });

            if (enemiesCities.Count == 0)
                return false;

            if (GameRandom.Changce(90))
                return false;

            return true;
        }

        public static bool AIIntriorBalance(City city, Scenario scenario)
        {
            if (city.freePersons.Count <= 1) return true;

            if (city.commerce >= city.agriculture)
            {
                Person[] people = ForceAI.CounsellorRecommendFarming(city.freePersons);
                if (people == null) return true;
                if (city.JobFarming(people))
                {

                }
            }
            else
            {
                Person[] people = ForceAI.CounsellorRecommendDevelop(city.freePersons);
                if (people == null) return true;
                if (city.JobDevelop(people))
                {
                }
            }
            return true;
        }

        public static bool AISecurity(City city, Scenario scenario)
        {
            if (city.freePersons.Count < 2 || city.gold < 400)
                return true;

            int barracksNum = city.GetIntriorBuildingComplateNumber((int)BuildingKindType.PatrolBureau);
            if (barracksNum <= 0) return true;

            if (GameRandom.Changce((90 - city.security) * 3 / 2))
            {
                Person[] people = ForceAI.CounsellorRecommendDevelop(city.freePersons);
                if (people == null) return true;
                if (city.JobInspection(people))
                {
                }
            }
            return true;
        }

        public static bool AITrainTroop(City city, Scenario scenario)
        {
            if (city.freePersons.Count < 2)
                return true;

            int barracksNum = city.GetIntriorBuildingComplateNumber((int)BuildingKindType.PatrolBureau);
            if (barracksNum <= 0) return true;

            if (city.morale < 50)
            {
                Person[] people = ForceAI.CounsellorRecommendTrainTroop(city.freePersons);
                if (people == null) return true;
                if (city.JobTrainTroop(people))
                {
                }
            }
            else
            {
                if (GameRandom.Changce((95 - city.morale) * 3 / 2))
                {
                    Person[] people = ForceAI.CounsellorRecommendTrainTroop(city.freePersons);
                    if (people == null) return true;
                    if (city.JobTrainTroop(people))
                    {
                    }
                }
            }
            return true;
        }

        public static bool AICreateItems(City city, Scenario scenario)
        {
            if (city.freePersons.Count < 2 || city.gold < 1000)
                return true;

            if (city.itemStore.TotalNumber >= city.StoreLimit - 1000) return true;

            int BlacksmithShopnum = city.GetIntriorBuildingComplateNumber((int)BuildingKindType.BlacksmithShop);
            int StableNum = city.GetIntriorBuildingComplateNumber((int)BuildingKindType.Stable);
            if (BlacksmithShopnum <= 0 && StableNum <= 0)
                return true;

            int totalNum = 0;
            for (int itemTypeId = 2; itemTypeId <= 5; itemTypeId++)
                // 获取总兵装
                totalNum += city.itemStore.GetNumber(itemTypeId);

            if (totalNum > city.troops * 2)
                return true;

            // 统计适应偏向
            int[] levelTotal = new int[4] { 1, 1, 1, 1 };
            city.allPersons.ForEach(x =>
            {
                levelTotal[0] += x.SpearLv;
                levelTotal[1] += x.HalberdLv;
                levelTotal[2] += x.CrossbowLv;
                levelTotal[3] += x.HorseLv;
            });

            int sumTotal = 0;
            if (BlacksmithShopnum > 0)
                sumTotal = sumTotal + levelTotal[0] + levelTotal[1] + levelTotal[2];

            if (StableNum > 0)
                sumTotal = sumTotal + levelTotal[3];

            for (int itemTypeId = 2; itemTypeId <= 5; itemTypeId++)
            {
                if (itemTypeId == 5 && StableNum <= 0)
                    continue;

                if (itemTypeId < 5 && BlacksmithShopnum <= 0)
                    continue;

                int itemNum = city.itemStore.GetNumber(itemTypeId);
                if (itemNum < levelTotal[itemTypeId - 2] * city.troops / sumTotal + 5000)
                {
                    Person[] people = ForceAI.CounsellorRecommendCreateItems(city.freePersons);
                    if (people == null) return true;
                    ItemType itemType = Scenario.Cur.GetObject<ItemType>(itemTypeId);
                    if (city.JobCreateItems(people, itemType, itemTypeId == 5 ? StableNum : BlacksmithShopnum))
                    {

                    }
                    return true;
                }
            }

            return true;
        }

        public static Troop AIMakeTroop(City city, int minTurn, bool isAttack, Scenario scenario)
        {
            if (isAttack)
            {
                if (city.freePersons.Count < 3) return null;
                if (city.troops < 20000) return null;
                if (city.food < 5000) return null;
            }
            else
            {
                if (city.troops < 4000) return null;
                if (city.food < 500) return null;
            }

            // 所有满足5000兵的部队类型
            List<TroopType> costEnoughTroopTypes = new List<TroopType>();
            TroopType.GetCostEnoughTroopTypeList(city, costEnoughTroopTypes, 5000);

            // 去掉剑兵(进攻不允许)
            costEnoughTroopTypes.RemoveAll(x => x.Id == 1);

            if (costEnoughTroopTypes.Count == 0)
                return null;

            int maxPersonCount = city.freePersons.Count > 13 ? 3 : (city.freePersons.Count < 6 ? 1 : 2);

            // 优先组建特殊
            TroopType spType = costEnoughTroopTypes.Find(x => x.validItemId > 0);

            // 尝试移除不够组建的特殊兵种
            while (spType != null && !spType.CheckCost(city, 5000))
            {
                costEnoughTroopTypes.Remove(spType);
                spType = costEnoughTroopTypes.Find(x => x.validItemId > 0);
            }

            if (spType == null)
                spType = costEnoughTroopTypes[GameRandom.Range(0, costEnoughTroopTypes.Count)];

            Person[] people = ForceAI.CounsellorRecommendMakeTroop(city.freePersons, spType, maxPersonCount);

            // 确定兵数
            int maxTroopNum = people[0].TroopsLimit;
            if (city.troops < maxTroopNum * 2)
                maxTroopNum = (city.troops / 2000) * 1000;

            maxTroopNum = city.itemStore.CheckCostMin(spType.costItems, maxTroopNum);

            // 粮食
            int turnCostFood = (int)(maxTroopNum * scenario.Variables.baseFoodCostInTroop);
            int food = turnCostFood * minTurn;
            while (city.food < food)
            {
                if (isAttack) return null;

                minTurn = minTurn - 4;
                if (minTurn < 5)
                    return null;

                food = turnCostFood * minTurn;
            }

            spType.Cost(city, maxTroopNum);
            city.troops -= maxTroopNum;
            city.food -= food;
            for (int p = 0; p < people.Length; p++)
                city.freePersons.Remove(people[p]);

            Troop troop = new Troop()
            {
                energy = city.energy,
                morale = city.morale,
                Leader = people[0],
                TroopType = spType,
                troops = people[0].TroopsLimit,
                food = food,
                missionType = (int)city.TroopMissionType,
                missionTarget = city.TroopMissionTargetId,
            };

            if (people.Length > 1) troop.Member1 = people[1];
            if (people.Length > 2) troop.Member1 = people[2];
            city.Render?.UpdateRender();
            return troop;
        }

    }


}
