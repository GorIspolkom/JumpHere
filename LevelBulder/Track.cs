using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class Track 
    {
        [SerializeField]
        public Node _lastNode;
        [SerializeField]
        private List<Node> _nodes;
        [SerializeField]
        private int _nodeNums;

        public Track(Vector3 startPos)
        {
            _nodes = new List<Node>();
            _nodeNums = (int)Random.Range(2f, 7f);
            for (int i = 0; i < _nodeNums; i++)
            {
                _nodes.Add(new Node(BlockManager.GetBlockManager().ChooseBlockType(), 7, startPos));
                startPos = startPos + new Vector3(0, 0, 7);
            }
            _lastNode = _nodes[_nodes.Count - 1];
        }
    }
}
