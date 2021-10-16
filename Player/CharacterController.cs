using Assets.Scripts.Level;
using ECM.Controllers;
using HairyEngine.HairyCamera;
using System;
using System.Collections;
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

        Vector3 _influence;
        Vector3 _velocity;

        public override void Awake()
        {
            base.Awake();
            _instance = this;
            _direction = Vector3.forward;
            _anim = GetComponent<Animator>();
            playerTransform = transform;
        }
        private void Start()
        {
            moveDirection = Vector3.forward;
            _anim.SetBool("Run", true);
        }
        public override void Update()
        {
            base.Update();
            Vector3 velocity = movement.velocity.NablaMultiply(TrackBuilder.Instance.CurrentDirect);
            Data.AddPath(velocity.magnitude * Time.deltaTime);

            //if (movement.isGrounded)
            //    if (moveDirection.x != 0 && moveDirection.z == 0|| moveDirection.z != 0 && moveDirection.x == 0)
            //    {
            //        _influence = Vector3.SmoothDamp(_influence, -TrackBuilder.Instance.ToCenterVelocity(playerTransform.position), ref _velocity, 1f, Mathf.Infinity, Time.deltaTime);
            //        moveDirection += _influence;
            //    }
        }
        protected override void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                moveDirection = Vector3.zero;
                _anim.SetBool("Run", false);
            }
            else if(Input.GetKeyUp(KeyCode.G))
            {
                moveDirection = _direction;
                _anim.SetBool("Run", true);
            }
            jump = Input.GetButton("Jump");
            if(jump)
                moveDirection = _direction;
            if (TrackBuilder.Instance.IsTurnPosible && jump)// && Input.GetKeyDown(KeyCode.F))
            {
                //StartCoroutine(JumpOffset());
                TrackBuilder.Instance.TurnAxis();
                moveDirection = TrackBuilder.Instance.CurrentDirect;
                _direction = TrackBuilder.Instance.CurrentDirect;
            }
        }
        IEnumerator JumpOffset()
        {
            yield return new WaitForSeconds(0.3f);
            jump = true;
            Jump();
        }
        public void SpeedChange(float speedChange)
        {
            speed -= speedChange;
            //movement.velocity -= Vector3.one * speed;
        }
    }
}
