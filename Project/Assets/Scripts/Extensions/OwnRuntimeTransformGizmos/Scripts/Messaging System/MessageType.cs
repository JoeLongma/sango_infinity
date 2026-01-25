namespace RTEditor
{
    /// <summary>
    /// 此枚举包含可以发送给侦听器的可能类型的消息
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 当控件变换其控制的游戏对象时，可以发送此消息
        /// </summary>
        GizmoTransformedObjects = 0,

        /// <summary>
        /// 当通过变换控件应用于游戏对象集合的变换撤消时，会发送此消息
        /// </summary>
        GizmoTransformOperationWasUndone,

        /// <summary>
        /// 当通过变换控件应用于游戏对象集合的变换被重做时，会发送此消息
        /// </summary>
        GizmoTransformOperationWasRedone,

        /// <summary>
        /// 此消息是在启用顶点捕捉时发送的
        /// </summary>
        VertexSnappingEnabled,

        /// <summary>
        /// 禁用顶点捕捉时会发送此消息
        /// </summary>
        VertexSnappingDisabled,
    }
}