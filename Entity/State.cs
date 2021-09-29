using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public abstract class State
    {
        protected Entity _entity;
        public void SetContext(Entity entity) => _entity = entity;
        public abstract void Action();
    }
}