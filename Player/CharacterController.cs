using ECM.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player
{
    class CharacterController : BaseCharacterController
    {
        private static BaseCharacterController _instance;
        public static BaseCharacterController Instance => _instance;

        public override void Awake()
        {
            base.Awake();
            _instance = this;
        }
        public override void Update()
        {
            base.Update();
            Vector3 velocity = movement.velocity;
            velocity.y = 0;
            Data.AddPath(velocity.magnitude * Time.deltaTime);
        }
    }
}
