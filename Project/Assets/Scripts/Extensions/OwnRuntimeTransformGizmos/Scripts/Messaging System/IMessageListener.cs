namespace RTEditor
{
    /// <summary>
    /// 这是一个必须由所有必须侦听和响应消息的类实现的接口
    /// </summary>
    public interface IMessageListener
    {
        #region Interface Methods
        /// <summary>
        /// 所有侦听器都必须实现此方法来响应不同类型的消息
        /// </summary>
        void RespondToMessage(Message message);
        #endregion
    }
}
