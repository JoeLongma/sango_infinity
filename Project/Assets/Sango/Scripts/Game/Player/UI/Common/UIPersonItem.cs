using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIPersonItem : MonoBehaviour
    {
        public Text name;
        public Image headIcon;
        public Text feature;

        public void SetPerson(Person person)
        {
            if (person != null)
            {
                name.text = person.Name;
                if (person.FeatureList != null && person.FeatureList.Count > 0)
                    feature.text = person.FeatureList[0].Name;
                else
                    feature.text = "";
                name.text = person.Name;
                headIcon.sprite = GameRenderHelper.LoadHeadIcon(person.headIconID);
                headIcon.enabled = true;
            }
            else
            {
                headIcon.enabled = false;
                name.text = "";
                feature.text = "";
            }
        }
    }
}