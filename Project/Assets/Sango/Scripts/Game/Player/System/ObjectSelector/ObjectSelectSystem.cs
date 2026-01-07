using Sango.Game.Render.UI;
using System;
using System.Collections.Generic;

namespace Sango.Game.Player
{
    public class ObjectSelectSystem : ObjectsDisplaySystem
    {
        public List<SangoObject> selected = new List<SangoObject>();
        protected Action<List<SangoObject>> sureAction;
        public int selectLimit = 0;

        public void Start(List<SangoObject> sangoObjects, List<SangoObject> resultList, int limit, Action<List<SangoObject>> action, List<ObjectSortTitle> customSortTitles, string cutomSortTitleName)
        {
            selectLimit = limit;
            Objects = new List<SangoObject>(sangoObjects);
            sureAction = action;
            selected = resultList;
            resultList.RemoveAll(x => x == null);
            customSortItems = customSortTitles;
            this.customSortTitleName = cutomSortTitleName;
            PlayerCommand.Instance.Push(this);
        }
        public void OnSure()
        {
            sureAction?.Invoke(selected);
            PlayerCommand.Instance.Back();
        }

        public bool IsPersonLimit()
        {
            return selectLimit <= selected.Count;
        }

        public bool IsPersonEmpty()
        {
            return selected.Count <= 0;
        }

        public void Add(int index)
        {
            selected.Add(Objects[index]);
        }

        public void Remove(int index)
        {
            selected.Remove(Objects[index]);
        }
        public int RemoveFront()
        {
            if (selected.Count == 0) return -1;
            SangoObject sangoObject = selected[0];
            selected.RemoveAt(0);
            return Objects.IndexOf(sangoObject);
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
                if (uIObjectSelector != null)
                {
                    uIObjectSelector.Init(this);
                }
            }
        }
    }
}
