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
            if (GameRandom.Chance(p1, 10000))
            {
                Fire fire = spellCell.fire;
                if (fire == null)
                {
                    fire = new Fire()
                    {
                        damage = 300,
                        intelligence = troop.Intelligence,
                        cell = spellCell,
                        counter = GameRandom.Range(2, Math.Max(2, troop.Intelligence / 20))
                    };
                    Scenario.Cur.Add(fire);
                    fire.Init(Scenario.Cur);
                }
                else
                {
                    fire.intelligence = troop.Intelligence;
                    fire.counter = GameRandom.Range(2, Math.Max(2, troop.Intelligence / 20));
                }

                spellCell.fire = fire;
                fire.Action();
            }
        }
    }
}
