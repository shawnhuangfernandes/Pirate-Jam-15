using System;
using UnityEngine;

namespace PirateJam.Scripts
{
    public class BillboardSprite : MonoBehaviour
    {
        private Transform _trans, _cameraTrans;
        public bool inFocus = false;

        private void Start()
        {
            _trans = transform;
            _cameraTrans = Camera.main.transform;
        }

        private void LateUpdate()
        {
            if (!inFocus) return;
            
            _trans.forward = _cameraTrans.forward;
        }
    }
}
