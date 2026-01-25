using UnityEngine;
using System.Collections;

namespace RTEditor
{
    /// <summary>
    /// 用来旋转相机的静态类
    /// </summary>
    public static class EditorCameraRotation
    {
        #region Public Static Functions
        /// <summary>
        /// 使用指定的旋转量旋转指定的摄影机
        /// </summary>
        /// <remarks>
        /// 该函数将首先绕摄影机右轴旋转，然后绕全局Y轴旋转
        /// </remarks>
        /// <param name="camera">
        /// 要旋转的摄影机对象
        /// </param>
        /// <param name="degreesCameraRight">
        /// 绕摄影机的右轴旋转的角度（旋转量以度为单位）
        /// </param>
        /// <param name="degreesGlobalUp">
        /// 绕全局 Y 轴旋转的角度（旋转量以度为单位）
        /// </param>
        public static void RotateCamera(Camera camera, float degreesCameraRight, float degreesGlobalUp)
        {   //分别绕摄影机右轴和全局Y轴旋转
            //获取摄影机的变换组件 cameraTransform
            Transform cameraTransform = camera.transform;   
            //使用 cameraTransform 的右轴，绕世界空间旋转 degreesCameraRight 角度
            cameraTransform.Rotate(cameraTransform.right, degreesCameraRight, Space.World);
            //使用全局 Y 轴（Vector3.up），绕世界空间旋转 degreesGlobalUp 角度
            cameraTransform.Rotate(Vector3.up, degreesGlobalUp, Space.World);
        }
        #endregion
    }
}
