using Assets.Scripts.Level;
using ECM.Controllers;
using HairyEngine.HairyCamera;
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
        public static CharacterController Instance => _instance == null ? FindObjectOfType<CharacterController>() : _instance;
        public Transform playerTransform { get; private set; }
        private Vector3 _direction;
        private Animator _anim;

        public override void Awake()
        {
            base.Awake();
            _instance = this;
            _direction = Vector3.forward;
            _anim = GetComponent<Animator>();
            playerTransform = transform;
        }
        public override void Update()
        {
            base.Update();
            Vector3 velocity = movement.velocity.NablaMultiply(TrackBuilder.Instance.CurrentDirect);
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
            if (TrackBuilder.Instance.IsTurnPosible && jump)
            {
                Debug.Log("Jump");
                TrackBuilder.Instance.TurnAxis();
                _direction = TrackBuilder.Instance.CurrentDirect;
            }
        }

        public void SpeedChange(float speedChange)
        {
            speed -= speedChange;
            //movement.velocity -= Vector3.one * speed;
        }
    }
}
