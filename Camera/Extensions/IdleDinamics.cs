using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HairyEngine.HairyCamera
{
    class IdleDinamics : BaseCameraScript, IPreMove
    {
        public float MaxHorizontalInfluence = 3f;
        public float MaxVerticalInfluence = 2f;

        public float SmoothVelocity = 1f;

        private Vector3 generatedPosition;
        private Vector3 currentInflunce;
        private Vector3 startInflunce;
        private float time;
        private float currentTime;

        bool isIdleCondition;

        public int PriorityOrder => 1;

        private void Awake()
        {
            isIdleCondition = false;
        }

        public void HandleStartMove(Vector3 position)
        {
            if(enabled)
            {
                if (isIdleCondition)
                {
                    if (BaseCameraController.Targets.IsMovement)
                    {
                        Debug.Log("I move");
                        isIdleCondition = false;
                        return;
                    } 

                    if (currentTime > time)
                    {
                        Generate();
                    }

                    currentInflunce = Vector3.Lerp(startInflunce, generatedPosition, currentTime / time);
                    currentTime += Time.deltaTime;

                    BaseCameraController.ApplyInfluence(currentInflunce);
                }
                else if (BaseCameraController.PrevVelocity < .0001f && !BaseCameraController.Targets.IsMovement)
                {
                    isIdleCondition = true;
                    currentInflunce = Vector3.zero;
                    Generate();
                }
            }
        }
        private void Generate()
        {
            float hInflunce = UnityEngine.Random.Range(-1f, 1f);
            float vInflunce = UnityEngine.Random.Range(-1f, 1f);
            generatedPosition = new Vector3(hInflunce * MaxHorizontalInfluence, vInflunce * MaxVerticalInfluence);
            startInflunce = currentInflunce;
            currentTime = 0f;
            time = (currentInflunce - generatedPosition).magnitude / SmoothVelocity;
        }
    }
}
