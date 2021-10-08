using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HairyEngine.HairyCamera
{
    public class CenterDistanceInflunce : BaseCameraScript, IPreMove
    {
        public float MaxHorizontalInfluence = 3f;
        public float MaxVerticalInfluence = 2f;

        public float InfluenceSmoothness = .2f;

        Vector2 _influence;
        Vector2 _velocity;
        public int PriorityOrder { get => 2; }

        public void HandleStartMove(Vector3 position)
        {
            if (enabled)
            {
                var direction = BaseCameraController.Targets.velocity.normalized;

                var hInfluence = direction.x * MaxHorizontalInfluence;
                var dInfluence = direction.z * MaxVerticalInfluence;

                Vector2 influnce = new Vector3(hInfluence, dInfluence, 0);

                _influence = Vector2.SmoothDamp(_influence, influnce, ref _velocity, InfluenceSmoothness, Mathf.Infinity, Time.deltaTime);

                BaseCameraController.ApplyInfluence(_influence);
            }
        }
    }
}