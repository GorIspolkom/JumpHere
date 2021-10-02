using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public abstract class State
    {
        protected Player _player;
        protected State(Player player) => _player = player;
        public abstract void Action();
    }
}