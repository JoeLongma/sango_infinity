using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RTEditor
{
    /// <summary>
    /// Gizmo 系统管理所有的 Gizmo。它根据场景中发生的事件（例如用户如何移动鼠标、选择了哪些对象等）来控制它们的位置和方向
    /// </summary>
    public class EditorGizmoSystem : MonoSingletonBase<EditorGizmoSystem>, IMessageListener
    {
        public delegate void ActiveGizmoTypeChangedHandler(GizmoType newGizmoType);
        public event ActiveGizmoTypeChangedHandler ActiveGizmoTypeChanged;

        #region Private Variables
        /// <summary>
        /// Shortcut keys.
        /// </summary>
        [SerializeField]
        private ShortcutKeys _activateTranslationGizmoShortcut = new ShortcutKeys("Activate move gizmo", 1)
        {
            Key0 = KeyCode.W,
            UseModifiers = false,
            UseMouseButtons = false,
            UseStrictMouseCheck = true
        };
        [SerializeField]
        private ShortcutKeys _activateRotationGizmoShortcut = new ShortcutKeys("Activate rotation gizmo", 1)
        {
            Key0 = KeyCode.E,
            UseModifiers = false,
            UseMouseButtons = false,
            UseStrictMouseCheck = true
        };
        [SerializeField]
        private ShortcutKeys _activateScaleGizmoShortcut = new ShortcutKeys("Activate scale gizmo", 1)
        {
            Key0 = KeyCode.R,
            UseModifiers = false,
            UseMouseButtons = false,
            UseStrictMouseCheck = true
        };
        [SerializeField]
        private ShortcutKeys _activateVolumeScaleGizmoShortcut = new ShortcutKeys("Activate volume scale gizmo", 1)
        {
            Key0 = KeyCode.U,
            UseModifiers = false,
            UseMouseButtons = false,
            UseStrictMouseCheck = true
        };
        [SerializeField]
        private ShortcutKeys _activateGlobalTransformShortcut = new ShortcutKeys("Activate global transform", 1)
        {
            Key0 = KeyCode.G,
            UseModifiers = false,
            UseMouseButtons = false,
            UseStrictMouseCheck = true
        };
        [SerializeField]
        private ShortcutKeys _activateLocalTransformShortcut = new ShortcutKeys("Activate local transform", 1)
        {
            Key0 = KeyCode.L,
            UseModifiers = false,
            UseMouseButtons = false,
            UseStrictMouseCheck = true
        };
        [SerializeField]
        private ShortcutKeys _turnOffGizmosShortcut = new ShortcutKeys("Turn off gizmos", 1)
        {
            Key0 = KeyCode.Q,
            UseModifiers = false,
            UseMouseButtons = false,
            UseStrictMouseCheck = true
        };
        [SerializeField]
        private ShortcutKeys _togglePivotShortcut = new ShortcutKeys("Toggle pivot", 1)
        {
            Key0 = KeyCode.P,
            UseModifiers = false,
            UseMouseButtons = false,
            UseStrictMouseCheck = true
        };

        /// <summary>
        /// 用于在场景中移动对象的平移gizmo控件
        /// </summary>
        [SerializeField]
        private TranslationGizmo _translationGizmo;

        /// <summary>
        /// 用于旋转场景中对象的旋转gizmo控件
        /// </summary>
        [SerializeField]
        private RotationGizmo _rotationGizmo;

        /// <summary>
        /// 用于缩放场景中对象的缩放gizmo控件
        /// </summary>
        [SerializeField]
        private ScaleGizmo _scaleGizmo;

        [SerializeField]
        private VolumeScaleGizmo _volumeScaleGizmo;

        /// <summary>
        /// 这是当前用于变换场景中对象的gizmo控件
        /// </summary>
        /// <remarks>
        /// 所谓“active激活”是指它是由用户选择用于对象操作的
        /// 这并不一定意味着它在场景中是活动的。例如，当场景中没有选定对象时，“_activeGizmo”将引用处于非活动场景的gizmo对象
        /// </remarks>
        private Gizmo _activeGizmo;

        /// <summary>
        /// 这是小控件将在其中变换其受控对象的变换空间。您可以在Inspector GUI中对此进行更改，以建立初始变换空间
        /// </summary>
        [SerializeField]
        private TransformSpace _transformSpace = TransformSpace.Global;

        /// <summary>
        /// 存储当前活动gizmo的类型。可以在Inspector GUI中对此进行更改，以建立必须在第一次对象选择操作中激活的初始变换gizmo
        /// </summary>
        [SerializeField]
        private GizmoType _activeGizmoType = GizmoType.Translation;

        /// <summary>
        /// 这是所有Gizmo都必须使用的变换轴心点，以变换其对象
        /// </summary>
        [SerializeField]
        private TransformPivotPoint _transformPivotPoint = TransformPivotPoint.Center;

        [SerializeField]
        private bool[] _gizmoTypeAvailableFlags = new bool[]
        {
            true, true, true, true
        };

        /// <summary>
        /// 如果此变量设置为true，则小控件将关闭。这意味着当选择对象时，将不显示小控件。当用户希望执行简单的对象选择而不必担心小控件时，此模式非常有用
        /// </summary>
        private bool _areGizmosTurnedOff = false;
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取/设置关联的平移 Gizmo。将 Gizmo 设置为 null 或预制体将不会产生任何效果。只允许非空的场景对象实例
        /// </summary>
        public TranslationGizmo TranslationGizmo
        {
            get { return _translationGizmo; }
            set
            {
                if (value == null) return;

                // Allow only scene objects
                #if UNITY_EDITOR
                if (value.gameObject.IsSceneObject()) _translationGizmo = value;
                else Debug.LogWarning("RTEditorGizmoSystem.TranslationGizmo: Only scene gizmo object instances are allowed.");
                #else
                _translationGizmo = value;
                #endif
            }
        }

        /// <summary>
        /// 获取/设置关联的旋转 Gizmo。将 Gizmo 设置为 null 或预制体将不会产生任何效果。只允许非空的场景对象实例
        /// </summary>
        public RotationGizmo RotationGizmo
        {
            get { return _rotationGizmo; }
            set
            {
                if (value == null) return;

                // 只允许场景对象
                #if UNITY_EDITOR
                if (value.gameObject.IsSceneObject()) _rotationGizmo = value;
                else Debug.LogWarning("EditorGizmoSystem.RotationGizmo: Only scene gizmo object instances are allowed.");
                #else
                _rotationGizmo = value;
                #endif
            }
        }

        /// <summary>
        /// 获取/设置关联的缩放 Gizmo。将 Gizmo 设置为 null 或预制体将不会产生任何效果。只允许非空的场景对象实例
        /// </summary>
        public ScaleGizmo ScaleGizmo
        {
            get { return _scaleGizmo; }
            set
            {
                if (value == null) return;

                // 只允许场景对象
                #if UNITY_EDITOR
                if (value.gameObject.IsSceneObject()) _scaleGizmo = value;
                else Debug.LogWarning("EditorGizmoSystem.ScaleGizmo: Only scene gizmo object instances are allowed.");
                #else
                _scaleGizmo = value;
                #endif
            }
        }

        public VolumeScaleGizmo VolumeScaleGizmo
        {
            get { return _volumeScaleGizmo; }
            set
            {
                if (value == null) return;

                // 只允许场景对象
                #if UNITY_EDITOR
                if (value.gameObject.IsSceneObject()) _volumeScaleGizmo = value;
                else Debug.LogWarning("EditorGizmoSystem.VolumeScaleGizmo: Only scene gizmo object instances are allowed.");
                #else
                _volumeScaleGizmo = value;
                #endif
            }
        }

        public TransformSpace TransformSpace { get { return _transformSpace; } set { ChangeTransformSpace(value); } }
        public GizmoType ActiveGizmoType { get { return _activeGizmoType; } set { ChangeActiveGizmo(value); } }
        public Gizmo ActiveGizmo { get { return _activeGizmo; } }
        public TransformPivotPoint TransformPivotPoint { get { return _transformPivotPoint; } set { ChangeTransformPivotPoint(value); } }
        public bool AreGizmosTurnedOff { get { return _areGizmosTurnedOff; } }

        public ShortcutKeys ActivateTranslationGizmoShortcut { get { return _activateTranslationGizmoShortcut; } }
        public ShortcutKeys ActivateRotationGizmoShortcut { get { return _activateRotationGizmoShortcut; } }
        public ShortcutKeys ActivateScaleGizmoShortcut { get { return _activateScaleGizmoShortcut; } }
        public ShortcutKeys ActivateVolumeScaleGizmoShortcut { get { return _activateVolumeScaleGizmoShortcut; } }
        public ShortcutKeys ActivateGlobalTransformShortcut { get { return _activateGlobalTransformShortcut; } }
        public ShortcutKeys ActivateLocalTransformShortcut { get { return _activateLocalTransformShortcut; } }
        public ShortcutKeys TurnOffGizmosShortcut { get { return _turnOffGizmosShortcut; } }
        public ShortcutKeys TogglePivotShortcut { get { return _togglePivotShortcut; } }
        #endregion

        #region Public Methods
        public void SetGizmoTypeAvailable(GizmoType gizmoType, bool available)
        {
            _gizmoTypeAvailableFlags[(int)gizmoType] = available;

            if(!available && gizmoType == _activeGizmoType)
            {
                int firstAvailable = GetFirstAvailableGizmoTypeIndex();
                if (firstAvailable >= 0) ActiveGizmoType = (GizmoType)firstAvailable;
                else DeactivateAllGizmoObjects();
            }
        }

        public bool IsGizmoTypeAvailable(GizmoType gizmoType)
        {
            return _gizmoTypeAvailableFlags[(int)gizmoType];
        }

        public bool IsAnyGizmoTypeAvailable()
        {
            foreach(var availableFlag in _gizmoTypeAvailableFlags)
            {
                if (availableFlag) return true;
            }

            return false;
        }

        public int GetFirstAvailableGizmoTypeIndex()
        {
            for (int typeIndex = 0; typeIndex < _gizmoTypeAvailableFlags.Length; ++typeIndex)
            {
                if (_gizmoTypeAvailableFlags[typeIndex]) return typeIndex;
            }

            return -1;
        }

        /// <summary>
        /// 检查活动的 Gizmo 是否准备好进行对象操作
        /// </summary>
        /// <remarks>
        /// 如果活动的 Gizmo 在场景中未激活，则该方法返回 false
        /// </remarks>
        public bool IsActiveGizmoReadyForObjectManipulation()
        {
            if (_activeGizmo == null || !_activeGizmo.gameObject.activeSelf) return false;
            return _activeGizmo.IsReadyForObjectManipulation();
        }

        /// <summary>
        /// 此方法将关闭所有 Gizmo 对象。调用此方法后，场景中将没有任何 Gizmo 处于活动状态，即使用户在选择对象时也是如此
        /// </summary>
        /// <remarks>
        /// 通过设置 'ActiveGizmoType' 属性，可以再次打开 Gizmos
        /// </remarks>
        public void TurnOffGizmos()
        {
            _areGizmosTurnedOff = true;
            DeactivateAllGizmoObjects();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 执行任何必要的初始化
        /// </summary>
        private void Start()
        {
            // 确保所有属性都是有效的
            ValidatePropertiesForRuntime();

            DeactivateAllGizmoObjects(); // 最初，所有小工具Gizmo对象都被停用。每当用户选择第一个对象（或对象组）时，正确的小工具Gizmo将被激活
            ConnectObjectSelectionToGizmos(); // 确保小工具Gizmo知道它们控制哪些对象
            ChangeActiveGizmo(_activeGizmoType); // 确保最初使用正确的小工具Gizmo
            ChangeTransformPivotPoint(_transformPivotPoint); // 确保变换中心点设置正确

            // 注册为监听器
            MessageListenerDatabase listenerDatabase = MessageListenerDatabase.Instance;
            listenerDatabase.RegisterListenerForMessage(MessageType.GizmoTransformedObjects, this);
            listenerDatabase.RegisterListenerForMessage(MessageType.GizmoTransformOperationWasUndone, this);
            listenerDatabase.RegisterListenerForMessage(MessageType.GizmoTransformOperationWasRedone, this);
            listenerDatabase.RegisterListenerForMessage(MessageType.VertexSnappingDisabled, this);
            EditorObjectSelection.Instance.SelectionChanged += OnSelectionChanged;
        }

        private void Update()
        {
            if (_activateTranslationGizmoShortcut.IsActiveInCurrentFrame()) ChangeActiveGizmo(GizmoType.Translation);
            else
            if (_activateRotationGizmoShortcut.IsActiveInCurrentFrame()) ChangeActiveGizmo(GizmoType.Rotation);
            else
            if (_activateScaleGizmoShortcut.IsActiveInCurrentFrame()) ChangeActiveGizmo(GizmoType.Scale);
            else
            if (_activateVolumeScaleGizmoShortcut.IsActiveInCurrentFrame()) ChangeActiveGizmo(GizmoType.VolumeScale);

            if (_activateGlobalTransformShortcut.IsActiveInCurrentFrame()) ChangeTransformSpace(TransformSpace.Global);
            else if (_activateLocalTransformShortcut.IsActiveInCurrentFrame()) ChangeTransformSpace(TransformSpace.Local);

            if (_turnOffGizmosShortcut.IsActiveInCurrentFrame()) TurnOffGizmos();
            else
            if (_togglePivotShortcut.IsActiveInCurrentFrame())
            {
                TransformPivotPoint newPivotPoint = _transformPivotPoint == TransformPivotPoint.Center ? TransformPivotPoint.MeshPivot : TransformPivotPoint.Center;
                ChangeTransformPivotPoint(newPivotPoint);
            }
        }

        /// <summary>
        /// 该方法确保所有属性都是有效的，以便可以在运行时使用gizmo系统
        /// </summary>
        private void ValidatePropertiesForRuntime()
        {
            // 确保所有属性都已正确设置
            bool allPropertiesAreValid = true;
            if (_translationGizmo == null)
            {
                Debug.LogError("EditorGizmoSystem.Start: Missing translation gizmo. Please assign a game object with the 'TranslationGizmo' script attached to it.");
                allPropertiesAreValid = false;
            }

            if (_rotationGizmo == null)
            {
                Debug.LogError("EditorGizmoSystem.Start: Missing rotation gizmo. Please assign a game object with the 'RotationGizmo' script attached to it.");
                allPropertiesAreValid = false;
            }

            if (_scaleGizmo == null)
            {
                Debug.LogError("EditorGizmoSystem.Start: Missing scale gizmo. Please assign a game object with the 'ScaleGizmo' script attached to it.");
                allPropertiesAreValid = false;
            }

            if(_volumeScaleGizmo == null)
            {
                Debug.LogError("EditorGizmoSystem.Start: Missing volume scale gizmo. Please assign a game object with the 'VolumeScaleGizmo' script attached to it.");
                allPropertiesAreValid = false;
            }

            // 如果没有正确设置所有属性，我们将退出应用程序
            if (!allPropertiesAreValid) ApplicationHelper.Quit();
        }

        /// <summary>
        /// 从“Start”调用此方法，以便连接3个Gizmo的对象选择集合。我们需要这样做，因为Gizmo小控件需要了解它们控制的对象
        /// </summary>
        private void ConnectObjectSelectionToGizmos()
        {
            EditorObjectSelection objectSelection = EditorObjectSelection.Instance;
            objectSelection.ConnectObjectSelectionToGizmo(_translationGizmo);
            objectSelection.ConnectObjectSelectionToGizmo(_rotationGizmo);
            objectSelection.ConnectObjectSelectionToGizmo(_scaleGizmo);
            objectSelection.ConnectObjectSelectionToGizmo(_volumeScaleGizmo);
        }

        /// <summary>
        /// 禁用所有gizmo对象
        /// </summary>
        private void DeactivateAllGizmoObjects()
        {
            _translationGizmo.gameObject.SetActive(false);
            _rotationGizmo.gameObject.SetActive(false);
            _scaleGizmo.gameObject.SetActive(false);
            _volumeScaleGizmo.gameObject.SetActive(false);
        }

        /// <summary>
        /// 将活动gizmo更改为由指定类型标识的gizmo
        /// </summary>
        /// <remarks>
        /// 调用此方法会将“_areGizmosTurnedOff”布尔值设置为false（即将重新启用Gizmo）
        /// </remarks>
        public void ChangeActiveGizmo(GizmoType gizmoType)
        {
            if (!IsGizmoTypeAvailable(gizmoType)) return;

            // Gizmo不再关闭
            _areGizmosTurnedOff = false;

            // 我们稍后需要这个
            Gizmo oldActiveGizmo = _activeGizmo;

            // 更改活动gizmo类型
            bool sameGizmoType = (gizmoType == _activeGizmoType);
            _activeGizmoType = gizmoType;
            _activeGizmo = GetGizmoByType(gizmoType);

            // 停用旧的活动gizmo，并确保新gizmo的位置和方向相应更新
            if (oldActiveGizmo != null)
            {
                // 停用旧gizmo
                oldActiveGizmo.gameObject.SetActive(false);

                EstablishActiveGizmoPosition();
                UpdateActiveGizmoRotation();
            }

            // 如果选择了任何对象，我们将确保新的活动gizmo在场景中处于活动状态。如果没有选择任何对象，我们将停用它
            // 这样做是因为我们只想在场景中有选定对象时绘制活动gizmo。如果未选择任何对象，则没有要变换的对象
            if (EditorObjectSelection.Instance.NumberOfSelectedObjects != 0) _activeGizmo.gameObject.SetActive(true);
            else _activeGizmo.gameObject.SetActive(false);

            // 更改活动gizmo时，请始终确保禁用平移gizmo的顶点捕捉。否则，如果在启用顶点捕捉的情况下从平移更改为旋转，然后再次启用平移gizmo，则它将在启用顶点捕获的情况下激活，这不是真正需要的
            // TODO: Remove this????    TODO:删除此_translationGizmo
            //_translationGizmo.SnapSettings.IsVertexSnappingEnabled = false;   SnapSettings(快照设置)

            if (!sameGizmoType && ActiveGizmoTypeChanged != null) ActiveGizmoTypeChanged(_activeGizmoType);
        }

        /// <summary>
        /// 将活动变换空间更改为指定值
        /// </summary>
        private void ChangeTransformSpace(TransformSpace transformSpace)
        {
            if (transformSpace == _transformSpace) return;

            // 设置新的变换空间，并确保活动gizmo的旋转已相应更新
            _transformSpace = transformSpace;
            UpdateActiveGizmoRotation();
        }

        /// <summary>
        /// 将变换轴心点更改为指定值
        /// </summary>
        private void ChangeTransformPivotPoint(TransformPivotPoint transformPivotPoint)
        {
            if (_transformPivotPoint == transformPivotPoint) return;

            // 存储新的轴心点
            _transformPivotPoint = transformPivotPoint;

            // 为每个gizmo设置轴心点
            _translationGizmo.TransformPivotPoint = _transformPivotPoint;
            _rotationGizmo.TransformPivotPoint = _transformPivotPoint;
            _scaleGizmo.TransformPivotPoint = _transformPivotPoint;
            _volumeScaleGizmo.TransformPivotPoint = _transformPivotPoint;

            // 建立活动gizmo的位置
            EstablishActiveGizmoPosition();
        }

        /// <summary>
        /// 该方法将返回由gizmo系统管理的一个gizmo，该gizmo与指定的gizmo类型相对应
        /// </summary>
        private Gizmo GetGizmoByType(GizmoType gizmoType)
        {
            if (gizmoType == GizmoType.Translation) return _translationGizmo;
            else if (gizmoType == GizmoType.Rotation) return _rotationGizmo;
            else if (gizmoType == GizmoType.Scale) return _scaleGizmo;
            return _volumeScaleGizmo;
        }

        /// <summary>
        /// 只要需要更新活动gizmo的位置，就会调用此方法
        /// </summary>
        private void EstablishActiveGizmoPosition()
        {
            EditorObjectSelection objectSelection = EditorObjectSelection.Instance;
            if (_activeGizmo.GetGizmoType() != GizmoType.VolumeScale && _activeGizmo != null /*&& !_activeGizmo.IsTransformingObjects()*/) // TODO: 为什么会在这里？似乎没有必要
            {
                // 基于指定的变换轴心点更新位置。如果变换轴心点设置为“MeshPivot”，我们将把gizmo的位置设置为最后一个选定游戏对象的位置。否则，我们将把它设置为选择的中心
                if (_transformPivotPoint == TransformPivotPoint.MeshPivot && objectSelection.LastSelectedGameObject != null) _activeGizmo.transform.position = objectSelection.LastSelectedGameObject.transform.position;
                else _activeGizmo.transform.position = objectSelection.GetSelectionWorldCenter();
            }

            if(_volumeScaleGizmo != null && objectSelection.NumberOfSelectedObjects == 1)
            {
                _volumeScaleGizmo.transform.position = objectSelection.LastSelectedGameObject.transform.position;
                _volumeScaleGizmo.RefreshTargets();
            }
        }

        /// <summary>
        /// 通过考虑所有必要因素（如活动gizmo变换空间）来更新活动gizmo的旋转
        /// </summary>
        private void UpdateActiveGizmoRotation()
        {
            EditorObjectSelection objectSelection = EditorObjectSelection.Instance;
            if(_activeGizmoType == GizmoType.VolumeScale)
            {
                if (objectSelection.NumberOfSelectedObjects == 1)
                    _activeGizmo.transform.rotation = objectSelection.LastSelectedGameObject.transform.rotation;
                return;
            }
            else
            {
                // 如果使用全局变换空间，我们将把gizmo的旋转设置为identity。否则，我们将把旋转设置为场景中最后一个选定对象的旋转
                // 【注意】：缩放gizmo将始终在上一个选定对象的局部空间中定向，因为缩放gizmo始终沿对象的局部轴缩放
                if ((_transformSpace == TransformSpace.Global && _activeGizmoType != GizmoType.Scale) || objectSelection.LastSelectedGameObject == null) _activeGizmo.transform.rotation = Quaternion.identity;
                else _activeGizmo.transform.rotation = objectSelection.LastSelectedGameObject.transform.rotation;
            }
        }
        #endregion

        private void OnSelectionChanged(ObjectSelectionChangedEventArgs selChangedEventArgs)
        {
            EditorObjectSelection objectSelection = EditorObjectSelection.Instance;
            if (_activeGizmo == null) return;

            // 如果未选择任何对象，我们将停用活动gizmo小控件
            if (objectSelection.NumberOfSelectedObjects == 0) _activeGizmo.gameObject.SetActive(false);
            else
            // 如果小控件没有关闭，我们可能需要启用场景中的活动小控件
            if (!_areGizmosTurnedOff)
            {
                // 如果选择了对象，我们将确保在场景中启用活动gizmo.
                if (objectSelection.NumberOfSelectedObjects != 0 && !_activeGizmo.gameObject.activeSelf) _activeGizmo.gameObject.SetActive(true);
            }

            // 确保活动gizmo的位置已正确更新
            EstablishActiveGizmoPosition();

            // 现在，我们必须确保活动gizmo的方向相应
            UpdateActiveGizmoRotation();
        }

        #region Message Handlers
        /// <summary>
        /// 'IMessageListener' 接口方法实现
        /// </summary>
        public void RespondToMessage(Message message)
        {
            switch (message.Type)
            {
                case MessageType.GizmoTransformedObjects:

                    RespondToMessage(message as GizmoTransformedObjectsMessage);
                    break;

                case MessageType.GizmoTransformOperationWasUndone:

                    RespondToMessage(message as GizmoTransformOperationWasUndoneMessage);
                    break;
                
                case MessageType.GizmoTransformOperationWasRedone:

                    RespondToMessage(message as GizmoTransformOperationWasRedoneMessage);
                    break;

                case MessageType.VertexSnappingDisabled:

                    RespondToMessage(message as VertexSnappingDisabledMessage);
                    break;
            }
        }

        private void RespondToMessage(GizmoTransformedObjectsMessage message)
        {
            UpdateActiveGizmoRotation();
            EstablishActiveGizmoPosition();
        }

        /// <summary>
        /// 调用此方法是为了响应gizmo变换操作撤消的消息
        /// </summary>
        private void RespondToMessage(GizmoTransformOperationWasUndoneMessage message)
        {
            // 变换操作撤消后，意味着对象的位置/旋转/比例已更改，因此我们必须重新计算活动gizmo的位置和方向
            EstablishActiveGizmoPosition();
            UpdateActiveGizmoRotation();
        }

        /// <summary>
        /// 调用此方法是为了响应gizmo变换操作重做消息
        /// </summary>
        private void RespondToMessage(GizmoTransformOperationWasRedoneMessage message)
        {
            // 重做变换操作时，意味着对象的位置/旋转/比例已更改，因此我们必须重新计算活动gizmo的位置和方向
            EstablishActiveGizmoPosition();
            UpdateActiveGizmoRotation();
        }

        /// <summary>
        /// 调用此方法是为了响应顶点捕捉禁用消息
        /// </summary>
        private void RespondToMessage(VertexSnappingDisabledMessage message)
        {
            // 禁用顶点捕捉后，请确保相应地定位活动gizmo，因为使用顶点捕捉时，其位置将更改为对象网格顶点的位置
            EstablishActiveGizmoPosition();
        }
        #endregion
    }
}