using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class Track 
    {
        [SerializeField]
        public Node lastNode { get; private set; }
        [SerializeField]
        private List<Node> _nodes;
        [SerializeField]
        private int _nodeNums;

        public Track(Vector3 startPos, float weight, Vector3 direction)
        {
            _nodes = new List<Node>();
            _nodeNums = (int)Random.Range(2f, 7f);
            Debug.Log("Nums of block in track " + _nodeNums);
            for (int i = 0; i < _nodeNums; i++)
            {
                _nodes.Add(new Node(BlockManager.GetBlockManager().ChooseBlockType(), weight, startPos));
                startPos = startPos + direction;
            }
            lastNode = _nodes[_nodes.Count - 1];
            SetNodesStatus();
        }
        public void DeleteTrack()
        {
            foreach (Node node in _nodes)
                node.DestroyNode();
        }
        private void SetNodesStatus() 
        {
            _nodes[0].panel.AddComponent<BoxCollider>().isTrigger = true;
            _nodes[0].panel.AddComponent<StartTrigger>();
        }
    }
}
