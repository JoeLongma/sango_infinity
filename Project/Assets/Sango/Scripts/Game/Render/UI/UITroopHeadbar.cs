using Sango.Loader;
using System.Text;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UITroopHeadbar : UGUIWindow
    {
        public Image headIcon;
        public Text name;
        public Image state;
        public Image food;
        public Image energy;
        public Image angry;
        public Text number;
        public AnimationText skillText;
        public UIAnimationText aniText;
        public Troop troop;

        public void Init(Troop troop)
        {
            aniText = null;
            this.troop = troop;
            name.text = troop.Name;
            headIcon.sprite = GameRenderHelper.LoadHeadIcon(troop.Leader.headIconID);
            skillText.Clear();
            UpdateState(troop);
        }

        public void UpdateState(Troop troop)
        {
            state.enabled = false;
            energy.fillAmount = (float)troop.morale / 100.0f;
            angry.fillAmount = 0;
            number.text = troop.troops.ToString();
        }

        public void ShowInfo(int damage, int damageType = 0)
        {
            aniText = UIAnimationText.Show(aniText, troop, damage, damageType);
            if (aniText != null)
            {
                aniText.onAnimationComplate = OnAnimationComplate;
            }
        }
        void OnAnimationComplate()
        {
            aniText = null;
        }

        public void ShowSkill(Skill skill)
        {
            skillText.Create(skill.Name, 1);
        }
    }
}
