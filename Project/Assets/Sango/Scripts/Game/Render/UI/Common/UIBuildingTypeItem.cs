using Sango.Loader;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIBuildingTypeItem : MonoBehaviour
    {
        public Image icon;
        public Text nameLabel;
        public Text costLabel;
        public Image select;
        public int index;
        public delegate void OnSelect(UIBuildingTypeItem item);
        public OnSelect onSelected;
        public bool IsSelected()
        {
            return select.gameObject.activeSelf;
        }

        public UIBuildingTypeItem SetIndex(int i)
        {
            index = i;
            return this;
        }

        public UIBuildingTypeItem SetSelected(bool b)
        {
            select.gameObject.SetActive(b);
            return this;
        }

        public UIBuildingTypeItem SetBuildingType(BuildingType buildingType)
        {
            nameLabel.text = buildingType.Name;
            costLabel.text = buildingType.cost.ToString();
            icon.sprite = GameRenderHelper.LoadBuildingTypeIcon(buildingType.icon);
            return this;
        }

        public void OnClick()
        {
            onSelected?.Invoke(this);
        }

    }
}