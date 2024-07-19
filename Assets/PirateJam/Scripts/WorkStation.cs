using PirateJam.Scripts.App;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PirateJam.Scripts
{
    public abstract class WorkStation : MonoBehaviour
    {
        [SerializeField] private string map;
        [SerializeField] protected GameObject screen;
        public virtual void Open()
        {
            //Swap Input
            InputManager.Instance.SwapInputMaps(map);
            
            //Open scene
            screen.SetActive(true);
        }
    }
}
