using PirateJam.Scripts.App;
using UnityEngine;

namespace PirateJam.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMove3D : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 3f;
        
        private VectorAction _moveAction;
        private Rigidbody _body;
        
        private void Start()
        {
            _moveAction = InputManager.Instance.GetInput("BasicMove", "Move") as VectorAction;
            _body = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            var delta = Vector2.zero;

            if (_moveAction.IsPressed)
            {
                delta = _moveAction.Value;
            }

            _body.position += new Vector3(delta.x,0,delta.y) * (Time.fixedDeltaTime * moveSpeed);
        }
    }
}