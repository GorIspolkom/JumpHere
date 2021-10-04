using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class Track 
    {
        public Node lastNode { get; private set; }
        private List<Node> _nodes;
        private int _nodeNums;

        public Track(Vector3 startPos, float weight, Vector3 direction)
        {
            _nodes = new List<Node>();
            _nodeNums = Random.Range(2, 8);
            Debug.Log("Nums of block in track " + _nodeNums);
            for (int i = 0; i < _nodeNums; i++)
            {
                _nodes.Add(new Node(LevelBuilder.GetLevelBuilder().ChooseBlockType(), weight, startPos));
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
            //
            _nodes[0].panel.AddComponent<BoxCollider>().isTrigger = true;
            _nodes[0].panel.GetComponent<BoxCollider>().size = new Vector3(7f, 7f, 7f);
            _nodes[0].panel.AddComponent<StartTrigger>();
        }
    }
}
