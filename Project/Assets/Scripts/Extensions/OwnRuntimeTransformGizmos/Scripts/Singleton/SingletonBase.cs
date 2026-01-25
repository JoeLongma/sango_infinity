using UnityEngine;

namespace RTEditor
{
    /// <summary>
    /// 抽象的基本单例类，它可以由所有非单例行为类派生，以便访问单例行为
    /// </summary>
    /// <remarks>
    /// The implementation requires that the derived classes have a public parameterless
    /// constructor. That means that the client code can instantiate those classes thus
    /// breaking the singleton pattern. This can be solved using reflection, but it seems
    /// much cleaner to avoid reflection and just keep in mind of the limitation.
    /// </remarks>
    public abstract class SingletonBase<T> where T : class, new()
    {
        #region Private Static Variables
        /// <summary>
        /// 单例
        /// </summary>
        private static T _instance = new T();
        #endregion

        #region Public Static Properties
        /// <summary>
        /// 返回单例
        /// </summary>
        public static T Instance { get { return _instance; } }
        #endregion
    }
}
