using UnityEngine;

namespace RTEditor
{
    /// <summary>
    /// 用于实现相机平移功能的静态类
    /// </summary>
    public static class EditorCameraPan
    {
        #region Public Static Functions
        /// <summary>
        /// 通过指定的平移量，对指定的相机进行平移操作
        /// </summary>
        /// <param name="camera">
        /// 要进行平移的相机
        /// </param>
        /// <param name="panAmountRight">
        /// 向右平移的世界单位/秒数
        /// </param>
        /// <param name="panAmountUp">
        /// 向上平移的世界单位/秒数
        /// </param>
        public static void PanCamera(Camera camera, float panAmountRight, float panAmountUp)
        {
            // 使用指定的平移量沿相机的右轴和上轴平移
            // 首先获取相机的 Transform 组件，然后根据指定的平移量沿着相机的右侧轴和上方轴进行平移操作
            Transform cameraTransform = camera.transform;
            // 相机的位置会增加 (cameraTransform.right * panAmountRight + cameraTransform.up * panAmountUp) 的偏移量
            cameraTransform.position += (cameraTransform.right * panAmountRight + cameraTransform.up * panAmountUp);
        }
        #endregion
    }
}
