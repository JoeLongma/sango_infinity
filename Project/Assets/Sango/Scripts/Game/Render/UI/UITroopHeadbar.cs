using Sango.Loader;
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
        public AnimationText aniText;
        public void Init(Troop troop)
        {
            name.text = troop.Name;
            headIcon.sprite = GameRenderHelper.LoadHeadIcon(troop.Leader.headIconID);
            UpdateState(troop);
        }

        public void UpdateState(Troop troop)
        {
            state.enabled = false;
            energy.fillAmount = (float)troop.morale / 100.0f;
            angry.fillAmount = 0;
            number.text = troop.troops.ToString();
        }

        // 上升A 下降B 钱C 矛D 兵E 戟F 弩G 马H 冲车J 投石机K 船M 投石船N 气力O 耐久P 治安Q
        public void ShowDamage(int damage)
        {
            aniText.Create(damage.ToString(), "BE", UnityEngine.Color.red, 2);
        }
    }
}
