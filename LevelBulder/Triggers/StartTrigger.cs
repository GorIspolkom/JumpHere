using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class StartTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Debug.Log("Start track");
                LevelBuilder.GetLevelBuilder().NewTrack();
            }
        }
    }
}
