using Sango.Hexagon;
using Sango.Render;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sango.Game
{
    public class Cell
    {
        public Vector2Int coords;
        public int terrainType;
        public int terrainState;
        public int areaId;

        public City BelongCity {  get; internal set; }
        public TerrainType TerrainType { get; set; }
        public Hexagon.Hex Cub { get; set; }// { get { return Hexagon.Coord.OffsetToCube(x, y); } }
        public Vector3 Position { get; set; }// { get { return Scenario.Cur.Map.Coords2Position(x, y); } }
        public int x { get { return coords.x; } }
        public int y { get { return coords.y; } }
        //public float Fertility { get; set; }
        //public float Prosperity { get; set; }

        public Cell[] Neighbors = new Cell[6];

        public Troop troop;
        public BuildingBase building;
        public Fire fire;
        public bool moveAble;
        public MapObject interiorModel;

        internal int _cost = 0;
        internal bool _isZOC = false;
        internal bool _isChecked = false;

        public bool IsInterior => HasGridState(Sango.Render.MapGrid.GridState.Interior);

        public Cell()
        {

        }

        public Cell(ushort x, ushort y)
        {
            coords = new Vector2Int()
            {
                x = x,
                y = y
            };
            Cub = Hexagon.Coord.OffsetToCube(x, y);
        }

        public Cell(byte terrainTypeId, uint status, ushort x, ushort y)
        {
            terrainType = terrainTypeId;
            coords = new Vector2Int()
            {
                x = x,
                y = y
            };
            Cub = Hexagon.Coord.OffsetToCube(x, y);
        }

        public void Init(Map map)
        {
            Vector3 pos = Scenario.Cur.Map.Coords2Position(x, y);
            if (TerrainType.isWater)
                pos.y = MapRender.Instance.mapGrid.GetGridWaterHeight(x, y);
            else
                pos.y = MapRender.Instance.mapGrid.GetGridHeight(x, y);
            Position = pos;
            for (int i = 0; i < 6; i++)
            {
                Hexagon.Hex neighbor = Cub.Neighbor(i);
                Cell neighborCell = map.GetCell(neighbor);
                if (neighborCell != null)
                    Neighbors[i] = neighborCell;
            }
        }

        public bool CanPassThrough(Troop troops)
        {
            return (this.troop == null || this.troop.BelongForce == troops.BelongForce) &&
                         (this.building == null || this.building.BelongForce == troops.BelongForce);
        }
        public bool CanMove(Troop troops)
        {
            return TerrainType != null && TerrainType.CanMoveBy(troops);
        }
        public bool CanStay(Troop troops)
        {
            return this.troop == null && this.building == null && CanMove(troops);
        }
        public bool IsEmpty()
        {
            return this.troop == null && this.building == null;
        }

        public Cell OffsetCell(int offsetX, int offsetY)
        {
            return Scenario.Cur.Map.GetCell(x + offsetX, y + offsetY);
        }
        public int Distance(Cell other)
        {
            return Cub.Distance(other.Cub);
        }
        public void Ring(int radius, List<Cell> list)
        {
            Scenario.Cur.Map.GetRing(this, radius, list);
        }
        public void Ring(int radius, Action<Cell> action)
        {
            Scenario.Cur.Map.RingAction(this, radius, action);
        }
        public void Spiral(int radius, Action<Cell> action)
        {
            Scenario.Cur.Map.SpiralAction(this, radius, action);
        }

        public void DirectionLine(Cell to, int length, Action<Cell> action)
        {
            int dir = Cub.DirectionTo(to.Cub);
            if (length < 0)
            {
                dir += 3;
                while (dir > 5)
                    dir -= 6;
            }
            Hex start = Cub;
            int absLength = Math.Abs(length);
            for (int i = 0; i < absLength; ++i)
            {
                start = start.Neighbor(dir);
                Cell cell = Scenario.Cur.Map.GetCell(start);
                if (cell != null && action != null)
                    action(cell);
            }
        }

        public void GetDirectionLine(int dir, int length, List<Cell> action)
        {
            Cell dest = this;
            for (int i = 0; i < length; ++i)
            {
                dest = dest.GetNrighbor(dir);
                if (dest != null && action != null)
                    action.Add(dest);
            }
        }

        public void DirectionLine(int dir, int length, Action<Cell> action)
        {
            Cell dest = this;
            for (int i = 0; i < length; ++i)
            {
                dest = dest.GetNrighbor(dir);
                if (dest != null && action != null)
                    action(dest);
            }
        }

        public Cell DirectionLineValidEnd(int dir, int length)
        {
            Cell dest = this;
            for (int i = 0; i < length; ++i)
                dest = dest.GetNrighbor(dir);
            return dest;
        }

        public int DirectionTo(Cell to)
        {
            return Cub.DirectionTo(to.Cub);
        }

        public Cell GetNrighbor(int dir)
        {
            while (dir < 0)
                dir += 6;
            while (dir >= 6)
                dir -= 6;
            return Neighbors[dir];
        }

        public bool HasGridState(MapGrid.GridState state)
        {
            int stateValue = (1 << (int)state);
            return (terrainState & stateValue) == stateValue;
        }

        public void CreateInteriorModel()
        {
            if (interiorModel == null)
            {
                interiorModel = MapObject.Create($"内政地{x}-{y}");
                interiorModel.objType = 0;
                interiorModel.modelId = 0;
                interiorModel.modelAsset = $"Assets/Model/Prefab/4622.prefab";
                interiorModel.transform.position = Position + new Vector3(0, 1, 0);
                interiorModel.transform.rotation = Quaternion.Euler(new Vector3(0, GameRandom.Range(0,10) * 90, 0));
                interiorModel.transform.localScale = Vector3.one;
                interiorModel.bounds = new Sango.Tools.Rect(0, 0, 32, 32);
                MapRender.Instance.AddStatic(interiorModel);
            }
        }

        public void ClearInteriorModel()
        {
            if (interiorModel != null)
            {
                interiorModel.Clear();
                interiorModel = null;
            }
        }

        public void SetInteriorModelVisible(bool b)
        {
            if (interiorModel != null)
            {
                interiorModel.gameObject.SetActive(b);
            }
        }
    }
}
