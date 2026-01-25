namespace RTEditor
{
    /// <summary>
    /// 表示控件轴。此枚举由所有gizmo类型用于区分它们的X、Y和Z轴
    /// </summary>
    public enum GizmoAxis
    {
        /// <summary>
        /// 控件X轴
        /// </summary>
        X = 0,

        /// <summary>
        /// 控件Y轴.
        /// </summary>
        Y,

        /// <summary>
        /// 控件 Z轴
        /// </summary>
        Z,

        /// <summary>
        /// 用于某些情况，如指定当前未选择gizmo轴
        /// </summary>
        None
    }
}
