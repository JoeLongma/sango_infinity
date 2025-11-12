using UnityEngine;
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
        public Animation foodAni;
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
            energy.fillAmount = (float)troop.morale / troop.MaxMorale;
            angry.fillAmount = 0;
            number.text = troop.troops.ToString();
            bool isWithoutFood = troop.IsWithOutFood() <= 1;
            food.enabled = isWithoutFood;
            if (isWithoutFood)
            {
                foodAni.Play();
            }
            else
            {
                foodAni.Stop();
            }
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

        public void ShowSkill(Skill skill, bool isFail, bool isCritical)
        {
            if (isFail)
            {
                skillText.flipY = true;
                skillText.Create(skill.Name + "(失败)", UnityEngine.Color.gray, 1f);
            }
            else
            {
                if (isCritical)
                {
                    skillText.flipY = false;
                    skillText.Create(skill.Name + "<暴击!!>", UnityEngine.Color.red, 1f);
                }
                else
                {
                    skillText.flipY = false;
                    skillText.Create(skill.Name, UnityEngine.Color.green, 1f);
                }
            }
        }
    }
}
