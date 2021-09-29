using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities 
{
    public class Entity 
    {
        private State _state = null;
        public Entity(State state)
        {
            ChangeState(state);
        }
        public void ChangeState(State state)
        {
            _state = state;
            _state.SetContext(this);
        }
        public void DoAction() => _state.Action();
        public virtual void Move() { }
        public virtual void Jump() { }
    }
}

