using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIBuildingTypeItem : MonoBehaviour
    {
        public Image icon;
        public Text nameLabel;
        public Text costLabel;
        public Text numLabel;
        public Image select;
        public int index;
        public delegate void OnSelect(UIBuildingTypeItem item);
        public OnSelect onSelected;

        public bool IsSelected()
        {
            return select.gameObject.activeSelf;
        }

        public UIBuildingTypeItem SetValid(bool b)
        {
            Color color = b ? new Color(0.85f, 0.85f, 0.85f) : Color.gray;
            icon.color = color;
            GetComponent<Button>().interactable = b;
            nameLabel.color = color;
            costLabel.color = color;
            numLabel.color = color;
            select.color = color;
            return this;
        }

        public UIBuildingTypeItem SetIndex(int i)
        {
            index = i;
            return this;
        }

        public UIBuildingTypeItem SetSelected(bool b)
        {
            Color color = b ? Color.white : new Color(0.85f, 0.85f, 0.85f);
            icon.color = color;
            select.gameObject.SetActive(b);
            return this;
        }

        public UIBuildingTypeItem SetNum(int c)
        {
            if (c < 0)
                numLabel.text = "";
            else
                numLabel.text = c.ToString();
            return this;
        }

        public UIBuildingTypeItem SetBuildingType(BuildingType buildingType)
        {
            if (buildingType == null)
            {
                icon.enabled = false;
                nameLabel.text = "";
                costLabel.text = "";
                numLabel.text = "";
            }
            else
            {
                icon.enabled = true;
                nameLabel.text = buildingType.Name;
                costLabel.text = buildingType.cost.ToString();
                icon.sprite = GameRenderHelper.LoadBuildingTypeIcon(buildingType.icon);
                numLabel.text = "";
            }
            return this;
        }

        public UIBuildingTypeItem SetItemType(ItemType itemType)
        {
            if (itemType == null)
            {
                icon.enabled = false;
                nameLabel.text = "";
                costLabel.text = "";
                numLabel.text = "";
            }
            else
            {
                icon.enabled = true;
                nameLabel.text = itemType.Name;
                costLabel.text = itemType.cost.ToString();
                icon.sprite = GameRenderHelper.LoadBuildingTypeIcon(itemType.icon);
                numLabel.text = "";
                icon.color = new Color(0.85f, 0.85f, 0.85f);
            }
            return this;
        }

        public UIBuildingTypeItem SetTroopType(TroopType troopType)
        {
            if (troopType == null)
            {
                icon.enabled = false;
                nameLabel.text = "";
                costLabel.text = "";
                numLabel.text = "";
            }
            else
            {
                icon.enabled = true;
                nameLabel.text = troopType.Name;
                costLabel.text = "";
                icon.sprite = GameRenderHelper.LoadBuildingTypeIcon(troopType.icon);
                numLabel.text = "";
                icon.color = new Color(0.85f, 0.85f, 0.85f);
            }
            return this;
        }

        public void OnClick()
        {
            onSelected?.Invoke(this);
        }

    }
}