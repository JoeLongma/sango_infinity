using System;
using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class PersonSelectSystem : ObjectSelectSystem
    {
        Action<List<Person>> finishAction;

        public void Start(List<Person> persons, List<Person> resultList, int limit, Action<List<Person>> action, List<ObjectSortTitle> customSortTitles, string cutomSortTitleName)
        {
            selectLimit = limit;
            Objects = new List<SangoObject>(persons);
            finishAction = action;
            sureAction = OnBaseSure;
            selected = new List<SangoObject>(resultList);
            customSortItems = customSortTitles;
            this.customSortTitleName = cutomSortTitleName;
            PlayerCommand.Instance.Push(this);
        }

        public void OnBaseSure(List<SangoObject> objects)
        {
            List<Person> people = new List<Person>();
            foreach (SangoObject obj in objects)
            {
                people.Add((Person)obj);
            }
            finishAction?.Invoke(people);
        }

        public override List<ObjectSortTitle> GetSortTitleGroup(int index)
        {
            if (index == 0) return customSortItems;

            List<ObjectSortTitle> sortTitles = new List<ObjectSortTitle>();
            PersonSortFunction.Instance.GetSortTitleGroup((PersonSortGroupType)index, sortTitles);
            return sortTitles;
        }

        public override string GetSortTitleGroupName(int index)
        {
            return PersonSortFunction.Instance.GetSortTitleGroupName((PersonSortGroupType)index);
        }
    }
}
