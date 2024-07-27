using System;
using UnityEngine;

/*
 * This script makes an object follow the mouse with optional constraints 
 */

namespace PirateJam.Scripts.WorkStations.DragonDrop
{
    public class MouseFollower : MonoBehaviour
    {

        [Tooltip("The layers this object will view as a trackable surface")]
        [SerializeField] private LayerMask layerMask;

        private bool _isFollowingMouse = false;

        private Vector3 _objectFollowPos;

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (_isFollowingMouse == false) return;

            GetMousePosition();
        }

        private void GetMousePosition()
        {   
            var mousePosRay = _mainCamera.ScreenPointToRay(Input.mousePosition);        

            if (Physics.Raycast(mousePosRay, out var hitInfo, Mathf.Infinity, layerMask)) 
            {
                _objectFollowPos = hitInfo.point;
            }

            transform.position = _objectFollowPos;
        }

        public void SetFollow(bool shouldFollow) => _isFollowingMouse = shouldFollow;   
    }
}
