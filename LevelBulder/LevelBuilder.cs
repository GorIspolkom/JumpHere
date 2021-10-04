using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField]
        private Track[] _tracks = new Track[5];
        [SerializeField]
        // ass overflow
        private float _panelSize { get { return _panelSize; } set { _panelSize = (float)Data._path / 3; } }
        [SerializeField]
        private enum Direction { XDirection, ZDirection};
        [SerializeField]
        private Direction _currentDirection;
        [SerializeField]
        private Vector3 _startPosition;
        [SerializeField]
        private float _distanceJump;
        [SerializeField]
        private GameObject[] _blocks;
        private enum BlockType { Block, BlockShooting, BlockObstacles };

        private void Awake()
        {
            //
            _levelBuilder = this;
            _tracks[0] = new Track(_startPosition, 0.9f, new Vector3(0, 0, 7));
            for (int i = 1; i < 5; i++)
                _tracks[i] = CreateNewTrack(_tracks[i - 1].lastNode.pos, _distanceJump);
            //_tracks[1] = CreateNewTrack(_tracks[0].lastNode.pos, _distanceJump);
            //_tracks[2] = CreateNewTrack(_tracks[1].lastNode.pos, _distanceJump);
            //_tracks[3] = CreateNewTrack(_tracks[2].lastNode.pos, _distanceJump);
            //_tracks[4] = CreateNewTrack(_tracks[3].lastNode.pos, _distanceJump);
        }
        private static LevelBuilder _levelBuilder;
        public static LevelBuilder GetLevelBuilder() =>
            _levelBuilder == null ? FindObjectOfType<LevelBuilder>() : _levelBuilder;
        public GameObject ChooseBlockType()
        {
            GameObject block;
            BlockType type = (BlockType)Random.Range(0, 3);
            Debug.Log("Num of block type " + type);
            switch (type)
            {
                case BlockType.Block:
                    block = _blocks[0];
                    break;
                case BlockType.BlockShooting:
                    block = _blocks[1];
                    break;
                case BlockType.BlockObstacles:
                    block = _blocks[2];
                    break;
                default:
                    block = null;
                    break;
            }
            return block;
        }
        public IEnumerator NewTrack()
        {
            //
            yield return new WaitForSeconds(5f);
            _tracks[0].DeleteTrack();
            _tracks[0] = _tracks[1];
            _tracks[1] = _tracks[2];
            _tracks[2] = _tracks[3];
            _tracks[3] = _tracks[4];
            _tracks[4] = CreateNewTrack(_tracks[3].lastNode.pos, _distanceJump);
        }
        private Track CreateNewTrack(Vector3 lastPos, float distance) 
        {
            Vector3 direction;
            Debug.Log("Last node pos " + lastPos);
            switch (_currentDirection)
            {
                case Direction.XDirection:
                    lastPos.x += distance;
                    _currentDirection = Direction.ZDirection;
                    direction = new Vector3(7, 0, 0);
                    break;
                case Direction.ZDirection:
                    lastPos.z += distance;
                    _currentDirection = Direction.XDirection;
                    direction = new Vector3(0, 0, 7);
                    break;
                default:
                    direction = new Vector3();
                    break;
            }
            Debug.Log("Create new track init");
            return new Track(lastPos, 0.9f, direction);
        }
    }
}
