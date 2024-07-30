using System;
using System.Collections.Generic;
using Cinemachine;
using FMODUnity;
using PirateJam.Scripts.App;
using PirateJam.Scripts.WorkStations;
using UnityEditor;
using UnityEngine;
using Yarn.Unity;

namespace PirateJam.Scripts
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("SceneReferences"),Space ]
        
        [ Header("Cameras"),SerializeField] private CinemachineVirtualCamera MenuCam;

        [SerializeField] private CinemachineVirtualCamera MoveCam;

        [Space,SerializeField] private CinemachineVirtualCamera WorkStation0;
        [SerializeField] private CinemachineVirtualCamera WorkStation1;
        [SerializeField] private CinemachineVirtualCamera WorkStation2;
        [SerializeField] private CinemachineVirtualCamera WorkStation3;

        [Header("Objects")] [SerializeField] private GameObject TEMPwinscreen;
        [SerializeField] private DialogueRunner runner;

       [HideInInspector] public InMemoryVariableStorage VariableStorage;

       [SerializeField] private FMODUnity.EventReference musicEvent;
       [SerializeField] private FMODUnity.EventReference ambienceEvent;
       [SerializeField] private FMODUnity.EventReference menuEndEvent;



        public enum GameState
        {
            None,
            Menu,
            Move,
            WorkStation,
            Pause
        }

        [Header("Stats"),SerializeField] private GameState currentState = GameState.None;
        [ShowOnly] public List<WorkStation> workStations;

        private GameObject currentCamera;

        // Start is called before the first frame update
        public void Start()
        {
            Initialize();
            VariableStorage = runner.gameObject.GetComponent<InMemoryVariableStorage>();
        }
        
        public void Initialize()
        {
            currentCamera = MenuCam.gameObject;
            SwapGameState(GameState.Menu);
            RuntimeManager.PlayOneShot(ambienceEvent);
        }

        public void MenuStartPlay()
        {
            SwapGameState(GameState.Move);
            RuntimeManager.PlayOneShot(menuEndEvent);
            
            RunExposition();
        }


        public void SwapGameState(GameState state, int workstation = 0)
        {
            if (state == currentState) return;

            switch (state)
            {
                case GameState.Menu:
                    InputManager.Instance.SwapInputMaps("Menu");
                    RuntimeManager.PlayOneShot(musicEvent);
                   // musicStartInstance.start();
                    currentCamera.SetActive(false);
                    currentCamera = MenuCam.gameObject;
                    currentCamera.SetActive(true);
                    break;
                case GameState.Move:
                    InputManager.Instance.SwapInputMaps("BasicMove");
                    CharacterManager.Instance.AppearPlayer();
                    currentCamera.SetActive(false);
                    currentCamera = MoveCam.gameObject;
                    currentCamera.SetActive(true);
                    break;
                case GameState.WorkStation:
                    InputManager.Instance.SwapInputMaps("Workstation");
                    CharacterManager.Instance.DisappearPlayer();
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

        public bool GameComplete()
        {
            foreach (var station in workStations)
            {
                if (!station.IsDone()) return false;
            }
            
            //TODO: GAME COMPLETE SEQUENCE

            Debug.Log("GAME COMPLETE!");
            TEMPwinscreen.SetActive(true);
            SwapGameState(GameState.Pause);
            return true;
        }

        public void RunQuip(string node)
        {
            runner.StartDialogue(node);
        }

        public void RunExposition()
        {
            runner.StartDialogue("Start");
        }
        

    }
}