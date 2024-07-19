using PirateJam.Scripts.App;
using UnityEngine;

namespace PirateJam.Scripts
{
    public class StationInteract : MonoBehaviour
    {
        [SerializeField] private GameObject indicator;
        [SerializeField] private WorkStation work;
        
        private bool isActive = false;

        private BoolAction _interactInput;

        // Start is called before the first frame update
        void Start()
        {
            _interactInput = InputManager.Instance.GetInput("BasicMove", "Interact") as BoolAction;
        }

        // Update is called once per frame
        void Update()
        {
            if (isActive && _interactInput.IsPressed)
            {
                //open activity
                work.Open();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            indicator.SetActive(true);

            isActive = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            indicator.SetActive(false);

            isActive = false;
        }
    }
}