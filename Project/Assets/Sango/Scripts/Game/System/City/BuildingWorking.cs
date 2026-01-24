using Sango.Game.Render;
using Sango.Game.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Sango.Game
{
    /// <summary>
    /// 建筑工作模块
    /// 可指派武将到建筑工作以提升建筑的功能产出
    /// </summary>
    [GameSystem(order = 100)]
    public class BuildingWorking : GameSystem
    {
        public Building TargetBuilding { get; set; }

        public override void Init()
        {
            // 增加建筑菜单
            GameEvent.OnBuildingContextMenuShow += OnBuildingContextMenuShow;
            GameEvent.OnBuildingTurnEnd += OnBuildingTurnEnd;
            GameEvent.OnCityMonthStart += OnCityMonthStart;
            GameEvent.OnCitySeasonStart += OnCitySeasonStart;
            GameEvent.OnCityTurnStart += OnCityTurnStart;
            GameEvent.OnCityTurnEnd += OnCityTurnEnd;
        }

        public override void Clear()
        {
            // 增加建筑菜单
            GameEvent.OnBuildingContextMenuShow -= OnBuildingContextMenuShow;
            GameEvent.OnBuildingTurnEnd -= OnBuildingTurnEnd;
            GameEvent.OnCityMonthStart -= OnCityMonthStart;
            GameEvent.OnCitySeasonStart -= OnCitySeasonStart;
            GameEvent.OnCityTurnStart -= OnCityTurnStart;
            GameEvent.OnCityTurnEnd -= OnCityTurnEnd;
        }

        protected virtual void OnBuildingContextMenuShow(IContextMenuData menuData, BuildingBase building)
        {
            if (building.BelongCity != null && building.BelongForce != null && building.BelongForce.IsPlayer && building.BelongForce == Scenario.Cur.CurRunForce)
            {
                TargetBuilding = building as Building;

                // 工作/设置
                menuData.Add(GameLanguage.GetString(10000001), 10, null, OnClickMenuItem_WorkerSet, true);

                // 工作/自动
                menuData.Add(GameLanguage.GetString(10000002), 11, null, OnClickMenuItem_AutoWorkerSet, true);
            }
        }

        protected virtual void OnClickMenuItem_WorkerSet(IContextMenuItem contextMenuItem)
        {
            // 工作设置


        }

        protected virtual void OnClickMenuItem_AutoWorkerSet(IContextMenuItem contextMenuItem)
        {
            // 自动设置
            City belongCity = TargetBuilding.BelongCity;
            if (belongCity == null) return;

            BuildingType targetBuildingType = TargetBuilding.BuildingType;

            if (TargetBuilding.Workers != null)
            {
                TargetBuilding.Workers.ForEach((person) =>
                {
                    person.workingBuilding = null;
                });
                TargetBuilding.Workers.Clear();
            }

            List<Person> persons = new List<Person>();
            for (int i = 0; i < belongCity.FreePersonCount; i++)
            {
                Person person = belongCity.freePersons[i];
                if (person == null) continue;
                if (person.workingBuilding != null)
                    continue;
                persons.Add(person);
            }

            if (persons.Count > 0)
            {
                persons.Sort((a, b) => -a.GetAttribute(targetBuildingType.effectAttrType).CompareTo(b.GetAttribute(targetBuildingType.effectAttrType)));
                if (TargetBuilding.Workers == null)
                {
                    TargetBuilding.Workers = new SangoObjectList<Person>();
                }
            }

            for (int i = 0; i < persons.Count; i++)
            {
                if (i < targetBuildingType.workerLimit)
                {
                    Person person = persons[i];
                    TargetBuilding.Workers.Add(person);
                    person.workingBuilding = TargetBuilding;
                }
            }
            TargetBuilding.Render.UpdateRender();

        }

        public void AutoSetWorker(Building target)
        {
            City belongCity = target.BelongCity;
            if (belongCity == null) return;

            BuildingType targetBuildingType = target.BuildingType;

            if (target.Workers != null)
            {
                target.Workers.ForEach((person) =>
                {
                    person.workingBuilding = null;
                });
                target.Workers.Clear();
            }

            List<Person> persons = new List<Person>();
            for (int i = 0; i < belongCity.FreePersonCount; i++)
            {
                Person person = belongCity.freePersons[i];
                if (person == null) continue;
                if (person.workingBuilding != null)
                    continue;
                persons.Add(person);
            }

            if (persons.Count > 0)
            {
                persons.Sort((a, b) => -a.GetAttribute(targetBuildingType.effectAttrType).CompareTo(b.GetAttribute(targetBuildingType.effectAttrType)));
                if (target.Workers == null)
                {
                    target.Workers = new SangoObjectList<Person>();
                }
            }

            for (int i = 0; i < persons.Count; i++)
            {
                if (i < targetBuildingType.workerLimit)
                {
                    Person person = persons[i];
                    target.Workers.Add(person);
                    person.workingBuilding = target;
                }
            }
        }

        void AutoSetWorker(City city, List<PBuilding> pBuildings)
        {
            City belongCity = city;
            if (belongCity == null) return;

            for (int i = 0; i < pBuildings.Count; i++)
            {
                Building target = pBuildings[i].building;
                if (target.Workers != null)
                {
                    target.Workers.ForEach((person) =>
                    {
                        person.workingBuilding = null;
                    });
                    target.Workers.Clear();
                }
            }

            List<Person> persons = new List<Person>();
            for (int i = 0; i < belongCity.FreePersonCount; i++)
            {
                Person person = belongCity.freePersons[i];
                if (person == null) continue;
                if (person.workingBuilding != null)
                    continue;
                persons.Add(person);
            }

            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < pBuildings.Count; i++)
                {
                    Building target = pBuildings[i].building;
                    BuildingType targetBuildingType = target.BuildingType;
                    if (target.Workers == null)
                        target.Workers = new SangoObjectList<Person>();
                    if (persons.Count > 0 && target.Workers.Count < targetBuildingType.workerLimit)
                    {
                        persons.Sort((a, b) => -a.GetAttribute(targetBuildingType.effectAttrType).CompareTo(b.GetAttribute(targetBuildingType.effectAttrType)));

                        Person person = persons[0];
                        target.Workers.Add(person);
                        persons.RemoveAt(0);
                        person.workingBuilding = target;
                    }
                }
            }
        }



        int GetCityLeaderInfuse(City city, int effectAttrType)
        {
            Person leader = city.Leader;
            int leaderAttrValue = 0;
            if (leader != null)
                leaderAttrValue = leader.GetAttribute(effectAttrType);
            // 计算公式
            return 100 + (int)((Mathf.Pow(Mathf.Max(40, leaderAttrValue), 1.5f) / 10 - 25) / 3f);
        }

        List<Person> worker_list = new List<Person>();
        /// <summary>
        /// 在回合末计算产出
        /// </summary>
        /// <param name="building"></param>
        /// <param name="scenario"></param>
        void OnBuildingTurnEnd(Building building, Scenario scenario)
        {
            if (!building.IsIntorBuilding())
                return;

            City belongCity = building.BelongCity;
            if (belongCity == null) return;
            if (!building.isComplate) return;
            BuildingType buildingType = building.BuildingType;

            // 计算太守对于收入的影响
            int leader_factor = GetCityLeaderInfuse(belongCity, buildingType.effectAttrType);
            worker_list.Clear();

            int person_factor = 100;
            int index = 1;
            bool hasWorker = false;
            if (building.Workers != null)
            {
                for (int i = 0; i < buildingType.workerLimit; i++)
                {
                    Person person = null;
                    if (i < building.Workers.Count)
                        person = building.Workers.Get(i);
                    if (person != null && person.IsFree && !person.ActionOver)
                    {
                        worker_list.Add(person);
                        hasWorker = true;
                        person_factor += (int)(Mathf.Pow(Mathf.Max(40, person.GetAttribute(buildingType.effectAttrType)), 0.5f) * 100 / (8 * index));
                        index++;
                    }
                }
            }

            Person[] personArray = worker_list.ToArray();

            int totalFactor = leader_factor * person_factor;
            int jobId = buildingType.jobId;
            JobType jobType = Scenario.Cur.CommonData.JobTypes.Get(jobId);
            GameUtility.InitJobFeature(building.Workers, belongCity, building);
            Tools.OverrideData<int> overrideData = null;

            // 建筑累积收益
            if (buildingType.foodGain > 0)
            {
                int value = buildingType.foodGain * totalFactor / 10000;
                overrideData = GameUtility.IntOverrideData.Set(value);
                GameEvent.OnBuildingCalculateFoodGain?.Invoke(building, overrideData);
                building.AccumulatedFood += overrideData.Value;
            }

            if (buildingType.goldGain > 0)
            {
                int value = buildingType.goldGain * totalFactor / 10000;
                overrideData = GameUtility.IntOverrideData.Set(value);
                GameEvent.OnBuildingCalculateGoldGain?.Invoke(building, overrideData);
                building.AccumulatedGold += overrideData.Value;
            }

            if (buildingType.populationGain > 0)
            {
                int value = buildingType.populationGain * totalFactor / 10000;
                overrideData = GameUtility.IntOverrideData.Set(value);
                GameEvent.OnBuildingCalculatePopulationGain?.Invoke(building, overrideData);
                building.AccumulatedPopulation += overrideData.Value;
            }

            int productCost = buildingType.productCost;
            int techniquePointGain = jobType.tpGain;
            int meritGain = jobType.meritGain;
            if (hasWorker)
            {
                // 计算工作消耗
                overrideData = GameUtility.IntOverrideData.Set(productCost);
                GameEvent.OnCityCheckJobCost?.Invoke(belongCity, jobId, personArray, overrideData);
                productCost = overrideData.Value;

                // 计算技巧值获取
                overrideData.Value = techniquePointGain;
                GameEvent.OnCityJobGainTechniquePoint?.Invoke(belongCity, jobId, personArray, overrideData);
                techniquePointGain = overrideData.Value;

                // 计算经验获取
                overrideData.Value = meritGain;
                GameEvent.OnCityJobGainMerit?.Invoke(belongCity, jobId, personArray, overrideData);
                techniquePointGain = overrideData.Value;

                // 经验获取
                for (int i = 0; i < personArray.Length; i++)
                {
                    Person person = personArray[i];
                    if (person == null) continue;
                    person.merit += meritGain;
                    person.GainExp(meritGain);
                    person.ActionOver = true;
                }

                belongCity.BelongForce.GainTechniquePoint(techniquePointGain);
            }

            if (buildingType.product > 0)
            {
                if (hasWorker && belongCity.gold > productCost)
                {
                    // 计算建筑实际产出
                    int product = buildingType.product;
                    overrideData = GameUtility.IntOverrideData.Set(product);
                    GameEvent.OnBuildingCalculateProduct?.Invoke(building, overrideData);
                    product = overrideData.Value;

                    // 计算单独产出逻辑
                    switch (buildingType.kind)
                    {
                        case (int)BuildingKindType.Barracks:
                            {
                                int value = product * totalFactor / 10000;
                                overrideData = GameUtility.IntOverrideData.Set(value);
                                GameEvent.OnCityJobResult?.Invoke(belongCity, jobId, personArray, overrideData);

                                // 治安对征兵的影响
                                overrideData.Value = (int)(overrideData.Value * (1f - Mathf.Max(0, (100 - belongCity.security)) * scenario.Variables.securityInfluenceRecruitTroops));
                                // 额外降低治安
                                int s = -GameRandom.Range(2, 5);
                                belongCity.AddSecurity(s);
                                building.Render?.ShowInfo(s, (int)InfoType.Security);
                                belongCity.morale = (belongCity.troops * belongCity.morale + overrideData.Value * 30) / (belongCity.troops + overrideData.Value);
                                building.AccumulatedProduct += overrideData.Value;

                            }
                            break;
                        default:
                            {
                                int value = product * totalFactor / 10000;
                                overrideData = GameUtility.IntOverrideData.Set(value);
                                GameEvent.OnCityJobResult?.Invoke(belongCity, jobId, personArray, overrideData);
                                building.AccumulatedProduct += overrideData.Value;
                            }
                            break;
                    }

                    if (productCost > 0)
                    {
                        belongCity.gold -= buildingType.productCost;
                        building.Render?.ShowInfo(-buildingType.productCost, (int)InfoType.Gold);
                    }
                }
                else
                {
                    // 闲置产出
                    int value = buildingType.emptyProduct * leader_factor / 100;
                    overrideData = GameUtility.IntOverrideData.Set(value);
                    GameEvent.OnBuildingCalculateProduct?.Invoke(building, overrideData);
                    building.AccumulatedProduct += overrideData.Value;
                }
            }

            GameUtility.ClearJobFeature();
        }

        /// <summary>
        /// 季度收入粮食
        /// </summary>
        /// <param name="scenario"></param>
        /// <returns></returns>
        public void OnCitySeasonStart(City city, Scenario scenario)
        {
            if (city.BelongCorps == null)
                return;

            // 计算太守对于收入的影响
            int leader_factor = GetCityLeaderInfuse(city, (int)AttributeType.Politics);
            int totalValue = city.BaseGainFood * leader_factor / 100;
            city.allBuildings.ForEach(building =>
            {
                totalValue += building.AccumulatedFood;
                building.AccumulatedFood = 0;
            });

            //totalValue = GameRandom.Random(totalValue, 0.05f);
            city.AddFood(totalValue);
#if SANGO_DEBUG
            Sango.Log.Print($"城市：{city.Name}, 收获粮食：{totalValue}, 现有粮食: {city.food}");
#endif
            city.Render?.ShowInfo(totalValue, (int)InfoType.Food);
        }

        /// <summary>
        /// 月度金钱收入
        /// </summary>
        /// <param name="scenario"></param>
        /// <returns></returns>
        public void OnCityMonthStart(City city, Scenario scenario)
        {
            if (city.BelongCorps == null)
                return;

            // 计算太守对于收入的影响
            int leader_factor = GetCityLeaderInfuse(city, (int)AttributeType.Politics);
            int totalValue = city.BaseGainGold * leader_factor / 100;
            //Sango.Log.Error($"太守P:{leader_factor}, city.BaseGainGold{city.BaseGainGold}, t:{totalValue}");
            city.allBuildings.ForEach(building =>
            {

                if(building.AccumulatedGold > 0)
                {
                    totalValue += building.AccumulatedGold;
                    //Sango.Log.Error($"建筑:{building.Name}, AccumulatedGold{building.AccumulatedGold}, t:{totalValue}");
                    building.AccumulatedGold = 0;
                }
            });

            //Sango.Log.Error($"all: t:{totalValue}");
            //totalFoodt = GameRandom.Random(totalFood, 0.05f);
            city.AddGold(totalValue);

#if SANGO_DEBUG
            Sango.Log.Print($"城市：{city.Name}, 收获资金：{totalValue}, 现有资金: {city.gold}");
#endif
            city.Render?.ShowInfo(totalValue, (int)InfoType.Gold);

        }

        struct PBuilding
        {
            public float p;
            public Building building;
        }


        /// <summary>
        /// 委任工作
        /// </summary>
        /// <param name="scenario"></param>
        public void AppointWorking(City city, Scenario scenario)
        {
            /*
                治安预留1人, 训练看气力, 征兵看粮食收入, 生产看兵器缺口
             */
            switch (city.workingAppointType)
            {
                // 重视内政
                // 重视军事
                // 均衡发展
            }

            city.allBuildings.ForEach((building) => { if (building.isComplate) building.Workers?.Clear(); });
            city.allPersons.ForEach((person) => { person.workingBuilding = null; });

            float targetGold = 5000f;
            float targetFood = city.troops * 2.5f;
            float targetTroop = city.food / 2f;
            float targetItemNumber = city.troops * 1.2f;
            float targetHorseNumber = city.troops;
            float targetBoatNumber = 100f;
            float targetMachineNumber = 100f;
            float targetSecurity = 90;
            float targetMorale = 100;

            float goldP = city.gold / targetGold;
            float foodP = city.food / targetFood;
            float troopP = city.troops / targetTroop;
            int totalWeaponNum = city.itemStore.GetNumber(new int[] { 2, 3, 4 });
            float itemP = totalWeaponNum / targetItemNumber;
            int totalHorseNum = city.itemStore.GetNumber(5);
            float horseP = totalHorseNum / targetHorseNumber;
            int totalBoatNum = city.itemStore.GetNumber(9);
            float boatP = totalBoatNum / targetBoatNumber;
            int totalMachineNum = city.itemStore.GetNumber(new int[] { 7, 8 });
            float machineP = totalMachineNum / targetMachineNumber;
            float securityP = (city.security - 50) / (targetSecurity - 50);
            float moraleP = city.morale / targetMorale;

            List<PBuilding> pBuildings = new List<PBuilding>();
            city.allBuildings.ForEach((building) =>
            {
                if (!building.isComplate) return;

                BuildingType buildingType = building.BuildingType;
                switch (buildingType.kind)
                {
                    case (int)BuildingKindType.Farm:
                    case (int)BuildingKindType.Barn:
                    case (int)BuildingKindType.MilitaryGarrison:
                        pBuildings.Add(new PBuilding() { p = foodP, building = building });
                        break;
                    case (int)BuildingKindType.Market:
                    case (int)BuildingKindType.FishMarket:
                    case (int)BuildingKindType.BigMarket:
                    case (int)BuildingKindType.BlackMarket:
                    case (int)BuildingKindType.Mint:
                        pBuildings.Add(new PBuilding() { p = goldP, building = building });
                        break;
                    case (int)BuildingKindType.Barracks:
                        if (troopP < 1)
                            pBuildings.Add(new PBuilding() { p = troopP, building = building });
                        break;
                    case (int)BuildingKindType.BlacksmithShop:
                        {
                            if (goldP > 1 || itemP < 1)
                            {
                                pBuildings.Add(new PBuilding() { p = itemP, building = building });
                                // 设置产出
                                // 统计适应偏向
                                int[] levelTotal = new int[3] { 1, 1, 1 };
                                city.allPersons.ForEach(x =>
                                {
                                    levelTotal[0] += x.SpearLv;
                                    levelTotal[1] += x.HalberdLv;
                                    levelTotal[2] += x.CrossbowLv;
                                });

                                int sumTotal = levelTotal[0] + levelTotal[1] + levelTotal[2];

                                for (int itemTypeId = 2; itemTypeId <= 4; itemTypeId++)
                                {
                                    int itemNum = city.itemStore.GetNumber(itemTypeId);
                                    int destNum = totalWeaponNum * levelTotal[itemTypeId - 2] / sumTotal;
                                    levelTotal[itemTypeId - 2] = Mathf.Max(1, (destNum - itemNum) / 100);
                                }

                                int targetIndex = GameRandom.RandomWeightIndex(levelTotal);
                                building.ProductItemId = targetIndex + 2;
                                
                            }
                        }
                        break;
                    case (int)BuildingKindType.Stable:
                        if (goldP > 1 || horseP < 1)
                            pBuildings.Add(new PBuilding() { p = horseP, building = building });
                        break;
                    case (int)BuildingKindType.BoatFactory:
                        {
                            if (goldP > 1 || boatP < 1)
                            {
                                pBuildings.Add(new PBuilding() { p = boatP, building = building });
                                int targetBoatId = 12;
                                ItemType itemType = scenario.GetObject<ItemType>(targetBoatId);
                                if (itemType.IsValid(city.BelongForce))
                                {
                                    building.ProductItemId = targetBoatId;
                                }
                                else
                                {
                                    building.ProductItemId = targetBoatId - 1;
                                }
                            }
                        }
                        break;
                    case (int)BuildingKindType.MechineFactory:
                        {
                            if (goldP > 1 || machineP < 1)
                            {
                                pBuildings.Add(new PBuilding() { p = machineP, building = building });
                                int monsterNum = city.itemStore.GetNumber(7);
                                int towerNum = city.itemStore.GetNumber(9);

                                int tagetItemId;
                                int totalNum;
                                ItemType targetItemType;
                                if (towerNum > monsterNum)
                                {
                                    tagetItemId = 7;
                                    totalNum = monsterNum;
                                    targetItemType = scenario.GetObject<ItemType>(tagetItemId);
                                    if (!targetItemType.IsValid(city.BelongForce))
                                        tagetItemId--;
                                }
                                else
                                {
                                    tagetItemId = 9;
                                    totalNum = towerNum;
                                    targetItemType = scenario.GetObject<ItemType>(tagetItemId);
                                    if (!targetItemType.IsValid(city.BelongForce))
                                        tagetItemId--;
                                }
                                building.ProductItemId = tagetItemId;
                            }
                        }
                        break;
                    case (int)BuildingKindType.PatrolBureau:
                        if (securityP < 1)
                            pBuildings.Add(new PBuilding() { p = securityP, building = building });
                        break;
                    case (int)BuildingKindType.TrainTroopBuilding:
                        if (moraleP < 1)
                            pBuildings.Add(new PBuilding() { p = moraleP, building = building });
                        break;
                    case (int)BuildingKindType.RecruitBuilding:
                        pBuildings.Add(new PBuilding() { p = 100, building = building });
                        break; ;
                }
            });

            pBuildings.Sort((a, b) => a.p.CompareTo(b.p));

            AutoSetWorker(city, pBuildings);
        }

        /// <summary>
        /// 回合结束搜索
        /// </summary>
        /// <param name="scenario"></param>
        /// <returns></returns>
        public void OnCityTurnEnd(City city, Scenario scenario)
        {
            if (city.BelongCorps == null)
                return;
            city.allBuildings.ForEach((building) =>
            {
                if (!building.isComplate)
                    return;

                BuildingType buildingType = building.BuildingType;
                switch (buildingType.kind)
                {
                    // 自动搜索
                    case (int)BuildingKindType.RecruitBuilding:
                        if (building.Workers != null)
                        {
                            building.Workers.ForEach((worker) =>
                            {
                                CityPersonSearchingEvent te = new CityPersonSearchingEvent()
                                {
                                    city = city,
                                    person = worker,
                                };
                                RenderEvent.Instance.Add(te);
                            });
                        }
                        break;
                }
            });

        }

        /// <summary>
        /// 回合收入
        /// </summary>
        /// <param name="scenario"></param>
        /// <returns></returns>
        public void OnCityTurnStart(City city, Scenario scenario)
        {
            if (city.BelongCorps == null)
                return;

            // 获取收入
            city.allBuildings.ForEach((building) =>
            {
                if (!building.isComplate)
                    return;

                BuildingType buildingType = building.BuildingType;
                switch (buildingType.kind)
                {
                    case (int)BuildingKindType.Barracks:
                        if (building.AccumulatedProduct > 0)
                        {
                            city.AddTroops(building.AccumulatedProduct);
                            city.Render?.UpdateRender();
                            building.Render?.ShowInfo(building.AccumulatedProduct, (int)InfoType.Troop);
                            building.AccumulatedProduct = 0;
                        }
                        break;
                    case (int)BuildingKindType.BlacksmithShop:
                        if (building.AccumulatedProduct > 0)
                        {
                            city.AddItem(building.ProductItemId, building.AccumulatedProduct);
                            city.Render?.UpdateRender();
                            switch (building.ProductItemId)
                            {
                                case 2:
                                    building.Render?.ShowInfo(building.AccumulatedProduct, (int)InfoType.Spear);
                                    break;
                                case 3:
                                    building.Render?.ShowInfo(building.AccumulatedProduct, (int)InfoType.Sword);
                                    break;
                                case 4:
                                    building.Render?.ShowInfo(building.AccumulatedProduct, (int)InfoType.CrossBow);
                                    break;
                            }
                            building.AccumulatedProduct = 0;
                        }
                        break;
                    case (int)BuildingKindType.Stable:
                        if (building.AccumulatedProduct > 0)
                        {
                            city.AddItem(5, building.AccumulatedProduct);
                            city.Render?.UpdateRender();
                            building.Render?.ShowInfo(building.AccumulatedProduct, (int)InfoType.Horse);
                            building.AccumulatedProduct = 0;
                        }
                        break;
                    case (int)BuildingKindType.BoatFactory:
                        if (building.AccumulatedProduct > 0)
                        {
                            ItemType itemType = scenario.GetObject<ItemType>(building.ProductItemId);
                            city.AddItem(itemType.subKind, building.AccumulatedProduct);
                            city.Render?.UpdateRender();
                            building.Render?.ShowInfo(building.AccumulatedProduct, building.ProductItemId);
                            building.AccumulatedProduct = 0;
                        }
                        break;
                    case (int)BuildingKindType.MechineFactory:
                        if (building.AccumulatedProduct > 0)
                        {
                            ItemType itemType = scenario.GetObject<ItemType>(building.ProductItemId);
                            city.AddItem(itemType.subKind, building.AccumulatedProduct);
                            city.Render?.UpdateRender();
                            building.Render?.ShowInfo(building.AccumulatedProduct, building.ProductItemId - 1);
                            building.AccumulatedProduct = 0;
                        }
                        break;
                    case (int)BuildingKindType.PatrolBureau:
                        if (building.AccumulatedProduct > 0)
                        {
                            city.AddSecurity(building.AccumulatedProduct);
                            city.Render?.UpdateRender();
                            building.Render?.ShowInfo(building.AccumulatedProduct, (int)InfoType.Security);
                            building.AccumulatedProduct = 0;
                        }
                        break;
                    case (int)BuildingKindType.TrainTroopBuilding:
                        if (building.AccumulatedProduct > 0)
                        {
                            city.AddMorale(building.AccumulatedProduct);
                            city.Render?.UpdateRender();
                            building.Render?.ShowInfo(building.AccumulatedProduct, (int)InfoType.Morale);
                            building.AccumulatedProduct = 0;
                        }
                        break;
                }
            });


            AppointWorking(city, scenario);
        }
    }
}
