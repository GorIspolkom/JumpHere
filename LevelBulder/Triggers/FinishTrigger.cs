using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class FinishTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Players.CharacterController.Instance.ChangeDirection();
                Debug.Log("Finish trigger worked");
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}


