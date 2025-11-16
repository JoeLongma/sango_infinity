using System;
using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class PersonSelectSystem : CommandSystemBase<PersonSelectSystem>
    {


        public List<Person> People;
        public List<Person> selected = new List<Person>();
        Action<List<Person>> sureAction;
        PersonSortGroupType personSortGroupType;
        public List<SortTitle> sorltItems;
        public int selectLimit = 0;

        public Troop TargetTroop { get; set; }
        public void Start(List<Person> persons, List<Person> resultList, int limit, Action<List<Person>> action, PersonSortGroupType personSortGroupType = PersonSortGroupType.Custom)
        {
            selectLimit = limit;
            People = new List<Person>(persons);
            sureAction = action;
            selected = resultList;
            this.personSortGroupType = personSortGroupType;
            PlayerCommand.Instance.Push(this);
        }
        public void OnSure()
        {
            sureAction?.Invoke(selected);
            PlayerCommand.Instance.Back();
        }

        public void OnCancel()
        {
            PlayerCommand.Instance.Back();
        }

        /// <summary>
        /// 进入当前命令的时候触发
        /// </summary>
        public override void OnEnter()
        {
            Window.Instance.ShowWindow("window_person_selector");
        }

        /// <summary>
        /// 离开当前命令的时候触发
        /// </summary>
        public override void OnExit() {; }

        /// <summary>
        /// 当前命令被重新拾起的时候触发(返回)
        /// </summary>
        public override void OnBack() {; }

        /// <summary>
        /// 当前命令被舍弃的时候触发
        /// </summary>
        public override void OnDestroy()
        {
            Window.Instance.HideWindow("window_person_selector");
        }

        /// <summary>
        /// 结束整个命令链的时候触发
        /// </summary>
        public override void OnDone() {; }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public override void Update() {; }

        public override void HandleEvent(CommandEventType eventType, Cell cell) {; }
    }
}
