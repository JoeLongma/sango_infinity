using Sango.Loader;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UICityBuildingSlot : MonoBehaviour
    {
        public Image select;
        public int index;
        public delegate void OnSelect(UICityBuildingSlot item);
        public OnSelect onSelected;
        public bool IsSelected()
        {
            return select.gameObject.activeSelf;
        }

        public UICityBuildingSlot SetIndex(int i)
        {
            index = i;
            return this;
        }

        public UICityBuildingSlot SetSelected(bool b)
        {
            select.gameObject.SetActive(b);
            return this;
        }

        public UICityBuildingSlot SetBuilding(Building building)
        {
            return this;
        }

        public void OnClick()
        {
            onSelected?.Invoke(this);
        }

    }
}