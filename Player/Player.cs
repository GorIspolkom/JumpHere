using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Players 
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private State _state;
        [SerializeField]
        private GameObject _player;
        [SerializeField]
        private PlayerCharacters _data;
        [SerializeField]
        private Animation _anim;
        private Rigidbody _rb;

        public Player() => SetState(new Moving(this));
        public Animation GetAnimation() => _anim;
        public void Request() => _state.Action();
        public void SetState(State state) => _state = state;
        public void Jump() => _rb.AddForce(new Vector3(0, 1, 0) * _data.GetJumpPower(), ForceMode.Impulse);
        public void Move() => _rb.AddForce(new Vector3(0, 0, 1) * _data.GetVelocity(), ForceMode.Acceleration);

        private void Awake() =>
            _rb = _player.GetComponent<Rigidbody>();
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W)) { }
            else if (Input.GetKeyDown(KeyCode.Space) && !_data.GetInJump()) { Jump(); _data.SetInJump(true); }
            else { Move(); }
        }
        private void OnTriggerEnter(Collider other)
        {
            _data.SetInJump(false);
        }
    }
}


