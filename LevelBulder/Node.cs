using System.Collections;
using System;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class Node : MonoBehaviour
    {
        [SerializeField]
        public Vector3 pos { get; private set; }
        [SerializeField]
        public GameObject panel { get; private set; }

        public Node(GameObject panel, float weight, Vector3 pos)
        {
            this.pos = pos;
            Vector3 scale = panel.transform.localScale;
            this.panel = Instantiate(panel, pos, Quaternion.identity);
            this.panel.transform.localScale = new Vector3(weight, scale.y, scale.z);
        }
        public void DestroyNode() => Destroy(panel);
    }
}
