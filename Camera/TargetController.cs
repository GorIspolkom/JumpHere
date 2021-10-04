using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HairyEngine.HairyCamera
{
    [Serializable]
    class TargetController
    {
        [HideInInspector]
        public Vector3 prevCenter;
        [HideInInspector]
        public Vector3 currentCenter;
        [HideInInspector]
        public Vector2 minSizes;
        public bool IsMovement => prevCenter != currentCenter;
        private Func<Vector3, Vector3> Vector3D;

        [SerializeField] List<CameraTarget2D> _targets;

        public TargetController()
        {
            _targets = new List<CameraTarget2D>();
            currentCenter = Vector3.zero;
            prevCenter = Vector3.zero;
            minSizes = Vector3.zero;
        }
        public void InitAxis(MovementAxis axis)
        {
            switch (axis)
            {
                case MovementAxis.XY:
                    Vector3D = (vector) => new Vector3(vector.x, vector.y, vector.z);
                    break;
                case MovementAxis.XZ:
                    Vector3D = (vector) => new Vector3(vector.x, vector.z, vector.y);
                    break;
                case MovementAxis.YZ:
                    Vector3D = (vector) => new Vector3(vector.z, vector.y, vector.x);
                    break;
            }

        }
        public void Update()
        {
            prevCenter = currentCenter;
            if (_targets.Count == 0)
                return;
            else
            {
                currentCenter = Vector2.zero;
                Vector3 position = Vector3D(_targets[0].TargetPosition);
                Vector2 width = Vector2.one * position.x;
                Vector2 height = Vector2.one * position.y;
                foreach (CameraTarget2D cameraTarget2D in _targets)
                {
                    if (cameraTarget2D.TargetTransform == null)
                    {
                        _targets.Remove(cameraTarget2D);
                        continue;
                    }
                    position = Vector3D(cameraTarget2D.TargetPosition);
                    currentCenter += cameraTarget2D.TargetPosition;
                    if (position.x > width.y)
                        width.y = position.x;
                    else if(position.x < width.x)
                        width.x = position.x;

                    if (position.y > height.y)
                        height.y = position.y;
                    else if (position.y < height.x)
                        height.x = position.y;
                }
                currentCenter /= _targets.Count;
                minSizes.x = width.y - width.x;
                minSizes.x = minSizes.x > 10f ? minSizes.x : 10f;
                minSizes.y = height.y - height.x;
                minSizes.y = minSizes.y > 10f ? minSizes.y : 10f;
            }
        }
        public void AddTarget(Transform target)
        {
            _targets.Add(new CameraTarget2D(target));
            Update();
            prevCenter = currentCenter;
        }
        public void RemoveTarget(Transform target)
        {
            foreach(CameraTarget2D cameraTarget in _targets)
                if(cameraTarget.Equals(_targets))
                _targets.Remove(cameraTarget);
        }
    }
}
