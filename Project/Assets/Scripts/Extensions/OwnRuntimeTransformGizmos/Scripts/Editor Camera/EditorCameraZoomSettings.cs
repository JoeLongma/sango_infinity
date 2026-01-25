using UnityEngine;
using System;

namespace RTEditor
{
    /// <summary>
    /// 用于保存与编辑器相机关联的缩放设置
    /// </summary>
    [Serializable]
    public class EditorCameraZoomSettings
    {
        #region Private Variables
        /// <summary>
        /// 相机缩放模式
        /// </summary>
        [SerializeField]
        private EditorCameraZoomMode _zoomMode = EditorCameraZoomMode.Standard;

        /// <summary>
        /// 用于根据需要切换相机缩放的开关
        /// </summary>
        [SerializeField]
        private bool _isZoomEnabled = true;

        /// <summary>
        /// 当缩放模式设置为“Smooth”且相机工作在正交模式下时使用的平滑值
        /// </summary>
        [SerializeField]
        private float _orthographicSmoothValue = 0.1f;

        /// <summary>
        /// 当缩放模式设置为“Smooth”且相机工作在透视模式下时使用的平滑值
        /// </summary>
        [SerializeField]
        private float _perspectiveSmoothValue = 0.2f;

        /// <summary>
        /// 当缩放模式设置为“Standard”且相机工作在正交模式下时的相机缩放速度
        /// </summary>
        [SerializeField]
        private float _orthographicStandardZoomSpeed = 10.0f;

        /// <summary>
        /// 当缩放模式设置为“Standard”且相机工作在透视模式下时的相机缩放速度
        /// </summary>
        [SerializeField]
        private float _perspectiveStandardZoomSpeed = 400.0f;

        /// <summary>
        /// 当缩放模式设置为“Smooth”且相机工作在正交模式下时的相机缩放速度
        /// </summary>
        [SerializeField]
        private float _orthographicSmoothZoomSpeed = 65.0f;

        /// <summary>
        /// 当缩放模式设置为“Smooth”且相机工作在透视模式下时的相机缩放速度
        /// </summary>
        [SerializeField]
        private float _perspectiveSmoothZoomSpeed = 400.0f;
        #endregion

        #region Public Static Properties
        /// <summary>
        /// 适用于正交和透视相机的最小缩放速度
        /// </summary>
        public static float MinZoomSpeed { get { return 0.01f; } }

        /// <summary>
        /// 最小的平滑值
        /// </summary>
        public static float MinSmoothValue { get { return 1e-5f; } }

        /// <summary>
        /// 最大的平滑值
        /// </summary>
        public static float MaxSmoothValue { get { return 1.0f; } }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取/设置缩放模式
        /// </summary>
        public EditorCameraZoomMode ZoomMode { get { return _zoomMode; } set { _zoomMode = value; } }

        /// <summary>
        /// 获取/设置指定相机缩放是否启用的布尔标志
        /// </summary>
        public bool IsZoomEnabled { get { return _isZoomEnabled; } set { _isZoomEnabled = value; } }

        /// <summary>
        /// 获取/设置当缩放模式设置为“Smooth”且相机工作在正交模式下时使用的平滑值
        /// 该属性的值可以在 [MinSmoothValue, MaxSmoothValue] 区间内。超出此区间的值将被相应地夹紧
        /// </summary>
        public float OrthographicSmoothValue { get { return _orthographicSmoothValue; } set { _orthographicSmoothValue = Mathf.Min(MaxSmoothValue, Mathf.Max(MinSmoothValue, value)); } }

        /// <summary>
        /// 获取/设置当缩放模式设置为“Smooth”且相机工作在透视模式下时使用的平滑值
        /// 该属性的值可以在 [MinSmoothValue, MaxSmoothValue] 区间内。如果设置的值超出此区间，它将被相应地夹紧
        /// </summary>
        public float PerspectiveSmoothValue { get { return _perspectiveSmoothValue; } set { _perspectiveSmoothValue = Mathf.Min(MaxSmoothValue, Mathf.Max(MinSmoothValue, value)); } }

        /// <summary>
        /// 获取/设置当缩放模式设置为“Standard”且相机工作在正交模式下时的相机缩放速度
        /// 最小缩放速度由 MinZoomSpeed 属性给出。如果设置的值小于该最小速度，它将被相应地夹紧
        /// </summary>
        public float OrthographicStandardZoomSpeed { get { return _orthographicStandardZoomSpeed; } set { _orthographicStandardZoomSpeed = Mathf.Max(value, MinZoomSpeed); } }

        /// <summary>
        /// 获取/设置当缩放模式设置为“Standard”且相机工作在透视模式下时的相机缩放速度
        /// 最小缩放速度由 MinZoomSpeed 属性给出。如果设置的值小于该最小速度，它将被相应地夹紧
        /// </summary>
        public float PerspectiveStandardZoomSpeed { get { return _perspectiveStandardZoomSpeed; } set { _perspectiveStandardZoomSpeed = Mathf.Max(value, MinZoomSpeed); } }

        /// <summary>
        /// 获取/设置当缩放模式设置为“Smooth”且相机工作在正交模式下时的相机缩放速度
        /// 最小缩放速度由 MinZoomSpeed 属性给出。如果设置的值小于该最小速度，它将被相应地夹紧
        /// </summary>
        public float OrthographicSmoothZoomSpeed { get { return _orthographicSmoothZoomSpeed; } set { _orthographicSmoothZoomSpeed = Mathf.Max(value, MinZoomSpeed); } }

        /// <summary>
        /// 获取/设置当缩放模式设置为“Smooth”且相机工作在透视模式下时的相机缩放速度
        /// 最小缩放速度由 MinZoomSpeed 属性给出。如果设置的值小于该最小速度，它将被相应地夹紧
        /// </summary>
        public float PerspectiveSmoothZoomSpeed { get { return _perspectiveSmoothZoomSpeed; } set { _perspectiveSmoothZoomSpeed = Mathf.Max(value, MinZoomSpeed); } }
        #endregion
    }
}