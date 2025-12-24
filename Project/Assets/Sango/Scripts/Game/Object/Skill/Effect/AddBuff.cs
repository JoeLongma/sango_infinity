using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sango.Game.Render;
using UnityEngine;

namespace Sango.Game
{
    /// <summary>
    /// 燃烧某个地方
    /// </summary>
    public class AddBuff : SkillEffect
    {
        Condition condition;
        int probability;
        int[] turn;
        int[] weight;
        int buffId;

        public override void Init(JObject p, Skill master)
        {
            base.Init(p, master);

            buffId = p.Value<int>("buffId");

            JArray array = p.Value<JArray>("turn");
            List<int> list = new List<int>();
            for (int i = 0; i < array.Count; i++)
            {
                list.Add(array[i].Value<int>());
            }
            turn = list.ToArray();

            array = p.Value<JArray>("weight");
            list.Clear();
            for (int i = 0; i < array.Count; i++)
            {
                list.Add(array[i].Value<int>());
            }

            weight = list.ToArray();

            JObject conObj = p.Value<JObject>("condition");
            if (conObj != null)
            {
                condition = Condition.Create(conObj.Value<string>("class"));
                condition.Init(conObj, master);
            }
        }

        public override void Action(SkillInstance skillInstance, Troop troop, Cell spellCell, List<Cell> atkCellList)
        {
            Troop target = spellCell.troop;
            if (target == null) return;

            if (!GameRandom.Chance(probability, 10000))
                return;

            if (condition != null && !condition.Check(troop, target, master))
                return;

            int index = GameRandom.RandomWeightIndex(weight);
            int finalCount = turn[index];

            target.AddBuff(buffId, finalCount, troop);
        }
    }
}
