using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Sango.Game
{
    /// <summary>
    /// 燃烧某个地方
    /// probability : 概率,万分比
    /// condition: 条件
    /// turn : 回合数集合[]
    /// weight : 回合数命中的权重[]
    /// </summary>
    public class Burn : SkillEffect
    {
        Condition condition;
        int probability;
        int [] turn;
        int [] weight;

        public override void Init(JObject p, SkillInstance master)
        {
            base.Init(p, master);
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
            if (!GameRandom.Chance(probability, 10000))
                return;

            if (condition != null && !condition.Check(troop, spellCell.troop, master))
                return;

            int index = GameRandom.RandomWeightIndex(weight);
            int finalCount = turn[index];

            Fire fire = spellCell.fire;
            if (fire == null)
            {
                fire = new Fire()
                {
                    damage = 300,
                    intelligence = troop.Intelligence,
                    cell = spellCell,
                    counter = finalCount
                };
                Scenario.Cur.Add(fire);
                fire.Init(Scenario.Cur);
            }
            else
            {
                fire.intelligence = troop.Intelligence;
                fire.counter = finalCount;
            }

            spellCell.fire = fire;
            fire.Action();
        }
    }
}
