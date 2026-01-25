using UnityEngine;
using System;

namespace RTEditor
{
    /// <summary>
    /// 用于存储与编辑器相机关联的平移设置   使用这个类，可以设置和获取编辑器相机的平移设置，包括平移模式、平滑值、平移速度和轴反转等
    /// </summary>
    [Serializable]
    public class EditorCameraPanSettings
    {
        #region Private Variables
        /// <summary>
        /// 相机平移模式，默认为 EditorCameraPanMode.Standard
        /// </summary>
        private EditorCameraPanMode _panMode = EditorCameraPanMode.Standard;

        /// <summary>
        /// 当平移模式设置为 "Smooth" 时使用的平滑值，默认为 0.15
        /// </summary>
        private float _smoothValue = 0.15f;

        /// <summary>
        /// 当平移模式设置为 "Standard" 时的相机平移速度（以世界单位/秒为单位），默认为 3.0
        /// </summary>
        [SerializeField]
        private float _standardPanSpeed = 3.0f;

        /// <summary>
        /// 当平移模式设置为 "Smooth" 时的相机平移速度（以世界单位/秒为单位），默认为 3.0
        /// </summary>
        [SerializeField]
        private float _smoothPanSpeed = 3.0f;

        /// <summary>
        /// 指定是否反转用于平移的 X 轴，默认为 false
        /// </summary>
        [SerializeField]
        private bool _invertXAxis = false;

        /// <summary>
        /// 指定是否反转用于平移的 Y 轴，默认为 false
        /// </summary>
        [SerializeField]
        private bool _invertYAxis = false;
        #endregion

        #region Public Static Properties
        /// <summary>
        /// 相机平移速度的最小值，默认为 0.01
        /// </summary>
        public static float MinPanSpeed { get { return 0.01f; } }

        /// <summary>
        /// 平滑值的最小值，默认为 1e-5
        /// </summary>
        public static float MinSmoothValue { get { return 1e-5f; } }

        /// <summary>
        /// 平滑值的最大值，默认为 1.0
        /// </summary>
        public static float MaxSmoothValue { get { return 1.0f; } }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取/设置相机的平移模式
        /// </summary>
        public EditorCameraPanMode PanMode { get { return _panMode; } set { _panMode = value; } }

        /// <summary>
        /// 获取/设置平滑值。该属性的值应在 [MinSmoothValue, MaxSmoothValue] 的范围内，超出范围的值将被截断
        /// </summary>
        public float SmoothValue { get { return _smoothValue; } set { _smoothValue = Mathf.Min(MaxSmoothValue, Mathf.Max(MinSmoothValue, value)); } }

        /// <summary>
        /// 获取/设置相机的标准平移速度。相机平移速度的最小值由 MinPanSpeed 属性定义，小于该值的将被截断
        /// </summary>
        public float StandardPanSpeed { get { return _standardPanSpeed; } set { _standardPanSpeed = Mathf.Max(value, MinPanSpeed); } }

        /// <summary>
        /// 获取/设置相机的平滑平移速度。相机平移速度的最小值由 MinPanSpeed 属性定义，小于该值的将被截断
        /// </summary>
        public float SmoothPanSpeed { get { return _smoothPanSpeed; } set { _smoothPanSpeed = Mathf.Max(value, MinPanSpeed); } }

        /// <summary>
        /// 获取/设置是否反转用于平移的 X 轴
        /// </summary>
        public bool InvertXAxis { get { return _invertXAxis; } set { _invertXAxis = value; } }

        /// <summary>
        /// 获取/设置是否反转用于平移的 Y 轴
        /// </summary>
        public bool InvertYAxis { get { return _invertYAxis; } set { _invertYAxis = value; } }
        #endregion
    }
}
