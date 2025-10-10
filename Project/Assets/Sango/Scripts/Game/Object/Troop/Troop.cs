using Newtonsoft.Json;
using Sango.Game.Render;
using Sango.Tools;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Troop : SangoObject
    {
        public override SangoObjectType ObjectType { get { return SangoObjectType.Troops; } }
        public virtual bool AIFinished { get; set; }
        public virtual bool AIPrepared { get; set; }
        /// <summary>
        /// 所属势力
        /// </summary>
        public Force BelongForce => Leader.BelongForce;

        /// <summary>
        /// 所属势力
        /// </summary>
        public Corps BelongCorps => Leader.BelongCorps;

        /// <summary>
        /// 所属城池
        /// </summary>
        public City BelongCity => Leader.BelongCity;

        /// <summary>
        /// 统领
        /// </summary>
        [JsonConverter(typeof(Id2ObjConverter<Person>))]
        [JsonProperty]
        public Person Leader { get; set; }

        /// <summary>
        /// 副将1
        /// </summary>
        [JsonConverter(typeof(Id2ObjConverter<Person>))]
        [JsonProperty]
        public Person Member1 { get; set; }

        /// <summary>
        /// 副将2
        /// </summary>
        [JsonConverter(typeof(Id2ObjConverter<Person>))]
        [JsonProperty]
        public Person Member2 { get; set; }

        /// <summary>
        /// 部队类型
        /// </summary>
        [JsonConverter(typeof(Id2ObjConverter<TroopType>))]
        [JsonProperty]
        public TroopType TroopType { get; set; }

        /// <summary>
        /// 兵种适应力
        /// </summary>
        public int TroopTypeLv { get; private set; }

        /// <summary>
        /// 俘虏
        /// </summary>
        [JsonConverter(typeof(SangoObjectListIDConverter<Person>))]
        [JsonProperty]
        public SangoObjectList<Person> captiveList = new SangoObjectList<Person>();

        /// <summary>
        /// 部队名
        /// </summary>
        public override string Name => Leader.Name;

        /// <summary>
        /// 所在格子
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(XY2CellConverter))]
        public Cell cell;

        /// <summary>
        /// 坐标x
        /// </summary>
        public int x => cell?.x ?? 0;

        /// <summary>
        /// 坐标y
        /// </summary>
        public int y => cell?.y ?? 0;

        /// <summary>
        /// 当前兵力
        /// </summary>
        [JsonProperty] public int troops;

        public int MaxTroops => Leader.TroopsLimit;
        public bool IsFull => troops >= MaxTroops;

        /// <summary>
        /// 当前伤兵
        /// </summary>
        [JsonProperty] public int woundedTroops;

        /// <summary>
        /// 战意
        /// </summary>
        [JsonProperty]
        public int energy;

        /// <summary>
        /// 士气
        /// </summary>
        [JsonProperty]
        public int morale;

        /// <summary>
        /// 移动能力
        /// </summary>
        public int MoveAbility => TroopType.move;

        /// <summary>
        /// 携带粮食
        /// </summary>
        [JsonProperty] public int food;

        /// <summary>
        /// 携带金钱
        /// </summary>
        [JsonProperty] public int gold;

        /// <summary>
        /// 携带人口
        /// </summary>
        [JsonProperty] public int population;

        /// <summary>
        /// 携带道具
        /// </summary>
        [JsonProperty] public ItemStore itemStore = new ItemStore();

        /// <summary>
        /// 是否行动完毕
        /// </summary>
        [JsonProperty] public override bool ActionOver { get; set; }

        /// <summary>
        /// 当前任务类型
        /// </summary>
        [JsonProperty] public int missionType;

        /// <summary>
        /// 任务目标
        /// </summary>
        [JsonProperty] public int missionTarget;

        /// <summary>
        /// 当前技能
        /// </summary>
        public List<Skill> skills = new List<Skill>();


        public int SpearLv { get; private set; }
        public int HalberdLv { get; private set; }
        public int CrossbowLv { get; private set; }
        public int HorseLv { get; private set; }
        public int WaterLv { get; private set; }
        public int MachineLv { get; private set; }

        /// <summary>
        /// 攻击力
        /// </summary>
        public int Attack { get; private set; }

        /// <summary>
        /// 防御力
        /// </summary>
        public int Defence { get; private set; }

        /// <summary>
        /// 建设力
        /// </summary>
        public int BuildPower { get; private set; }

        /// <summary>
        /// 统率
        /// </summary>
        public int Command { get; private set; }

        /// <summary>
        /// 武力
        /// </summary>
        public int Strength { get; private set; }

        /// <summary>
        /// 智力
        /// </summary>
        public int Intelligence { get; private set; }

        /// <summary>
        /// 政治
        /// </summary>
        public int Politics { get; private set; }

        /// <summary>
        /// 魅力
        /// </summary>
        public int Glamour { get; private set; }


        public TroopRender Render { get; private set; }
        bool isMissionPrepared = false;
        public int foodCost = 0;

        public override void Init(Scenario scenario)
        {
            CalculateAttribute();
            Render = new TroopRender(this);
        }

        public virtual bool Run(Corps corps, Force force, Scenario scenario)
        {
            return false;
            //yield return Event.OnTroopsStart?.Invoke(this, corps, force, scenario);
            //yield return Event.OnTroopsAI?.Invoke(this, corps, force, scenario);
            //yield return Event.OnTroopsEnd?.Invoke(this, corps, force, scenario);
        }
        public override void OnScenarioPrepare(Scenario scenario)
        {
            foreach (Person person in captiveList)
            {
                if (person.BelongForce != null)
                    person.BelongForce.CaptiveList.Add(person);
            }
            foodCost = (int)System.Math.Ceiling(scenario.Variables.baseFoodCostInTroop * (troops + woundedTroops) * TroopType.foodCostFactor);
            //MemberList?.InitCache();// = new SangoObjectList<Person>().FromString(_memberListStr, scenario.personSet);
        }
        public override bool OnTurnStart(Scenario scenario)
        {
            ActionOver = false;
            AIFinished = false;
            AIPrepared = false;
            isMissionPrepared = false;
            if (food <= 0)
            {
                // 伤兵直接抛弃
                woundedTroops = 0;
                // 减少士气
                morale = (int)Math.Ceiling(morale * 0.5f);
                if (morale < 0)
                    morale = 0;
                if (troops < 500)
                {
                    Clear();
                }
                else
                {
                    // 每回合死亡当前兵力的30%;
                    int damage = (int)Math.Ceiling(troops * 0.3f);
                    troops -= damage;
                }
            }
            else
            {
                foodCost = (int)System.Math.Ceiling(scenario.Variables.baseFoodCostInTroop * (troops + woundedTroops) * TroopType.foodCostFactor);
                ChangeFood(-foodCost);
            }

            if (Render != null)
            {
                Render.UpdateRender();
            }

            return true;
        }

        public int IsWithOutFood()
        {
            if (food == 0) return 0;
            if (food < foodCost * 3) return 1;
            return 2;
        }

        public void ForEachMember(Action<Person> action)
        {
            if (Member1 != null) action(Member1);
            if (Member2 != null) action(Member2);
        }

        public void ForEachPerson(Action<Person> action)
        {
            action(Leader);
            if (Member1 != null) action(Member1);
            if (Member2 != null) action(Member2);
        }

        /// <summary>
        /// 计算属性
        /// </summary>
        public void CalculateAttribute()
        {

            // 计算能力,能力取最大
            ForEachPerson((p) =>
            {
                Command = Math.Max(Command, p.Command);
                Strength = Math.Max(Strength, p.Strength);
                Intelligence = Math.Max(Intelligence, p.Intelligence);
                Politics = Math.Max(Politics, p.Politics);
                Glamour = Math.Max(Glamour, p.Glamour);
                TroopTypeLv = Math.Max(TroopTypeLv, CheckTroopTypeLevel(TroopType, p));
            });

            // 准备技能
            for (int i = 0; i < TroopType.skills.Count; i++)
            {
                Skill skill = Scenario.Cur.GetObject<Skill>(TroopType.skills[i]);
                if (skill != null && skill.CanAddToTroop(this))
                    skills.Add(skill);
            }

            // 防御力 = (70%统率+30%智力) * 兵种防御力 / 100 
            Defence = (Command * 7000 + Intelligence * 3000) * TroopType.def / 1000000;

            // 攻击力 = (70%武力+30%统率) * 兵种攻击力 / 100 
            Attack = (Strength * 7000 + Command * 3000) * TroopType.atk / 1000000;

            // 建设能力 = 政治 * 67% + 50;
            BuildPower = Politics * 6700 / 10000 + 50;

            // 计算粮耗
            foodCost = (int)System.Math.Ceiling(Scenario.Cur.Variables.baseFoodCostInTroop * (troops + woundedTroops) * TroopType.foodCostFactor);
        }

        public int MoveCost(Cell cell)
        {
            return TroopType.MoveCost(cell);
        }
        public bool IsAlliance(BuildingBase other)
        {
            return IsAlliance(BelongForce, other.BelongForce);
        }

        public bool IsEnemy(BuildingBase other)
        {
            return IsEnemy(BelongForce, other.BelongForce);
        }

        public bool IsSameForce(BuildingBase other)
        {
            return IsSameForce(BelongForce, other.BelongForce);
        }

        public bool IsAlliance(Troop other)
        {
            return IsAlliance(BelongForce, other.BelongForce);
        }

        public bool IsEnemy(Troop other)
        {
            return IsEnemy(BelongForce, other.BelongForce);
        }

        public bool IsSameForce(Troop other)
        {
            return IsSameForce(BelongForce, other.BelongForce);
        }

        public float GetAttackBackFactor(Skill skill, int distance)
        {
            if (skill.IsRange() && distance > 1)
                return 0;
            else if (!skill.IsRange() && distance == 1)
                return 0.5f;
            return 0;
        }

        public void AttackBuilding(Cell buildingInCell)
        {

        }

        public void AttackBuilding(BuildingBase buildingBase)
        {

        }


        //@param attacker Troops
        //@param defender Troops
        //@param skill Skill
        public static int CalculateSkillDamage(Troop attacker, Troop target, Skill skill)
        {
            var attack_troops_type = attacker.TroopType;
            var defender_troops_type = target.TroopType;

            ScenarioVariables Variables = Scenario.Cur.Variables;

            //基础伤害
            float base_dmg = (Variables.fight_base_damage * (
                (attacker.Strength * Variables.fight_base_strength_damage_factor) +
                (attacker.Intelligence * Variables.fight_base_intelligence_damage_factor)) +
                attack_troops_type.atk - defender_troops_type.def);

            //兵力加成系数
            var troops_add = (attacker.troops - Variables.fight_base_troops_need) / Variables.fight_base_troop_count * Variables.fight_base_troop_factor_per_count;

            //基础减伤
            var base_reduce = Variables.fight_base_reduce_percent * target.Command / 100f;

            var damage = base_dmg * (1 + troops_add) * (1 - base_reduce);

            //士气矫正后的伤害
            damage = damage * (UnityEngine.Mathf.Max(attacker.morale - Variables.fight_morale_decay_below, 0) / (100 - Variables.fight_morale_decay_below) *
            Variables.fight_morale_add + (1 - Variables.fight_morale_decay_percent) + UnityEngine.Mathf.Min(UnityEngine.Mathf.Max(attacker.morale, 0), Variables.fight_morale_decay_below) / Variables.fight_morale_decay_below * Variables.fight_morale_decay_percent);


            if (skill != null)
            {
                damage = damage * ((float)skill.atk / 100f);
            }

            damage = damage * CalculateTroopsLevelBoost(attacker, attack_troops_type);
            damage = damage * CalculateRestrainBoost(attack_troops_type, defender_troops_type);
            if (skill != null)
            {
                float crit_P;
                if (CalculateSkillCriticalBoost(attacker, target, skill, out crit_P))
                    damage = damage * crit_P;

            }

            return (int)System.Math.Ceiling(damage);
        }

        public static int CalculateSkillDamage(Troop attacker, BuildingBase target, Skill skill)
        {
            var attack_troops_type = attacker.TroopType;

            ScenarioVariables Variables = Scenario.Cur.Variables;

            //基础伤害
            float base_dmg = (Variables.fight_base_durability_damage * (
                (attacker.Strength * Variables.fight_durability_base_strength_damage_factor) +
                (attacker.Intelligence * Variables.fight_durability_base_intelligence_damage_factor)) +
                attack_troops_type.durabilityDmg);

            //兵力加成系数
            var troops_add = (attacker.troops - Variables.fight_base_troops_need) / Variables.fight_base_troop_count * Variables.fight_base_troop_factor_per_count;

            //基础减伤
            var base_reduce = Variables.fight_base_reduce_percent * target.GetBaseCommand() / 100f;

            var damage = base_dmg * (1 + troops_add) * (1 - base_reduce);

            //士气矫正后的伤害
            damage = damage * (UnityEngine.Mathf.Max(attacker.morale - Variables.fight_morale_decay_below, 0) / (100 - Variables.fight_morale_decay_below) *
            Variables.fight_morale_add + (1 - Variables.fight_morale_decay_percent) + UnityEngine.Mathf.Min(attacker.morale, Variables.fight_morale_decay_below) / Variables.fight_morale_decay_below * Variables.fight_morale_decay_percent);


            if (skill != null)
            {
                damage = damage * ((float)skill.atkDurability / 100f);
            }

            damage = damage * CalculateTroopsLevelBoost(attacker, attack_troops_type);
            if (skill != null)
            {
                float crit_P;
                if (CalculateSkillCriticalBoost(attacker, target, skill, out crit_P))
                    damage = damage * crit_P;

            }

            return (int)System.Math.Ceiling(damage);
        }
        public static int CalculateSkillDamage(BuildingBase attacker, Troop target, Skill skill)
        {

            ScenarioVariables Variables = Scenario.Cur.Variables;

            //基础伤害
            float base_dmg = attacker.GetBaseDamage();

            //基础减伤
            var base_reduce = Variables.fight_base_reduce_percent * target.Command / 100f;

            var damage = base_dmg * (1 - base_reduce);

            if (skill != null)
            {
                damage = damage * (1 + (float)skill.atk / 100f);
            }

            return (int)System.Math.Ceiling(damage);
        }

        // 暴击判断
        public static bool CalculateSkillCriticalBoost(Troop attacker, Troop defender, Skill skill, out float p)
        {
            //TODO 完善暴击逻辑
            p = 1;
            return false;
        }

        // 暴击判断
        public static bool CalculateSkillCriticalBoost(Troop attacker, BuildingBase defender, Skill skill, out float p)
        {
            //TODO 完善暴击逻辑
            p = 1;
            return false;
        }

        // 克制系数
        public static float CalculateRestrainBoost(TroopType attack_troops_type, TroopType defender_troops_type)
        {
            ScenarioVariables Variables = Scenario.Cur.Variables;
            if (attack_troops_type.Id < 0 || attack_troops_type.Id > Variables.troops_type_restraint.Length)
                return 1;
            float[] t_map = Variables.troops_type_restraint[attack_troops_type.Id];
            if (defender_troops_type.Id < 0 || defender_troops_type.Id > t_map.Length)
                return 1;
            return t_map[defender_troops_type.Id];
        }

        public static int CheckTroopTypeLevel(TroopType troopType, Person person)
        {
            switch (troopType.influenceAbility)
            {
                case (int)AbilityType.Spear:
                    return person.SpearLv;
                case (int)AbilityType.Halberd:
                    return person.HalberdLv;
                case (int)AbilityType.Water:
                    return person.WaterLv;
                case (int)AbilityType.Crossbow:
                    return person.CrossbowLv;
                case (int)AbilityType.Horse:
                    return person.HorseLv;
                case (int)AbilityType.Machine:
                    return person.MachineLv;
            }
            return 0;
        }


        // 克制系数
        //@param attacker Troops
        public static float CalculateTroopsLevelBoost(Troop attacker, TroopType attack_troops_type)
        {
            ScenarioVariables Variables = Scenario.Cur.Variables;
            attack_troops_type = attack_troops_type ?? attacker.TroopType;
            int abilityValue = -1;
            switch (attack_troops_type.influenceAbility)
            {
                case (int)AbilityType.Spear:
                    abilityValue = attacker.SpearLv;
                    break;
                case (int)AbilityType.Halberd:
                    abilityValue = attacker.HalberdLv;
                    break;
                case (int)AbilityType.Water:
                    abilityValue = attacker.WaterLv;
                    break;
                case (int)AbilityType.Crossbow:
                    abilityValue = attacker.CrossbowLv;
                    break;
                case (int)AbilityType.Horse:
                    abilityValue = attacker.HorseLv;
                    break;
                case (int)AbilityType.Machine:
                    abilityValue = attacker.MachineLv;
                    break;
            }

            if (abilityValue < 0 || abilityValue > Variables.troops_adaptation_level_boost.Length)
                return 1;

            return Variables.troops_adaptation_level_boost[abilityValue];
        }

        internal static List<Cell> tempCellList = new List<Cell>(256);
        internal static List<Cell> tempMoveRange = new List<Cell>(256);
        internal static List<TroopMoveEvent> tempMoveEventList = new List<TroopMoveEvent>(32);
        internal static List<Cell> spellRangeCells = new List<Cell>(256);
        internal bool isMoving = false;
        IRenderEventBase moveRenderEvent = null;
        IRenderEventBase skillRenderEvent = null;

        public bool MoveTo(Cell destCell)
        {
            if (destCell == cell)
            {
                moveRenderEvent = null;
                isMoving = false;
                return true;
            }

            if (moveRenderEvent != null && moveRenderEvent.IsDone)
            {
                moveRenderEvent = null;
                isMoving = false;
                return true;
            }

            if (!isMoving)
            {
                //if (!destCell.IsEmpty())
                //{
                //    UnityEngine.Debug.LogError("not empty cell!!");
                //}
                tempCellList.Clear();
                tempMoveEventList.Clear();
                //TODO: 移动
                Scenario.Cur.Map.GetMovePath(this, destCell, tempCellList);

                isMoving = true;
                Cell start = cell;
                for (int i = 1; i < tempCellList.Count; i++)
                {
                    bool isLast = i == tempCellList.Count - 1;
                    Cell dest = tempCellList[i];
                    TroopMoveEvent @event = new TroopMoveEvent()
                    {
                        troop = this,
                        dest = dest,
                        start = start,
                        isLastMove = isLast
                    };

                    if (isLast)
                        moveRenderEvent = @event;

                    RenderEvent.Instance.Add(@event);
                    start = dest;
                }

                if (moveRenderEvent == null)
                {
                    isMoving = false;
                    return true;
                }
            }
            return false;

            //if (tempMoveEventList.Count == 0)
            //{
            //    Sango.Log.Print($"{BelongForce.Name}的[{Name} 部队 移动到=> ({x},{y})]");
            //    isMoving = false;
            //    return true;
            //}

            //if (!Render.IsVisible())
            //{
            //    while (tempMoveEventList.Count > 0)
            //    {
            //        TroopMoveEvent @event = tempMoveEventList[0];
            //        UpdateCell(@event.dest, @event.start, tempMoveEventList.Count == 1);
            //        tempMoveEventList.RemoveAt(0);
            //    }
            //    isMoving = false;
            //    return true;
            //}
            //else
            //{
            //    TroopMoveEvent @event = tempMoveEventList[0];
            //    if (@event.Update(UnityEngine.Time.deltaTime))
            //    {
            //        UpdateCell(@event.dest, @event.start, tempMoveEventList.Count == 1);
            //        tempMoveEventList.RemoveAt(0);
            //    }
            //    return false;
            //}
        }

        public bool TryMoveToSpell(Cell destCell, Skill skill)
        {
            if (!isMoving)
            {
                tempCellList.Clear();
                tempMoveEventList.Clear();
                spellRangeCells.Clear();
                //TODO: 移动
                Scenario.Cur.Map.GetMovePath(this, destCell, tempCellList);

                isMoving = true;
                Cell start = cell;
                for (int i = 1; i < tempCellList.Count; i++)
                {
                    Cell dest = tempCellList[i];

                    bool findSpell = false;
                    skill.GetSpellRange(this, cell, spellRangeCells);
                    for (int k = 0; k < spellRangeCells.Count; k++)
                    {
                        Cell spellCell = spellRangeCells[k];
                        if (spellCell == destCell)
                        {
                            findSpell = true;
                            break;
                        }
                    }

                    RenderEvent.Instance.Add(new TroopMoveEvent()
                    {
                        troop = this,
                        dest = dest,
                        start = start,
                        isLastMove = i == tempCellList.Count - 1
                    });
                    start = dest;

                    if (findSpell)
                    {
                        break;
                    }
                }
            }

            //if (tempMoveEventList.Count == 0)
            //{
            //    Sango.Log.Print($"{BelongForce.Name}的[{Name} 部队 移动到=> ({x},{y})]");
            //    isMoving = false;
            //    return true;
            //}

            //if (!Render.IsVisible())
            //{
            //    while (tempMoveEventList.Count > 0)
            //    {
            //        TroopMoveEvent @event = tempMoveEventList[0];
            //        UpdateCell(@event.dest, @event.start, tempMoveEventList.Count == 1);
            //        tempMoveEventList.RemoveAt(0);
            //    }
            //    return false;
            //}
            //else
            //{
            //    TroopMoveEvent @event = tempMoveEventList[0];
            //    if (@event.Update(UnityEngine.Time.deltaTime))
            //    {
            //        UpdateCell(@event.dest, @event.start, tempMoveEventList.Count == 1);
            //        tempMoveEventList.RemoveAt(0);
            //    }
            //    return false;
            //}

            return false;

        }

        //public bool MoveToClosest(Cell destCell)
        //{
        //    if (!isMoving)
        //    {
        //        tempCellList.Clear();
        //        tempMoveEventList.Clear();
        //        //TODO: 移动
        //        Scenario.Cur.Map.GetClosestMovePath(this, destCell, tempCellList);
        //        isMoving = true;
        //        Cell start = cell;
        //        for (int i = 1; i < tempCellList.Count; i++)
        //        {
        //            Cell dest = tempCellList[i];
        //            tempMoveEventList.Add(new TroopMoveEvent()
        //            {
        //                troop = this,
        //                dest = dest,
        //                start = start,
        //                isLastMove = i == tempCellList.Count - 1
        //            });
        //            start = dest;
        //        }
        //    }

        //    if (tempMoveEventList.Count == 0)
        //    {
        //        Sango.Log.Print($"{BelongForce.Name}的[{Name} 部队 移动到=> ({x},{y})]");
        //        isMoving = false;
        //        return true;
        //    }
        //    TroopMoveEvent @event = tempMoveEventList[0];
        //    if (!@event.IsVisible() || @event.Update(UnityEngine.Time.deltaTime))
        //    {
        //        UpdateCell(@event.dest, @event.start, tempMoveEventList.Count == 1);
        //        tempMoveEventList.RemoveAt(0);
        //    }

        //    return false;
        //}


        public bool ChangeFood(int num)
        {
            food += num;
            if (food < 0)
            {
                food = 0;
                return true;
            }
            return false;
        }

        public bool ChangeTroops(int num, SangoObject atk)
        {
            if (Render != null)
                Render.ShowDamage(num, 2);

            troops = troops + num;
            if (num < 0)
            {
                int absNum = Math.Abs(num);
                woundedTroops += (int)Math.Ceiling(absNum * 0.14f);
                int _foodCost = (int)System.Math.Ceiling(Scenario.Cur.Variables.baseFoodCostInTroop * absNum * TroopType.foodCostFactor) / 2;
                int divFood = 0;
                // 有概率保留部分
                if (GameRandom.Changce(80))
                    divFood += _foodCost;
                if (GameRandom.Changce(50))
                    divFood += _foodCost;
                ChangeFood(-divFood);
            }

            IsAlive = troops > 0;
            if (!IsAlive)
            {
                // 移除
                Clear();
                Sango.Log.Print($"{BelongForce.Name}的[{Name} 部队 溃灭!!");
            }

            if (Render != null)
            {
                Render.UpdateRender();
            }

            return IsAlive;
        }

        public int GetTroopsNum()
        {
            return troops;
        }

        public bool SpellSkill(Skill skill, Cell spellCell)
        {
            if (skillRenderEvent != null)
            {
                if (skillRenderEvent.IsDone)
                {
                    skillRenderEvent = null;
                    return true;
                }
                else
                    return false;
            }

            TroopSpellSkillEvent @event = new TroopSpellSkillEvent()
            {
                troop = this,
                skill = skill,
                spellCell = spellCell,
            };
            skillRenderEvent = @event;
            RenderEvent.Instance.Add(@event);

            return false;
        }
        Cell tryToDest;
        public bool TryMoveTo(Cell destCell)
        {
            if (!isMoving)
            {      //TODO: 尝试移动
                tempCellList.Clear();
                tryToDest = null;
                //TODO: 移动
                Scenario.Cur.Map.GetDirectMovePath(this, destCell, tempCellList);
                int totaleMoveAbility = MoveAbility;
                int checkIndex = 0;
                for (int i = 1; i < tempCellList.Count; i++)
                {
                    Cell dest = tempCellList[i];
                    int destCost = MoveCost(dest);
                    if (totaleMoveAbility > destCost && !Scenario.Cur.Map.IsZOC(this, dest))
                    {
                        totaleMoveAbility -= destCost;
                    }
                    else
                    {
                        checkIndex = i;
                        break;
                    }
                }

                for (int i = checkIndex - 1; i >= 1; i--)
                {
                    Cell dest = tempCellList[i];
                    if (dest.IsEmpty())
                    {
                        tryToDest = dest;
                        break;
                    }
                }
            }

            if (tryToDest == null)
                return true;

            return MoveTo(tryToDest);
        }
        public bool TryCloseTo(Cell destCell)
        {
            if (!isMoving)
            {      //TODO: 尝试移动
                tempCellList.Clear();
                tryToDest = null;

                Map map = Scenario.Cur.Map;
                //TODO: 移动
                map.GetDirectMovePath(this, destCell, tempCellList);
#if SANGO_DEBUG_AI
                GameAIDebug.Instance.ShowTargetDirectPath(tempCellList, this);
#endif

                int totaleMoveAbility = MoveAbility;
                for (int i = 1; i < tempCellList.Count; i++)
                {
                    Cell dest = tempCellList[i];
                    int destCost = MoveCost(dest);
                    if (totaleMoveAbility > destCost)
                    {
                        if (map.IsZOC(this, dest))
                        {
                            totaleMoveAbility = 0;
                        }
                        else
                        {
                            totaleMoveAbility -= destCost;
                        }
                        tryToDest = dest;
                    }
                    else
                    {
                        break;
                    }
                }

                if (tryToDest != null)
                {
                    List<Cell> temp = new List<Cell>();
                    map.GetMoveRange(this, temp);
                    PriorityQueue<Cell> nearnestCellInMoveRange = new PriorityQueue<Cell>();
                    for (int i = 0; i < temp.Count; i++)
                    {
                        Cell cell = temp[i];
                        if (cell.IsEmpty())
                        {
                            nearnestCellInMoveRange.Push(cell, map.Distance(cell, tryToDest));
                        }
                    }
                    tryToDest = nearnestCellInMoveRange.Lower();
                }
            }

            if (tryToDest == null)
                return true;

#if SANGO_DEBUG_AI

            if (GameAIDebug.Instance.WaitForTargetDirectPath())
                return false;
#endif


            return MoveTo(tryToDest);
        }
        public bool TryMoveToCity(City city)
        {
            if (!isMoving)
            {      //TODO: 尝试移动
                tempCellList.Clear();
                tryToDest = null;

                tempMoveRange.Clear();

                // 先检查移动范围内是否可达目标
                Map map = Scenario.Cur.Map;
                map.GetMoveRange(this, tempMoveRange);
                for (int i = 1; i < tempMoveRange.Count; ++i)
                {
                    Cell cell = tempMoveRange[i];
                    if (cell.building == city)
                    {
                        tryToDest = cell;
                        break;
                    }
                }

                if (tryToDest == null)
                {
                    //TODO: 移动
                    map.GetDirectMovePath(this, city.CenterCell, tempCellList);

                    int totaleMoveAbility = MoveAbility;
                    for (int i = 1; i < tempCellList.Count; i++)
                    {
                        Cell dest = tempCellList[i];
                        int destCost = MoveCost(dest);
                        if (totaleMoveAbility > destCost)
                        {
                            if (map.IsZOC(this, dest))
                            {
                                totaleMoveAbility = 0;
                            }
                            else
                            {
                                totaleMoveAbility -= destCost;
                            }
                            tryToDest = dest;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (tryToDest != null)
                    {
                        //List<Cell> temp = new List<Cell>();
                        //map.GetMoveRange(this, temp);
                        PriorityQueue<Cell> nearnestCellInMoveRange = new PriorityQueue<Cell>();
                        for (int i = 0; i < tempMoveRange.Count; i++)
                        {
                            Cell cell = tempMoveRange[i];
                            if (cell.IsEmpty())
                            {
                                nearnestCellInMoveRange.Push(cell, map.Distance(cell, tryToDest));
                            }
                        }
                        tryToDest = nearnestCellInMoveRange.Lower();
                    }
                }
            }

            if (tryToDest == null)
            {
                return true;
            }

            return MoveTo(tryToDest);
        }

        public void UpdateCell(Cell destCell, Cell lastCell, bool isEndMove)
        {
            //TODO: 地格更新,需要处理一些事件
            if (lastCell != null)
                ScenarioEvent.Event.OnTroopLeaveCell?.Invoke(lastCell, destCell);

            ScenarioEvent.Event.OnTroopEnterCell?.Invoke(cell, lastCell);
#if SANGO_DEBUG
            Sango.Log.Print($"{BelongForce.Name}的[{Name} 部队 移动=> ({destCell.x},{destCell.y})]");
#endif
            if (isEndMove)
            {
                destCell.troop = this;
                cell.troop = null;
                cell = destCell;
                if (Render.MapObject != null)
                {
                    Render.MapObject.position = cell.Position;
                }
                //else
                //{
                //    Sango.Log.Error($"why {Name}->Render.MapObject is null");
                //}
            }
        }

        public void EnterCity(City city)
        {
            cell.troop = null;
            city.gold += gold;
            city.food += food;
            city.troops += troops;
            city.woundedTroops += woundedTroops;
            // 设置了,
            IsAlive = false;

            Scenario.Cur.Remove(this);
            ForEachPerson((person) =>
            {
                person.BelongTroop = null;
                person.ActionOver = true;
            });
           
            Render.Clear();
            ActionOver = true;
            if (city == BelongCity)
            {
                Sango.Log.Print($"{BelongForce.Name}的[{Name}]部队回到{city.BelongForce?.Name}的城池:<{city.Name}>");
                return;
            }
            ForEachPerson((person) =>
            {
                person.ChangeCity(city);
            });

            Sango.Log.Print($"{BelongForce.Name}的[{Name}]部队进入{city.BelongForce?.Name}的城池:<{city.Name}>");
        }

        public override void Clear()
        {
            Scenario.Cur.Remove(this);
            ForEachPerson((person) =>
            {
                person.BelongTroop = null;
            });
            base.Clear();
            IsAlive = false;
            ActionOver = true;
            Render.Clear();
            if (cell != null && cell.troop == this)
                cell.troop = null;
        }

        public void RemovePerson(Person person)
        {
            if(person == null) return;

            if (Member1 == person)
            {
                Member1.BelongTroop = null;
                Member1 = null;
            }
            else if (Member2 == person)
            {
                Member2.BelongTroop = null;
                Member2 = null;
            }
        }

        /// <summary>
        /// 加入某个势力,需要指定一个城市
        /// </summary>
        /// <param name="city"></param>
        public bool JoinToForce(City city)
        {
            if(Leader.IsSameForce(city)) return true;
            ForEachMember(mem =>
            {
                RemovePerson(mem);
                mem.SetMission(MissionType.PersonReturn, mem.BelongCity, 1);
                mem.ActionOver = true;
            });
            CalculateAttribute();
            return true;
        }

        public void OnPersonChangeCity(Person person, City old_city, City new_city)
        {

        }


        public List<Cell> MoveRange = new List<Cell>();
        public void SetMission(MissionType missionType, int missionTarget)
        {
            this.missionType = (int)missionType;
            this.missionTarget = missionTarget;
        }

        TroopMissionBehaviour troopMissionBehaviour;
        public TroopMissionBehaviour TroopMissionBehaviour
        {
            get
            {
                if (missionType == 0)
                {
                    missionType = (int)MissionType.ReturnCity;
                    missionTarget = BelongCity.Id;
                    NeedPrepareMission();
                }

                if (this.troopMissionBehaviour == null || (int)troopMissionBehaviour.MissionType != missionType)
                {
                    switch (missionType)
                    {
                        case (int)MissionType.DestroyTroop:
                            troopMissionBehaviour = new TroopDestroyTroop();
                            break;
                        case (int)MissionType.DestroyBuilding:
                            troopMissionBehaviour = new TroopDestroyBuilding();
                            break;
                        case (int)MissionType.OccupyCity:
                            troopMissionBehaviour = new TroopOccupyCity();
                            break;
                        case (int)MissionType.BanishTroop:
                            troopMissionBehaviour = new TroopBanishTroop();
                            break;
                        case (int)MissionType.ProtectBuilding:
                            troopMissionBehaviour = new TroopProtectBuilding();
                            break;
                        case (int)MissionType.ProtectTroop:
                            troopMissionBehaviour = new TroopProtectTroop();
                            break;
                        case (int)MissionType.ProtectCity:
                            troopMissionBehaviour = new TroopProtectCity();
                            break;
                        case (int)MissionType.MovetoCity:
                            troopMissionBehaviour = new TrooprMovetoCity();
                            break;
                        case (int)MissionType.ReturnCity:
                            troopMissionBehaviour = new TroopReturnCity();
                            break;
                        default:
                            troopMissionBehaviour = new TroopReturnCity();
                            break;
                    }
                    isMissionPrepared = false;
                    troopMissionBehaviour.Prepare(this, Scenario.Cur);
                    isMissionPrepared = true;
                }
                return troopMissionBehaviour;
            }
        }

        public void NeedPrepareMission()
        {
            isMissionPrepared = false;
        }

        public override bool DoAI(Scenario scenario)
        {
            if (AIFinished)
                return true;

            if (!AIPrepared)
            {
                AIPrepare(scenario);
                AIPrepared = true;
                scenario.Event.OnTroopAIStart?.Invoke(this, scenario);
            }

            TroopMissionBehaviour temp = TroopMissionBehaviour;
            if (!isMissionPrepared)
            {
                temp.Prepare(this, scenario);
                isMissionPrepared = true;
            }
#if SANGO_DEBUG_AI
            if (GameAIDebug.Instance.WaitForShowAIPrepare())
                return false;
#endif

            if (!TroopMissionBehaviour.DoAI(this, scenario))
                return false;

            scenario.Event.OnTroopAIEnd?.Invoke(this, scenario);
            AIFinished = true;
            ActionOver = true;
            return true;
        }

        public void AIPrepare(Scenario scenario)
        {
            // 永不退缩
            //if ((morale <= 5 && GameRandom.Changce(60)) ||
            //    (troops < 500 && GameRandom.Changce(80)) ||
            //    food < (int)System.Math.Ceiling(scenario.Variables.baseFoodCostInTroop * (troops + woundedTroops) * TroopType.foodCostFactor) * 3 && GameRandom.Changce(80)
            //    )
            //{
            //    missionType = (int)MissionType.ReturnCity;
            //    missionTarget = BelongCity.Id;
            //}
        }

    }
}
