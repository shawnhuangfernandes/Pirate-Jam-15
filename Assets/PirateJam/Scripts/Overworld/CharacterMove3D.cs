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
        private CharacterManager _manager;
        
        private void Start()
        {
            _moveAction = InputManager.Instance.GetInput("BasicMove", "Move") as VectorAction;
            _body = GetComponent<Rigidbody>();
            _manager = GetComponent<CharacterManager>();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            var delta = Vector2.zero;

            if (_moveAction.IsPressed)
            {
                delta = _moveAction.Value;
                transform.localScale = delta.x < 0 ? Vector3.one : new Vector3(-1, 1, 1);
            }

            _manager.isMoving = _moveAction.IsPressed;
           
            _body.velocity = new Vector3(delta.x, 0, delta.y) * (Time.fixedDeltaTime * moveSpeed);
        }
    }
}