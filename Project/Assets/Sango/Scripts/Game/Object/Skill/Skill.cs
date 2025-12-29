using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Skill : SangoObject
    {
        public override SangoObjectType ObjectType { get { return SangoObjectType.Skill; } }

        /// <summary>
        /// 技能类型
        /// </summary>
        [JsonProperty] public int kind;

        /// <summary>
        /// 基础攻击力
        /// </summary>
        [JsonProperty] public int atk;

        /// <summary>
        /// 基础攻城值
        /// </summary>
        [JsonProperty] public int atkDurability;

        /// <summary>
        /// 释放所需能量
        /// </summary>
        [JsonProperty] public int costEnergy;

        /// <summary>
        /// 是否可以对部队造成伤害
        /// </summary>
        [JsonProperty] public bool canDamageTroop;

        /// <summary>
        /// 是否可以对器械造成伤害
        /// </summary>
        [JsonProperty] public bool canDamageMachine;

        /// <summary>
        /// 是否可以对船只造成伤害
        /// </summary>
        [JsonProperty] public bool canDamageBoat;

        /// <summary>
        /// 是否可以对建筑造成伤害
        /// </summary>
        [JsonProperty] public bool canDamageBuilding;

        /// <summary>
        /// 是否可误伤
        /// </summary>
        [JsonProperty] public bool canDamageTeam;

        /// <summary>
        /// 是否可空放
        /// </summary>
        [JsonProperty] public bool canSpellToCell;

        /// <summary>
        /// 是否只可对友军释放
        /// </summary>
        [JsonProperty] public bool onlySpellToTeam;

        /// <summary>
        /// 是否为远程技能
        /// </summary>
        [JsonProperty] public bool isRange;

        /// <summary>
        /// 施法范围
        /// </summary>
        [JsonProperty] public int[] spellRanges;

        /// <summary>
        /// 所需适应等级
        /// </summary>
        [JsonProperty] public int needAblilityLevel;

        /// <summary>
        /// 基础成功率
        /// </summary>
        [JsonProperty] public int successRate;

        /// <summary>
        /// 额外的攻击位置
        /// </summary>
        [JsonProperty] public List<int> atkOffsetPoint;

        /// <summary>
        /// 位移配置
        /// </summary>
        [JsonProperty] public int[] offsetAction;

        /// <summary>
        /// 碰撞伤害系数
        /// </summary>
        [JsonProperty] public int blockFactor;

        /// <summary>
        /// 技能效果
        /// </summary>
        [JsonProperty] public JArray skillEffects;

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
                                spell.Ring(radius, (cell) => { if (cell.moveAble) cells.Add(cell); });
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
                                    if (cell.moveAble)
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
                                atker.cell.Ring(radius, (cell) => { if (cell.moveAble) cells.Add(cell); });
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
                            Cell nCell = atker.cell.GetNrighbor(dir + 1);
                            if (nCell.moveAble)
                                cells.Add(nCell);
                            nCell = atker.cell.GetNrighbor(dir - 1);
                            if (nCell.moveAble)
                                cells.Add(nCell);
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
                                spell.Spiral(radius, (cell) => { if (cell.moveAble) cells.Add(cell); });
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

        public bool IsRange()
        {
            return isRange;
        }

        public bool IsStrategy()
        {
            return kind == 3;
        }

    }
}
