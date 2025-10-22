using System;
using System.Collections.Generic;

namespace Sango.Game
{
    public class ItemStore : IAarryDataObject
    {
        public Dictionary<int, int> Items = new Dictionary<int, int>();
        public int TotalNumber { get; private set; }

        public IAarryDataObject FromArray(int[] values)
        {
            TotalNumber = 0;
            Items.Clear();
            if (values == null || values.Length == 0) return this;
            for (int i = 0; i < values.Length; i += 2)
            {
                int itemTypeId = values[i];
                int number = values[i + 1];
                TotalNumber += number;
                ItemType itemType = Scenario.Cur.CommonData.ItemTypes.Get(itemTypeId);
                if (itemType == null) continue;
                Items.Add(itemTypeId, number);
            }
            return this;
        }

        public int[] ToArray()
        {
            List<int> ints = new List<int>();
            foreach (int itemTypeId in Items.Keys)
            {
                int number = Items[itemTypeId];
                if (number > 0)
                {
                    ints.Add(itemTypeId);
                    ints.Add(number);
                }
            }
            return ints.ToArray();
        }

        public ItemStore Copy()
        {
            ItemStore copy = new ItemStore();
            foreach (int itemTypeId in Items.Keys)
            {
                int number = Items[itemTypeId];
                if (number > 0)
                    copy.Items.Add(itemTypeId, number);
            }
            copy.TotalNumber = TotalNumber;
            return copy;
        }

        public int Add(int itemTypeId, int number)
        {
            if (Items.TryGetValue(itemTypeId, out int has))
            {
                has += number;
                Items[itemTypeId] = has;
                TotalNumber += number;
                return has;
            }

            TotalNumber += number;
            Items.Add(itemTypeId, number);
            return number;
        }


        public void Add(ItemStore itemStore)
        {
            foreach (int itemTypeId in itemStore.Items.Keys)
            {
                int number = itemStore.Items[itemTypeId];
                if (number > 0)
                    Add(itemTypeId, number);
            }
        }

        public int Remove(int itemTypeId)
        {
            if (Items.TryGetValue(itemTypeId, out int has))
            {
                Items.Remove(itemTypeId);
                TotalNumber -= has;
                return has;
            }
            return 0;
        }

        public int Remove(int itemTypeId, int number)
        {
            if (Items.TryGetValue(itemTypeId, out int has))
            {
                number = Math.Min(has, number);
                has -= number;
                Items[itemTypeId] = has;
                TotalNumber -= number;
                return has;
            }
            return 0;
        }

        public int GetNumber(int itemTypeId)
        {
            if (Items.TryGetValue(itemTypeId, out int has))
                return has;
            return 0;
        }

        public int this[int itemTypeId]
        {
            get { return GetNumber(itemTypeId); }
            set { Add(itemTypeId, value); }
        }

        public bool CheckItemEnough(int[] cost, int number)
        {
            if (cost == null || cost.Length == 0) return true;
            for (int i = 0; i < cost.Length; i += 2)
            {
                int itemTypeId = cost[i];
                int costN = cost[i + 1];
                int have = GetNumber(itemTypeId);
                if (have < costN * number)
                    return false;
            }
            return true;
        }

        public void Cost(int[] cost, int number)
        {
            if (cost == null || cost.Length == 0) return;
            for (int i = 0; i < cost.Length; i += 2)
            {
                int itemTypeId = cost[i];
                int costN = cost[i + 1] * number;
                Remove(itemTypeId, costN);
            }
        }

        public void Gain(int[] cost, int number)
        {
            if (cost == null || cost.Length == 0) return;
            for (int i = 0; i < cost.Length; i += 2)
            {
                int itemTypeId = cost[i];
                int costN = cost[i + 1] * number;
                Add(itemTypeId, costN);
            }
        }

        public int CheckCostMin(int[] cost, int number)
        {
            if (cost == null || cost.Length == 0) return number;
            for (int i = 0; i < cost.Length; i += 2)
            {
                int itemTypeId = cost[i];
                int costN = cost[i + 1];
                int have = GetNumber(itemTypeId) / costN;
                number = Math.Min(number, have);
            }
            return number;
        }

        /// <summary>
        /// 分割一部分, part: 1到100的整数
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        public ItemStore Split(int part)
        {
            ItemStore itemStore = new ItemStore();
            if (part <= 0) return itemStore;
            if (part > 100) part = 100;
            foreach (int itemTypeId in Items.Keys)
            {
                int number = Items[itemTypeId];
                if (number > 0)
                {
                    int partNum = number * part / 100;
                    itemStore.Add(itemTypeId, partNum);
                  
                }
            }

            foreach (int itemTypeId in itemStore.Items.Keys)
            {
                int number = Items[itemTypeId];
                if (number > 0)
                {
                    Items[itemTypeId] = Items[itemTypeId] - number;
                    TotalNumber -= number;
                }
            }

            return itemStore;
        }

    }
}
