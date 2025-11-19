using Sango.Loader;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UICityBuildingSlot : MonoBehaviour
    {
        public UIBuildingTypeItem uIBuildingTypeItem;
        public UIPersonItem[] uIPersonItems;
        public Text leftLabel;
        public Text infoLabel;
        public Image select;
        public int index;
        public delegate void OnSelect(UICityBuildingSlot item);
        public Transform personNode;
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
            if (building == null)
            {
                leftLabel.text = "";
                infoLabel.text = "";
                uIBuildingTypeItem.SetBuildingType(null);
                uIBuildingTypeItem.nameLabel.text = "-(空地)-";
                personNode.gameObject.SetActive(false);
            }
            else
            {
                uIBuildingTypeItem.SetBuildingType(building.BuildingType);
                if (!building.isComplte)
                {
                    personNode.gameObject.SetActive(true);
                    leftLabel.text = $"剩:{building.BuidlLefCounter}回";
                    infoLabel.text = "修建中..";
                    for (int i = 0; i < uIPersonItems.Length; i++)
                    {
                        UIPersonItem uIPersonItem = uIPersonItems[i];
                        if (i < building.Builders.Count)
                            uIPersonItem.SetPerson(building.Builders[i]);
                        else
                            uIPersonItem.SetPerson(null);
                    }
                }
                else if (building.isUpgrading)
                {
                    personNode.gameObject.SetActive(true);
                    leftLabel.text = $"剩:{building.BuidlLefCounter}回";
                    infoLabel.text = "升级中..";
                    for (int i = 0; i < uIPersonItems.Length; i++)
                    {
                        UIPersonItem uIPersonItem = uIPersonItems[i];
                        if (i < building.Builders.Count)
                            uIPersonItem.SetPerson(building.Builders[i]);
                        else
                            uIPersonItem.SetPerson(null);
                    }
                }
                else
                {
                    leftLabel.text = "";
                    infoLabel.text = "";
                    personNode.gameObject.SetActive(false);
                }
            }
            return this;
        }

        public void OnClick()
        {
            onSelected?.Invoke(this);
        }

    }
}