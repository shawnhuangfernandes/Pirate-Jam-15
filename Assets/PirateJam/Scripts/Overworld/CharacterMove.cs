using PirateJam.Scripts.App;
using UnityEngine;

namespace PirateJam.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMove : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 3f;
        
        private VectorAction _moveAction;
        private Rigidbody2D _body;
        
        private void Start()
        {
            _moveAction = InputManager.Instance.GetInput("BasicMove", "Move") as VectorAction;
            _body = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            var delta = Vector2.zero;

            if (_moveAction.IsPressed)
            {
                delta = _moveAction.Value;
            }

            _body.position += delta * (Time.fixedDeltaTime * moveSpeed);
        }
    }
}