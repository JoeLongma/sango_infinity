using System;
using System.Collections.Generic;

namespace Sango.Game.Player
{
    public class CitySelectSystem : ObjectSelectSystem
    {
        Action<List<City>> finishAction;

        public void Start(List<City> cities, List<City> resultList, int limit, Action<List<City>> action, List<ObjectSortTitle> customSortTitles, string cutomSortTitleName)
        {
            selectLimit = limit;
            Objects = new List<SangoObject>(cities);
            finishAction = action;
            sureAction = OnBaseSure;
            selected = new List<SangoObject>(resultList);
            customSortItems = customSortTitles;
            this.customSortTitleName = cutomSortTitleName;
            GameSystemManager.Instance.Push(this);
        }

        public void OnBaseSure(List<SangoObject> objects)
        {
            List<City> people = new List<City>();
            foreach (SangoObject obj in objects)
            {
                people.Add((City)obj);
            }
            finishAction?.Invoke(people);
        }

        public override List<ObjectSortTitle> GetSortTitleGroup(int index)
        {
            if (index == 0) return customSortItems;

            List<ObjectSortTitle> sortTitles = new List<ObjectSortTitle>();
            CitySortFunction.Instance.GetSortTitleGroup((CitySortGroupType)index, sortTitles);
            return sortTitles;
        }

        public override string GetSortTitleGroupName(int index)
        {
            return CitySortFunction.Instance.GetSortTitleGroupName((CitySortGroupType)index);
        }
    }
}
