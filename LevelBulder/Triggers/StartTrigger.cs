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
                StartCoroutine(LevelBuilder.GetLevelBuilder().NewTrack());
                Debug.Log("Trigger worked");
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
