using System.Collections;
using System;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class Node : MonoBehaviour
    {
        [SerializeField]
        private GameObject _panel;
        [SerializeField]
        private Vector3 _pos;

        public Node(GameObject panel, float weight, Vector3 pos)
        {
            _panel = panel;
            _pos = pos;
            Vector3 scale = _panel.transform.localScale;
            _panel.transform.localScale = new Vector3(weight, scale.y, scale.z);
            Instantiate(_panel, _pos, Quaternion.identity);
        }
    }

}
