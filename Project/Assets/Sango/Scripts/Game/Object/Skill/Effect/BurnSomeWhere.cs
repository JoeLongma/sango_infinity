using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sango.Game.Render;
using UnityEngine;

namespace Sango.Game
{
    /// <summary>
    /// 燃烧某个地方
    /// </summary>
    public class BurnSomeWhere : SkillEffect
    {
        public override void Action(SkillInstance skillInstance, Troop troop, Cell spellCell, List<Cell> atkCellList)
        {
            if (GameRandom.Changce(p1, 10000))
            {
                Fire fire = new Fire()
                {
                    intelligence = troop.Intelligence,
                    cell = spellCell,
                    counter = GameRandom.Range(2, Math.Max(2, troop.Intelligence / 20))
                };
                spellCell.fire = fire;
                Scenario.Cur.Add(fire);
                fire.Init(Scenario.Cur);
                fire.Action();
            }
        }
    }
}
