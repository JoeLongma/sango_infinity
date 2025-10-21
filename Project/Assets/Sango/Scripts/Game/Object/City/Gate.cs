using Newtonsoft.Json;
using Sango.Game.Render;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Gate : City
    {
        /// <summary>
        /// 所属城池
        /// </summary>
        [JsonConverter(typeof(Id2ObjConverter<City>))]
        [JsonProperty]
        public City BelongCity;

        public override void OnScenarioPrepare(Scenario scenario)
        {
            //x *= 2;
            //y *= 2;

            base.OnScenarioPrepare(scenario);
            //TroopList?.InitCache();// = new SangoObjectList<Troop>().FromString(_troopListStr, scenario.troopSet);
            //NeighborList?.InitCache();// = new SangoObjectList<City>().FromString(_neighborListStr, scenario.citySet);
            //CityLevelType = scenario.CommonData.CityLevelTypes.Get(_cityLevelTypeId);
            innerSlot = new int[InsideSlot];
            if (durability <= 0)
                durability = DurabilityLimit;
            buildingCountMap.Clear();
            // 地格占用
            effectCells = new System.Collections.Generic.List<Cell>();
            scenario.Map.GetSpiral(x, y, BuildingType.radius, cell_list);
            foreach (Cell cell in cell_list)
                cell.building = this;

            effectCells.Clear();
            scenario.Map.GetDirectSpiral(CenterCell, BuildingType.radius + 1, BuildingType.radius + 10, effectCells);

            for (int i = 0; i < effectCells.Count; i++)
            {
                Cell cell = effectCells[i];
                if (cell.HasGridState(Sango.Render.MapGrid.GridState.Defence))
                    defenceCellList.Add(cell);
            }


            foreach (Person person in CaptiveList)
            {
                if (person.BelongForce != null)
                    person.BelongForce.CaptiveList.Add(person);
            }

            Render = new GateRender(this);
        }

    }
}
