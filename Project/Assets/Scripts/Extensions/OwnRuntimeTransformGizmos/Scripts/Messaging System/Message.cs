using UnityEngine;

namespace RTEditor
{
    /// <summary>
    /// 此类表示可以发送给侦听器的消息它是一个基本抽象类，必须由可以发送的每种类型的消息派生
    /// </summary>
    public abstract class Message
    {
        #region Private Variables
        /// <summary>
        /// 消息类型
        /// </summary>
        private MessageType _type;
        #endregion

        #region Public Properties
        /// <summary>
        /// 返回消息类型
        /// </summary>
        public MessageType Type { get { return _type; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor创建者
        /// </summary>
        public Message(MessageType type)
        {
            _type = type;
        }
        #endregion
    }
}
