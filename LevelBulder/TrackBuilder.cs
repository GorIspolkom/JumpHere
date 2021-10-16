using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Players;

namespace Assets.Scripts.Level
{
    public enum BlockType { Block, BlockShooting, BlockObstacles };
    class TrackBuilder : MonoBehaviour
    {
        private static TrackBuilder _instance;
        public static TrackBuilder Instance =>
            _instance == null ? FindObjectOfType<TrackBuilder>() : _instance;
        public Vector3 Direction { get; private set; }
        public Vector3 CurrentDirect { get; private set; }
        public bool IsTurnPosible { get; private set; }

        public float obstacleInflunce = 1f;
        [SerializeField] int maxSavedTiles = 10;
        [SerializeField] int offsetForDeleteTile = 1;
        [SerializeField] float chanceRiseForObstacleSpawn = 0.3f;
        [SerializeField] float chanceRiseForTurnDirection = 0.05f;
        [SerializeField] GameObject[] _floors;
        [SerializeField] GameObject[] _obstacles;
        [SerializeField] float _distanceJump;
        private Func<Vector3, float> GetDinstance;
        private Floor[] _spawnedFloor;
        private int currentindex;
        private int lastIndex;
        private float obstacleCounter = 0f;
        private float rotateCounter = -0.1f;
        private GameObject RandomFloor => _floors[UnityEngine.Random.Range(0, _floors.Length)];
        private GameObject RandomObstacle => _obstacles[UnityEngine.Random.Range(0, _obstacles.Length)];
        public Vector3 ToCenterVelocity(Vector3 from)
        {
            Vector3 delta = (from - _spawnedFloor[currentindex].position);
            delta.x *= Direction.z;
            delta.y = 0;
            delta.z *= Direction.x;
            return delta;
        }

        private void Awake()
        {
            _instance = this;
            currentindex = 0;
        }
        private void Start()
        {
            GetDinstance = v => v.z;
            Direction = Vector3.forward;
            CurrentDirect = Vector3.forward;

            _spawnedFloor = new Floor[maxSavedTiles];
            _spawnedFloor[0] = new Floor(Players.CharacterController.Instance.playerTransform.position + Vector3.down * 2f, _floors[0]);
            for (; lastIndex < maxSavedTiles-1; lastIndex++)
                _spawnedFloor[lastIndex + 1] = GenerateNextTile();
            offsetForDeleteTile *= -1;

            StartCoroutine(Generator());
        }
        private Floor GenerateNextTile()
        {
            //randomness rotate of direction
            float val = UnityEngine.Random.value;
            float offset = rotateCounter == 0 ? _distanceJump : 0f;
            Transform floorTransform = Instantiate(RandomFloor).transform;
            floorTransform.rotation = Quaternion.Euler(0, 90 * Direction.z, 0);
            Floor floor = _spawnedFloor[lastIndex].GetNextTile(floorTransform, Direction, offset);
            if (val < 0.5f + rotateCounter && val > 0.5f - rotateCounter)
            {
                rotateCounter = 0f;
                Direction = new Vector3(Direction.z, Direction.y, Direction.x);
            }
            else
            {
                rotateCounter += chanceRiseForTurnDirection;
            }

            //GenerateFloor
            if (rotateCounter > chanceRiseForTurnDirection)
                GenerateObstacle(floor);
            else
                obstacleCounter += chanceRiseForObstacleSpawn;
            return floor;
        }
        private void GenerateObstacle(Floor floor)
        {
            float val = UnityEngine.Random.value;
            if (val < 0.5f + obstacleCounter && val > 0.5f - obstacleCounter)
            {
                obstacleCounter = 0;
                Instantiate(RandomObstacle, floor.centerPosition + Vector3.up * 0.65f, Quaternion.Euler(0, 90 * Direction.z, 0), floor.floorObject);
            }
            else
            {
                obstacleCounter += chanceRiseForObstacleSpawn;
            }
        }

        IEnumerator Generator()
        {
            while (true)
            {
                yield return new WaitUntil(() => GetDinstance(Players.CharacterController.Instance.playerTransform.position) > GetDinstance(_spawnedFloor[currentindex].position) && !IsTurnPosible);
                //yield return new WaitForSeconds(0.5f);

                Vector3 prevPosition = _spawnedFloor[currentindex].position;
                currentindex = (currentindex + 1) % maxSavedTiles;

                if (offsetForDeleteTile > -1)
                {
                    _spawnedFloor[offsetForDeleteTile].DeleteTrack();
                    _spawnedFloor[offsetForDeleteTile] = GenerateNextTile();
                    lastIndex = (lastIndex + 1) % maxSavedTiles;

                    IsTurnPosible = GetDinstance(_spawnedFloor[currentindex].position - prevPosition) == 0;
                    if (IsTurnPosible)
                    {
                        if (CurrentDirect.x == 0)
                            GetDinstance = v => v.x;
                        else
                            GetDinstance = v => v.z;
                        Debug.Log("povorachivai dolboeb");
                    }
                }
                offsetForDeleteTile = (offsetForDeleteTile + 1) % maxSavedTiles;
            }
        }
        public void TurnAxis()
        {
            CurrentDirect = new Vector3(CurrentDirect.z, 0, CurrentDirect.x);
            IsTurnPosible = false;
        }
    }
}
