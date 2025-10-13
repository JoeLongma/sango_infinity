using Sango.Tools;
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

            if (city.CurActiveTroopList.Count > 0)
            {
                Troop troop = city.CurActiveTroopList[0];
                if (!troop.DoAI(scenario))
                    return false;
                city.CurActiveTroopList.RemoveAt(0);
                if (city.CurActiveTroopList.Count > 0)
                {
                    troop = city.CurActiveTroopList[0];
                    troop = city.EnsureTroop(troop, scenario, 20);
#if SANGO_DEBUG
                    City targetCity = scenario.citySet.Get(troop.missionTarget);
                    Sango.Log.Print($"{scenario.GetDateStr()}{city.BelongForce.Name}3势力在{city.Name}由{troop.Leader.Name}率领军队出城 进攻{targetCity.BelongForce?.Name}的{targetCity.Name}!");
#endif
                }
                return false;
            }

            if (AICanDefense(city, scenario))
            {
                city.troopTempList.Clear();
                city.AutoMakeTroop(city.troopTempList, 5, false);
                if (city.troopTempList.Count <= 0) return true;

                Troop troop = city.troopTempList[0];
                city.troopTempList.RemoveAt(0);
                troop = city.EnsureTroop(troop, scenario, 10);
                troop.missionType = (int)MissionType.ProtectCity;
                troop.missionTarget = city.Id;
#if SANGO_DEBUG
                Sango.Log.Print($"{city.BelongForce.Name}势力在{city.Name}由{troop.Leader.Name}率领军队出城防守!");
#endif
                city.CurActiveTroop = troop;
                city.Render?.UpdateRender();
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
                    if (city.troops < UnityEngine.Mathf.Min(lastTargetCity.troops, lastTargetCity.allPersons.Count * 5000))
                        return true;

                    city.AutoMakeTroop(city.CurActiveTroopList, 10, false);
                    if (city.CurActiveTroopList.Count > 0)
                    {
                        for (int i = 0; i < city.CurActiveTroopList.Count; i++)
                        {
                            Troop troop = city.CurActiveTroopList[i];
                            troop.missionType = (int)MissionType.OccupyCity;
                            troop.missionTarget = lastTargetCity.Id;
                        }

                        Troop troop1 = city.CurActiveTroopList[0];
                        troop1 = city.EnsureTroop(troop1, scenario, 20);
#if SANGO_DEBUG
                        Sango.Log.Print($"{scenario.GetDateStr()}{city.BelongForce.Name}2势力在{city.Name}由{troop1.Leader.Name}率领军队出城 进攻{lastTargetCity.BelongForce?.Name}的{lastTargetCity.Name}!");
#endif
                    }
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
                            if (city.troops > UnityEngine.Mathf.Min(x.troops, x.allPersons.Count * 5000) - 10000)
                            {
                                // 范围大约在
                                int weight = (int)(1000 * (float)city.virtualFightPower / (float)x.virtualFightPower);
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
                            if (city.troops < UnityEngine.Mathf.Min(targetCity.troops, targetCity.allPersons.Count * 5000))
                                continue;

                            city.AutoMakeTroop(city.CurActiveTroopList, 10, false);
                            if (city.CurActiveTroopList.Count > 0)
                            {
                                for (int j = 0; j < city.CurActiveTroopList.Count; j++)
                                {
                                    Troop troop = city.CurActiveTroopList[j];
                                    troop.missionType = (int)MissionType.OccupyCity;
                                    troop.missionTarget = targetCity.Id;
                                }

                                Troop troop1 = city.CurActiveTroopList[0];
                                troop1 = city.EnsureTroop(troop1, scenario, 20);
#if SANGO_DEBUG
                                Sango.Log.Print($"{scenario.GetDateStr()}{city.BelongForce.Name}1势力在{city.Name}由{troop1.Leader.Name}率领军队出城 进攻{targetCity.BelongForce?.Name}的{targetCity.Name}!");
#endif
                            }
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

            AITrainTroop(city, scenario);
            AISecurity(city, scenario);

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

                int buildTurn;
                Person[] people = ForceAI.CounsellorRecommendBuild(city.freePersons, buildingType, out buildTurn);
                if (people != null && buildTurn <= 6)
                {
                    city.BuildBuilding(index, people, buildingType);
                }
            }

            return true;
        }

        /// <summary>
        /// 军事
        /// </summary>
        /// <param name="scenario"></param>
        public static bool AITroop(City city, Scenario scenario)
        {
            AIRecuritTroop(city, scenario);
            //AITroopCreate(scenario);
            //AITroopLevelUp(scenario);
            //AITroopMerge(scenario);
            //AITroopTrain(scenario);
            return true;
        }

        public static bool AIRecuritTroop(City city, Scenario scenario)
        {
            if (city.freePersons.Count <= 2) return true;

            int expectationTroops = (int)(city.totalGainFood / (9 * scenario.Variables.baseFoodCostInCity)) + city.food;
            if (city.troops >= expectationTroops)
                return true;

            int barracksNum = city.GetIntriorBuildingComplateNumber((int)BuildingKindType.Barracks);
            if (barracksNum <= 0) return true;

            Person[] people = ForceAI.CounsellorRecommendRecuritTroop(city.freePersons, out int totalValue);
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
            //if (troopList.Count == 0 || transferable.food < 10000)
            //    return false;

            //if (crossbow + spear + halberd + horse < 10000)
            //    return false;

            if (city.IsRoadBlocked())
                return false;

            City.EnemyInfo enemyInfo;
            // 兵临城下且敌军存活
            if (city.IsEnemiesRound(10) && city.CheckEnemiesIfAlive(out enemyInfo))
                return true;

            return false;
        }

        public static bool AICanAttack(City city, Scenario scenario)
        {
            if (city.troops < 20000)
                return false;

            if (city.morale < 80)
                return false;

            if (city.freePersons.Count <= 0)
                return false;

            if (city.durability <= city.DurabilityLimit * 80 / 100)
                return false;

            if (!city.IsBorderCity)
                return false;

            if (city.IsEnemiesRound())
                return false;

            List<City> enemiesCities = new List<City>();
            city.ForeachNeighborCities(x =>
            {
                if (x.IsEnemy(city))
                    enemiesCities.Add(x);
            });

            if (enemiesCities.Count == 0)
                return false;

            if (city.food < city.troops * 2)
                return false;

            //if (allTroops.Count > 0 && allTroops.Count > TroopList.Count)
            //    return false;

            if (GameRandom.Changce(90))
                return false;

            return true;
        }

        public static bool AIIntriorBalance(City city, Scenario scenario)
        {
            if (city.freePersons.Count <= 1) return true;

            if (city.commerce >= city.agriculture)
            {
                Person[] people = ForceAI.CounsellorRecommendFarming(city.freePersons, out int totalValue);
                if (people == null) return true;
                if (city.JobFarming(people))
                {

                }
            }
            else
            {
                Person[] people = ForceAI.CounsellorRecommendDevelop(city.freePersons, out int totalValue);
                if (people == null) return true;
                if (city.JobDevelop(people))
                {
                }
            }
            return true;
        }
        public static bool AISecurity(City city, Scenario scenario)
        {
            if (city.freePersons.Count > 1 && city.gold > 400)
            {
                if (GameRandom.Changce((90 - city.security) * 3 / 2))
                {
                    Person[] people = ForceAI.CounsellorRecommendDevelop(city.freePersons, out int totalValue);
                    if (people == null) return true;
                    if (city.JobInspection(people))
                    {
                    }
                }
            }
            return true;
        }

        public static bool AITrainTroop(City city, Scenario scenario)
        {
            if (city.freePersons.Count > 1 && city.gold > 400)
            {
                if (city.morale < 50)
                {
                    Person[] people = ForceAI.CounsellorRecommendTrainTroop(city.freePersons, out int totalValue);
                    if (people == null) return true;
                    if (city.JobTrainTroop(people))
                    {
                    }
                }
                else
                {
                    if (GameRandom.Changce((95 - city.morale) * 3 / 2))
                    {
                        Person[] people = ForceAI.CounsellorRecommendTrainTroop(city.freePersons, out int totalValue);
                        if (people == null) return true;
                        if (city.JobTrainTroop(people))
                        {
                        }
                    }
                }


            }
            return true;
        }
    }


}
