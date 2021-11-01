using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.UI
{
    class OnLoseListner : MonoBehaviour
    {
        [SerializeField] UnityEvent onLoseAction;
        private void Start()
        {
            GameHandler.Instance.SubscribeOnLose(() => onLoseAction.Invoke());
        }
    }
}
