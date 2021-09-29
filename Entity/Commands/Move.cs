using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Move : ICommand
    {
        private Entity _entity;
        public Move(Entity entity) => _entity = entity;
        public void Execute()
        {
            _entity.Move();
            Debug.Log("Entity move");
        }
    }
}

