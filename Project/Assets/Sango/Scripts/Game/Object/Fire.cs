using Newtonsoft.Json;
using Sango.Game.Render;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Fire : SangoObject
    {
        [JsonProperty]
        public int intelligence;

        [JsonProperty]
        public int counter;

        /// <summary>
        /// 所在格子
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(XY2CellConverter))]
        public Cell cell;

        public FireRender Render { get; private set; }

        public override void Init(Scenario scenario)
        {
            Render = new FireRender(this);
        }

        public override void OnScenarioPrepare(Scenario scenario)
        {

        }

        public override bool OnTurnStart(Scenario scenario)
        {
            ActionOver = false;
            counter--;
            if (counter <= 0)
            {
                cell.fire = null;
                Render.Clear();
                Render = null;
                scenario.fireSet.Remove(this);
                ActionOver = true;
                IsAlive = false;
            }
            return true;
        }

        public override bool OnTurnEnd(Scenario scenario)
        {
            if (!ActionOver)
            {
                Action();
            }
            return true;
        }

        public void Action()
        {
            if (cell.troop != null)
            {
                BurnTroop(cell.troop);
            }
            else if (cell.building != null)
            {
                BurnBuildiong(cell.building);
            }
            ActionOver = true;
        }
        public void BurnTroop(Troop troop)
        {
            if (troop == null) return;

            int dmg = intelligence * 4;
            troop.ChangeTroops(-dmg, this, false);
            FireDamageEvent @event = new FireDamageEvent()
            {
                fire = this,
                targetTroop = troop,
                damage = -dmg
            };
            RenderEvent.Instance.Add(@event);
        }

        public void BurnTroopFast(Troop troop)
        {
            if (troop == null) return;

            int dmg = intelligence * 4;
            troop.ChangeTroops(-dmg, this, false);
            FireDamageEvent @event = new FireDamageEvent()
            {
                fire = this,
                targetTroop = troop,
                damage = -dmg,
                actTime = 0f
            };
            RenderEvent.Instance.Add(@event);
        }

        public void BurnBuildiong(BuildingBase building)
        {
            if (building == null) return;

            // 火焰不能决定城池归属
            int dmg = intelligence * 2;
            if (building.durability < dmg)
                dmg = building.durability - 1;
            if (dmg > 0)
            {
                building.ChangeDurability(-dmg, this, false);
                FireDamageEvent @event = new FireDamageEvent()
                {
                    fire = this,
                    targetBuilding = building,
                    damage = -dmg
                };
                RenderEvent.Instance.Add(@event);
            }
        }

    }
}
