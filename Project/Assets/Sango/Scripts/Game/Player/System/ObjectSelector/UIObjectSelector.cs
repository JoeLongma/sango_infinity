using Sango.Game.Player;
using UnityEngine;

namespace Sango.Game.Render.UI
{
    public class UIObjectSelector : UIObjectDisplay
    {
        public RectTransform[] uIObjectListItemsRect;
        ObjectSelectSystem objectSelectSystem;
        bool dragFlag = false;

        public override void Init(ObjectsDisplaySystem objectSelectSystem)
        {
            this.objectSelectSystem = objectSelectSystem as ObjectSelectSystem;
            base.Init(objectSelectSystem);
        }

        public override void UpdateItemStartIndex(int startIndex)
        {
            for (int i = 0; i < uIObjectListItems.Length; i++)
            {
                UIObjectListItem listItem = uIObjectListItems[i];
                int destIndex = i + startIndex;
                listItem.index = destIndex;
                if (destIndex < objectSelectSystem.Objects.Count)
                {
                    SangoObject sango = objectSelectSystem.Objects[destIndex];
                    bool isSelected = objectSelectSystem.selected.Contains(sango);
                    for (int j = 0; j < sortItems.Count; j++)
                    {
                        ObjectSortTitle sortTitle = sortItems[j];
                        listItem.Set(j, sortTitle.GetValueStr(sango));
                    }
                    listItem.SetSelected(isSelected);
                }
                else
                {
                    listItem.SetSelected(false);
                }
            }
        }

        public void OnSure()
        {
            objectSelectSystem.OnSure();
        }

        public void OnPersonListSelected(UIObjectListItem item)
        {
            if (item.index >= objectSelectSystem.Objects.Count)
                return;

            if (!item.IsSelected() && objectSelectSystem.IsPersonLimit())
            {
                int lastIndex = objectSelectSystem.RemoveFront();
                if(lastIndex >= 0)
                {
                    for (int i = 0; i < uIObjectListItems.Length; i++)
                    {
                        UIObjectListItem listItem = uIObjectListItems[i];
                        int destIndex = i + startIndex;
                        if (destIndex == lastIndex)
                        {
                            listItem.SetSelected(false);
                        }
                    }
                }
                item.SetSelected(true);
                objectSelectSystem.Add(item.index);
                return;
            }   

            item.SetSelected(!item.IsSelected());
            if (item.IsSelected())
            {
                objectSelectSystem.Add(item.index);
            }
            else
            {
                objectSelectSystem.Remove(item.index);
            }
        }

        public void OnPointDownPersonListSelected(UIObjectListItem item)
        {
            dragFlag = !item.IsSelected();
        }

        public void OnDragPersonListSelected(UIObjectListItem item)
        {
            if (dragFlag && objectSelectSystem.IsPersonLimit())
                return;

            if (!dragFlag && objectSelectSystem.IsPersonEmpty())
                return;

            if (item.index >= objectSelectSystem.Objects.Count)
                return;

            if (item.IsSelected() && !dragFlag)
            {
                item.SetSelected(false);
                objectSelectSystem.Remove(item.index);
            }
            else if (!item.IsSelected() && dragFlag)
            {
                item.SetSelected(true);
                objectSelectSystem.Add(item.index);
            }

            for (int i = 0; i < uIObjectListItems.Length; i++)
            {
                RectTransform itemRect = uIObjectListItemsRect[i];
                UIObjectListItem listItem = uIObjectListItems[i];
                if (listItem != item && RectTransformUtility.RectangleContainsScreenPoint(itemRect, Input.mousePosition, Sango.Game.Game.Instance.UICamera))
                {
                    if (listItem.IsSelected() && !dragFlag)
                    {
                        listItem.SetSelected(false);
                        objectSelectSystem.Remove(listItem.index);
                    }
                    else if (!listItem.IsSelected() && dragFlag)
                    {
                        listItem.SetSelected(true);
                        objectSelectSystem.Add(listItem.index);
                    }
                }
            }
        }
    }
}
