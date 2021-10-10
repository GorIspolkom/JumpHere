using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Camera.Cinematics
{
    [Serializable]
    class CinematicStructure
    {
        [Tooltip("position which is next by a scenario")]
        [SerializeField] Vector3 nextPosition;
        [Tooltip("position which is next by a scenario")]
        [SerializeField] Vector3 nextRotation;
        [Tooltip("time which spend an object for finishing path from his position to nextPosition")]
        [SerializeField] float duration;
        [SerializeField] Transform _transform;
    }
}
