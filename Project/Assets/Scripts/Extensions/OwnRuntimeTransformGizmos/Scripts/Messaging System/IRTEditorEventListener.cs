using UnityEngine;

namespace RTEditor
{
    /// <summary>
    /// Monobhaviors可以实现这个接口，以便他们能够倾听不同类型的事件并根据需要采取行动
    /// </summary>
    public interface IRTEditorEventListener
    {
        /// <summary>
        /// 在将要选择对象之前调用。如果可以选择对象，则必须返回true，否则必须返回false
        /// </summary>
        bool OnCanBeSelected(ObjectSelectEventArgs selectEventArgs);

        /// <summary>
        /// 在选择对象时调用
        /// </summary>
        void OnSelected(ObjectSelectEventArgs selectEventArgs);

        /// <summary>
        /// 在取消选择对象时调用
        /// </summary>
        void OnDeselected(ObjectDeselectEventArgs deselectEventArgs);

        /// <summary>
        /// 当变换控件更改（移动、旋转或缩放）对象时调用
        /// </summary>
        /// <param name="gizmo">
        /// 改变对象的变换控件
        /// </param>
        void OnAlteredByTransformGizmo(Gizmo gizmo);
    }
}
