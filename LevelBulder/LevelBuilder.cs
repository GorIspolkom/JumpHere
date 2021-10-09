using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField]
        private Track[] _tracks = new Track[10];
        [SerializeField]
        // ass overflow
        private float _panelSize { get { return _panelSize; } set { _panelSize = (float)Data.path / 3; } }
        [SerializeField]
        private enum Direction { XDirection, ZDirection };
        [SerializeField]
        private Direction _currentDirection;
        [SerializeField]
        private Vector3 _startPosition;
        [SerializeField]
        private float _distanceJump;
        [SerializeField]
        private GameObject[] _tracksPanels;
        private enum BlockType { Block, BlockShooting, BlockObstacles };

        private void Awake()
        {
            //
            _levelBuilder = this;
            _tracks[0] = new Track(_startPosition, _tracksPanels[0], _distanceJump, 7, new Vector3(0,0,0));
            for (int i = 0; i < 9; i++)
                _tracks[i + 1] = _tracks[i].GetNextTrack(ChooseTrack(), _distanceJump, 7, ChangeDirection());
        }
        private static LevelBuilder _levelBuilder;
        public static LevelBuilder GetLevelBuilder() =>
            _levelBuilder == null ? FindObjectOfType<LevelBuilder>() : _levelBuilder;
        private GameObject ChooseTrack() => _tracksPanels[Random.Range(1, _tracksPanels.Length)];
        private Vector3 ChangeDirection()
        {
            Vector3 direction;
            switch (_currentDirection)
            {
                case Direction.XDirection:
                    direction = new Vector3(0, 90, 0);
                    _currentDirection = Direction.ZDirection;
                    break;
                case Direction.ZDirection:
                    direction = new Vector3(0, 0, 0);
                    _currentDirection = Direction.XDirection;
                    break;
                default:
                    direction = Vector3.one;
                    break;
            }
            return direction;
        }
        public IEnumerator NewTrack()
        {
            for(int i = 0; i < 9; i++)
                _tracks[i] = _tracks[i + 1];
            _tracks[9] = _tracks[8].GetNextTrack(ChooseTrack(), _distanceJump, 7, ChangeDirection());

            yield return null;
        }
    }
}
