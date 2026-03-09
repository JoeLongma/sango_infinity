using Sango.Game.Render.UI;
using System.Collections.Generic;

namespace Sango.Game.Player
{
    [GameSystem(auto = true)]
    public class ObjectsDisplaySystem : GameSystem
    {
        public List<SangoObject> Objects;
        public string customSortTitleName;
        public List<ObjectSortTitle> customSortItems;
        public struct ButtonData
        {
            public string title;
            public int style;
            public System.Action action;
        }

        public List<ButtonData> buttonDatas;

        /// <summary>
        /// 点选模式
        /// </summary>
        public bool ClickMode { get; set; }

        public void Start(List<SangoObject> sangoObjects, List<ObjectSortTitle> customSortTitles, string cutomSortTitleName)
        {
            Objects = new List<SangoObject>(sangoObjects);
            customSortItems = customSortTitles;
            this.customSortTitleName = cutomSortTitleName;
            GameSystemManager.Instance.Push(this);
        }

        public void OnCancel()
        {
            GameSystemManager.Instance.Back();
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

        public override void OnDestroy()
        {
            Window.Instance.Close("window_object_selector");
        }


        public virtual List<ObjectSortTitle> GetSortTitleGroup(int index)
        {
            return customSortItems;
        }

        public virtual string GetSortTitleGroupName(int index)
        {
            return "";
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClickUp:
                    OnCancel(); break;
            }

        }
    }
}
