using System;
using UnityEngine;

namespace Assets.Scripts.Level
{
    [Serializable]
    abstract class FloorSpawner
    {
        [SerializeField] protected float chanceRise;
        protected float counter;
        protected bool IsSpawnReady
        {
            get
            {
                float val = UnityEngine.Random.value;
                return (val < 0.5f + counter && val > 0.5f - counter);
            }
        }
        public void Reset() => counter = 0f;
        public void Update() => counter += chanceRise;
        public abstract bool TryGenerate(ref TrackData data);
    }
    [Serializable]
    class DefaultSpawner : FloorSpawner
    {
        [SerializeField] float chanceForMovingPlatform;
        [SerializeField] GameObject[] obstacles;
        [SerializeField] GameObject[] floorObstacle;
        GameObject RandomObstacle => obstacles[UnityEngine.Random.Range(0, obstacles.Length)];
        GameObject RandomFloorObstacle => floorObstacle[UnityEngine.Random.Range(0, floorObstacle.Length)];
        public override bool TryGenerate(ref TrackData data)
        {
            if (IsSpawnReady)
            {
                counter = 0f;
                if (UnityEngine.Random.value > chanceForMovingPlatform)
                {
                    AbstractFloor floor = data.SpawnNextFloor(data.RandomFloor);
                    floor.AddObstacle(GameObject.Instantiate(RandomObstacle).transform);
                }
                else
                    data.SpawnNextFloor(RandomFloorObstacle);
            }
            else
                data.SpawnNextFloor(data.RandomFloor);
            return true;
        }
    }
    [Serializable]
    class TurnSpawner : FloorSpawner
    {
        [SerializeField] float chanceForObstacleOnTurn;
        [SerializeField] GameObject[] breakObstacles;
        GameObject RandomBreakObstacle => breakObstacles[UnityEngine.Random.Range(0, breakObstacles.Length)];
        public override bool TryGenerate(ref TrackData data)
        {
            //if (counter == chanceRise)
            //{
            //    data.SpawnNextFloor(data.RandomFloor, true);
            //    return true;
            //}
            if (IsSpawnReady)
            {
                counter = 0f;
                data.SpawnNextFloor(data.RandomFloor);
                data.TurnDirection();
                AbstractFloor floor = data.SpawnNextFloor(data.RandomFloor, true, true);
                if (chanceForObstacleOnTurn > UnityEngine.Random.value)
                    floor.AddObstacle(GameObject.Instantiate(RandomBreakObstacle).transform, Vector3.back * (floor.Sizes.x + data.distanceJump / 2f));
                return true;
            }
            else
                return false;
        }
    }
    [Serializable]
    class LiftSpawner : FloorSpawner
    {
        [SerializeField] GameObject[] lifts;
        GameObject RandomLift => lifts[UnityEngine.Random.Range(0, lifts.Length)];
        public override bool TryGenerate(ref TrackData data)
        {
            if (IsSpawnReady)
            {
                counter = 0f;
                data.SpawnNextFloor(RandomLift);
                return true;
            }
            return false;
        }
    }
    [Serializable]
    struct TrackData
    {
        [HideInInspector] public Vector3 Direction;
        [HideInInspector] public bool IsCurrentDirectX;
        public AbstractFloor PrevFloor => _spawnedFloor[(currentindex + maxSavedTiles - 2) % maxSavedTiles];
        //{
        //    get
        //    {
        //        int index = currentindex - 2;
        //        switch (index)
        //        {
        //            case -2:
        //                return _spawnedFloor[maxSavedTiles - 2].Position;
        //            case -1:
        //                return _spawnedFloor[maxSavedTiles - 1].Position;
        //            default:
        //                return _spawnedFloor[currentindex].Position;
        //        }
        //    }
        //}
        public AbstractFloor CurrentFloor => _spawnedFloor[(currentindex + maxSavedTiles - 1) % maxSavedTiles];
        public AbstractFloor NextFloor => _spawnedFloor[currentindex];
        public int NextLastIndex(int index) => (index + 1) % maxSavedTiles;

        public int maxSavedTiles;
        public int offsetForDeleteTile;
        public int offsetForExtraAdd;
        public float distanceJump;
        [SerializeField] GameObject[] floors;
        public GameObject RandomFloor => floors[UnityEngine.Random.Range(0, floors.Length)];
        private AbstractFloor[] _spawnedFloor;
        private int currentindex;
        private int lastIndex;
        public Vector3 ToCenterDelta(Vector3 from)
        {
            Vector3 delta = from - _spawnedFloor[currentindex == 0 ? maxSavedTiles - 1 : currentindex - 1].Center;
            if (IsCurrentDirectX)
                delta.x = 0;
            else
                delta.z = 0;
            return delta;
        }
        public Vector3 DeltaFromFloor(Vector3 from)
        {
            return from - _spawnedFloor[currentindex].Position;
        }
        public void ResetData(Vector3 startPosition)
        {
            currentindex = 2;
            lastIndex = 0;
            offsetForExtraAdd = offsetForDeleteTile;
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
        }
        public AbstractFloor SpawnNextFloor(GameObject floorPrefab, bool isExtra = false, bool isJump = false)
        {
            int index = NextLastIndex(lastIndex);
            if (_spawnedFloor[index] != null)
            {
                _spawnedFloor[index].DeleteFloor();
            }
            _spawnedFloor[index] = _spawnedFloor[lastIndex].GetNextTile(SpawnFloor(floorPrefab), Direction, isJump ? distanceJump : 0f);
            if (isExtra)
                offsetForExtraAdd++;
            lastIndex = index;
            return _spawnedFloor[lastIndex];
        }
        public void MoveNext() =>
            currentindex = NextLastIndex(currentindex);
        private Transform SpawnFloor(GameObject floorPrefab)
        {
            Transform floorTransform = GameObject.Instantiate(floorPrefab).transform;
            floorTransform.rotation = Quaternion.Euler(0, 90 * Direction.x, 0);
            return floorTransform;
        }
        public void TurnDirection() => Direction = new Vector3(Direction.z, Direction.y, Direction.x);
    }
    //добавить генерацию повторных двигающихся платформ с разной амплитудой и скоростью
    class TrackBuilder2 : MonoBehaviour
    {
        [SerializeField] TrackData _trackData;
        [SerializeField] DefaultSpawner floorWithObstacle;
        [SerializeField] LiftSpawner liftSpawner;
        [SerializeField] TurnSpawner turnSpawner;
        public TrackData data => _trackData;
        public void StartGenerateWay(Vector3 startPosition)
        {
            _trackData.ResetData(startPosition);
            for(int i = 0; i < data.maxSavedTiles; i++)
                GenerateNextTile();
        }
        private void Start()
        {
            StartGenerateWay(Vector3.zero);
        }
        private void GenerateNextTile()
        {
            if (_trackData.offsetForExtraAdd > 0)
            {
                _trackData.offsetForExtraAdd--;
                return;
            }
            if (turnSpawner.TryGenerate(ref _trackData)) { }
            else if (liftSpawner.TryGenerate(ref _trackData)) { }
            else if (floorWithObstacle.TryGenerate(ref _trackData)) { }

            turnSpawner.Update();
            floorWithObstacle.Update();
            liftSpawner.Update();
        }

        public void GeneratorNext()
        {
            _trackData.MoveNext();
            GenerateNextTile();
        }
        public void TurnCurrentDirection() =>
           _trackData.IsCurrentDirectX = !_trackData.IsCurrentDirectX;
    }
}
