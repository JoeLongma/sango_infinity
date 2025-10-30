using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Skill : SangoObject
    {
        public override SangoObjectType ObjectType { get { return SangoObjectType.Skill; } }

        [JsonProperty] public int atk;
        [JsonProperty] public int atkDurability;
        [JsonProperty] public int costEnergy;
        [JsonProperty] public bool canDamageTroop;
        [JsonProperty] public bool canDamageMachine;
        [JsonProperty] public bool canDamageBoat;
        [JsonProperty] public bool canDamageBuilding;
        [JsonProperty] public bool canDamageTeam;
        [JsonProperty] public bool isRange;
        [JsonProperty] public int level;
        [JsonProperty] public int[] spellRanges;
        [JsonProperty] public int needAblilityLevel;
        [JsonProperty] public int successRate;
        [JsonProperty] public int[] successRateAdd;

        [JsonProperty] public List<int> atkOffsetPoint;

        [JsonProperty] public int[] offsetAction;
        [JsonProperty] public int blockFactor;

        [JsonConverter(typeof(SangoObjectListIDConverter<SkillEffect>))]
        [JsonProperty] public SangoObjectList<SkillEffect> skillEffects;

        //TODO:技能效果配置

        public void GetSpellRange(Troop atker, Cell where, List<Cell> cells)
        {
            if (spellRanges == null || spellRanges.Length == 0)
            {
                cells.Add(where);
            }
            else
            {
                for (int i = 0; i < spellRanges.Length; i++)
                {
                    Scenario.Cur.Map.GetRing(where, spellRanges[i], cells);
                }
            }
        }

        ////从攻击位置反找一个施法位置
        //public void GetSpellCells(Cell atkCell, List<Cell> cells)
        //{

        //}

        public void GetAttackCells(Troop atker, Cell spell, List<Cell> cells)
        {
            if (atkOffsetPoint == null || atkOffsetPoint.Count == 0)
            {
                cells.Add(spell);
            }
            else
            {
                SkillAttackOffsetType aopType = (SkillAttackOffsetType)atkOffsetPoint[0];
                switch (aopType)
                {
                    // 0
                    case SkillAttackOffsetType.Customize:
                        {
                            for (int i = 1; i < atkOffsetPoint.Count; i += 2)
                            {
                                int offsetX = atkOffsetPoint[i];
                                int offsetY = atkOffsetPoint[i + 1];
                                Cell dest = spell.OffsetCell(offsetX, offsetY);
                                if (dest != null) cells.Add(dest);
                            }
                        }
                        break;
                    // 1
                    case SkillAttackOffsetType.Ring:
                        {
                            if (atkOffsetPoint.Count > 1)
                            {
                                int radius = atkOffsetPoint[1];
                                if (radius <= 0)
                                {
                                    cells.Add(spell);
                                    return;
                                }
                                spell.Ring(radius, (cell) => { cells.Add(cell); });
                            }
                            else
                                Sango.Log.Error("技能命中配置不正确!!");
                        }
                        break;
                    // 2
                    case SkillAttackOffsetType.DirectionLine:
                        {
                            if (atkOffsetPoint.Count > 1)
                            {
                                int length = atkOffsetPoint[1];
                                if (length <= 0)
                                {
                                    cells.Add(spell);
                                    return;
                                }

                                atker.cell.DirectionLine(spell, length, (cell) =>
                                {
                                    cells.Add(cell);
                                });
                            }
                            else
                                Sango.Log.Error("技能命中配置不正确!!");
                        }
                        break;
                    //3
                    case SkillAttackOffsetType.SelfRing:
                        {
                            if (atkOffsetPoint.Count > 1)
                            {
                                int radius = atkOffsetPoint[1];
                                if (radius <= 0)
                                {
                                    cells.Add(spell);
                                    return;
                                }
                                atker.cell.Ring(radius, (cell) => { cells.Add(cell); });
                            }
                            else
                                Sango.Log.Error("技能命中配置不正确!!");
                        }
                        break;
                    //4
                    case SkillAttackOffsetType.SpellNeighbors:
                        {
                            int dir = atker.cell.Cub.DirectionTo(spell.Cub);
                            cells.Add(spell);
                            cells.Add(atker.cell.GetNrighbor(dir + 1));
                            cells.Add(atker.cell.GetNrighbor(dir - 1));
                        }
                        break;
                    // 5
                    case SkillAttackOffsetType.Spiral:
                        {
                            if (atkOffsetPoint.Count > 1)
                            {
                                int radius = atkOffsetPoint[1];
                                if (radius <= 0)
                                {
                                    cells.Add(spell);
                                    return;
                                }
                                spell.Spiral(radius, (cell) => { cells.Add(cell); });
                            }
                            else
                                Sango.Log.Error("技能命中配置不正确!!");
                        }
                        break;
                    default:
                        cells.Add(spell);
                        break;
                }
            }
        }

        public bool IsSingleSkill()
        {
            return atkOffsetPoint == null || atkOffsetPoint.Count == 0;
        }

        public bool HasEffect()
        {
            return skillEffects == null || skillEffects.Count == 0;
        }


        public bool CanAddToTroop(Troop troop)
        {
            return troop.TroopTypeLv >= needAblilityLevel;
        }

        public bool CanBeSpell(Troop troop)
        {
            //TODO: 完善技能释放规则
            if (costEnergy > troop.morale)
                return false;

            return true;
        }

        public bool CanSpeellToHere(Troop who, Cell where)
        {
            if (canDamageTroop && where.troop != null && where.troop.IsEnemy(who))
                return true;
            if (canDamageBuilding && where.building != null && where.building.IsEnemy(who))
                return true;
            return false;
        }

        public bool IsRange()
        {
            return isRange;
        }

        /// <summary>
        /// 成功率判断
        /// </summary>
        /// <param name="troop"></param>
        /// <param name="spellCell"></param>
        /// <returns></returns>
        public bool CheckSuccess(Troop troop, Cell spellCell)
        {
            // TODO: 特殊状态必定成功
            int baseSuccessRate = successRate + Math.Max(0, troop.TroopTypeLv - 1) * Scenario.Cur.Variables.skillSuccessRateAddByAbility;

            // TODO: 其他加成
            Tools.OverrideData<int> overrideData = new Tools.OverrideData<int>(baseSuccessRate);
            GameEvent.OnTroopCalculateSkillSuccess?.Invoke(troop, this, spellCell, overrideData);
            baseSuccessRate = overrideData.Value;

#if SANGO_DEBUG
            Sango.Log.Print($"{troop.BelongForce.Name}的[{Name} 部队 准备释放技能: {Name} =>({spellCell.x},{spellCell.y})] 成功率:{baseSuccessRate}");
#endif
            return GameRandom.Chance(baseSuccessRate);
        }

        /*
        *【战法爆击率】
           1、如果有必爆特技则爆击率为100％，无必爆则执行战法爆击率判断。
           2、战法爆击率 ＝ A + B + C
           A：部队武力爆击加成：武力60以下＝0％；武力在60～79之间＝1％；武力大于等于80＝2％
           B：部队适性爆击加成：C＝0％，B＝1％，A＝2％，S＝3％，依次推类
           C：主副将关系爆击加成：
            如果副将亲爱主将＋2％；
            如果副将与主将结义或结婚＋4％；
            如果副将厌恶主将－5％；
           注：每名副将单独计算，即2员副将都亲爱主将＋4％，一仲介一厌恶则－1％；

        * 
        **/
        /// <summary>
        /// 暴击率检查
        /// </summary>
        /// <param name="troop"></param>
        /// <param name="spellCell"></param>
        /// <returns></returns>
        public int CheckCritical(Troop troop, Cell spellCell)
        {
            // TODO: 特殊状态必定暴击
            ScenarioVariables scenarioVariables = Scenario.Cur.Variables;

            int basCriticalRate = scenarioVariables.baseSkillCriticalRate + troop.TroopTypeLv * Scenario.Cur.Variables.skillCriticalRateAddByAbility;
            basCriticalRate += Math.Max(0, (troop.Strength - 60) * scenarioVariables.skillCriticalRateAddByStength / 10);

            // TODO: 其他加成
            Tools.OverrideData<int> overrideData = new Tools.OverrideData<int>(basCriticalRate);
            GameEvent.OnTroopCalculateSkillCritical?.Invoke(troop, this, spellCell, overrideData);
            basCriticalRate = overrideData.Value;

            int criticalFactor = 100;
            if (GameRandom.Chance(basCriticalRate))
            {
                criticalFactor = scenarioVariables.skillCriticalFactor;
                overrideData = new Tools.OverrideData<int>(criticalFactor);
                GameEvent.OnTroopCalculateSkillCriticalFactor?.Invoke(troop, this, spellCell, overrideData);
                criticalFactor = overrideData.Value;
            }

            return criticalFactor;
        }


        public bool UpdateRender(Troop troop, Cell spellCell, Scenario scenario, float time, Action action)
        {
            if (time <= 0f)
            {
                troop.Render.FaceTo(spellCell.Position);
                troop.Render.SetAniShow(1);
            }
            if (time > 1f)
                action();
            if (time > 2.5f)
            {
                troop.Render.SetAniShow(0);
                return true;
            }
            return false;
        }

        List<Cell> tempCellList = new List<Cell>();

        public void Action(Troop troop, Cell spellCell, int criticalFactor)
        {

            ScenarioVariables scenarioVariables = Scenario.Cur.Variables;
            Troop targetTroop = spellCell.troop;
            BuildingBase targetBuilding = spellCell.building;
            //TODO: 释放技能
            tempCellList.Clear();
            GetAttackCells(troop, spellCell, tempCellList);
            int targetDamage = 0;
            for (int i = 0; i < tempCellList.Count; i++)
            {
                Cell atkCell = tempCellList[i];
                Troop beAtkTroop = atkCell.troop;
                if (beAtkTroop != null && this.canDamageTroop && (troop.IsEnemy(beAtkTroop) || this.canDamageTeam))
                {
                    int damage = Troop.CalculateSkillDamage(troop, beAtkTroop, this) * criticalFactor / 100;
                    if (damage < 0)
                    {
                        damage = 0;
                    }
                    beAtkTroop.ChangeTroops(-damage, troop);
                    int ep = damage / 100;
                    if (!beAtkTroop.IsAlive) ep += 50;
                    troop.ForEachPerson(p =>
                    {
                        p.GainExp(ep);
                        p.merit += ep;
                    });
#if SANGO_DEBUG
                    Sango.Log.Print($"{troop.BelongForce.Name}的[{troop.Name} - {troop.TroopType.Name}] 使用<{this.Name}> 攻击 {beAtkTroop.BelongForce.Name}的[{beAtkTroop.Name} - {beAtkTroop.TroopType.Name}], 造成伤害:{damage}, 目标剩余兵力: {beAtkTroop.GetTroopsNum()}");
#endif
                    // 反击
                    if (beAtkTroop.IsAlive && targetTroop == beAtkTroop)
                    {
                        targetDamage = damage;
                        float hitBack = beAtkTroop.GetAttackBackFactor(this, Scenario.Cur.Map.Distance(troop.cell, atkCell));
                        if (hitBack > 0)
                        {
                            if (this.IsRange())
                            {
                                int hitBackDmg = (int)System.Math.Ceiling(hitBack * Troop.CalculateSkillDamage(beAtkTroop, troop, beAtkTroop.NormalRangeSkill?.Skill));
                                troop.ChangeTroops(-hitBackDmg, beAtkTroop);
#if SANGO_DEBUG
                                Sango.Log.Print($"{troop.BelongForce.Name}的[{troop.Name} - {troop.TroopType.Name}] 受到 {beAtkTroop.BelongForce.Name}的[{beAtkTroop.Name} - {beAtkTroop.TroopType.Name}]反击伤害:{hitBackDmg}, 目标剩余兵力: {troop.GetTroopsNum()}");
#endif
                                ep = hitBackDmg / 100;
                                if (!troop.IsAlive) ep += 50;
                                beAtkTroop.ForEachPerson(p =>
                                {
                                    p.GainExp(ep);
                                    p.merit += ep;
                                });
                            }
                            else
                            {
                                int hitBackDmg = (int)System.Math.Ceiling(hitBack * Troop.CalculateSkillDamage(beAtkTroop, troop, beAtkTroop.NormalSkill?.Skill));
                                troop.ChangeTroops(-hitBackDmg, beAtkTroop);
#if SANGO_DEBUG
                                Sango.Log.Print($"{troop.BelongForce.Name}的[{troop.Name} - {troop.TroopType.Name}] 受到 {beAtkTroop.BelongForce.Name}的[{beAtkTroop.Name} - {beAtkTroop.TroopType.Name}]反击伤害:{hitBackDmg}, 目标剩余兵力: {troop.GetTroopsNum()}");
#endif
                                ep = hitBackDmg / 100;
                                if (!troop.IsAlive)
                                {
                                    // 获取对方部分钱粮
                                    int getFood = troop.food * scenarioVariables.defeatTroopCanGainFoodFactor / 100;
                                    int getGold = troop.gold * scenarioVariables.defeatTroopCanGainGoldFactor / 100;
                                    if (getFood > 0)
                                    {
                                        beAtkTroop.ChangeFood(getFood);
                                    }
                                    if (getGold > 0)
                                    {
                                        beAtkTroop.ChangeGold(getGold);
                                    }

                                    ep += 50;
                                }
                                beAtkTroop.ForEachPerson(p =>
                                {
                                    p.GainExp(ep);
                                    p.merit += ep;
                                });

                            }
                        }
                    }
                }

                BuildingBase beAtkBuildingBase = atkCell.building;
                if (beAtkBuildingBase != null && this.canDamageBuilding && (troop.IsEnemy(beAtkBuildingBase) || this.canDamageTeam))
                {
                    int damage = Troop.CalculateSkillDamage(troop, beAtkBuildingBase, this) * criticalFactor / 100;
#if SANGO_DEBUG
                    Sango.Log.Print($"{troop.BelongForce.Name}的[{troop.Name} - {troop.TroopType.Name}] 使用<{this.Name}> 攻击 {beAtkBuildingBase.BelongForce?.Name}的 [{beAtkBuildingBase.Name}], 造成伤害:{damage}, 目标剩余耐久: {beAtkBuildingBase.durability}");
#endif
                    int ep = damage / 10;
                    if (beAtkBuildingBase is City)
                    {
                        City city = (City)beAtkBuildingBase;
                        int damage_troops = Troop.CalculateSkillDamageTroopOnCity(troop, city, this) * criticalFactor / 100;
                        if (!city.ChangeTroops(-damage_troops, troop, city.BelongForce != null))
                        {
                            ep += 100;
                            troop.ForEachPerson(p =>
                            {
                                int ep = damage / 100;
                                p.GainExp(ep);
                                p.merit += ep;
                            });
#if SANGO_DEBUG
                            Sango.Log.Print($"{troop.BelongForce.Name}的[{troop.Name} - {troop.TroopType.Name}] 攻破城池: <{beAtkBuildingBase.Name}>");
#endif
                            city.OnFall(troop);
                            return;
                        }
                    }

                    if (beAtkBuildingBase.ChangeDurability(-damage, troop))
                    {
                        ep += 100;
                        troop.ForEachPerson(p =>
                        {
                            int ep = damage / 100;
                            p.GainExp(ep);
                            p.merit += ep;
                        });
#if SANGO_DEBUG

                        Sango.Log.Print($"{troop.BelongForce.Name}的[{troop.Name} - {troop.TroopType.Name}] 破坏建筑: <{beAtkBuildingBase.Name}>");
#endif
                    }
                    else
                    {
                        troop.ForEachPerson(p =>
                        {
                            int ep = damage / 100;
                            p.GainExp(ep);
                            p.merit += ep;
                        });

                        // 城池反击
                        if (targetBuilding == beAtkBuildingBase)
                        {
                            float hitBack = beAtkBuildingBase.GetAttackBackFactor(this, Scenario.Cur.Map.Distance(troop.cell, atkCell));
                            if (hitBack > 0)
                            {
                                int atkBack = beAtkBuildingBase.GetAttackBack();
                                if (atkBack > 0)
                                {
                                    int hitBackDmg = (int)System.Math.Ceiling(hitBack * Troop.CalculateSkillDamage(beAtkBuildingBase, troop, atkBack));
                                    troop.ChangeTroops(-hitBackDmg, beAtkBuildingBase);
#if SANGO_DEBUG
                                    Sango.Log.Print($"{troop.BelongForce.Name}的[{troop.Name} - {troop.TroopType.Name}] 受到 {beAtkBuildingBase.BelongForce?.Name}的[{beAtkBuildingBase.Name}]反击伤害:{hitBackDmg}, 目标剩余兵力: {troop.GetTroopsNum()}");
#endif
                                }
                            }
                        }
                    }
                }
            }

            DoOffset(troop, targetTroop, targetDamage);

            DoEffect(troop, spellCell, tempCellList);

            if (troop.IsAlive)
            {
                troop.morale -= this.costEnergy;
                if (troop.morale < 0)
                    troop.morale = 0;
                troop.Render.UpdateRender();
            }
        }

        public void DoOffset(Troop troop, Troop targetTroop, int targetDamage)
        {
            if (targetTroop == null || offsetAction == null || offsetAction.Length <= 0) return;
            List<Cell> checkList = new List<Cell>();
            int dir = troop.cell.DirectionTo(targetTroop.cell);
            for (int k = 0; k < this.offsetAction.Length; k += 2)
            {
                int offsetType = this.offsetAction[k];
                int offsetLength = this.offsetAction[k + 1];
                int targetDir = dir;
                int absOffsetLength = Mathf.Abs(offsetLength);
                if (offsetLength < 0)
                    targetDir -= 3;
                checkList.Clear();
                switch (offsetType)
                {
                    case (int)SkillCellOffsetType.Master:
                        {
                            if (troop.IsAlive)
                            {
                                troop.cell.GetDirectionLine(targetDir, absOffsetLength, checkList);
                                for (int i = 0; i < checkList.Count; i++)
                                {
                                    Cell c = checkList[i];
                                    if (c.CanStay(troop))
                                        troop.UpdateCell(c, troop.cell, true);
                                    else
                                        break;
                                }
                            }
                        }
                        break;
                    case (int)SkillCellOffsetType.Target:
                        {
                            if (targetTroop.IsAlive)
                            {
                                targetTroop.cell.GetDirectionLine(targetDir, absOffsetLength, checkList);
                                for (int i = 0; i < checkList.Count; i++)
                                {
                                    Cell c = checkList[i];
                                    if (c.CanStay(targetTroop))
                                        targetTroop.UpdateCell(c, targetTroop.cell, true);
                                    else
                                        break;
                                }
                            }
                        }
                        break;
                    case (int)SkillCellOffsetType.MasterBlock:
                        {
                            if (troop.IsAlive)
                            {
                                troop.cell.GetDirectionLine(targetDir, absOffsetLength, checkList);
                                for (int i = 0; i < checkList.Count; i++)
                                {
                                    Cell c = checkList[i];
                                    if (c.CanStay(troop))
                                        troop.UpdateCell(c, troop.cell, true);
                                    else
                                    {
                                        Troop blockTroop = c.troop;
                                        if (this.blockFactor > 0 && blockTroop != null && blockTroop.IsEnemy(troop))
                                        {
                                            int blockDmg = targetDamage * this.blockFactor / 100;
                                            blockTroop.ChangeTroops(-blockDmg, troop);
                                            int ep = blockDmg / 100;
                                            if (!blockTroop.IsAlive) ep += 50;
                                            troop.ForEachPerson(p =>
                                            {
                                                p.GainExp(ep);
                                                p.merit += ep;
                                            });
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    case (int)SkillCellOffsetType.TargetBlock:
                        {
                            if (targetTroop.IsAlive)
                            {
                                targetTroop.cell.GetDirectionLine(targetDir, absOffsetLength, checkList);
                                for (int i = 0; i < checkList.Count; i++)
                                {
                                    Cell c = checkList[i];
                                    if (c.CanStay(troop))
                                        targetTroop.UpdateCell(c, targetTroop.cell, true);
                                    else
                                    {
                                        Troop blockTroop = c.troop;
                                        if (this.blockFactor > 0 && blockTroop != null && blockTroop.IsEnemy(troop))
                                        {
                                            int blockDmg = targetDamage * this.blockFactor / 100;
                                            blockTroop.ChangeTroops(-blockDmg, troop);
                                            int ep = blockDmg / 100;
                                            if (!blockTroop.IsAlive) ep += 50;
                                            troop.ForEachPerson(p =>
                                            {
                                                p.GainExp(ep);
                                                p.merit += ep;
                                            });
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    case (int)SkillCellOffsetType.MasterJustCheckEnd:
                        {
                            if (troop.IsAlive)
                            {
                                troop.cell.GetDirectionLine(targetDir, absOffsetLength, checkList);
                                if (checkList.Count == 0)
                                {
                                    int cc = 123;
                                    cc++;
                                }
                                for (int i = checkList.Count - 1; i >= 0; i--)
                                {
                                    Cell c = checkList[i];
                                    if (c.CanStay(troop))
                                    {
                                        troop.UpdateCell(c, troop.cell, true);
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }

        public void DoEffect(Troop troop, Cell spellCell, List<Cell> atkCellList)
        {
            if (skillEffects == null || skillEffects.Count == 0) return;

            skillEffects.ForEach(s => s.Action(null, troop, spellCell, atkCellList));

        }
    }
}
