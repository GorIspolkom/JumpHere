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
    public class CharacterAutoRunController : BaseCharacterController
    {
        private static CharacterAutoRunController _instance;
        public static CharacterAutoRunController Instance => _instance == null ? FindObjectOfType<CharacterAutoRunController>() : _instance;
        public Transform playerTransform { get; private set; }
        public Vector3 CurrentDirect { get; private set; }
        //public float OffsetInDirection
        //{
        //    get
        //    {
        //        Vector3 delta = TrackBuilder.Instance.ToCenterDelta(playerTransform.position);
        //        if (delta.magnitude < 0.02f)
        //            return UnityEngine.Random.Range(-1f, 1f);
        //        else
        //        {
        //            if (delta.x == 0)
        //                return UnityEngine.Random.value * Mathf.Sign(delta.z);
        //            else
        //                return UnityEngine.Random.value * Mathf.Sign(delta.x);
        //        }
        //    }
        //}
        private Vector3 cacheDirection;
        private Animator _anim;

        public override void Awake()
        {
            base.Awake();
            _instance = this;
            _anim = GetComponent<Animator>();
            playerTransform = transform;
        }
        private void Start()
        {
            moveDirection = Vector3.forward;
            cacheDirection = Vector3.forward;
            CurrentDirect = Vector3.forward;
        }
        protected override void HandleInput()
        {
            Vector3 cacheDirection = Vector3.zero;
            cacheDirection.x = Input.GetAxisRaw("Horizontal");
            if (cacheDirection.x == 0)
                cacheDirection.z = Input.GetAxisRaw("Vertical");
            if (Input.GetKey(KeyCode.G))
            {
                moveDirection = Vector3.zero;
                _anim.SetBool("Run", false);
            }
            else if (cacheDirection != Vector3.zero)
            {
                this.cacheDirection = cacheDirection;
                moveDirection = cacheDirection;
                _anim.SetBool("Run", true);
            }
            else
            {
                moveDirection = this.cacheDirection;
                _anim.SetBool("Run", true);
            }
            jump = Input.GetButton("Jump");
        }
    }
}
