using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Assets.Scripts.Players
{
    [Serializable]
    public struct PlayerCharacters 
    {
        [SerializeField]
        private float _velocity;
        [SerializeField]
        private float _jumpPower;
        [SerializeField]
        private bool _inJump;
        [SerializeField]
        private bool _isAlive;

        public PlayerCharacters(float velocity, float jumpPower)
        {
            _isAlive = true;
            _inJump = false;
            _velocity = velocity;
            _jumpPower = jumpPower;
        }

        public float GetVelocity() => _velocity;
        public float GetJumpPower() => _jumpPower;
        public bool GetInJump() => _inJump;
        public bool GetIsAlive() => _isAlive;
        public void SetInJump(bool inJump) => _inJump = inJump;
    }
}


