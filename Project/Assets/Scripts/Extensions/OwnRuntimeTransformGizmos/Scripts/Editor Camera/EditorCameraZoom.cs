using UnityEngine;

namespace RTEditor
{
    /// <summary>
    /// 对相机进行缩放操作   定义了用于应用缩放效果到相机的静态类
    /// </summary>
    public static class EditorCameraZoom
    {
        #region Public Static Functions
        /// <summary>
        /// 通过指定的量对相机进行缩放
        /// </summary>
        /// <param name="camera">
        /// 需要进行缩放的相机
        /// </param>
        /// <param name="zoomAmount">
        /// 缩放的量。正值将放大相机视图，负值将缩小相机视图
        /// </param>
        public static void ZoomCamera(Camera camera, float zoomAmount)
        {
            // 函数会根据相机的类型调用 ZoomOrthoCameraViewVolume，即使相机不是正交相机，也会调用该函数以确保在透视和正交相机之间切换时能够正确工作
            float zoomAmountScale = 1.0f;
            zoomAmountScale = ZoomOrthoCameraViewVolume(camera, zoomAmount);

            // 如果相机不是正交相机，将不会限制缩放的量
            if (!camera.orthographic) zoomAmountScale = 1.0f;
            // else camera.nearClipPlane += zoomAmount;

            // 无论使用何种类型的相机，都会移动相机的位置。这是必要的，因为相机有近裁剪面和远裁剪面
            // 如果不沿着相机的视线方向移动相机，对象可能会被近裁剪面或远裁剪面裁剪，而不考虑应用到相机的缩放因素
            // 注意：确保将缩放量按照 zoomAmountScale 变量进行缩放
            Transform cameraTransform = camera.transform;
            cameraTransform.position += cameraTransform.forward * zoomAmount * zoomAmountScale;
        }

        /// <summary>
        /// 应用缩放效果到正交相机的视图体积
        /// </summary>
        /// <returns>
        /// 返回值：一个缩放因子，可以用于缩放应用到相机的缩放量以实现缩放效果
        /// 相机的位置必须根据此值进行调整（例如，pos = pos + look * zoomAmount * zoomScale）
        /// </returns>
        public static float ZoomOrthoCameraViewVolume(Camera camera, float zoomAmount)
        {
            // 参数 camera：需要应用缩放效果的相机    参数 zoomAmount：缩放的量
            // 函数会从 1.0 的缩放比例开始。如果需要，会进行修改
            float zoomAmountScale = 1.0f;

            // 函数使用一个最小值来限制正交大小。这是因为如果允许大小小于 0，场景将被翻转。将其设置为 0 也不好，因为会引发异常
            const float minOrthoSize = 0.001f;

            // 计算新的正交大小
            float newOrthoSize = camera.orthographicSize - zoomAmount * 0.5f;

            // 如果新的正交大小小于允许的最小值，则需要进行处理：
            // Note: If it is, what we would normally have to do is to just clamp the size to the
            //       minimum value. However, we must calculate the the zoom ammount scale factor
            //       so that we can correctly return it from the function.
            if (newOrthoSize < minOrthoSize)
            {
                float delta = minOrthoSize - newOrthoSize;                  // 计算需要从缩放中减去的量
                float percentageOfRemovedZoom = delta / zoomAmount;         // 计算已移除的缩放量的百分比

                // 将新的正交大小限制为允许的最小值
                newOrthoSize = minOrthoSize;

                // 计算缩放因子：从 1 开始减去之前移除的百分比
                zoomAmountScale = 1.0f - percentageOfRemovedZoom;
            }

            // 设置新的正交大小
            camera.orthographicSize = Mathf.Max(minOrthoSize, newOrthoSize);

            // 返回确定的缩放量比例
            return zoomAmountScale;
        }
        #endregion
    }
}
