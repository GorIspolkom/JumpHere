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
                Debug.Log(1);
                StartCoroutine(LevelBuilder.GetLevelBuilder().NewTrack());
            }
        }
    }
}
