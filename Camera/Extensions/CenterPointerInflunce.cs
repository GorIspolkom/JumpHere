using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HairyEngine.HairyCamera
{
    class CenterPointerInflunce : BaseCameraScript, IPreMove
    {
        public float MaxHorizontalInfluence = 3f;
        public float MaxVerticalInfluence = 2f;

        public float InfluenceSmoothness = .2f;

        Vector2 _influence;
        Vector2 _velocity;
        public int PriorityOrder { get => 1; }

        public void HandleStartMove(Vector3 direction)
        {
            if (enabled)
            {
                Debug.Log(enabled);
                var mousePosViewport = BaseCameraController.GameCamera.ScreenToViewportPoint(Input.mousePosition);

                var mousePosViewportH = mousePosViewport.x.Remap(0, 1, -1, 1);
                var mousePosViewportV = mousePosViewport.y.Remap(0, 1, -1, 1);

                var hInfluence = mousePosViewportH * MaxHorizontalInfluence;
                var vInfluence = mousePosViewportV * MaxVerticalInfluence;

                _influence = Vector2.SmoothDamp(_influence, new Vector2(hInfluence, vInfluence), ref _velocity, InfluenceSmoothness, Mathf.Infinity, Time.deltaTime);

                BaseCameraController.ApplyInfluence(_influence);
            }
        }
    }
}
