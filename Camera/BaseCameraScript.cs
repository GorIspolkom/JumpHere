using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HairyEngine.HairyCamera
{
    public class BaseCameraScript : MonoBehaviour
    {
        public CameraHandler BaseCameraController
        {
            get
            {
                if (_mainCamera != null) return _mainCamera;

                _mainCamera = GetComponent<CameraHandler>();

                if (_mainCamera == null && Camera.main != null)
                    _mainCamera = Camera.main.GetComponent<CameraHandler>();

                if (_mainCamera == null)
                    _mainCamera = FindObjectOfType(typeof(CameraHandler)) as CameraHandler;

                return _mainCamera;
            }

            set { _mainCamera = value; }
        }

        [SerializeField]
        private CameraHandler _mainCamera;
        protected Transform _transform;

        protected Func<Vector3, float> Vector3H;
        protected Func<Vector3, float> Vector3V;
        protected Func<Vector3, float> Vector3D;
        protected Func<float, float, Vector3> VectorHV;
        protected Func<float, float, float, Vector3> VectorHVD;

        bool _enabled;

        protected virtual void Awake()
        {
            _transform = transform;
            //BaseCameraController.AddExtension(this);

            if (enabled)
                Enable();
        }

        protected virtual void OnEnable()
        {
            Enable();
        }

        protected virtual void OnDisable()
        {
            Disable();
        }

        protected virtual void OnDestroy()
        {
            Disable();
        }

        /// <summary>Called when the method Reset is called on the Core. Use it to reset an extension.</summary>
        public virtual void OnReset()
        {
        }

        void Enable()
        {
            if (!(_enabled || _mainCamera == null))
            {
                _enabled = true;
            }
        }

        void Disable()
        {
            if (_mainCamera != null && _enabled)
            {
                _enabled = false;
            }
        }
    }
}
