namespace RTEditor
{
    /// <summary>
    /// 用于标识不同的相机平移模式
    /// </summary>
    public enum EditorCameraPanMode
    {
        /// <summary>
        /// 标准的平移模式
        /// </summary>
        Standard = 0,

        /// <summary>
        /// 平滑的平移模式。平移速度会随着时间的推移逐渐减慢
        /// </summary>
        Smooth
    }
}
