using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField]
        private Track[] _tracks = new Track[3];
        [SerializeField]
        private float _panelSize { get { return _panelSize; } set { _panelSize = (float)Data._path / 3; } }
        [SerializeField]
        private enum Direction { XDirection, ZDirection};
        [SerializeField]
        private Direction _currentDirection;
        [SerializeField]
        private Vector3 _startPosition;
        [SerializeField]
        private float _distanceJump;

        private void Awake()
        {
            _levelBuilder = this;
            _tracks[0] = new Track(_startPosition, 7, new Vector3(0, 0, 7));
            _tracks[1] = CreateNewTrack(_tracks[0].lastNode.pos, _distanceJump);
        }
        private static LevelBuilder _levelBuilder;
        public static LevelBuilder GetLevelBuilder() =>
            _levelBuilder == null ? FindObjectOfType<LevelBuilder>() : _levelBuilder;
        public void NewTrack()
        {
            _tracks[2] = CreateNewTrack(_tracks[1].lastNode.pos, _distanceJump);
            _tracks[0].DeleteTrack();
            _tracks[0] = _tracks[1];
            _tracks[1] = _tracks[2];
            _tracks[2] = null;
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
            return new Track(lastPos, 7, direction);
        }
    }
}
