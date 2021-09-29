using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        [SerializeField]
        public int _velocity { get; set; }
        [SerializeField]
        public GameObject _player { get; set; }
        [SerializeField]
        public bool _isAlive { get; set; }
        [SerializeField]
        public double _points { get; set; }
        [SerializeField]
        public double _id { get; private set; }
        [SerializeField]
        public int _jumpPower { get; private set; }
        [SerializeField]
        public int _weight { get; private set; }

        public PlayerData(int velocity, GameObject player, int jumpPower, int weight)
        {
            _points = 0;
            _isAlive = true;
            _velocity = velocity;
            _player = player;
            _jumpPower = jumpPower;
            _weight = weight;
        }
    }
}

