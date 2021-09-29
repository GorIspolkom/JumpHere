using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Jump : ICommand
    {
        private Entity _entity;
        public Jump(Entity entity) => _entity = entity;
        public void Execute()
        {
            _entity.Jump();
            Debug.Log("Entity jump");
        }
    }
}
