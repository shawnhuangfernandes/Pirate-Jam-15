using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace PirateJam.Scripts
{
    public class CharacterManager : Singleton<CharacterManager>
    {
        //Use a gameobejct reference for now, will change to do proper visuals
        [SerializeField] private GameObject playerVisual;
        [SerializeField] private GameObject runVisual;
        [SerializeField] private GameObject idleVisual;

        private Animator _animator;

        [HideInInspector] public bool isMoving = false;
        private bool _isRunning = false;
        private static readonly int IsRunning = Animator.StringToHash("isRunning");

        private void Start()
        {
            _animator = playerVisual.GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if (isMoving == _isRunning) return;
            
            if (_isRunning)
            {
                runVisual.SetActive(false);
                idleVisual.SetActive(true);
            }
            else
            {
                runVisual.SetActive(true);
                idleVisual.SetActive(false);
            }

            _isRunning = isMoving;
                
            _animator.SetBool(IsRunning,_isRunning);
        }

        public void DisappearPlayer()
        {
            //TODO: fancy transition
            playerVisual.SetActive(false);
        }

        public void AppearPlayer()
        {
            //TODO: fancy transition
            playerVisual.SetActive(true);
        }
    }
}
