using HairyEngine.HairyCamera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects
{
    enum PositionOnView
    {
        FreeAspect,
        Max,
        Min,
        Center
    }
    class OnCameraViewObject : BaseCameraScript, IPostZoom
    {
        [SerializeField] PositionOnView xPositionOnView = PositionOnView.FreeAspect;
        [SerializeField] float x;
        [SerializeField] PositionOnView yPositionOnView = PositionOnView.FreeAspect;
        [SerializeField] float y;
        Transform _transform;

        public int PriorityOrder => 1000;

        private void Awake()
        {
            _transform = transform;
        }
        private void Start()
        {
            CameraHandler.Instance.AddExtension(this);
            _transform.parent = BaseCameraController.transform;
        }
        private float GetPosition(PositionOnView positionOnView, float size, float defaultValue, float offset)
        {
            switch (positionOnView)
            {
                case PositionOnView.FreeAspect:
                    return defaultValue - offset * Mathf.Sign(defaultValue);
                case PositionOnView.Max:
                    return size - offset;
                case PositionOnView.Min:
                    return -size + offset;
                case PositionOnView.Center:
                    return 0f;
                default:
                    return defaultValue;
            }
        }

        public void HandleZoomChange(Vector2 newSize)
        {
            Vector3 scale = Vector3.one * newSize.y / 10f;
            _transform.localScale = scale;
            _transform.localPosition = new Vector3(GetPosition(xPositionOnView, newSize.x, x, scale.x), GetPosition(yPositionOnView, newSize.y, y, scale.y), scale.z);
            Debug.Log(newSize.x);
            Debug.Log(newSize.y);
        }
    }
}
