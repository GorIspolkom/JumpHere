using ECM.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class CharacterController : BaseCharacterController
    {
        //private static BaseCharacterController _instance;
        //public static BaseCharacterController Instance => _instance;
        private static CharacterController _instance;
        public static CharacterController Instance => _instance;
        private Vector3 _direction;

        public override void Awake()
        {
            base.Awake();
            _instance = this;
            _direction = Vector3.forward;
        }
        public override void Update()
        {
            base.Update();
            Vector3 velocity = movement.velocity;
            velocity.y = 0;
            Data.AddPath(velocity.magnitude * Time.deltaTime);
        }
        protected override void HandleInput()
        {
            if (Input.GetKey(KeyCode.G))
                moveDirection = Vector3.zero;
            else
                moveDirection = _direction;
            jump = Input.GetButton("Jump");
        }

        public void ChangeDirection() =>
            _direction = (_direction != Vector3.forward) ? Vector3.forward : Vector3.right;
            
    }
}
