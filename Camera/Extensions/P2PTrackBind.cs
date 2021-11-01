using Assets.Scripts.Data;
using Assets.Scripts.Level;
using System;
using UnityEngine;

namespace HairyEngine.HairyCamera
{
    class P2PTrackBind : P2PDriver
    {
        [SerializeField] TrackBuilder2 trackBuilder;
        protected override Tuple<Vector3, Vector3> GetP2PLine()
        {
            if (GameHandler.Instance.IsGame)
            {
                Vector3 curPoint = trackBuilder.data.CurrentFloor.Center;
                Vector3 nextPoint = trackBuilder.data.NextFloor.Center;
                return new Tuple<Vector3, Vector3>(curPoint, nextPoint);
            }
            return new Tuple<Vector3, Vector3>(trackBuilder.data.CurrentFloor.Center, trackBuilder.data.CurrentFloor.Center);
         }
    }
}