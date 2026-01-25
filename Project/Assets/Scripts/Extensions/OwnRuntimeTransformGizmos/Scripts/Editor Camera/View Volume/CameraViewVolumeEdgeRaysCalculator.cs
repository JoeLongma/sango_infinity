using UnityEngine;

namespace RTEditor
{
    /// <summary>
    /// 这个类允许客户端代码计算相机视图体的点之间连接的边缘射线
    /// </summary>
    public static class CameraViewVolumeEdgeRaysCalculator
    {
        #region Public Methods
        /// <summary>
        /// 计算并返回连接指定相机视图体上点的边缘射线
        /// </summary>
        public static Ray3D[] CalculateWorldSpaceVolumeEdgeRays(CameraViewVolume cameraViewVolume)
        {   // 创建一个包含12条射线的数组，用于存储世界空间中的边缘射线
            var worldSpaceVolumeEdgeRays = new Ray3D[12];

            // 计算连接近平面上点的射线
            worldSpaceVolumeEdgeRays[0] = new Ray3D(cameraViewVolume.TopLeftPointOnNearPlane, cameraViewVolume.TopRightPointOnNearPlane - cameraViewVolume.TopLeftPointOnNearPlane);
            worldSpaceVolumeEdgeRays[1] = new Ray3D(cameraViewVolume.TopRightPointOnNearPlane, cameraViewVolume.BottomRightPointOnNearPlane - cameraViewVolume.TopRightPointOnNearPlane);
            worldSpaceVolumeEdgeRays[2] = new Ray3D(cameraViewVolume.BottomRightPointOnNearPlane, cameraViewVolume.BottomLeftPointOnNearPlane - cameraViewVolume.BottomRightPointOnNearPlane);
            worldSpaceVolumeEdgeRays[3] = new Ray3D(cameraViewVolume.BottomLeftPointOnNearPlane, cameraViewVolume.TopLeftPointOnNearPlane - cameraViewVolume.BottomLeftPointOnNearPlane);

            // 计算连接远平面上点的射线
            worldSpaceVolumeEdgeRays[4] = new Ray3D(cameraViewVolume.TopLeftPointOnFarPlane, cameraViewVolume.TopRightPointOnFarPlane - cameraViewVolume.TopLeftPointOnFarPlane);
            worldSpaceVolumeEdgeRays[5] = new Ray3D(cameraViewVolume.TopRightPointOnFarPlane, cameraViewVolume.BottomRightPointOnFarPlane - cameraViewVolume.TopRightPointOnFarPlane);
            worldSpaceVolumeEdgeRays[6] = new Ray3D(cameraViewVolume.BottomRightPointOnFarPlane, cameraViewVolume.BottomLeftPointOnFarPlane - cameraViewVolume.BottomRightPointOnFarPlane);
            worldSpaceVolumeEdgeRays[7] = new Ray3D(cameraViewVolume.BottomLeftPointOnFarPlane, cameraViewVolume.TopLeftPointOnFarPlane - cameraViewVolume.BottomLeftPointOnFarPlane);

            // 计算连接近平面和远平面之间点的射线
            worldSpaceVolumeEdgeRays[8] = new Ray3D(cameraViewVolume.TopLeftPointOnNearPlane, cameraViewVolume.TopLeftPointOnFarPlane - cameraViewVolume.TopLeftPointOnNearPlane);
            worldSpaceVolumeEdgeRays[9] = new Ray3D(cameraViewVolume.TopRightPointOnNearPlane, cameraViewVolume.TopRightPointOnFarPlane - cameraViewVolume.TopRightPointOnNearPlane);
            worldSpaceVolumeEdgeRays[10] = new Ray3D(cameraViewVolume.BottomRightPointOnNearPlane, cameraViewVolume.BottomRightPointOnFarPlane - cameraViewVolume.BottomRightPointOnNearPlane);
            worldSpaceVolumeEdgeRays[11] = new Ray3D(cameraViewVolume.BottomLeftPointOnNearPlane, cameraViewVolume.BottomLeftPointOnFarPlane - cameraViewVolume.BottomLeftPointOnNearPlane);

            return worldSpaceVolumeEdgeRays;
        }
        #endregion
    }
}
