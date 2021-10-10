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

        private void Awake() =>
            _rb = _player.GetComponent<Rigidbody>();
        private void Update()
        {
            //_rb.velocity = new Vector3(0, 0, 1);
        }
    }
}


