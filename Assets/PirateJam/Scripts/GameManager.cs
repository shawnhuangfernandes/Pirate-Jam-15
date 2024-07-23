using System;
using Cinemachine;
using PirateJam.Scripts.App;
using UnityEditor;
using UnityEngine;

namespace PirateJam.Scripts
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private CinemachineVirtualCamera MenuCam;

        [SerializeField] private CinemachineVirtualCamera MoveCam;

        [Header("Cameras"), SerializeField] private CinemachineVirtualCamera WorkStation0;
        [SerializeField] private CinemachineVirtualCamera WorkStation1;
        [SerializeField] private CinemachineVirtualCamera WorkStation2;
        [SerializeField] private CinemachineVirtualCamera WorkStation3;


        public enum GameState
        {
            Menu,
            Move,
            WorkStation,
            Pause
        }

        [SerializeField] private GameState currentState;

        private GameObject currentCamera;

        // Start is called before the first frame update
        public void Start()
        {
            Initialize();
        }
        
        public void Initialize()
        {
            SwapGameState(GameState.Menu);
            currentCamera = MenuCam.gameObject;
        }

        public void MenuStartPlay()
        {
            SwapGameState(GameState.Move);
        }


        public void SwapGameState(GameState state, int workstation = 0)
        {
            if (state == currentState) return;

            switch (state)
            {
                case GameState.Menu:
                    InputManager.Instance.SwapInputMaps("Menu");
                    currentCamera.SetActive(false);
                    currentCamera = MenuCam.gameObject;
                    currentCamera.SetActive(true);
                    break;
                case GameState.Move:
                    InputManager.Instance.SwapInputMaps("BasicMove");
                    currentCamera.SetActive(false);
                    currentCamera = MoveCam.gameObject;
                    currentCamera.SetActive(true);
                    break;
                case GameState.WorkStation:
                    InputManager.Instance.SwapInputMaps("Workstation");
                    currentCamera.SetActive(false);
                    currentCamera = workstation switch
                    {
                        0 => WorkStation0.gameObject,
                        1 => WorkStation1.gameObject,
                        2 => WorkStation2.gameObject,
                        3 => WorkStation3.gameObject,
                        _ => throw new Exception("Workstation number doesn't exist")
                    };
                    currentCamera.SetActive(true);
                    break;
                case GameState.Pause:
                    //TODO: Throw PauseMenu 
                    InputManager.Instance.SwapInputMaps("Menu");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            currentState = state;
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}