using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private ICommand _command;
        public void SetCommand(ICommand command) => _command = command;
        public PlayerController()
        {

        }
    }
}
