using Newtonsoft.Json;
using Sango.Game.Render;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]
    public class City : BuildingBase
    {
        public virtual bool AIFinished { get; set; }
        public virtual bool AIPrepared { get; set; }
        public override SangoObjectType ObjectType { get { return SangoObjectType.City; } }

        /// <summary>
        /// 粮食
        /// </summary>
        [JsonProperty] public int food;

        /// <summary>
        /// 金钱
        /// </summary>
        [JsonProperty] public int gold;

        /// <summary>
        /// 人口
        /// </summary>
        [JsonProperty] public int population;

        /// <summary>
        /// 兵役人口
        /// </summary>
        [JsonProperty] public int troopPopulation;

        /// <summary>
        /// 库存
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(ItemStoreConverter))]
        public ItemStore itemStore = new ItemStore();

        /// <summary>
        /// 商业值
        /// </summary>
        [JsonProperty] public int commerce;

        /// <summary>
        /// 农业值
        /// </summary>
        [JsonProperty] public int agriculture;

        /// <summary>
        /// 民心
        /// </summary>
        [JsonProperty] public byte popularSupport;

        /// <summary>
        /// 治安
        /// </summary>
        [JsonProperty] public int security;

        /// <summary>
        /// 战意
        /// </summary>
        [JsonProperty] public int energy;

        /// <summary>
        /// 士气
        /// </summary>
        [JsonProperty] public int morale;

        /// <summary>
        /// 是否有商人, 数字为兑换比例 0为没有商人
        /// </summary>
        [JsonProperty] public byte hasBusiness;

        /// <summary>
        /// 当前兵力
        /// </summary>
        [JsonProperty] public int troops;

        /// <summary>
        /// 当前伤兵
        /// </summary>
        [JsonProperty] public int woundedTroops;

        /// <summary>
        /// 可容纳兵力
        /// </summary>
        [JsonProperty] public int troopsLimit;
        public int TroopsLimit => troopsLimit + CityLevelType.troopsLimitAdd;

        /// <summary>
        /// 仓库大小
        /// </summary>
        [JsonProperty] public int storeLimit;
        public int StoreLimit => storeLimit + CityLevelType.storeLimitAdd;

        /// <summary>
        /// 金库大小
        /// </summary>
        [JsonProperty] public int goldLimit;
        public int GoldLimit => goldLimit + CityLevelType.goldLimitAdd;

        /// <summary>
        /// 粮仓大小
        /// </summary>
        [JsonProperty] public int foodLimit;
        public int FoodLimit => foodLimit + CityLevelType.foodLimitAdd;

        /// <summary>
        /// 城内建筑槽位
        /// </summary>
        [JsonProperty] public int insideSlot;
        public int InsideSlot => insideSlot + CityLevelType.insideSlotAdd;

        /// <summary>
        /// 城外建筑槽位
        /// </summary>
        [JsonProperty] public int outsideSlot;
        public int OutsideSlot => outsideSlot + CityLevelType.outsideSlotAdd;

        /// <summary>
        /// 村庄槽位
        /// </summary>
        [JsonProperty] public int villageSlot;
        public int VillageSlot => villageSlot + CityLevelType.villageSlotAdd;

        /// <summary>
        /// 基础金钱收入 基础收入 = 基础收入 * 当前商业值 / 最大商业值
        /// </summary>
        [JsonProperty] public int baseGainGold;
        public int BaseGainGold => baseGainGold + CityLevelType.baseGainGoldAdd;

        /// <summary>
        /// 基础粮食收入 基础收入 = 基础粮食收入 * 当前农业值 / 最大农业值
        /// </summary>
        [JsonProperty] public int baseGainFood;
        public int BaseGainFood => baseGainFood + CityLevelType.baseGainFoodAdd;

        /// <summary>
        /// 最大商业值
        /// </summary>
        [JsonProperty] public int commerceLimit;
        public int CommerceLimit => commerceLimit + CityLevelType.commerceLimitAdd;

        /// <summary>
        /// 最大农业值
        /// </summary>
        [JsonProperty] public int agricultureLimit;

        public int AgricultureLimit => agricultureLimit + CityLevelType.agricultureLimitAdd;
        /// <summary>
        /// 最大耐久
        /// </summary>
        [JsonProperty] public int durabilityLimit;

        public override int DurabilityLimit => durabilityLimit + CityLevelType.durabilityLimitAdd;

        /// <summary>
        /// 是否,满兵
        /// </summary>
        public bool TroopsIsFull => troops + woundedTroops >= TroopsLimit;

        /// <summary>
        /// 太守
        /// </summary>
        [JsonConverter(typeof(Id2ObjConverter<Person>))]
        [JsonProperty]
        public Person Leader;

        /// <summary>
        /// 所属州
        /// </summary>
        [JsonConverter(typeof(Id2ObjConverter<State>))]
        [JsonProperty]
        public State State;

        /// <summary>
        /// 相邻城市
        /// </summary>
        [JsonConverter(typeof(SangoObjectListIDConverter<City>))]
        [JsonProperty]
        public SangoObjectList<City> NeighborList = new SangoObjectList<City>();

        /// <summary>
        /// 城市等级数据
        /// </summary>
        [JsonConverter(typeof(Id2ObjConverter<CityLevelType>))]
        [JsonProperty]
        public CityLevelType CityLevelType;

        /// <summary>
        /// 俘虏
        /// </summary>
        [JsonConverter(typeof(SangoObjectListIDConverter<Person>))]
        [JsonProperty]
        public SangoObjectList<Person> CaptiveList = new SangoObjectList<Person>();


        public List<Building> villageList = new List<Building>();

        protected List<Cell> cell_list = new List<Cell>();

        public int totalGainFood = 0;
        public int totalGainGold = 0;

        /// <summary>
        /// 额外的影响倍率(事件等)
        /// </summary>
        public float extraGainFoodFactor = 0;
        public float extraGainGoldFactor = 0;
        public float extraPopulationFactor = 1;


        float population_increase_factor = 0;
        public override Cell CenterCell => cell_list[0];
        internal int borderLine;
        public bool IsBorderCity => borderLine == 0;
        public int BorderLine => borderLine;

        public List<Person> freePersons = new List<Person>();
        public List<Person> wildPersons = new List<Person>();

        public int FreePersonCount => freePersons.Count;
        public int PersonHole { get; set; }

        //public int eventId;
        //public int specialtyId;
        //public int model_wall;
        //public int model_city;
        internal int virtualFightPower;
        internal bool isUpdatedFightPower;
        internal bool boderLineChecked = false;

        /// <summary>
        /// 所有武将
        /// </summary>
        public SangoObjectList<Person> allPersons = new SangoObjectList<Person>();
        /// <summary>
        /// 所有设施
        /// </summary>
        public SangoObjectList<Building> allOutterBuildings = new SangoObjectList<Building>();

        public List<Troop> activedTroops = new List<Troop>();


        /// <summary>
        /// 所有内城设施
        /// </summary>
        public int[] innerSlot;

        public List<TroopType> activedTroopType = new List<TroopType>();
        int fightPower = 0;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<City, List<Cell>> raodToNeighborCache = new Dictionary<City, List<Cell>>();

        /// <summary>
        /// 所有内政设施
        /// </summary>
        public SangoObjectList<Building> allIntriorBuildings = new SangoObjectList<Building>();
        public Dictionary<int, int> buildingLevelCountMap = new Dictionary<int, int>();

        /// <summary>
        /// AI指令集
        /// </summary> 
        public List<System.Func<City, Scenario, bool>> AICommandList = new List<System.Func<City, Scenario, bool>>();

        public List<Cell> defenceCellList = new List<Cell>();

        public List<Cell> GetRoadToNeighbor(City city)
        {
            List<Cell> list;
            if (!raodToNeighborCache.TryGetValue(city, out list))
            {
                list = new List<Cell>();
                Scenario.Cur.Map.GetDirectPath(CenterCell, city.CenterCell, list);
                raodToNeighborCache.Add(city, list);
                city.raodToNeighborCache.Add(this, list);
            }
            return list;
        }

        public int FightPower => fightPower;
        public Person Add(Person person) { allPersons.Add(person); return person; }
        public Person Remove(Person person)
        {
            allPersons.Remove(person); freePersons.Remove(person);
            return person;
        }

        public void UpdateActiveTroopTypes()
        {
            activedTroopType.Clear();
            Scenario.Cur.CommonData.TroopTypes.ForEach(x =>
            {
                //if (x.activeCondition == null || (x.activeCondition != null && x.activeCondition.Check(null)))
                //    activedTroopType.Add(x);
            });
        }

        public override void OnScenarioPrepare(Scenario scenario)
        {
            //x *= 2;
            //y *= 2;

            base.OnScenarioPrepare(scenario);
            //TroopList?.InitCache();// = new SangoObjectList<Troop>().FromString(_troopListStr, scenario.troopSet);
            //NeighborList?.InitCache();// = new SangoObjectList<City>().FromString(_neighborListStr, scenario.citySet);
            //CityLevelType = scenario.CommonData.CityLevelTypes.Get(_cityLevelTypeId);
            innerSlot = new int[InsideSlot];
            if (durability <= 0)
                durability = DurabilityLimit;
            buildingLevelCountMap.Clear();
            // 地格占用
            effectCells = new System.Collections.Generic.List<Cell>();
            scenario.Map.GetSpiral(x, y, BuildingType.radius, cell_list);
            foreach (Cell cell in cell_list)
                cell.building = this;

            effectCells.Clear();
            scenario.Map.GetDirectSpiral(CenterCell, BuildingType.radius + 1, BuildingType.radius + 10, effectCells);

            for (int i = 0; i < effectCells.Count; i++)
            {
                Cell cell = effectCells[i];
                if (cell.HasGridState(Sango.Render.MapGrid.GridState.Defence))
                    defenceCellList.Add(cell);
            }


            foreach (Person person in CaptiveList)
            {
                if (person.BelongForce != null)
                    person.BelongForce.CaptiveList.Add(person);
            }
        }

        public override void OnPrepareRender()
        {
            Render = new CityRender(this);
        }

        public override void Init(Scenario scenario)
        {
            base.Init(scenario);
            if (BuildingType.Id == 1)
            {
                UpdateActiveTroopTypes();
                UpdateFightPower();
            }
            CalculateHarvest();
        }


        public void UpdateFightPower()
        {
            fightPower = 1000 + troops;
            //fightPower += UnityEngine.Mathf.Min(troops, allPersons.Count * 5000);
            isUpdatedFightPower = true;
            virtualFightPower = FightPower;
            ForeachNeighborCities(x =>
            {
                if (x.IsSameForce(this))
                {
                    if (!x.isUpdatedFightPower)
                    {
                        x.UpdateFightPower();
                        x.isUpdatedFightPower = true;
                    }
                    virtualFightPower += x.FightPower / 3;
                }
            });
        }

        public void CalculateHarvest()
        {
            ScenarioVariables variables = Scenario.Cur.Variables;

            // 人口增长率
            population_increase_factor = variables.populationIncreaseBaseFactor;

            // 计算基础收入
            totalGainFood = BaseGainFood + agriculture * variables.agriculture_add_food;
            totalGainGold = BaseGainGold + commerce * variables.commerce_add_gold;

            // 计算建筑收入
            allIntriorBuildings.ForEach(x =>
            {
                if (x.isComplte)
                {
                    population_increase_factor += x.BuildingType.populationGain;
                    totalGainFood += x.BuildingType.foodGain;
                    totalGainGold += x.BuildingType.goldGain;
                }
            });

            float securityInfluence = (((float)security / variables.securityInfluenceMax) - 1) * variables.securityInfluence;
            float popularSupportInfluence = (((float)popularSupport / variables.popularSupportInfluenceMax) - 1) * variables.popularSupportInfluence;
            float leftInfluence = 1.0f + securityInfluence + popularSupportInfluence;

            totalGainFood = Mathf.CeilToInt(leftInfluence * totalGainFood * (variables.foodFactor + extraGainFoodFactor));
            totalGainGold = Mathf.CeilToInt(leftInfluence * totalGainGold * (variables.goldFactor + extraGainGoldFactor));

            population_increase_factor *= extraPopulationFactor;
        }

        public void ForeachNeighborCities(Action<City> action)
        {
            for (int i = 0; i < NeighborList.Count; i++)
            {
                City c = NeighborList[i];
                if (c == null) continue;
                action(c);
            }
        }


        static bool _IsBorderCity(City city)
        {
            if (city.NeighborList == null)
                return false;
            for (int i = 0; i < city.NeighborList.Count; i++)
            {
                City c = city.NeighborList[i];
                if (c == null) continue;
                if (!city.IsSameForce(c)) return true;
            }
            return false;
        }

        static int _CheckBorder(City city, int len)
        {
            if (!_IsBorderCity(city))
            {
                city.boderLineChecked = true;
                for (int i = 0; i < city.NeighborList.Count; i++)
                {
                    City c = city.NeighborList[i];
                    if (c == null) continue;
                    if (!c.boderLineChecked && city.IsSameForce(c)) return _CheckBorder(c, len + 1);
                }
            }
            else
                return len;

            return 0;
        }

        /// <summary>
        /// 季度粮食收入
        /// </summary>
        /// <param name="scenario"></param>
        /// <returns></returns>
        public override bool OnSeasonStart(Scenario scenario)
        {
            if (BelongCorps == null)
                return true;

            int harvest = GameRandom.Random(totalGainFood, 0.05f);
            Render?.ShowInfo(harvest, (int)InfoTyoe.Food);
            food += harvest;
#if SANGO_DEBUG
            Sango.Log.Print($"城市：{Name}, 收获粮食：{harvest}, 现有粮食: {food}");
#endif
            if (Render != null)
                Render.UpdateRender();

            return base.OnSeasonStart(scenario);
        }


        /// <summary>
        /// 月度金钱收入
        /// </summary>
        /// <param name="scenario"></param>
        /// <returns></returns>
        public override bool OnMonthStart(Scenario scenario)
        {
            int pop = 0;
            int troopPop = 0;
            if (scenario.Variables.populationEnable)
            {
                pop = GameRandom.Random((int)(population * population_increase_factor), 0.1f);
                population += pop;
                troopPop = (int)(pop * 0.3f);
                troopPopulation += troopPop;
            }

            if (BelongCorps == null)
                return true;

            int inComingGold = GameRandom.Random(totalGainGold, 0.05f);
            inComingGold -= GoldCost(scenario);
            Render?.ShowInfo(inComingGold, (int)InfoTyoe.Gold);

            gold += inComingGold;
            if (gold < 0) gold = 0;
#if SANGO_DEBUG
            Sango.Log.Print($"城市：{Name}, 武将人数:{allPersons.Count}, 收入<-- 金钱:{inComingGold}, 人口:{pop}, 兵役:{troopPop}, 现有金钱: {gold}, 人口:{population}, 兵役:{troopPopulation}");
#endif
            if (Render != null)
                Render.UpdateRender();

            return base.OnMonthStart(scenario);
        }
        public override bool OnDayStart(Scenario scenario)
        {
            return base.OnDayStart(scenario);
        }

        public virtual void OnBuildingComplete(Building building, SangoObjectList<Person> builder)
        {
            this.CalculateHarvest();
        }

        public virtual void OnBuildingUpgradeComplete(Building building, SangoObjectList<Person> builder)
        {
            this.CalculateHarvest();
        }

        public void OnBuildingCreate(Building building)
        {
            if (building.BuildingType.isIntrior)
            {
                allIntriorBuildings.Add(building);
                innerSlot[building.SlotId] = building.Id;
            }
            else
            {
                allOutterBuildings.Add(building);
            }

            // 统计建筑数量
            if (buildingLevelCountMap.TryGetValue(building.BuildingType.kind, out int totalLevel))
            {
                totalLevel += building.BuildingType.level;
                buildingLevelCountMap[building.BuildingType.kind] = totalLevel;
            }
            else
            {
                buildingLevelCountMap.Add(building.BuildingType.kind, building.BuildingType.level);
            }
        }

        public void OnBuildingDestroy(Building building)
        {
            if (building.BuildingType.isIntrior)
            {
                allIntriorBuildings.Remove(building);
                innerSlot[building.SlotId] = 0;
                villageList.Remove(building);
            }
            else
            {
                allOutterBuildings.Remove(building);
            }
            buildingLevelCountMap[building.BuildingType.kind] = buildingLevelCountMap[building.BuildingType.kind] - building.BuildingType.level;
            this.CalculateHarvest();
        }

        public override bool OnForceTurnStart(Scenario scenario)
        {
            AIPrepared = false;
            AIFinished = false;
            ActionOver = false;
            boderLineChecked = false;
            CalculateHarvest();
            UpdateFightPower();
            JobHealingTroop();
            freePersons.Clear();
            allPersons.ForEach(person =>
            {
                if (!person.ActionOver && person.IsFree)
                    freePersons.Add(person);
            });

            FoodCost(scenario);

            if (durability < DurabilityLimit)
            {
                durability += Leader?.BaseBuildAbility * 2 + 50 ?? 50;
                if (durability > DurabilityLimit)
                    durability = DurabilityLimit;
            }

            if (Render != null)
                Render.UpdateRender();

            return base.OnDayStart(scenario);
        }
        public override bool OnForceTurnEnd(Scenario scenario)
        {
            CurActiveTroop = null;
            isUpdatedFightPower = false;
            return base.OnDayStart(scenario);
        }

        public void FoodCost(Scenario scenario)
        {
            if (food > 0)
            {
                int foodCost = 0;
                foodCost += (int)System.Math.Ceiling(scenario.Variables.baseFoodCostInCity * (troops + woundedTroops));
                int needFood = foodCost - food;
                if (needFood > 0)
                {
                    float runawayTroops = ((float)needFood / (float)foodCost) * scenario.Variables.runawayWhenCityFoodNotEnough;
                    troops = (int)System.Math.Ceiling(troops * (1.0f - runawayTroops));
                    if (woundedTroops > 100)
                    {
                        woundedTroops = (int)System.Math.Ceiling(woundedTroops * (1.0f - runawayTroops));
                    }
                    else
                    {
                        woundedTroops = 0;
                    }
                    food = 0;
                }
                else
                    food -= foodCost;

            }
            else
            {
                food = 0;
                float runawayTroops = scenario.Variables.runawayWhenCityFoodNotEnough;
                troops = (int)System.Math.Ceiling(troops * (1.0f - runawayTroops));
                if (woundedTroops > 100)
                {
                    woundedTroops = (int)System.Math.Ceiling(woundedTroops * (1.0f - runawayTroops));
                }
                else
                {
                    woundedTroops = 0;
                }
            }
        }
        public int GoldCost(Scenario scenario)
        {
            int goldCost = 0;
            allPersons.ForEach(person =>
            {
                if (person.Official != null)
                {
                    goldCost += person.Official.cost;
                }
            });
            return goldCost;
        }

        //public void CreateTroop(Troop troop)
        //{
        //    AddTroop(troop);

        //}

        //public void AddTroop(Troop troop)
        //{
        //    // 先加入剧本才能分配ID
        //    Add(troop);
        //    troop.Leader.BelongTroop = troop;
        //    for (int i = 0; i < troop.MemberList.Count; i++)
        //        troop.MemberList[i].BelongTroop = troop;

        //    troop.BelongCity = this;
        //    troop.cell = CenterCell;
        //    troop.cell.troop = troop;
        //    troop.x = troop.cell.x;
        //    troop.y = troop.cell.y;
        //}

        //public Person Add(Person person)
        //{
        //    allPersons.Add(person);
        //    if (BelongCorps == null)
        //    {
        //        Sango.Log.Error($"why {Name}->BelongCorps is null");
        //    }
        //    BelongCorps.Add(person);
        //    return person;
        //}
        //public Troop Add(Troop troops)
        //{
        //    allTroops.Add(BelongCorps.Add(troops));
        //    return troops;
        //}
        //public Building Add(Building building)
        //{
        //    allBuildings.Add(BelongCorps.Add(building));
        //    return building;
        //}
        //public Troop Add(Troop troop)
        //{
        //    this.TroopList.Add(troop);
        //    return troop;
        //}
        //public Person Remove(Person person)
        //{
        //    allPersons.Remove(person);
        //    BelongCorps.Remove(person);
        //    return person;
        //}
        //public Troop Remove(Troop troop)
        //{
        //    this.TroopList.Remove(troop);
        //    return troop;
        //}
        //public Troop Remove(Troop troops)
        //{
        //    allTroops.Remove(BelongCorps.Remove(troops));
        //    return troops;
        //}
        //public Building Remove(Building building)
        //{
        //    allBuildings.Remove(BelongCorps.Remove(building));
        //    return building;
        //}

        public override bool ChangeDurability(int num, SangoObject atk, bool showDamage = true)
        {
            bool rs = base.ChangeDurability(num, atk, showDamage);
            if (rs)
            {
                durability = 1500;
            }

            if (Render != null)
                Render.UpdateRender();

            return rs;
        }

        public City GetNearnestForceCity()
        {
            for (int i = 0; i < NeighborList.Count; i++)
            {
                City city = NeighborList[i];
                if (city.IsSameForce(this))
                    return city;
            }

            City nearnest = null;
            int distance = 100000;
            if (BelongForce != null)
            {
                BelongForce.ForEachCity(city =>
                {
                    if (city != this)
                    {
                        int dis = Scenario.Cur.Map.Distance(city.CenterCell, this.CenterCell);
                        if (dis < distance)
                        {
                            distance = dis;
                            nearnest = city;
                        }
                    }
                });
            }
            return nearnest;
        }

        public City GetNearnestForceCity(Force force)
        {
            for (int i = 0; i < NeighborList.Count; i++)
            {
                City city = NeighborList[i];
                if (city.BelongForce == force)
                    return city;
            }

            City nearnest = null;
            int distance = 100000;
            if (force != null)
            {
                force.ForEachCity(city =>
                {
                    if (city != this)
                    {
                        int dis = Scenario.Cur.Map.Distance(city.CenterCell, this.CenterCell);
                        if (dis < distance)
                        {
                            distance = dis;
                            nearnest = city;
                        }
                    }
                });
            }
            return nearnest;
        }


        public Corps ChangeCorps(Corps other)
        {
            Corps last = null;
            if (BelongCorps != other)
            {
                last = BelongCorps;
                BelongCorps = other;
                if (BelongForce != other.BelongForce)
                {
                    BelongForce = other.BelongForce;
                }
                Render?.UpdateRender();
            }
            return last;
        }

        public bool ChangeTroops(int num, SangoObject atk, bool showDamage = true)
        {
            if (showDamage)
                Render?.ShowInfo(num, (int)InfoTyoe.Troop);

            troops = troops + num;
            if (troops < 0)
                troops = 0;

            if (Render != null)
            {
                Render.UpdateRender();
            }
            return troops > 0;
        }

        public override void OnFall(SangoObject atker)
        {
            ScenarioVariables scenarioVariables = Scenario.Cur.Variables;
            Troop atk = atker as Troop;
            if (atk == null) return;

            // 城倒,俘虏逃
            for (int i = 0; i < CaptiveList.Count; i++)
            {
                Person person = CaptiveList[i];
                if (person.IsWild)
                {
                    person.BelongCity = this;
                    BelongCity.wildPersons.Add(person);
                }
                else
                {
                    person.BelongForce.Governor.BelongCity.allPersons.Add(person);
                    person.BelongCorps = person.BelongForce.Governor.BelongCorps;
                    person.BelongCity = person.BelongForce.Governor.BelongCity;
                    person.BelongCity.allPersons.Add(person);
                    person.SetMission(MissionType.PersonReturn, person.BelongCity, 1);
                }
            }

            CaptiveList.Clear();

            // 白城
            if (BelongCorps == null)
            {
                ChangeCorps(atk.BelongCorps);
                Leader = atk.Leader;
                atk.EnterCity(this);
                Render?.UpdateRender();
                CalculateHarvest();
                Scenario.Cur.Event.OnCityFall?.Invoke(this, atk, Scenario.Cur);
                return;
            }

            // 确认一个撤退城市
            City escapeCity = null;
            if (BelongForce.CityCount > 1)
            {
                if (this == BelongForce.Governor.BelongCity)
                    escapeCity = GetNearnestForceCity();
                else
                    escapeCity = BelongForce.Governor.BelongCity;
            }

            // 基础抓捕率
            int cacaptureChangce = escapeCity != null ? scenarioVariables.captureChangceWhenCityFall : scenarioVariables.captureChangceWhenLastCityFall;

            // 处理俘虏
            List<Person> captiveList = new List<Person>();

            // 必须优先处理队伍
            if (escapeCity == null)
            {
                // 灭亡后,队伍要清除
                Scenario.Cur.troopsSet.ForEach((troop) =>
                {
                    if (troop.IsAlive && troop.BelongForce == this.BelongForce)
                        troop.Clear();
                });
            }

            for (int i = allPersons.Count - 1; i >= 0; --i)
            {
                Person person = allPersons[i];

                // 没有执行任务的才能被捕获,暂时不能抓捕主公
                if (person.IsFree && person != person.BelongForce.Governor && GameRandom.Changce(cacaptureChangce))
                {
                    captiveList.Add(person);
                }
                else
                {
                    if (escapeCity != null)
                    {
                        person.ChangeCity(escapeCity);
                        if (person.BelongTroop == null)
                            person.SetMission(MissionType.PersonReturn, person.BelongCity, 1);
                    }
                    else
                    {
                        person.ClearMission();
                        person.LeaveToWild();
                    }
                }
            }

            //处理建筑
            for (int i = allIntriorBuildings.Count - 1; i >= 0; i--)
            {
                Building building = allIntriorBuildings[i];
                if (building.isComplte && GameRandom.Changce(30))
                {
                    building.ChangeCorps(atk.BelongCorps);
                }
                else
                {
                    building.OnFall(atk);
                }
            }

            for (int i = allOutterBuildings.Count - 1; i >= 0; i--)
            {
                Building building = allOutterBuildings[i];
                if (building.isComplte && GameRandom.Changce(30))
                {
                    building.ChangeCorps(atk.BelongCorps);
                }
                else
                {
                    building.OnFall(atk);
                }
            }

            if (escapeCity == null)
            {
                Scenario.Cur.corpsSet.Remove(BelongCorps);
#if SANGO_DEBUG
                Sango.Log.Print($"{BelongForce.Name} 灭亡!!!");
#endif
                Scenario.Cur.forceSet.Remove(BelongForce);

                WindowEvent windowEvent = new WindowEvent()
                {
                    windowName = "window_dialog",
                    arg1 = $"{BelongForce.Name} 灭亡!!!"
                };
                RenderEvent.Instance.Add(windowEvent);


                if (Scenario.Cur.forceSet.DataCount == 1)
                {
                    Sango.Log.Print($"{atk.BelongForce.Name} 统一!!!!!!!!!!!!!!");
                }
            }

            ChangeCorps(atk.BelongCorps);

            // 处理库存和钱粮,兵力
            food = food * scenarioVariables.cityFallCanKeepFoodFactor / 100;
            gold = gold * scenarioVariables.cityFallCanKeepGoldFactor / 100;
            troops = troops * scenarioVariables.cityFallCanKeepTroopsFactor / 100;
            itemStore.Split((100 - scenarioVariables.cityFallCanKeepItemFactor) / 100);

            if (Render != null)
                Render.UpdateRender();

            CalculateHarvest();

            //TODO: 玩家处理俘虏
            for (int i = 0; i < captiveList.Count; ++i)
            {
                Person person = captiveList[i];
                if (atk.BelongForce.Governor.Persuade(person))
                {
                    person.ChangeCorps(atk.BelongCorps);
                }
                else
                {
                    allPersons.Remove(person.BeCaptive(this));
                }
            }

            Scenario.Cur.Event.OnCityFall?.Invoke(this, atk, Scenario.Cur);

        }

        /// <summary>
        /// 获取城市之间的距离
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int Distance(City other)
        {
            Dictionary<City, int> _distanceMap = new Dictionary<City, int>();
            List<City> _openList = new List<City>
            {
                this
            };
            _distanceMap.Add(this, 0);
            while (_openList.Count > 0)
            {
                City current = _openList[0];
                _openList.RemoveAt(0);
                int currentLen = _distanceMap[current];
                if (current.NeighborList != null)
                {
                    for (int i = 0, count = current.NeighborList.Count; i < count; i++)
                    {
                        City c = current.NeighborList[i];
                        if (c == other)
                        {
                            return _distanceMap[current] + 1;
                        }
                        if (!_distanceMap.ContainsKey(c))
                        {
                            _distanceMap.Add(c, currentLen + 1);
                            _openList.Add(c);
                        }
                    }
                }
            }
            return 0;
        }

        public void OnPersonReturnCity(Person person)
        {
            Sango.Log.Print($"[{person.BelongForce.Name}]{person.Name}回到[{BelongForce.Name}]<{Name}>");
        }
        public void OnPersonTransformEnd(Person person)
        {

        }

        public Building BuildBuilding(Cell buildCenter, Troop builder, BuildingType buildingType)
        {
            Building building = new Building();
            building.BelongForce = BelongForce;
            building.BelongCorps = BelongCorps;
            building.BelongCity = this;
            building.BuildingType = buildingType;
            building.x = buildCenter.x;
            building.y = buildCenter.y;
            building.durability = 0;

            Scenario scenario = Scenario.Cur;

            // TODO: 获取高度
            // ------
            scenario.Add(building);
            building.Init(scenario);
            building.Builders = null;
            building.isComplte = false;
            building.durability = 0;
            building.ChangeDurability(GameUtility.Method_TroopBuildAbility(builder), null);

#if SANGO_DEBUG
            Sango.Log.Print($"[{BelongForce.Name}]在<{Name}>由{builder.Name}开始修建: {building.Name}");
#endif
            building.Render.UpdateRender();
            return building;
        }

        public int GetEmptySlot()
        {
            for (int i = 0; i < innerSlot.Length; ++i)
                if (innerSlot[i] <= 0)
                    return i;
            return -1;
        }

        public Building BuildBuilding(int slotId, Person[] builders, BuildingType buildingType)
        {
            Building building = new Building();
            building.BelongForce = BelongForce;
            building.BelongCorps = BelongCorps;
            building.BelongCity = this;
            building.BuildingType = buildingType;
            building.SlotId = slotId;
            building.durability = 0;

            Scenario scenario = Scenario.Cur;
#if SANGO_DEBUG
            StringBuilder stringBuilder = new StringBuilder();
#endif
            // TODO: 获取高度
            // ------
            scenario.buildingSet.Add(building);
            building.Init(scenario);
            SangoObjectList<Person> sangoObjectList = new SangoObjectList<Person>();
            foreach (Person person in builders)
            {
                if (person == null) continue;
#if SANGO_DEBUG
                stringBuilder.Append(person.Name);
                stringBuilder.Append(" ");
#endif
                person.SetMission(MissionType.PersonBuild, building, 0);
                person.ActionOver = true;
                freePersons.Remove(person);
                sangoObjectList.Add(person);
            }

            building.Builders = sangoObjectList;
            building.isComplte = false;
            building.durability = 1;
            gold -= buildingType.cost;

#if SANGO_DEBUG
            Sango.Log.Print($"@内政@[{BelongForce.Name}]在<{Name}>由{stringBuilder}开始修建: {building.Name}");
#endif
            return building;
        }

        public Building UpgradeBuilding(Building building, Person[] builders, BuildingType upgradeBuildingType)
        {
            building.isUpgrading = true;
            building.durability = 1;

            Scenario scenario = Scenario.Cur;
#if SANGO_DEBUG
            StringBuilder stringBuilder = new StringBuilder();
#endif
            // TODO: 获取高度
            // ------
            SangoObjectList<Person> sangoObjectList = new SangoObjectList<Person>();
            foreach (Person person in builders)
            {
                if (person == null) continue;
#if SANGO_DEBUG
                stringBuilder.Append(person.Name);
                stringBuilder.Append(" ");
#endif
                person.SetMission(MissionType.PersonBuild, building, 0);
                person.ActionOver = true;
                freePersons.Remove(person);
                sangoObjectList.Add(person);
            }

            building.Builders = sangoObjectList;
            gold -= upgradeBuildingType.cost;

#if SANGO_DEBUG
            Sango.Log.Print($"@内政@[{BelongForce.Name}]在<{Name}>由{stringBuilder}开始升级建筑: {building.Name}");
#endif
            return building;
        }

        /// <summary>
        /// 农业
        /// </summary>
        /// <param name="personList"></param>
        /// <returns></returns>
        public bool JobFarming(Person[] personList)
        {
            if (personList == null || personList.Length == 0) return false;
            if (agriculture >= AgricultureLimit) return false;
            Scenario scenario = Scenario.Cur;
            ScenarioVariables variables = scenario.Variables;
            int jobId = (int)CityJobType.Farming;

            // TODO: 特性对开发的影响
            int goldNeed = variables.jobCost[jobId];

            scenario.Event.OnCityCheckJobCost?.Invoke(this, jobId, personList,
                goldNeed, (x) => { goldNeed = x; });

            if (gold < goldNeed)
                return false;

            int meritGain = variables.jobMaxPersonCount[jobId];
            int techniquePointGain = variables.jobTechniquePoint[jobId];

#if SANGO_DEBUG
            StringBuilder stringBuilder = new StringBuilder();
#endif
            int totalValue = 0;
            for (int i = 0; i < personList.Length; i++)
            {
                Person person = personList[i];
                if (person == null) continue;

                totalValue += person.BaseAgricultureAbility;
                person.merit += meritGain;
                person.GainExp(meritGain);
                freePersons.Remove(person);
#if SANGO_DEBUG
                stringBuilder.Append(person.Name);
                stringBuilder.Append(",");
#endif
                person.ActionOver = true;
            }

            totalValue = GameUtility.Method_FarmingAbility(totalValue);

            scenario.Event.OnCityJobResult?.Invoke(this, jobId, personList,
                totalValue, (x) => { totalValue = x; });

            scenario.Event.OnCityJobGainTechniquePoint?.Invoke(this, jobId, personList,
                techniquePointGain, (x) => { techniquePointGain = x; });

            BelongForce.GainTechniquePoint(techniquePointGain);
            gold -= goldNeed;
            agriculture += totalValue;
            if (agriculture > AgricultureLimit)
                agriculture = AgricultureLimit;

#if SANGO_DEBUG
            Sango.Log.Print($"@内政@[{BelongForce.Name}]{stringBuilder}对<{Name}>进行了开垦!农业值达到了:{agriculture}");
#endif
            return true;
        }




        /// <summary>
        /// 开发
        /// </summary>
        /// <param name="personList"></param>
        /// <returns></returns>
        public bool JobDevelop(Person[] personList)
        {
            if (personList == null || personList.Length == 0) return false;
            if (commerce >= CommerceLimit) return false;
            Scenario scenario = Scenario.Cur;

            ScenarioVariables variables = scenario.Variables;
            int jobId = (int)CityJobType.Develop;

            // TODO: 特性对开发的影响
            int goldNeed = variables.jobCost[jobId];

            scenario.Event.OnCityCheckJobCost?.Invoke(this, jobId, personList,
               goldNeed, (x) => { goldNeed = x; });

            if (gold < goldNeed)
                return false;

            int meritGain = variables.jobMaxPersonCount[jobId];
            int techniquePointGain = variables.jobTechniquePoint[jobId];

#if SANGO_DEBUG
            StringBuilder stringBuilder = new StringBuilder();
#endif
            int totalValue = 0;
            for (int i = 0; i < personList.Length; i++)
            {
                Person person = personList[i];
                if (person == null) continue;

                totalValue += person.BaseCommerceAbility;
                person.merit += meritGain;
                person.GainExp(meritGain);

                freePersons.Remove(person);
#if SANGO_DEBUG
                stringBuilder.Append(person.Name);
                stringBuilder.Append(",");
#endif
                person.ActionOver = true;
            }

            totalValue = GameUtility.Method_DevelopAbility(totalValue);

            scenario.Event.OnCityJobResult?.Invoke(this, jobId, personList,
                totalValue, (x) => { totalValue = x; });

            scenario.Event.OnCityJobGainTechniquePoint?.Invoke(this, jobId, personList,
                techniquePointGain, (x) => { techniquePointGain = x; });

            BelongForce.GainTechniquePoint(techniquePointGain);
            gold -= goldNeed;
            commerce += totalValue;
            if (commerce > CommerceLimit)
                commerce = CommerceLimit;

#if SANGO_DEBUG
            Sango.Log.Print($"@内政@[{BelongForce.Name}]{stringBuilder}对<{Name}>进行了开发!商业值达到了:{commerce}");
#endif
            return true;
        }

        /// <summary>
        /// 治安巡视
        /// </summary>
        /// <param name="personList"></param>
        /// <returns></returns>
        public bool JobInspection(Person[] personList)
        {
            if (personList == null || personList.Length == 0) return false;
            if (security >= 100) return false;

            Scenario scenario = Scenario.Cur;

            int barracksLv = GetIntriorBuildingComplateTotalLevel((int)BuildingKindType.PatrolBureau);
            if (barracksLv == 0) return false;

            ScenarioVariables variables = scenario.Variables;
            int jobId = (int)CityJobType.Inspection;

            int goldNeed = variables.jobCost[jobId];

            scenario.Event.OnCityCheckJobCost?.Invoke(this, jobId, personList,
                goldNeed, (x) => { goldNeed = x; });


            if (gold < goldNeed)
                return false;

            int meritGain = variables.jobMaxPersonCount[jobId];
            int techniquePointGain = variables.jobTechniquePoint[jobId];

#if SANGO_DEBUG
            StringBuilder stringBuilder = new StringBuilder();
#endif
            int totalValue = 0;
            for (int i = 0; i < personList.Length; i++)
            {
                Person person = personList[i];
                if (person == null) continue;

                totalValue += person.BaseSecurityAbility;
                person.merit += meritGain;
                person.GainExp(meritGain);

                freePersons.Remove(person);
#if SANGO_DEBUG
                stringBuilder.Append(person.Name);
                stringBuilder.Append(",");
#endif
                person.ActionOver = true;
            }

            // 最终数值
            totalValue = GameUtility.Method_SecurityAbility(totalValue, barracksLv);

            // 
            scenario.Event.OnCityJobResult?.Invoke(this, jobId, personList,
                  totalValue, (x) => { totalValue = x; });

            scenario.Event.OnCityJobGainTechniquePoint?.Invoke(this, jobId, personList,
                techniquePointGain, (x) => { techniquePointGain = x; });

            BelongForce.GainTechniquePoint(techniquePointGain);
            gold -= goldNeed;
            security += totalValue;
            if (security > 100)
                security = 100;

#if SANGO_DEBUG
            Sango.Log.Print($"@内政@[{BelongForce.Name}]{stringBuilder}对<{Name}>进行了巡视!治安提升到了:{security}");
#endif
            return true;
        }

        /// <summary>
        /// 训练
        /// </summary>
        /// <param name="personList"></param>
        /// <returns></returns>
        public bool JobTrainTroop(Person[] personList)
        {
            if (personList == null || personList.Length == 0) return false;
            if (morale >= 100) return false;
            Scenario scenario = Scenario.Cur;

            int barracksLv = GetIntriorBuildingComplateTotalLevel((int)BuildingKindType.Barracks);
            if (barracksLv == 0) return false;

            ScenarioVariables variables = scenario.Variables;
            int jobId = (int)CityJobType.TrainTroop;

            int goldNeed = variables.jobCost[jobId];

            scenario.Event.OnCityCheckJobCost?.Invoke(this, jobId, personList,
                goldNeed, (x) => { goldNeed = x; });

            if (gold < goldNeed)
                return false;

            int meritGain = variables.jobMaxPersonCount[jobId];
            int techniquePointGain = variables.jobTechniquePoint[jobId];

#if SANGO_DEBUG
            StringBuilder stringBuilder = new StringBuilder();
#endif
            int totalValue = 0;
            for (int i = 0; i < personList.Length; i++)
            {
                Person person = personList[i];
                if (person == null) continue;

                totalValue += person.BaseTrainTroopAbility;
                person.merit += meritGain;
                person.GainExp(meritGain);

                freePersons.Remove(person);
#if SANGO_DEBUG
                stringBuilder.Append(person.Name);
                stringBuilder.Append(",");
#endif
                person.ActionOver = true;
            }

            // 最终数值
            totalValue = GameUtility.Method_TrainTroop(totalValue, barracksLv);

            scenario.Event.OnCityJobResult?.Invoke(this, jobId, personList,
                 totalValue, (x) => { totalValue = x; });

            scenario.Event.OnCityJobGainTechniquePoint?.Invoke(this, jobId, personList,
                techniquePointGain, (x) => { techniquePointGain = x; });

            BelongForce.GainTechniquePoint(techniquePointGain);
            gold -= goldNeed;
            morale += totalValue;
            if (morale > 100)
                morale = 100;

#if SANGO_DEBUG
            Sango.Log.Print($"@内政@[{BelongForce.Name}]{stringBuilder}对<{Name}>进行了训练!士气提升到了:{morale}");
#endif
            return true;
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="personList"></param>
        /// <returns></returns>
        public bool JobSearching(Person[] personList)
        {
            if (personList == null || personList.Length == 0) return false;

            Scenario scenario = Scenario.Cur;

            ScenarioVariables variables = scenario.Variables;
            int jobId = (int)CityJobType.Searching;

            int goldNeed = variables.jobCost[jobId];

            scenario.Event.OnCityCheckJobCost?.Invoke(this, jobId, personList,
             goldNeed, (x) => { goldNeed = x; });

            if (gold < goldNeed)
                return false;


            int meritGain = variables.jobMaxPersonCount[jobId];
            int techniquePointGain = variables.jobTechniquePoint[jobId];

            for (int i = 0; i < personList.Length; i++)
            {
                Person person = personList[i];
                if (person == null) continue;

                int ability_improve = (int)person.BaseSearchingAbility / 50;
                freePersons.Remove(person);
                // 发现人才
                if (wildPersons.Count > 0)
                {
                    Person wild_dest = null;
                    for (int j = 0; j < wildPersons.Count; j++)
                    {
                        Person wild = wildPersons[j];
                        if (wild != null && wild.IsAlive && wild.IsValid && !wild.beFinded)
                        {
                            wild_dest = wild;
                            break;
                        }
                    }

                    if (wild_dest != null)
                    {
                        scenario.Event.OnCityJobSearchingWild?.Invoke(this, jobId, person,
                           ability_improve, (x) => { ability_improve = x; });

                        if (GameRandom.Changce(10 * ability_improve))
                        {
#if SANGO_DEBUG
                            Sango.Log.Print($"@内政@[{BelongForce.Name}]<{Name}>的{person.Name}发现了人才->{wild_dest.Name}");
#endif
                            wild_dest.beFinded = true;
                            person.merit += meritGain;
                            person.GainExp(meritGain);

                            person.ActionOver = true;
                        }
                    }
                }

                //TODO: 搜索道具
                //if (!person.ActionOver && GameRandom.Changce((int)(3 * ability_improve)))
                //{
                //    person.ActionOver = true;
                //    continue;
                //}


                // 搜索钱财
                if (!person.ActionOver && GameRandom.Changce((int)(10 * ability_improve)))
                {
                    person.merit += meritGain;
                    person.GainExp(meritGain);

                    person.ActionOver = true;
                }

                //TODO: 触发事件

                // 什么也没找到
                person.merit += meritGain;
                person.GainExp(meritGain);

                person.ActionOver = true;
            }

            scenario.Event.OnCityJobGainTechniquePoint?.Invoke(this, jobId, personList,
                techniquePointGain, (x) => { techniquePointGain = x; });

            BelongForce.GainTechniquePoint(techniquePointGain);
            return true;
        }

        /// <summary>
        /// 治疗伤兵
        /// </summary>
        /// <returns></returns>
        public bool JobHealingTroop()
        {
            // 城池满了不再招募

            if (woundedTroops <= 0) return false;
            int recruitNum = agriculture + commerce;
            int rs = Math.Min(woundedTroops, recruitNum);
            troops += rs;
            woundedTroops -= rs;
#if SANGO_DEBUG
            Sango.Log.Print($"@内政@[{BelongForce.Name}]<{Name}>进行了士兵治愈!共治愈到{rs}人, 当前士兵提升到了:{troops}");
#endif
            return true;
        }

        /// <summary>
        /// 招募
        /// </summary>
        /// <param name="personList"></param>
        /// <returns></returns>
        public bool JobRecuritTroop(Person[] personList)
        {
            int barracksNum = GetIntriorBuildingComplateTotalLevel((int)BuildingKindType.Barracks);
            if (barracksNum <= 0) return true;
            return JobRecuritTroop(personList, barracksNum);
        }


        /// <summary>
        /// 招募
        /// </summary>
        /// <param name="personList"></param>
        /// <returns></returns>
        public bool JobRecuritTroop(Person[] personList, int barracksNum)
        {

            if (personList == null || personList.Length == 0) return false;
            // 城池满了不再招募
            if (TroopsIsFull) return false;

            Scenario scenario = Scenario.Cur;

            if (scenario.Variables.populationEnable && troopPopulation <= 500) return false;
            if (security < 60) return false;
            if (troops > food) return false;

            ScenarioVariables variables = scenario.Variables;
            int jobId = (int)CityJobType.TrainTroop;

            int goldNeed = variables.jobCost[jobId];

            scenario.Event.OnCityCheckJobCost?.Invoke(this, jobId, personList,
                goldNeed, (x) => { goldNeed = x; });

            if (gold < goldNeed)
                return false;

            int meritGain = variables.jobMaxPersonCount[jobId];
            int techniquePointGain = variables.jobTechniquePoint[jobId];

#if SANGO_DEBUG
            StringBuilder stringBuilder = new StringBuilder();
            int lastTroops = troops;
#endif
            int totalValue = 0;
            int maxValue = 0;
            Person maxPerson = null;
            for (int i = 0; i < personList.Length; i++)
            {
                Person person = personList[i];
                if (person == null) continue;
                if (person.BaseRecruitmentAbility > maxValue)
                {
                    maxPerson = person;
                    maxValue = person.BaseRecruitmentAbility;
                }

                person.merit += meritGain;
                person.GainExp(meritGain);

                freePersons.Remove(person);
#if SANGO_DEBUG
                stringBuilder.Append(person.Name);
                stringBuilder.Append(",");
#endif
                person.ActionOver = true;
            }

            // 最高属性武将获得100%加成,其余两个获取50%加成
            for (int i = 0; i < personList.Length; i++)
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

            scenario.Event.OnCityJobResult?.Invoke(this, jobId, personList,
                 totalValue, (x) => { totalValue = x; });

            scenario.Event.OnCityJobGainTechniquePoint?.Invoke(this, jobId, personList,
                techniquePointGain, (x) => { techniquePointGain = x; });

            if (Scenario.Cur.Variables.populationEnable)
            {
                totalValue = Math.Min(totalValue, troopPopulation);
                troopPopulation -= totalValue;
                population -= totalValue;
            }

            if (totalValue + troops > TroopsLimit)
                totalValue = TroopsLimit - troops;

            //士气减少
            morale = (troops * morale + totalValue * 30) / (troops + totalValue);
            troops += totalValue;
            //治安减少
            security -= Math.Min(6, 4 * totalValue / 1000);

            BelongForce.GainTechniquePoint(techniquePointGain);

#if SANGO_DEBUG
            Sango.Log.Print($"@内政@[{BelongForce.Name}]{stringBuilder}对<{Name}>进行了招募!共招募到{troops - lastTroops}人, 当前士兵人数提升到了:{troops}");
#endif
            return true;
        }

        /// <summary>
        /// 生产兵装
        /// </summary>
        /// <param name="personList"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public bool JobCreateItems(Person[] personList, ItemType itemType, int buildingTotalLevel)
        {
            if (personList == null || personList.Length == 0 || itemType == null) return false;
            if (itemStore.TotalNumber >= StoreLimit) return false;

            Scenario scenario = Scenario.Cur;
            int empty = StoreLimit - itemStore.TotalNumber;
            if (empty < 1000) return false;

            ScenarioVariables variables = scenario.Variables;
            int jobId = (int)CityJobType.CreateItems;

            int goldNeed = variables.jobCost[jobId] + itemType.cost;

            scenario.Event.OnCityCheckJobCost?.Invoke(this, jobId, personList,
                 goldNeed, (x) => { goldNeed = x; });

            if (gold < goldNeed)
                return false;

            int meritGain = variables.jobMaxPersonCount[jobId];
            int techniquePointGain = variables.jobTechniquePoint[jobId];

#if SANGO_DEBUG
            StringBuilder stringBuilder = new StringBuilder();
            int lastTroops = troops;
#endif
            int totalValue = 0;
            int maxValue = 0;
            Person maxPerson = null;
            for (int i = 0; i < personList.Length; i++)
            {
                Person person = personList[i];
                if (person == null) continue;
                if (person.BaseCreativeAbility > maxValue)
                {
                    maxPerson = person;
                    maxValue = person.BaseCreativeAbility;
                }
                person.merit += meritGain;
                person.GainExp(meritGain);

                freePersons.Remove(person);
#if SANGO_DEBUG
                stringBuilder.Append(person.Name);
                stringBuilder.Append(",");
#endif
                person.ActionOver = true;
            }

            // 最高属性武将获得100%加成,其余两个获取50%加成
            for (int i = 0; i < personList.Length; i++)
            {
                Person person = personList[i];
                if (person == null) continue;
                if (person != maxPerson)
                {
                    totalValue += maxPerson.BaseCreativeAbility * 5;
                }
                else
                {
                    totalValue += maxPerson.BaseCreativeAbility * 10;
                }
            }

            totalValue = GameUtility.Method_CreateItems(totalValue, buildingTotalLevel);

            scenario.Event.OnCityJobResult?.Invoke(this, jobId, personList,
                 totalValue, (x) => { totalValue = x; });

            scenario.Event.OnCityJobGainTechniquePoint?.Invoke(this, jobId, personList,
                techniquePointGain, (x) => { techniquePointGain = x; });

            totalValue = Math.Min(empty, totalValue);
            int exsistNumber = itemStore.Add(itemType.Id, totalValue);

            BelongForce.GainTechniquePoint(techniquePointGain);

#if SANGO_DEBUG
            Sango.Log.Print($"@内政@[{BelongForce.Name}]{stringBuilder}对<{Name}>进行了生产兵装!共生产了{totalValue}{itemType.Name}, 当前数量:{exsistNumber}");
#endif
            return true;
        }

        /// <summary>
        /// 交易粮食
        /// </summary>
        /// <param name="personList"></param>
        /// <returns></returns>
        public bool JobTradeFood(Person[] personList, int goldNum)
        {
            if (personList == null || personList.Length == 0 || goldNum <= 0) return false;

            Scenario scenario = Scenario.Cur;

            if (security < 60) return false;
            if (troops > food) return false;

            ScenarioVariables variables = scenario.Variables;
            int jobId = (int)CityJobType.TradeFood;

            int meritGain = variables.jobMaxPersonCount[jobId];
            int techniquePointGain = variables.jobTechniquePoint[jobId];

            Person person = personList[0];
            if (person == null) return false;

            person.merit += meritGain;
            person.GainExp(meritGain);
            freePersons.Remove(person);
            person.ActionOver = true;

            int totalValue = GameUtility.Method_Trade(person.Politics);

            scenario.Event.OnCityJobResult?.Invoke(this, jobId, personList,
                 totalValue, (x) => { totalValue = x; });

            scenario.Event.OnCityJobGainTechniquePoint?.Invoke(this, jobId, personList,
                techniquePointGain, (x) => { techniquePointGain = x; });
            // TODO : 城市粮价
            totalValue = totalValue * goldNum * 5 / 100;

            if (totalValue + food > foodLimit)
                totalValue = foodLimit - food;

            food += totalValue;
            gold -= goldNum;
            BelongForce.GainTechniquePoint(techniquePointGain);
#if SANGO_DEBUG
            Sango.Log.Print($"@内政@[{BelongForce.Name}]{person.Name}在<{Name}>花费{goldNum}交易到了{totalValue}粮食, 现有粮食:{food}");
#endif
            return true;
        }

        public bool JobRecuritPerson(Person person, Person dest)
        {
            if (dest.BelongCity == person.BelongCity)
            {
                // 直接招募
                person.JobRecuritPerson(dest);
                freePersons.Remove(person);
                wildPersons.Remove(dest);
            }
            else
            {
                person.SetMission(MissionType.PersonRecruitPerson, dest, Scenario.Cur.GetCityDistance(person.BelongCity, dest.BelongCity));
            }
            return true;
        }

        /// <summary>
        /// 获取城池的攻击力
        /// </summary>
        /// <returns></returns>
        public override int GetAttack()
        {
            ScenarioVariables Variables = Scenario.Cur.Variables;
            // 根据太守数值来计算基础伤害
            int atk = Math.Max(BuildingType.atk, (Leader?.Strength ?? 50 * 5000 + Leader?.Command ?? 50 * 5000) / 10000);

            return atk;
        }

        public override int GetAttackBack()
        {
            ScenarioVariables Variables = Scenario.Cur.Variables;
            // 根据太守数值来计算基础伤害
            int atk = Math.Max(BuildingType.atkBack, (Leader?.Strength ?? 50 * 5000 + Leader?.Command ?? 50 * 5000) / 10000);

            return atk;
        }


        /// <summary>
        /// 获取城池的防御力
        /// </summary>
        /// <returns></returns>
        public override int GetDefence()
        {
            ScenarioVariables Variables = Scenario.Cur.Variables;

            // 根据太守数值来计算基础防御
            int def = Math.Max(50, (Leader?.Intelligence ?? 40 * 3000 + Leader?.Command ?? 40 * 7000) / 10000);

            return def;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public override int GetSkillMethodAvaliabledTroops()
        {
            return Math.Max(5000, durability * troops / DurabilityLimit);
        }

        public struct EnemyInfo
        {
            public Troop troop;
            public int distance;
        }

        protected const int SAVE_ROUND = 15;
        protected List<EnemyInfo> enemies = new List<EnemyInfo>();
        protected bool[] enemiesRound = new bool[SAVE_ROUND];

        public Troop GetNearestEnemy(Cell checkCell)
        {
            Troop target = null;
            int dis = 999999;
            for (int i = 0; i < enemies.Count; i++)
            {
                EnemyInfo enemyInfo = enemies[i];
                int distance = Scenario.Cur.Map.Distance(enemyInfo.troop.cell, checkCell);
                if (distance < dis)
                {
                    target = enemyInfo.troop;
                }
            }
            return target;
        }

        public bool IsEnemiesRound(int round)
        {
            if (round < enemiesRound.Length)
                return enemiesRound[round];
            return false;
        }
        public bool IsEnemiesRound()
        {
            for (int i = 0; i < enemiesRound.Length; i++)
            {
                if (enemiesRound[i]) return true;
            }
            return false;
        }


        public bool CheckEnemiesIfAlive(out EnemyInfo enemyInfo)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                EnemyInfo check = enemies[i];
                if (check.troop.IsAlive)
                {
                    enemyInfo = check;
                    return true;
                }
            }
            enemyInfo = default;
            return false;
        }

        public bool CheckEnemiesIfAlive()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                EnemyInfo check = enemies[i];
                if (check.troop.IsAlive)
                {
                    return true;
                }
            }
            return false;
        }

        /// 这几个属性提供给AI使用
        internal Troop CurActiveTroop = null;
        internal MissionType TroopMissionType = MissionType.None;
        internal int TroopMissionTargetId;

        public Troop EnsureTroop(Troop troop, Scenario scenario)
        {
            // 先加入剧本才能分配ID
            troop.cell = this.CenterCell;
            this.CenterCell.troop = troop;
            scenario.Add(troop);
            troop.Init(scenario);
            return troop;
        }

        public override bool DoAI(Scenario scenario)
        {
            if (AIFinished)
                return true;

            if (!AIPrepared)
            {
                AIPrepare(scenario);
                scenario.Event.OnCityAIStart?.Invoke(this, scenario);
                AIPrepared = true;
            }

            if (CurActiveTroop != null)
            {
                if (!CurActiveTroop.DoAI(scenario))
                    return false;
                CurActiveTroop = null;
            }

            while (AICommandList.Count > 0)
            {
                System.Func<City, Scenario, bool> CurrentCommand = AICommandList[0];
                if (!CurrentCommand.Invoke(this, scenario))
                    return false;

                AICommandList.RemoveAt(0);
            }

            scenario.Event.OnCityAIEnd?.Invoke(this, scenario);
            AIFinished = true;
            ActionOver = true;
            return true;
        }

        public virtual void AIPrepare(Scenario scenario)
        {
            // 准备敌人信息
            enemies.Clear();
            for (int i = 0; i < enemiesRound.Length; i++)
                enemiesRound[i] = false;

            scenario.troopsSet.ForEach(x =>
            {
                if (x.IsEnemy(this))
                {
                    int round = scenario.Map.Distance(CenterCell, x.cell);
                    if (round < SAVE_ROUND)
                    {
                        enemies.Add(new EnemyInfo { troop = x, distance = round });
                        for (int j = round; j < enemiesRound.Length; j++)
                            enemiesRound[j] = true;
                    }
                }
            });

            if (enemies.Count > 1)
            {
                enemies.Sort((a, b) =>
                {
                    return a.distance.CompareTo(b.distance);
                });
            }

            UpdateActiveTroopTypes();
            UpdateFightPower();

            if (IsBorderCity)
            {
                AICommandList.Add(CityAI.AITradeFood);
                AICommandList.Add(CityAI.AIAttack);
                if (scenario.Info.day == 10)
                {
                    AICommandList.Add(CityAI.AIRecuritTroop);
                    AICommandList.Add(CityAI.AICreateItems);
                    AICommandList.Add(CityAI.AIIntrior);
                }
                else if (scenario.Info.day == 20)
                {
                    AICommandList.Add(CityAI.AIIntrior);
                    AICommandList.Add(CityAI.AIRecuritTroop);
                    AICommandList.Add(CityAI.AICreateItems);
                }
                else
                {
                    AICommandList.Add(CityAI.AICreateItems);
                    AICommandList.Add(CityAI.AIRecuritTroop);
                    AICommandList.Add(CityAI.AIIntrior);
                }
            }
            else
            {
                AICommandList.Add(CityAI.AITradeFood);
                // 物资输送
                AICommandList.Add(CityAI.AITransfrom);
                if (troops < itemStore.TotalNumber)
                    AICommandList.Add(CityAI.AIRecuritTroop);
                else
                    AICommandList.Add(CityAI.AICreateItems);
                AICommandList.Add(CityAI.AIIntrior);
            }

            scenario.Event.OnCityAIPrepare?.Invoke(this, scenario);
        }

        /// <summary>
        /// 检查是否太守需要重新设置
        /// </summary>
        /// <param name="person"></param>
        public void CheckIfLoseLeader(Person person)
        {
            if (Leader != person) return;

            Person dest = null;
            Official higher = null;
            int commandHigher = 0;
            for (int i = 0; i < allPersons.Count; i++)
            {
                Person checker = allPersons[i];
                if (checker != null && checker != Leader && checker.IsAlive)
                {
                    if (dest == null)
                    {
                        dest = checker;
                        higher = dest.Official;
                        commandHigher = dest.Command;
                    }
                    else
                    {
                        if (checker.Official.level > higher.level)
                        {
                            dest = checker;
                            higher = dest.Official;
                            commandHigher = dest.Command;
                        }
                        else if (checker.Official.level == higher.level)
                        {
                            if (checker.Command > commandHigher)
                            {
                                dest = checker;
                                higher = dest.Official;
                                commandHigher = dest.Command;
                            }
                        }
                    }
                }
            }
            Leader = dest;
        }

        /// <summary>
        /// 更新太守
        /// </summary>
        /// <param name="person"></param>
        public void UpdateLeader(Person person)
        {
            if (person.BelongCity == this)
            {
                return;
            }

            if (person == BelongForce.Governor)
            {
                person.BelongCity.CheckIfLoseLeader(person);
                Leader = person;
                return;
            }

            if (person.Official.level > Leader.Official.level)
            {
                person.BelongCity.CheckIfLoseLeader(person);
                Leader = person;
                return;
            }
            else if (person.Official.level == Leader.Official.level)
            {
                if (person.Command > Leader.Command)
                {
                    person.BelongCity.CheckIfLoseLeader(person);
                    Leader = person;
                    return;
                }
            }

        }

        /// <summary>
        /// 获取已建造完成的建筑类型数量
        /// </summary>
        /// <param name="buildingKindId"></param>
        /// <returns></returns>
        public int GetIntriorBuildingComplateNumber(int buildingKindId)
        {
            int complateNum = 0;
            for (int i = 0; i < allIntriorBuildings.Count; i++)
            {
                Building building = allIntriorBuildings[i];
                if (building.BuildingType.kind == buildingKindId && building.isComplte)
                {
                    complateNum++;
                }
            }
            return complateNum;
        }

        /// <summary>
        /// 获取已建造完成的建筑类型数量
        /// </summary>
        /// <param name="buildingTypeId"></param>
        /// <returns></returns>
        public int GetIntriorBuildingComplateTotalLevel(int buildingKindId)
        {
            int complateNum = 0;
            for (int i = 0; i < allIntriorBuildings.Count; i++)
            {
                Building building = allIntriorBuildings[i];
                if (building.BuildingType.kind == buildingKindId && building.isComplte)
                {
                    complateNum += building.BuildingType.level;
                }
            }
            return complateNum;
        }

        public int TroopsCount
        {
            get
            {
                int troopsCount = 0;
                SangoObjectSet<Troop> troopSet = Scenario.Cur.troopsSet;
                for (int i = 0; i < troopSet.Count; i++)
                {
                    Troop troop = troopSet[i];
                    if (troop != null && troop.IsAlive && troop.BelongCity == this)
                        troopsCount++;
                }
                return troopsCount;
            }
        }

        public int EnemyCount
        {
            get
            {
                return enemies.Count;
            }
        }
    }
}
