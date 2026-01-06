using Sango.Render;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game
{
    public class GameFormula
    {
        public delegate int RecuritPersonProbabilityCall(Person actor, Person target, int type);
        public delegate bool RecuritPersonProbabilityByRelationshipCall(Person actor, Person target, int type, out int probability);

        public static RecuritPersonProbabilityCall RecuritPersonProbabilityOverride;
        public static RecuritPersonProbabilityByRelationshipCall RecuritPersonProbabilityByRelationshipOverride;

        /// <summary>
        /// 目标武将和可执行武将間是否有特殊关系
        /// 返回是否有特殊关系，是否登用成功
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="target"></param>
        /// <param name="type">0,正常招募 1,俘虏招募 2.破城招募</param>
        /// <param name="probability"></param>
        /// <returns></returns>
        public static bool RecuritPersonProbabilityByRelationship(Person actor, Person target, int type, out int probability)
        {
            if (RecuritPersonProbabilityByRelationshipOverride != null)
                return RecuritPersonProbabilityByRelationshipOverride(actor, target, type, out probability);

            probability = 0;
            if (!target.IsAlive || !actor.IsAlive) return false;

            bool target_is_returnable = target.BelongForce != null;
            //目标武将势力消灭
            if (type == 2)
                target_is_returnable = false;

            //执行武将沒有君主时总是失敗
            if (actor.BelongForce == null) return false;

            // 当目标武将的禁止仕官君主是执行武将君主时，总是失敗
            if (target.bannedForceId == actor.BelongForce.Id)
                return false;

            //目标武将是君主时，总是失敗
            if (target_is_returnable && target == target.BelongForce.Governor)
                return false;

            //目标武将有义兄弟
            if (target.BrotherList != null)
            {
                for (int i = 0; i < target.BrotherList.Count; i++)
                {
                    // 目标武将与义兄弟在同一势力时，总是失敗
                    Person brother = target.BrotherList[i];
                    if (target_is_returnable && brother.BelongForce == target.BelongForce)
                        return false;

                    // 目标武将与执行武将是义兄弟或与执行武将君主时义兄弟时，总是成功
                    if (brother == actor || brother == actor.BelongForce.Governor)
                    {
                        probability = 100;
                        return true;
                    }
                    else if (brother.BelongForce != null && brother.BelongForce == actor.BelongForce)
                    {
                        probability = 100;
                        return true;
                    }
                    //目标武将的义兄弟属於执行武将以外势力时，总是失敗
                    else if (brother.BelongForce != null && brother.BelongForce != actor.BelongForce)
                        return false;

                }
            }

            //目标武将有配偶
            if (target.SpouseList != null)
            {
                for (int i = 0; i < target.SpouseList.Count; i++)
                {
                    // 目标武将与配偶在同一势力时，总是失敗
                    Person spouse = target.SpouseList[i];
                    if (target_is_returnable && spouse.BelongForce == target.BelongForce)
                        return false;
                    //目标武将的配偶属於执行武将以外势力时，总是失敗
                    else if (spouse.BelongForce != null && spouse.BelongForce != actor.BelongForce)
                        return false;
                    //目标武将与执行武将是配偶或与执行武将君主时配偶时，总是成功
                    else if (spouse == actor || spouse == actor.BelongForce.Governor)
                    {
                        probability = 100;
                        return true;
                    }
                    //目标武将的配偶在执行武将势力时，总是成功
                    else if (spouse.BelongForce != null && spouse.BelongForce == actor.BelongForce)
                    {
                        probability = 100;
                        return true;
                    }
                }
            }

            //目标武将有厌恶武将
            if (target.HatePersonList != null)
            {
                for (int i = 0; i < target.HatePersonList.Count; i++)
                {
                    // 目标武将的厌恶武将是执行武将时，总是失敗
                    Person person = target.HatePersonList[i];
                    if (person == actor)
                        return false;
                    //目标武将的厌恶武将是执行武将的君主时，总是失敗
                    else if (person == actor.BelongForce.Governor)
                        return false;
                }
            }


            //目标武将有亲爱武将
            if (target.LikePersonList != null)
            {
                for (int i = 0; i < target.LikePersonList.Count; i++)
                {
                    // 目标武将的亲爱武将是目标武将的君主时，总是失敗
                    Person person = target.LikePersonList[i];
                    if (person == actor)
                        return false;
                    //目标武将的亲爱武将是目标武将的君主时，总是失敗
                    else if (target_is_returnable && person == target.BelongForce.Governor)
                        return false;
                    //目标武将的亲爱武将是执行武将的君主时，总是成功
                    else if (person == actor.BelongForce.Governor)
                    {
                        probability = 100;
                        return true;
                    }
                }
            }

            return false;

        }

        public static int RecuritPersonProbability(Person actor, Person target, int type)
        {
            if (RecuritPersonProbabilityOverride != null)
                return RecuritPersonProbabilityOverride(actor, target, type);

            if (!target.IsAlive || !actor.IsAlive) return 0;

            //执行武将沒有君主时总是失敗
            if (actor.BelongForce == null) return 0;

            //检测是否有特殊关系
            if (RecuritPersonProbabilityByRelationship(actor, target, type, out int p))
                return p;

            int loyalty = target.loyalty;
            if (type == 0)
            {
                // 普通招募武将
                loyalty = target.IsWild ? 0 : target.loyalty;
                if (loyalty + target.argumentation.loyaltyAdd >= Scenario.Cur.Variables.recruitableLine)
                    return 0;
            }

            // 

            Argumentation argumentation = target.argumentation;

            Person actorGovernor = actor.BelongForce.Governor;
            Person targetGovernor = null;
            if (target.BelongForce != null)
                targetGovernor = target.BelongForce.Governor;

            int aishou = 25;
            //目标武将在野或是已灭亡势力的俘虏
            if (target.IsWild)
            {
                loyalty = 60 + Scenario.Cur.Variables.difficulty * 5;
                if (!target.IsPrisoner)
                    //义理_普通
                    argumentation = Scenario.Cur.GetObject<Argumentation>(3);
            }
            // 获取目标相性与君主的距离
            else
            {
                aishou = target.CompatibilityDistance(targetGovernor);
            }
            int n = 45 + (aishou - target.CompatibilityDistance(actorGovernor)) * 3 / 2;
            n -= (argumentation.loyaltyAdd + 18) * loyalty * 5 / 100;
            n += Math.Max(actor.Glamour, 30) * 3 / 5;
            n -= target.IsLike(targetGovernor) ? 15 : 0;
            n -= target.IsParentchild(targetGovernor) ? 15 : 0;
            n += target.IsHate(targetGovernor) ? 15 : 0;
            n += target.IsPrisoner ? 15 : 0;
            n += GameRandom.Range(0, Math.Max(0, 5 - argumentation.loyaltyAdd));
            // 主公魅力影响20%
            n += actorGovernor.Glamour / 5;
            n = Math.Max(n, 0);

            // 第一次发现,成功率+15;
            if (type == 3)
                n += 15;

            int giri = 10;
            if (type != 0)
                giri = Math.Min(15 - argumentation.loyaltyAdd * 2, 10);
            n = Math.Min(n * giri / 10, 100);

            return n;
        }

    }

}
