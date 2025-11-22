using Sango.Game.Render.UI;
using System;
using System.Collections.Generic;
using static Sango.Game.PersonSortFunction;

namespace Sango.Game.Player
{
    public class ObjectSelectSystem : CommandSystemBase
    {
        public List<SangoObject> Objects;
        public List<SangoObject> selected = new List<SangoObject>();
        protected Action<List<SangoObject>> sureAction;
        protected PersonSortGroupType sangoObjectSortGroupType;

        public string customSortTitleName;
        public List<ObjectSortTitle> customSortItems;

        public int selectLimit = 0;

        public Troop TargetTroop { get; set; }

        public void Start(List<SangoObject> sangoObjects, List<SangoObject> resultList, int limit, Action<List<SangoObject>> action, List<ObjectSortTitle> customSortTitles, string cutomSortTitleName)
        {
            selectLimit = limit;
            Objects = new List<SangoObject>(sangoObjects);
            sureAction = action;
            selected = resultList;
            customSortItems = customSortTitles;
            this.customSortTitleName = cutomSortTitleName;
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

        public bool IsPersonLimit()
        {
            return selectLimit <= selected.Count;
        }

        public void Add(int index)
        {
            selected.Add(Objects[index]);
        }

        public void Remove(int index)
        {
            selected.Remove(Objects[index]);
        }

        /// <summary>
        /// 进入当前命令的时候触发
        /// </summary>
        public override void OnEnter()
        {
            Window.WindowInterface win = Window.Instance.Open("window_object_selector");
            if (win != null)
            {
                UIObjectSelector uIObjectSelector = win.ugui_instance as UIObjectSelector;
                if(uIObjectSelector != null)
                {
                    uIObjectSelector.Init(this);
                }
            }
        }

        /// <summary>
        /// 当前命令被舍弃的时候触发
        /// </summary>
        public override void OnDestroy()
        {
            Window.Instance.Close("window_object_selector");
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition) {; }

        public virtual List<ObjectSortTitle> GetSortTitleGroup(int index)
        {
           return customSortItems;
        }

        public virtual string GetSortTitleGroupName(int index)
        {
            return "";
        }
    }
}
