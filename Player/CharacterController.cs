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
        private static CharacterController _instance;
        public static CharacterController Instance => _instance;
        private Vector3 _direction;
        private Animator _anim;

        public override void Awake()
        {
            base.Awake();
            _instance = this;
            _direction = Vector3.forward;
            _anim = GetComponent<Animator>();
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
            {
                moveDirection = Vector3.zero;
                _anim.SetBool("Run", false);
            }
            else
            {
                moveDirection = _direction;
                _anim.SetBool("Run", true);
            }
            jump = Input.GetButton("Jump");
            Debug.Log("Movement");
        }

        public void ChangeDirection() =>
            _direction = (_direction != Vector3.forward) ? Vector3.forward : Vector3.right;
            
    }
}
