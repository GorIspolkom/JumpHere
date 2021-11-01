using System;
using UnityEngine;

namespace Assets.Scripts.Level
{
    //добавить генерацию повторных двигающихся платформ с разной амплитудой и скоростью
    class TrackBuilder : MonoBehaviour
    {
        public Vector3 Direction { get; private set; }
        public bool IsCurrentDirectX { get; private set; }
        public Vector3 CurrentFloorPosition => _spawnedFloor[currentindex == 0 ? maxSavedTiles - 1 : currentindex - 1].Position;
        public Vector3 NextFloorPosition => _spawnedFloor[currentindex].Position;
        private int NextLastIndex(int index) => (index + 1) % maxSavedTiles;

        [SerializeField] int maxSavedTiles = 10;
        [SerializeField] int offsetForDeleteTile = 2;
        [SerializeField] float chanceRiseForObstacleSpawn = 0.3f;
        [SerializeField] float chanceRiseForTurnDirection = 0.05f;
        [SerializeField] float chanceRiseForLifter = 0.05f;
        [SerializeField] float chanceForObstacleOnTurn = 0.6f;
        [SerializeField] float chanceForMovingPlatform = 0.5f;
        [SerializeField] GameObject[] _floors;
        [SerializeField] GameObject[] _obstacles;
        [SerializeField] GameObject[] _floorObstacles;
        [SerializeField] GameObject[] _breakObstacle;
        [SerializeField] GameObject[] _lifts;
        [SerializeField] float _distanceJump;
        private AbstractFloor[] _spawnedFloor;
        private int currentindex;
        private int lastIndex;
        private float obstacleCounter;
        private float rotateCounter;
        private float liftingCounter;
        private GameObject RandomFloor => _floors[UnityEngine.Random.Range(0, _floors.Length)];
        private GameObject RandomObstacle => _obstacles[UnityEngine.Random.Range(0, _obstacles.Length)];
        private GameObject RandomFloorObstacle => _floorObstacles[UnityEngine.Random.Range(0, _floorObstacles.Length)];
        private GameObject RandomBreakObstacle => _breakObstacle[UnityEngine.Random.Range(0, _breakObstacle.Length)];
        private GameObject RandomLift => _lifts[UnityEngine.Random.Range(0, _lifts.Length)];
        public Vector3 ToCenterDelta(Vector3 from)
        {
            Vector3 delta = from - _spawnedFloor[currentindex == 0 ? maxSavedTiles - 1 : currentindex - 1].Center;
            if (IsCurrentDirectX)
                delta.x = 0;
            else
                delta.z = 0;
            return delta;
        }
        private void Awake()
        {
            currentindex = 1;
        }
        private void Start()
        {
            _spawnedFloor = new AbstractFloor[maxSavedTiles];
            StartGenerateWay(Vector3.zero);
        }
        public void StartGenerateWay(Vector3 startPosition)
        {
            obstacleCounter = 0f;
            rotateCounter = 0f;
            liftingCounter = 0f;
            currentindex = 1;
            Direction = SettingsData.StartDirection;
            IsCurrentDirectX = SettingsData.StartDirection.z == 0;
            //инициализация или очистка предыдущей генерации
            if (_spawnedFloor == null)
                _spawnedFloor = new AbstractFloor[maxSavedTiles];
            else
                for (int i = 0; i < maxSavedTiles; i++)
                    if (_spawnedFloor[i] != null)
                    {
                        _spawnedFloor[i].DeleteFloor();
                        _spawnedFloor[i] = null;
                    }

            _spawnedFloor[0] = AbstractFloor.InitFloor(SpawnFloor(RandomFloor), startPosition);
            for (lastIndex = 0; lastIndex < maxSavedTiles - offsetForDeleteTile;)
                GenerateNextTile();
        }
        private Transform SpawnFloor(GameObject floorPrefab)
        {
            Transform floorTransform = Instantiate(floorPrefab).transform;
            floorTransform.rotation = Quaternion.Euler(0, 90 * Direction.x, 0);
            return floorTransform;
        }
        private AbstractFloor SpawnNextFloor(GameObject floorPrefab, bool isJump = false)
        {
            int index = NextLastIndex(lastIndex);
            if (_spawnedFloor[index] != null)
            {
                _spawnedFloor[index].DeleteFloor();
            }
            _spawnedFloor[index] = _spawnedFloor[lastIndex].GetNextTile(SpawnFloor(floorPrefab), Direction, isJump ? _distanceJump : 0f);
            lastIndex = index;
            return _spawnedFloor[lastIndex];
        }
        private void GenerateNextTile()
        {
            //randomness rotate of direction
            float val = UnityEngine.Random.value;

            if (val < 0.5f + obstacleCounter && val > 0.5f - obstacleCounter)
            {
                obstacleCounter = 0;
                GenerateObstacle();
            }
            else if (val < 0.5f + rotateCounter && val > 0.5f - rotateCounter)
            {
                rotateCounter = 0f;
                Direction = new Vector3(Direction.z, Direction.y, Direction.x);
                AbstractFloor floor = SpawnNextFloor(RandomFloor, true);
                if (chanceForObstacleOnTurn > UnityEngine.Random.value)
                {
                    floor.AddObstacle(Instantiate(RandomBreakObstacle).transform, Vector3.back * (floor.Sizes.x + _distanceJump / 2f));
                }
            }
            else if (val < 0.5f + liftingCounter && val > 0.5f - liftingCounter)
            {
                liftingCounter = 0f;
                Direction += Vector3.up;
                SpawnNextFloor(RandomLift);
                Direction -= Vector3.up;
            }
            else
            {
                SpawnNextFloor(RandomFloor);
            }
            rotateCounter += chanceRiseForTurnDirection;
            obstacleCounter += chanceRiseForObstacleSpawn;
            liftingCounter += chanceRiseForLifter;
        }
        private AbstractFloor GenerateObstacle()
        {
            if (UnityEngine.Random.value > chanceForMovingPlatform)
            {
                AbstractFloor floor = SpawnNextFloor(RandomFloor);
                floor.AddObstacle(Instantiate(RandomObstacle).transform);
                return floor;
            }
            else
                return SpawnNextFloor(RandomFloorObstacle);
        }

        public void GeneratorNext()
        {
            currentindex = NextLastIndex(currentindex);
            GenerateNextTile();
        }
        public void TurnDirection() =>
            IsCurrentDirectX = !IsCurrentDirectX;
    }
}
