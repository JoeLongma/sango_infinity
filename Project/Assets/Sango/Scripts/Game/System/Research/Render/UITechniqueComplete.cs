using UnityEngine;

namespace Sango.Game.Render.UI
{
    /// <summary>
    /// 科技完成界面（UI窗口）
    /// </summary>
    public class UITechniqueComplete : UGUIWindow
    {
        public UITechniqueItem techniqueItem;
        public Animation animation;

        /// <summary>
        /// 窗口显示时的回调方法
        /// </summary>
        public override void OnShow(params object[] ps)
        {
            base.OnShow();
            techniqueItem.SetTechnique(ps[0] as Technique);
            animation.Play();
            Invoke("Hide", animation.clip.length);
        }
    }
}
