using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HairyEngine.HairyCamera
{
    class ZoomBaseOnTargetVelocity : BaseCameraScript, IViewSizeChanged
    {
        public float CamVelocityForZoomOut = 5f;
        public float CamVelocityForZoomIn = 2f;

        [Range(0f, 3f)]
        public float ZoomInSmoothness = 1f;
        [Range(0f, 3f)]
        public float ZoomOutSmoothness = 1f;

        public float MaxZoomInAmount = 2f;
        public float MaxZoomOutAmount = 2f;

        float _zoomVelocity;
        float _initialCamSize;
        float _previousCamSize;

        float _currentVelocity;

        public int PriorityOrder => 1;
        private void Awake()
        {
            _initialCamSize = BaseCameraController.ScreenSizeInWorldCoordinates.y;
        }

        public float HandleSizeChanged(float nextSize)
        {
            if (!enabled)
                return nextSize;

            // If the camera is bounded, reset the easing
            if (_previousCamSize == BaseCameraController.ScreenSizeInWorldCoordinates.y)
                _zoomVelocity = 0f;

            var currentSize = BaseCameraController.ScreenSizeInWorldCoordinates.y;
            var targetSize = currentSize;

            _currentVelocity = BaseCameraController.Targets.velocity.magnitude / Time.deltaTime;
            // Zoom out
            if (_currentVelocity > CamVelocityForZoomIn)
            {
                var speedPercentage = (_currentVelocity - CamVelocityForZoomIn) / (CamVelocityForZoomOut - CamVelocityForZoomIn);
                var newSize = MaxZoomOutAmount * Mathf.Clamp01(speedPercentage) * 2f;

                if (newSize > currentSize)
                    targetSize = newSize;
            }
            // Zoom in
            else
            {
                var speedPercentage = (1 - (_currentVelocity / CamVelocityForZoomIn)).Remap(0.0f, 1.0f, 0.5f, 1.0f);
                var newSize = (MaxZoomInAmount * speedPercentage);

                if (newSize < currentSize)
                    targetSize = newSize;
            }

            if (Mathf.Abs(currentSize - targetSize) > .001f)
            {
                float smoothness = (targetSize < currentSize) ? ZoomInSmoothness : ZoomOutSmoothness;
                targetSize = Mathf.SmoothDamp(currentSize, targetSize, ref _zoomVelocity, smoothness, Mathf.Infinity, Time.deltaTime);
                //targetSize = Mathf.Lerp(currentSize, targetSize, smoothness * Time.deltaTime);
            }
            else
                targetSize = currentSize;
            var zoomAmount = targetSize - currentSize;

            // Detect if the camera size is bounded
            _previousCamSize = BaseCameraController.ScreenSizeInWorldCoordinates.y;

            // Return the zoom delta - delta already factored in by SmoothDamp
            return nextSize + zoomAmount;
        }
    }
}
