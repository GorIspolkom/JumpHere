using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HairyEngine.HairyCamera
{
    public class CameraHandler : MonoBehaviour
    {
        public float hight = 15f;
        public bool FollowHorizontal = true;
        public float HorizontalFollowSmoothness = 0.15f;

        public bool FollowVertical = true;
        public float VerticalFollowSmoothness = 0.15f;

        public bool FollowHigh = true;
        public float HighFollowSmoothness = 0.15f;

        public float zoomFollowSmoothness = 0.15f;

        public Vector3? exclusiveTargetPosition;

        public Camera GameCamera => _gameCamera;
        public TargetController Targets => targetController;
        public Transform CameraTransform => _transform;
        public Vector3 WorldToLocalPlanty(Vector3 vector) => IsometricVector3(Vector3D(vector.x, vector.y, vector.z));
        public float PrevVelocity { get; private set; }
        public Vector3 CameraTargetPosition { get; private set; }
        public Vector3 CameraPosition { get => _transform.position; }
        public float CameraTargetSize { get; private set; }
        public Vector2 ScreenSizeInWorldCoordinates { get; private set; }
        public bool IsMovement => _prevCameraPos != _transform.position;
        public bool IsZoomOnTarget => Mathf.Abs(CameraTargetSize - ScreenSizeInWorldCoordinates.y) > 0.02f;
        public bool IsOutTarget {
            get
            {
                Vector2 r = targetController.currentCenter - _transform.position;
                return Vector2D(FollowHorizontal ? r.x : 0, FollowVertical ? r.y : 0).sqrMagnitude > 0.002f;
            }
        }
        public static CameraHandler Instance
        {
            get
            {
                if (Equals(_instance, null))
                {
                    _instance = FindObjectOfType(typeof(CameraHandler)) as CameraHandler;

                    if (Equals(_instance, null))
                        throw new UnityException("ProCamera2D does not exist.");
                }
                return _instance;
            }
        }

        [Range(-1f, 1f)]
        public float OffsetX;
        [Range(-1f, 1f)]
        public float OffsetY;

        [SerializeField] TargetController targetController;
        [SerializeField] MovementAxis axis;
        [SerializeField] Isometric isometricAxis;
        [SerializeField] bool isCenterOnTargetOnStart;
        List<BaseCameraScript> _cameraScripts;
        List<Vector3> _influences = new List<Vector3>();

        private static CameraHandler _instance;
        private Transform _transform;
        private Camera _gameCamera;
        private Vector3 _prevCameraPos;

        Func<Vector3, float> AxisX;
        Func<Vector3, float> AxisY;
        Func<Vector3, float> AxisZ;
        Func<float, float, Vector3> Vector2D;
        Func<float, float, float, Vector3> Vector3D;
        Func<Vector3, Vector3> IsometricVector3;

        private List<IPreMove> preMoveActions;
        private List<IPostMove> postMoveActions;
        private List<IPostZoom> postZoomActions;
        private List<IDeltaPositionChanger> deltaPositionChangers;
        private List<IPositionChanged> positionChangers;
        private List<IViewSizeDeltaChange> deltaViewSizeChangers;
        private List<IViewSizeChanged> viewSizeChangers;

        private void Awake()
        {
            _transform = transform;
            _prevCameraPos = _transform.position;
            _instance = this;
            _cameraScripts = new List<BaseCameraScript>();
            _gameCamera = GetComponent<Camera>();
            if (_gameCamera == null)
                Debug.LogError("CameraHandler should be on Camera but it isn't like that");
            ResetAxisFunctions();
        }
        private void Start()
        {
            foreach (BaseCameraScript component in _transform.GetComponents<BaseCameraScript>())
                _cameraScripts.Add(component);
            InitExtensionDelegates();

            CalculateScreenSize();
            targetController.Update();
            CameraTargetSize = targetController.minSizes.y > targetController.minSizes.x ? targetController.minSizes.y : targetController.minSizes.x / GameCamera.aspect;
            SetScreenSize(CameraTargetSize);
            if (isCenterOnTargetOnStart)
            {
                _transform.position = Vector3D(targetController.currentCenter.x, targetController.currentCenter.y, hight);
            }
        }

        private void Update()
        {
            targetController.Update();
            //if (targetController.IsMovement || IsOutTarget)
                Move();
            Zoom();
        }
        private void Zoom()
        {
            // Cycle through the size delta changers
            var deltaSize = 0f;
            foreach (IViewSizeDeltaChange viewSizeDeltaChange in deltaViewSizeChangers)
                deltaSize = viewSizeDeltaChange.AdjustSize(deltaSize);

            // Calculate the new size
            var newSize = ScreenSizeInWorldCoordinates.y + deltaSize;
            // Cycle through the size overriders
            foreach (IViewSizeChanged viewSizeChanger in viewSizeChangers)
            {
                newSize = viewSizeChanger.HandleSizeChanged(newSize);
            }

            // Apply the new size
            if (newSize != ScreenSizeInWorldCoordinates.y)
                SetScreenSize(newSize);

            Debug.Log(postZoomActions.Count);
            foreach (IPostZoom postZoomAction in postZoomActions)
            {
                postZoomAction.HandleZoomChange(ScreenSizeInWorldCoordinates);
            }
        }
        private void Move()
        {
            _prevCameraPos = _transform.position;
            Vector2 delta = targetController.currentCenter - _transform.position;

            foreach (IPreMove preMoveAction in preMoveActions)
                preMoveAction.HandleStartMove(targetController.currentCenter);

            if (exclusiveTargetPosition.HasValue)
            {
                exclusiveTargetPosition -= _transform.position;
                CameraTargetPosition = Vector3D(AxisX(exclusiveTargetPosition.Value), AxisY(exclusiveTargetPosition.Value), AxisZ(exclusiveTargetPosition.Value));
                exclusiveTargetPosition = null;
            }
            else
            {
                //Follow only on selected axis
                var cameraTargetPositionX = FollowHorizontal ? AxisX(targetController.currentCenter) : AxisX(_transform.position);
                var cameraTargetPositionY = FollowVertical ? AxisY(targetController.currentCenter) : AxisY(_transform.position);
                var cameraTargetPositionZ = FollowHigh ? AxisZ(targetController.currentCenter) + hight : AxisZ(_transform.position);
                CameraTargetPosition = Vector3D(cameraTargetPositionX, cameraTargetPositionY, cameraTargetPositionZ );
                
                //Calculate influences
                foreach (Vector3 influnce in _influences)
                    CameraTargetPosition += influnce;
                _influences.Clear();
            }
            //Add offset
            CameraTargetPosition += Vector2D(FollowHorizontal ? OffsetX : 0, FollowVertical ? OffsetY : 0);
            CameraTargetPosition = IsometricVector3(CameraTargetPosition);

            // Calculate the base delta movement
            var horizontalDeltaMovement = Mathf.Lerp(AxisX(_transform.position), AxisX(CameraTargetPosition), HorizontalFollowSmoothness * Time.deltaTime);
            var verticalDeltaMovement = Mathf.Lerp(AxisY(_transform.position), AxisY(CameraTargetPosition), VerticalFollowSmoothness * Time.deltaTime);
            var highDeltaMovement = Mathf.Lerp(AxisZ(_transform.position), AxisZ(CameraTargetPosition), HighFollowSmoothness * Time.deltaTime);

            horizontalDeltaMovement -= AxisX(_transform.position);
            verticalDeltaMovement -= AxisY(_transform.position);
            highDeltaMovement -= AxisZ(_transform.position);

            var deltaPosition = Vector3D(horizontalDeltaMovement, verticalDeltaMovement, highDeltaMovement);
            PrevVelocity = deltaPosition.magnitude;

            foreach (IDeltaPositionChanger positionChanger in deltaPositionChangers)
                deltaPosition = positionChanger.AdjustDelta(deltaPosition);

            Vector3 newPosition = _transform.position + deltaPosition;
            foreach (IPositionChanged positionChanger in positionChangers)
                newPosition = positionChanger.HandlePositionChange(newPosition);

            _transform.position = newPosition;

            foreach(IPostMove postMoveAction in postMoveActions)
                postMoveAction.HandleStopMove(newPosition);
        }
        void SetScreenSize(float newSize)
        {
            if (GameCamera.orthographic)
            {
                GameCamera.orthographicSize = newSize;
            }
            else
            {
                _transform.localPosition = Vector3D(
                    AxisX(_transform.localPosition),
                    AxisY(_transform.localPosition),
                    newSize / Mathf.Tan(GameCamera.fieldOfView * 0.5f * Mathf.Deg2Rad));
            }

            ScreenSizeInWorldCoordinates = new Vector2(newSize * GameCamera.aspect, newSize);
        }
        public void ApplyInfluence(Vector2 influence)
        {
            if (Time.deltaTime < .0001f || float.IsNaN(influence.x) || float.IsNaN(influence.y))
                return;

            _influences.Add(Vector2D(influence.x, influence.y));
        }

        public void AddExtension(BaseCameraScript extension)
        {
            _cameraScripts.Add(extension);
            InitExtensionDelegates();
        }

        public List<T> SortCameraComponents<T>() where T : ICameraComponent
        {
            List<T> components = new List<T>();
            foreach (object cameraScript in _cameraScripts)
                if (cameraScript is T)
                    components.Add((T)cameraScript);
            Debug.Log(_cameraScripts.Select(a => a is T) as List<BaseCameraScript>);
            return components.OrderBy(a => a.PriorityOrder).ToList();
        }

        public void AddTarget(Transform target)
        {
            targetController.AddTarget(target);
        }
        private void CalculateScreenSize()
        {
            GameCamera.ResetAspect();
            var p1 = GameCamera.ViewportToWorldPoint(new Vector3(0, 0, GameCamera.nearClipPlane));
            var p2 = GameCamera.ViewportToWorldPoint(new Vector3(1, 0, GameCamera.nearClipPlane));
            var p3 = GameCamera.ViewportToWorldPoint(new Vector3(1, 1, GameCamera.nearClipPlane));

            var width = (p2 - p1).magnitude / 2f;
            var hight = (p3 - p2).magnitude / 2f;
            ScreenSizeInWorldCoordinates = new Vector2(width, hight);
            CameraTargetSize = hight;
        }
        private void InitExtensionDelegates()
        {
            preMoveActions = SortCameraComponents<IPreMove>();
            postMoveActions = SortCameraComponents<IPostMove>();
            postZoomActions = SortCameraComponents<IPostZoom>();
            deltaPositionChangers = SortCameraComponents<IDeltaPositionChanger>();
            positionChangers = SortCameraComponents<IPositionChanged>();
            deltaViewSizeChangers = SortCameraComponents<IViewSizeDeltaChange>();
            viewSizeChangers = SortCameraComponents<IViewSizeChanged>();
        }
        void ResetAxisFunctions()
        {
            targetController.InitAxis(axis);
            switch (axis)
            {
                case MovementAxis.XY:
                    AxisX = vector => vector.x;
                    AxisY = vector => vector.y;
                    AxisZ = vector => vector.z;
                    Vector2D = (x, y) => new Vector3(x, y, 0);
                    Vector3D = (x, y, z) => new Vector3(x, y, z);
                    break;
                case MovementAxis.XZ:
                    AxisX = vector => vector.x;
                    AxisY = vector => vector.z;
                    AxisZ = vector => vector.y;
                    Vector2D = (x, y) => new Vector3(x, 0, y);
                    Vector3D = (x, y, z) => new Vector3(x, z, y);
                    break;
                case MovementAxis.YZ:
                    AxisX = vector => vector.z;
                    AxisY = vector => vector.y;
                    AxisZ = vector => vector.x;
                    Vector2D = (x, y) => new Vector3(0, y, x);
                    Vector3D = (x, y, z) => new Vector3(z, y, x);
                    break;
            }

            switch (isometricAxis)
            {
                case Isometric.None:
                    IsometricVector3 = (vector) => vector;
                    break;
                case Isometric.Isometric:
                    switch (axis)
                    {
                        case MovementAxis.XZ:
                            IsometricVector3 = (vector) =>
                            {
                                float cameraAngleX = _transform.eulerAngles.x * Mathf.Deg2Rad;
                                float cameraAngleY = _transform.eulerAngles.y * Mathf.Deg2Rad;
                                float cameraAngleZ = _transform.eulerAngles.z * Mathf.Deg2Rad;
                                float tanX = 1f / Mathf.Sin(cameraAngleX);
                                float tanY = Mathf.Tan(cameraAngleY);
                                float cosY = Mathf.Cos(cameraAngleY);
                                Vector3 offset = GameCamera.cameraToWorldMatrix * (new Vector3(0, 0, (-tanX - tanY) * cosY) * hight);
                                return new Vector3(vector.x - offset.x, vector.y, vector.z - offset.z);
                            };
                            break;
                    }
                    break;
            }
        }
    }
}

//public void AddPreMoveDelegate(IPreMove preMoveComponent) =>
//    onPreMoveAction += preMoveComponent.HandleStartMove;
//public void RemovePreMoveDelegate(IPreMove preMoveComponent) =>
//    onPreMoveAction -= preMoveComponent.HandleStartMove;

//public void AddMoveDelegate(IMove moveComponent) =>
//    onMoveAction += moveComponent.Move;
//public void RemoveMoveDelegate(IMove moveComponent) =>
//    onMoveAction -= moveComponent.Move;

//public void AddPostMoveDelegate(IPostMove postMoveComponent) =>
//    onPreMoveAction += postMoveComponent.HandleStopMove;
//public void RemovePostMoveDelegate(IPostMove postMoveComponent) =>
//    onPreMoveAction -= postMoveComponent.HandleStopMove;

//public void AddTargetChangeDelegate(ITargetPositionChange preMoveComponent) =>
//    onPreMoveAction += preMoveComponent.HandleTargetCollapse;
//public void RemoveTargetChangeDelegate(ITargetPositionChange preMoveComponent) =>
//    onPreMoveAction -= preMoveComponent.HandleTargetCollapse;