using Sango.Render;
using System.Collections.Generic;
using UnityEngine;


namespace Sango.Game.Render
{
    public class TroopSpellSkillEvent : RenderEventBase
    {
        public Troop troop;
        public Skill skill;
        public Cell spellCell;
        private bool isAction = false;
        private float time = 0;
        public override void Enter(Scenario scenario)
        {
            isAction = false;
            time = 0;
            if (IsVisible())
            {
                troop.Render.SetSmokeShow(true);
            }
            if (skill.costEnergy > 0)
                troop.Render.ShowSkill(skill);
        }

        public override void Exit(Scenario scenario)
        {
            if (IsVisible())
            {
                troop.Render.SetSmokeShow(false);
            }
        }

        public override bool IsVisible()
        {
            return troop.Render.IsVisible();
        }

        public override bool Update(Scenario scenario, float deltaTime)
        {
            if (!IsVisible())
            {
                Action();
                IsDone = true;
                return IsDone;
            }
            IsDone = skill.UpdateRender(troop, spellCell, scenario, time, Action);
            time += deltaTime;
            return IsDone;
        }


        public void Action()
        {
            if (isAction) return;

            Troop targetTroop = spellCell.troop;
            BuildingBase targetBuilding = spellCell.building;
            //TODO: 释放技能
            Troop.tempCellList.Clear();
            skill.GetAttackCells(troop, spellCell, Troop.tempCellList);
            int targetDamage = 0;
            for (int i = 0; i < Troop.tempCellList.Count; i++)
            {
                Cell atkCell = Troop.tempCellList[i];
                Troop beAtkTroop = atkCell.troop;
                if (beAtkTroop != null && skill.canDamageTroop && (troop.IsEnemy(beAtkTroop) || skill.canDamageTeam))
                {
                    int damage = Troop.CalculateSkillDamage(troop, beAtkTroop, skill);
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
                    Sango.Log.Print($"{troop.BelongForce.Name}的[{troop.Name} - {troop.TroopType.Name}] 使用<{skill.Name}> 攻击 {beAtkTroop.BelongForce.Name}的[{beAtkTroop.Name} - {beAtkTroop.TroopType.Name}], 造成伤害:{damage}, 目标剩余兵力: {beAtkTroop.GetTroopsNum()}");
#endif
                    // 反击
                    if (beAtkTroop.IsAlive && targetTroop == beAtkTroop)
                    {
                        targetDamage = damage;
                        float hitBack = beAtkTroop.GetAttackBackFactor(skill, Scenario.Cur.Map.Distance(troop.cell, atkCell));
                        if (hitBack > 0)
                        {
                            if (skill.IsRange())
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
                                if (!troop.IsAlive) ep += 50;
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
                if (beAtkBuildingBase != null && skill.canDamageBuilding && (troop.IsEnemy(beAtkBuildingBase) || skill.canDamageTeam))
                {
                    int damage = Troop.CalculateSkillDamage(troop, beAtkBuildingBase, skill);
#if SANGO_DEBUG
                    Sango.Log.Print($"{troop.BelongForce.Name}的[{troop.Name} - {troop.TroopType.Name}] 使用<{skill.Name}> 攻击 {beAtkBuildingBase.BelongForce?.Name}的 [{beAtkBuildingBase.Name}], 造成伤害:{damage}, 目标剩余耐久: {beAtkBuildingBase.durability}");
#endif
                    int ep = damage / 10;
                    if (beAtkBuildingBase is City)
                    {
                        City city = (City)beAtkBuildingBase;
                        if (city.BelongForce == null || city.troops <= 0)
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

                        Sango.Log.Print($"{troop.BelongForce.Name}的[{troop.Name} - {troop.TroopType.Name}] 攻破城池: <{beAtkBuildingBase.Name}>");
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
                            float hitBack = beAtkBuildingBase.GetAttackBackFactor(skill, Scenario.Cur.Map.Distance(troop.cell, atkCell));
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

            if (targetTroop != null && skill.offsetAction != null && skill.offsetAction.Length > 1)
            {
                List<Cell> checkList = new List<Cell>();
                int dir = troop.cell.DirectionTo(targetTroop.cell);
                for (int k = 0; k < skill.offsetAction.Length; k += 2)
                {
                    int offsetType = skill.offsetAction[k];
                    int offsetLength = skill.offsetAction[k + 1];
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
                                            if (skill.blockFactor > 0 && blockTroop != null && blockTroop.IsEnemy(troop))
                                            {
                                                int blockDmg = targetDamage * skill.blockFactor / 100;
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
                                            if (skill.blockFactor > 0 && blockTroop != null && blockTroop.IsEnemy(troop))
                                            {
                                                int blockDmg = targetDamage * skill.blockFactor / 100;
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

            if (troop.IsAlive)
            {
                troop.morale -= skill.costEnergy;
                if (troop.morale < 0)
                    troop.morale = 0;
                troop.Render.UpdateRender();
            }
            isAction = true;
        }

    }
}
