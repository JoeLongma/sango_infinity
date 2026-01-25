namespace RTEditor
{
    /// <summary>
    /// 标识不同的编辑器相机旋转模式
    /// </summary>
    public enum EditorCameraRotationMode
    {
        /// <summary>
        /// 简单的环顾式旋转模式，相机围绕全局 Y 轴和自身 X 轴旋转。模拟人在周围环境中环顾四周的动作
        /// </summary>
        LookAround = 0,

        /// <summary>
        /// 相机围绕空间中的一个点旋转。旋转会影响相机的位置和方向，相机始终朝向轨道点
        /// </summary>
        Orbit
    }
}
