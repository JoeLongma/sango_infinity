namespace RTEditor
{
    /// <summary>
    /// 用于标识不同类型的相机缩放模式
    /// </summary>
    public enum EditorCameraZoomMode
    {
        /// <summary>
        /// 标准缩放模式
        /// </summary>
        Standard = 0,

        /// <summary>
        /// 平滑缩放模式。相机的缩放速度会随着时间的推移而逐渐减慢
        /// </summary>
        Smooth
    }
}
