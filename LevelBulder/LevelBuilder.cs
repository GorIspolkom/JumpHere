using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField]
        private Track[] _tracks;

        private void Start()
        {
            new Track(new Vector3(1, 1, 1));
        }
    }
}
