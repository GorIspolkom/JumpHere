using Assets.Scripts.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HairyEngine.HairyCamera
{
    abstract class P2PDriver : BaseCameraScript, IPreMove
    {
        public int PriorityOrder => 100;

        public void HandleStartMove(Vector3 position)
        {
            if (enabled)
            {
                Tuple<Vector3, Vector3> line = GetP2PLine();
                Vector3 cor = line.Item2 - line.Item1;
                float hNewPosition = GetCoordInAxis(position.x, line.Item1.x, line.Item2.x, cor.x != 0);
                float vNewPosition = GetCoordInAxis(position.y, line.Item1.y, line.Item2.y, false);
                float dNewPosition = GetCoordInAxis(position.z, line.Item1.z, line.Item2.z, cor.z != 0);
                Vector3 newPosition = new Vector3(hNewPosition, vNewPosition, dNewPosition);
                BaseCameraController.Targets.SetNewCurrentPosition(newPosition);
            }
        }
        protected abstract Tuple<Vector3, Vector3> GetP2PLine();
        private float GetCoordInAxis(float val, float p1, float p2, bool cor)
        {
            if (cor)
                return val;
            float k = (p2 - p1);
            return Mathf.Lerp(p1, p2, k == 0 ? 0 : 1 + ((val - p1) / k));
        }
    }
}
