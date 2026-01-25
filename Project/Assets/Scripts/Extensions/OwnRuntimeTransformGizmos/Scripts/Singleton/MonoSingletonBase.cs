using UnityEngine;

namespace RTEditor
{
    /// <summary>
    /// 这是一个实现检索指定类型的单例实例的功能的类。该类可以由所有类继承，这些类必须派生自“MonoBehavior”，同时仍然利用singleton功能
    /// </summary>
    /// <typeparam name="T">
    /// Generic parameter which must be set to the type of the class that derives
    /// from this class.
    /// </typeparam>
    /// <remarks>
    /// The implementation of this class was inspired from here: http://wiki.unity3d.com/index.php/Singleton
    /// </remarks>
    public class MonoSingletonBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Private Variables
        /// <summary>
        /// We will use this to prevent our singleton instance from being accessed by
        /// more than one thread at a time (just in case multiple threads are running
        /// in the application).
        /// </summary>
        private static object _singletonLock = new object();

        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static T _instance;
        #endregion

        #region Public Properties
        /// <summary>
        /// Returns the singleton instance.
        /// </summary>
        public static T Instance
        {
            get
            {
                // Is the instance availanle?
                if (_instance == null)
                {
                    // Apply lock on our sync object
                    lock (_singletonLock)
                    {
                        // Retrieve the instance from the scene.
                        // Note: We will retrieve an array of instances and make sure that only one intance exists.
                        T[] singletonInstances = FindObjectsOfType(typeof(T)) as T[];
                        if (singletonInstances.Length == 0) return null;

                        // More than one singleton?
                        if (singletonInstances.Length > 1)
                        {
                            // Log warning message if running in editor mode and then return null
                            if (Application.isEditor) Debug.LogWarning("MonoSingleton<T>.Instance: Only 1 singleton instance can exist in the scene. Null will be returned.");
                            return null;
                        }

                        // If no singleton exists, we will create one
                       /* if (singletonInstances.Length == 0)
                        {
                            // Create the singleton game object
                            GameObject singletonInstance = new GameObject();

                            // Add the generic parameter type component to it
                            _instance = singletonInstance.AddComponent<T>();

                            // Just give it a default name
                            singletonInstance.name = "(singleton) " + typeof(T).ToString();
                        }
                        else*/
                        _instance = singletonInstances[0];     // Only one instance was found, so we can store it
                    }
                }

                // Return the singleton instance
                return _instance;
            }
        }
        #endregion
    }
}
