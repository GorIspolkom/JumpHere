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

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        public virtual void OnReset()
        {
        }
    }
}
