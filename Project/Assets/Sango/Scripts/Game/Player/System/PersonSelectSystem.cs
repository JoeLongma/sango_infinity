using Newtonsoft.Json.Serialization;
using Sango.Game.Render.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;
namespace Sango.Game.Player
{
    public class PersonSelectSystem : CommandSystemBase<PersonSelectSystem>
    {
        public delegate string PersonValueStrGet(Person person);

        public class SortTitle
        {
            public string name;
            public int width;
            public PersonValueStrGet valueGetCall;
        }

        public enum PersonSortGroupType
        {
            None = 0,
            Base,
            Max
        }

        public List<Person> People;
        public List<Person> selected = new List<Person>();
        Action<List<Person>> sureAction;
        PersonSortGroupType personSortGroupType;
        public List<SortTitle> sorltItems;

        public Troop TargetTroop { get; set; }
        public void Start(List<Person> persons, Action<List<Person>> action, PersonSortGroupType personSortGroupType = PersonSortGroupType.None)
        {
            People = new List<Person>(persons);
            sureAction = action;
            this.personSortGroupType = personSortGroupType;
        }

        public void Start(Person[] persons, Action<List<Person>> action, PersonSortGroupType personSortGroupType = PersonSortGroupType.None)
        {
            People = new List<Person>(persons);
            sureAction = action;
            this.personSortGroupType = personSortGroupType;
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
        public override void OnDestroy() {; }

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
