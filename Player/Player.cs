using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Player : Entity
    {
        [SerializeField]
        private PlayerData _data;
        private Rigidbody _rb;
        public Player(State state) : base(state) 
        {
            _rb = _data._player.GetComponent<Rigidbody>();
        }
    }
}
