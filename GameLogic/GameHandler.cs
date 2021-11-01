using Assets.Scripts.Level;
using Assets.Scripts.Players;
using Assets.Scripts.UI;
using HairyEngine.HairyCamera;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public class GameHandler : MonoBehaviour
    {
        private static GameHandler _instance;
        public static GameHandler Instance => _instance;
        [SerializeField] TrackBuilder2 trackBuilder;
        [SerializeField] CharacterRunnerController characterController;
        [SerializeField] ComponentPanelController losePanel;
        private Action onLoseAction; 
        public bool IsGame { get; private set; }
        public bool IsPause { get; private set; }
        private Func<Vector3, float> DinstanceByDirection;
        private float minHight;
        private void Awake()
        {
            _instance = this;
        }
        public void SubscribeOnLose(Action onLoseSub)
        {
            onLoseAction += onLoseSub;
        }
        public void StartGame()
        {
            GenerateGame();
            StartGeneratedGame();
        }
        public void GenerateGame()
        {
            StopAllCoroutines();
            SessionData.Init();
            characterController.Init(Vector3.zero);
            trackBuilder.StartGenerateWay(Vector3.zero);
            DinstanceByDirection = v => v.z;
            minHight = trackBuilder.data.CurrentFloor.Position.y - 5f;
        }
        public void StartGeneratedGame()
        {
            IsGame = true;
            IsPause = false;
            StartCoroutine(Generator());
        }
        public void SetPause(bool isPause)
        {
            IsPause = isPause;
            Time.timeScale = isPause ? 0 : 1;
            characterController.Stop();
        }
        void Start()
        {
            IsGame = false;
            IsPause = true;
            GenerateGame();
        }

        void Update()
        {
            if (IsGame)
            {
                characterController.UpdateForce(trackBuilder.data.ToCenterDelta(characterController.playerTransform.position));
                SessionData.AddPath(characterController.Velocity * Time.deltaTime);
                if (characterController.playerTransform.position.y < minHight)
                    LoseAction();
            }
            if (Input.GetKeyDown(KeyCode.R))
                StartGame();
        }
        public void LoseAction()
        {
            Debug.Log("Pososi y yy y y y");
            losePanel.OpenPanel();
            ProfileData.Instance.Update((long)SessionData.timer, (long)SessionData.points);
            IsGame = false;
            onLoseAction?.Invoke();
        }
        IEnumerator Generator()
        {
            while (IsGame)
            {
                yield return new WaitUntil(() => DinstanceByDirection(characterController.playerTransform.position) > DinstanceByDirection(trackBuilder.data.NextFloor.Position));
                minHight = trackBuilder.data.CurrentFloor.Position.y - 5f;
                Debug.Log(minHight);

                trackBuilder.GeneratorNext();
                Vector3 delta = trackBuilder.data.NextFloor.Position - trackBuilder.data.CurrentFloor.Position;
                if (DinstanceByDirection(delta) == 0)
                {
                    yield return new WaitUntil(() => Input.GetButton("Jump"));
                    characterController.TurnDirection();
                    trackBuilder.data.TurnDirection();

                    if (delta.z == 0)
                        DinstanceByDirection = v => v.x;
                    else
                        DinstanceByDirection = v => v.z;
                }
            }
        }
    }
}